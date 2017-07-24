using MySqlSharp.Internal;
using System.Linq;
using System;
using System.Text;
using Xunit;

namespace MySqlSharp.Tests
{
    public class NumberConverterTest
    {
        const int Offset = 5;

        static (byte[], int) MakeBytes(string s)
        {
            var str = Encoding.UTF8.GetBytes(s);
            return (new byte[] { 1, 2, 3, 4, 5 }.Concat(str).Concat(new byte[] { 1, 2, 3, 4, 5 }).ToArray(), str.Length);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("12345678")]
        [InlineData("123456789")]
        [InlineData("1234567810")]
        [InlineData("2147483647")]
        [InlineData("-1")]
        [InlineData("-12")]
        [InlineData("-123")]
        [InlineData("-1234")]
        [InlineData("-12345")]
        [InlineData("-123456")]
        [InlineData("-1234567")]
        [InlineData("-12345678")]
        [InlineData("-123456789")]
        [InlineData("-1234567810")]
        [InlineData("-2147483648")]
        public void ToInt32(string s)
        {
            var (bytes, len) = MakeBytes(s);
            NumberConverter.ToInt32(bytes, Offset, len).Is(int.Parse(s));
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("12345678")]
        [InlineData("123456789")]
        [InlineData("1234567810")]
        [InlineData("2147483647")]
        public void ToUInt32(string s)
        {
            var (bytes, len) = MakeBytes(s);
            NumberConverter.ToUInt32(bytes, Offset, len).Is(uint.Parse(s));
        }

        [Theory]
        [InlineData("-340282300000000000000000000000000000000")]
        [InlineData("0")]
        [InlineData("0.0")]
        [InlineData("1231410")]
        [InlineData("1231410.42523525")]
        [InlineData("-1231410")]
        [InlineData("-1231410.42523525")]
        [InlineData("340282300000000000000000000000000000000")]
        public void ToSingle(string s)
        {
            var (bytes, len) = MakeBytes(s);
            NumberConverter.ToSingle(bytes, Offset, len).Is(Single.Parse(s));
        }

        [Theory]
        [InlineData("-340282300000000000000000000000000000000342423423424")]
        [InlineData("-340282300000000000000000000000000000000")]
        [InlineData("0")]
        [InlineData("0.0")]
        [InlineData("1231410")]
        [InlineData("1231410.42523525")]
        [InlineData("-1231410")]
        [InlineData("-1231410.42523525")]
        [InlineData("340282300000000000000000000000000000000")]
        [InlineData("3402823000000000000000000000000000000006363636363")]
        public void ToDouble(string s)
        {
            var (bytes, len) = MakeBytes(s);
            var a = NumberConverter.ToDouble(bytes, Offset, len);
            var b = Double.Parse(s);
            a.ToString().Is(b.ToString());
        }
    }
}
