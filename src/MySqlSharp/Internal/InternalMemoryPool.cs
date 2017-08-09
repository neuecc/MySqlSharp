using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Internal
{
    internal static class InternalMemoryPool
    {
        [ThreadStatic]
        static byte[] buffer = null;

        [ThreadStatic]
        static char[] charBuffer = null;

        public static byte[] GetBuffer()
        {
            if (buffer == null)
            {
                buffer = new byte[65536];
            }
            return buffer;
        }

        public static char[] GetCharBuffer()
        {
            if (charBuffer == null)
            {
                charBuffer = new char[65536];
            }
            return charBuffer;
        }
    }
}
