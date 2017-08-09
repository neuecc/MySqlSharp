using System;
using System.Collections.Generic;
using System.Text;

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

            // TODO:require ensure buffer capacity

            var s = sql.Format;
            var i = 0;
            var index = 0;
            var end = s.Length;
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
                    if (argValue is string)
                    {
                        // TODO:If string, require escape
                        var strValue = argValue as string;

                        buffer[index++] = '\'';
                        strValue.CopyTo(0, buffer, index, strValue.Length);
                        index += strValue.Length;
                        buffer[index++] = '\'';
                    }
                    else
                    {
                        var strValue = argValue.ToString();
                        strValue.CopyTo(0, buffer, index, strValue.Length);
                        index += strValue.Length;
                    }
                    i++;
                }
                else
                {
                    buffer[index++] = s[i++];
                }
            }

            return index;
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
    }
}
