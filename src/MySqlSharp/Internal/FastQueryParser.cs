using System;
using System.Collections;
using System.Collections.Generic;

namespace MySqlSharp.Internal
{
    internal static class FastQueryParser
    {
        [ThreadStatic]
        static char[] argBuffer;

        public static int Parse(ref char[] buffer, FormattableString sql)
        {
            if (argBuffer == null)
            {
                argBuffer = new char[10];
            }

            var s = sql.Format;
            var i = 0;
            var index = 0;
            var end = s.Length;
            var finalLength = s.Length;

            EnsureCapacity(ref buffer, finalLength);
            while (i < end)
            {
                if (s[i] == '{')
                {
                    i++;
                    var argCount = 0;
                    while (s[i] != '}')
                    {
                        argBuffer[argCount++] = s[i++];
                    }
                    var argIndex = FormatArgumentToInt32(argBuffer, argCount);
                    var argValue = sql.GetArgument(argIndex);
                    WriteParameter(ref buffer, ref index, argValue, ref finalLength);
                    i++;
                }
                else
                {
                    buffer[index++] = s[i++];
                }
            }

            return index;
        }

        static void WriteParameter(ref char[] buffer, ref int index, object argValue, ref int finalLength)
        {
            switch (argValue)
            {
                case string strValue:
                    finalLength += (strValue.Length + 2); // quote + value + quote
                    EnsureCapacity(ref buffer, finalLength);

                    buffer[index++] = '\'';
                    for (int j = 0; j < strValue.Length; j++)
                    {
                        // https://dev.mysql.com/doc/refman/5.6/en/string-literals.html#character-escape-sequences

                        // mysql_real_escape_string
                        // https://github.com/mysql/mysql-server/blob/mysql-5.7.5/mysys/charset.c#L823-L932
                        switch (strValue[j])
                        {
                            case '\0':
                                EnsureCapacity(ref buffer, ++finalLength);
                                buffer[index++] = '\\';
                                buffer[index++] = '0';
                                break;
                            case '\n':
                                EnsureCapacity(ref buffer, ++finalLength);
                                buffer[index++] = '\\';
                                buffer[index++] = 'n';
                                break;
                            case '\r':
                                EnsureCapacity(ref buffer, ++finalLength);
                                buffer[index++] = '\\';
                                buffer[index++] = 'r';
                                break;
                            case '\\':
                                EnsureCapacity(ref buffer, ++finalLength);
                                buffer[index++] = '\\';
                                buffer[index++] = '\\';
                                break;
                            case '\'':
                                EnsureCapacity(ref buffer, ++finalLength);
                                buffer[index++] = '\\';
                                buffer[index++] = '\'';
                                break;
                            case '\"':
                                EnsureCapacity(ref buffer, ++finalLength);
                                buffer[index++] = '\\';
                                buffer[index++] = '\"';
                                break;
                            case (char)26:
                                EnsureCapacity(ref buffer, ++finalLength);
                                buffer[index++] = '\\';
                                buffer[index++] = 'Z';
                                break;
                            default:
                                buffer[index++] = strValue[j];
                                break;
                        }
                    }
                    buffer[index++] = '\'';
                    break;
                case IEnumerable collectionValue:
                    finalLength += 2; // "(" + ")"
                    EnsureCapacity(ref buffer, finalLength);

                    var isFirst = true;
                    buffer[index++] = '(';
                    foreach (var item in collectionValue)
                    {
                        if (isFirst)
                        {
                            isFirst = false;
                        }
                        else
                        {
                            finalLength++;
                            EnsureCapacity(ref buffer, finalLength);
                            buffer[index++] = ',';
                        }
                        WriteParameter(ref buffer, ref index, item, ref finalLength);
                    }
                    buffer[index++] = ')';
                    break;
                default:
                    // TODO:each argValue type to string, more case switch...!
                    var otherValue = argValue.ToString();
                    finalLength += otherValue.Length;
                    EnsureCapacity(ref buffer, finalLength);
                    otherValue.CopyTo(0, buffer, index, otherValue.Length);
                    index += otherValue.Length;
                    break;
            }
        }

        static Int32 FormatArgumentToInt32(char[] number, int count)
        {
            const byte Zero = 48;

            switch (count)
            {
                case 1:
                    return (System.Int32)(((Int32)(number[0] - Zero)));
                case 2:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 10) + ((Int32)(number[1] - Zero)));
                case 3:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 100) + ((Int32)(number[1] - Zero) * 10) + ((Int32)(number[2] - Zero)));
                case 4:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 1000) + ((Int32)(number[1] - Zero) * 100) + ((Int32)(number[2] - Zero) * 10) + ((Int32)(number[3] - Zero)));
                case 5:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 10000) + ((Int32)(number[1] - Zero) * 1000) + ((Int32)(number[2] - Zero) * 100) + ((Int32)(number[3] - Zero) * 10) + ((Int32)(number[4] - Zero)));
                case 6:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 100000) + ((Int32)(number[1] - Zero) * 10000) + ((Int32)(number[2] - Zero) * 1000) + ((Int32)(number[3] - Zero) * 100) + ((Int32)(number[4] - Zero) * 10) + ((Int32)(number[5] - Zero)));
                case 7:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 1000000) + ((Int32)(number[1] - Zero) * 100000) + ((Int32)(number[2] - Zero) * 10000) + ((Int32)(number[3] - Zero) * 1000) + ((Int32)(number[4] - Zero) * 100) + ((Int32)(number[5] - Zero) * 10) + ((Int32)(number[6] - Zero)));
                case 8:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 10000000) + ((Int32)(number[1] - Zero) * 1000000) + ((Int32)(number[2] - Zero) * 100000) + ((Int32)(number[3] - Zero) * 10000) + ((Int32)(number[4] - Zero) * 1000) + ((Int32)(number[5] - Zero) * 100) + ((Int32)(number[6] - Zero) * 10) + ((Int32)(number[7] - Zero)));
                case 9:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 100000000) + ((Int32)(number[1] - Zero) * 10000000) + ((Int32)(number[2] - Zero) * 1000000) + ((Int32)(number[3] - Zero) * 100000) + ((Int32)(number[4] - Zero) * 10000) + ((Int32)(number[5] - Zero) * 1000) + ((Int32)(number[6] - Zero) * 100) + ((Int32)(number[7] - Zero) * 10) + ((Int32)(number[8] - Zero)));
                case 10:
                    return (System.Int32)(((Int32)(number[0] - Zero) * 1000000000) + ((Int32)(number[1] - Zero) * 100000000) + ((Int32)(number[2] - Zero) * 10000000) + ((Int32)(number[3] - Zero) * 1000000) + ((Int32)(number[4] - Zero) * 100000) + ((Int32)(number[5] - Zero) * 10000) + ((Int32)(number[6] - Zero) * 1000) + ((Int32)(number[7] - Zero) * 100) + ((Int32)(number[8] - Zero) * 10) + ((Int32)(number[9] - Zero)));
                default:
                    throw new ArgumentException("number.Length out of range count");
            }
        }

        static void EnsureCapacity(ref char[] bytes, int finalLength)
        {
            if (finalLength > bytes.Length)
            {
                int num = finalLength;
                if (num < bytes.Length * 2)
                {
                    num = bytes.Length * 2;
                }

                var newArray = new char[num];
                Buffer.BlockCopy(bytes, 0, newArray, 0, bytes.Length * sizeof(char));
                bytes = newArray;
            }
        }
    }
}
