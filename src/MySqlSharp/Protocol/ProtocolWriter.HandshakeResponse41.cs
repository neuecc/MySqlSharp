using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Protocol
{
    // https://web.archive.org/web/20170404102640/https://dev.mysql.com/doc/internals/en/connection-phase-packets.html#packet-Protocol::HandshakeResponse41
    // https://github.com/pubnative/mysqlproto-go/blob/master/handshake_response41.go

    /*
     
4              capability flags, CLIENT_PROTOCOL_41 always set
4              max-packet size
1              character set
string[23]     reserved (all [0])
string[NUL]    username

if capabilities & CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA {
    lenenc-int     length of auth-response
    string[n]      auth-response
} else if capabilities & CLIENT_SECURE_CONNECTION {
    1              length of auth-response
    string[n]      auth-response
} else {
    string[NUL]    auth-response
}

if capabilities & CLIENT_CONNECT_WITH_DB {
    string[NUL]    database
}

if capabilities & CLIENT_PLUGIN_AUTH {
    string[NUL]    auth plugin name
}

if capabilities & CLIENT_CONNECT_ATTRS {
    lenenc-int     length of all key-values
    lenenc-str     key
    lenenc-str     value
    if-more data in 'length of all key-values', more keys and value pairs
}

    */

    public static partial class ProtocolWriter
    {
        static readonly byte[] Empty23Bytes = new byte[23];

        public static void WriteHandshakeResponse41(ref byte[] bytes, int offset, MySqlConnectionOptions options)
        {
            offset += BinaryUtil.WriteInt32(ref bytes, offset, (int)Protocol.CapabilitiesFlags.Protocol41); // TODO:Protocol41 and others...
            offset += BinaryUtil.WriteInt32(ref bytes, offset, 0x40000000); // size...
            offset += BinaryUtil.WriteByte(ref bytes, offset, (byte)CharacterSet.utf8mb4_bin);
            offset += BinaryUtil.WriteBytes(ref bytes, offset, Empty23Bytes);
            offset += BinaryUtil.WriteNullTerminatedString(ref bytes, offset, options.UserId);







            throw new NotImplementedException();
        }
    }
}
