using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Protocol
{

    /*
 
https://web.archive.org/web/20170404102640/https://dev.mysql.com/doc/internals/en/connection-phase-packets.html#packet-Protocol::Handshake 
https://github.com/pubnative/mysqlproto-go/blob/master/handshake_v10.go

Protocol::HandshakeV10 
 
1              [0a] protocol version
string[NUL]    server version
4              connection id
string[8]      auth-plugin-data-part-1
1              [00] filler
2              capability flags (lower 2 bytes)

if more data in the packet:
1              character set
2              status flags
2              capability flags (upper 2 bytes)

if capabilities & CLIENT_PLUGIN_AUTH {
    1              length of auth-plugin-data
} else {
    1              [00]
}

string[10]     reserved (all [00])

if capabilities & CLIENT_SECURE_CONNECTION {
    string[$len]   auth-plugin-data-part-2 ($len=MAX(13, length of auth-plugin-data - 8))
    if capabilities & CLIENT_PLUGIN_AUTH {
        string[NUL]    auth-plugin name
}

 */

    public class HandshakeV10
    {
        public byte ProtocolVersion { get; private set; }
        public string ServerVersion { get; private set; }
        public UInt32 ConnectionId { get; private set; }
        public string AuthPluginDataPart1 { get; private set; }
        public CapabilitiesFlags CapabilityFlags { get; private set; }
        public byte CharacterSet { get; private set; }
        public byte[] StatusFlags { get; private set; }
        public string AuthPluginName { get; private set; }

        public static HandshakeV10 Load(byte[] bytes, int offset, out int readSize)
        {
            var startOffset = offset;

            var header = ProtocolReader.ReadPacketHeader(bytes, offset, out var readOffset);
            offset += readOffset;

            var handshakeV10 = new HandshakeV10();

            handshakeV10.ProtocolVersion = BinaryUtil.ReadByte(bytes, ref offset);
            handshakeV10.ServerVersion = BinaryUtil.ReadNullTerminatedString(bytes, ref offset);
            handshakeV10.ConnectionId = BinaryUtil.ReadUInt32(bytes, ref offset);
            handshakeV10.AuthPluginDataPart1 = BinaryUtil.ReadString(bytes, ref offset, 8);

            if (BinaryUtil.ReadByte(bytes, ref offset) != 0) throw new Exception("TODO");

            var capabilityFlagsLow = BinaryUtil.ReadUInt16(bytes, ref offset);
            handshakeV10.CapabilityFlags = (CapabilitiesFlags)capabilityFlagsLow;

            // if more needs...

            handshakeV10.CharacterSet = BinaryUtil.ReadByte(bytes, ref offset);
            handshakeV10.StatusFlags = BinaryUtil.ReadBytes(bytes, ref offset, 2);
            var capabilityFlagsHigh = BinaryUtil.ReadUInt16(bytes, ref offset);

            handshakeV10.CapabilityFlags |= (CapabilitiesFlags)(capabilityFlagsHigh << 16);

            readSize = offset - startOffset;
            return handshakeV10;
        }
    }

    public static partial class ProtocolReader
    {
        // TODO:StreamReader?

        public static HandshakeV10 ReadHandshakeV10(byte[] bytes, int offset, out int readSize)
        {
            return HandshakeV10.Load(bytes, offset, out readSize);
        }
    }
}