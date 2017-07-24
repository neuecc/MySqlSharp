using System;

namespace MySqlSharp.Internal
{
    internal static partial class NumberConverter
    {
        // const byte Minus, Dot, Zero

        public static Single ToSingle(byte[] bytes, int offset, int count)
        {
            // search Dot.
            var limit = offset + count;
            var dotPoint = limit;
            for (int i = offset; i < limit; i++)
            {
                if (bytes[i] == Dot)
                {
                    dotPoint = i;
                    break;
                }
            }

            // prepare
            var isMinus = bytes[offset] == Minus;
            var integerDigits = dotPoint - offset - (isMinus ? 1 : 0);

            // loop
            var cursor = offset;
            var end = offset + count;

            if (isMinus) cursor += 1;

            float value = 0.0f;
            var loopCount = 0;
            while (cursor < dotPoint)
            {
                value += (bytes[cursor] - Zero) * (float)Math.Pow(10, integerDigits - 1 - loopCount);
                loopCount++;
                cursor++;
            }

            cursor++;
            loopCount = 0;
            while (cursor < end)
            {
                value += (bytes[cursor] - Zero) * (float)Math.Pow(0.1, loopCount + 1);
                loopCount++;
                cursor++;
            }

            return isMinus ? value * -1 : value;
        }

        public static Double ToDouble(byte[] bytes, int offset, int count)
        {
            // search Dot.
            var limit = offset + count;
            var dotPoint = limit;
            for (int i = offset; i < limit; i++)
            {
                if (bytes[i] == Dot)
                {
                    dotPoint = i;
                    break;
                }
            }

            // prepare
            var isMinus = bytes[offset] == Minus;
            var integerDigits = dotPoint - offset - (isMinus ? 1 : 0);

            // loop
            var cursor = offset;
            var end = offset + count;

            if (isMinus) cursor += 1;

            double value = 0.0;
            var loopCount = 0;
            while (cursor < dotPoint)
            {
                value += (bytes[cursor] - Zero) * (double)Math.Pow(10, integerDigits - 1 - loopCount);
                loopCount++;
                cursor++;
            }

            cursor++;
            loopCount = 0;
            while (cursor < end)
            {
                value += (bytes[cursor] - Zero) * (double)Math.Pow(0.1, loopCount + 1);
                loopCount++;
                cursor++;
            }

            return isMinus ? value * -1 : value;
        }

    }
}
