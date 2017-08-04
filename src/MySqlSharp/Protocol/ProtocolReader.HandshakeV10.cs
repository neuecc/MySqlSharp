using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Protocol
{

    /*
 
https://web.archive.org/web/20170404102640/https://dev.mysql.com/doc/internals/en/connection-phase-packets.html#packet-Protocol::Handshake 
https://mariadb.com/kb/en/mariadb/1-connecting-connecting/#initial-handshake-packet

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
        public byte[] AuthPluginData { get; private set; }
        public CapabilitiesFlags CapabilityFlags { get; private set; }
        public CharacterSet CharacterSet { get; private set; }
        public byte[] StatusFlags { get; private set; }
        public string AuthPluginName { get; private set; }

        public static HandshakeV10 Parse(ref PacketReader reader)
        {
            var handshakeV10 = new HandshakeV10();

            handshakeV10.ProtocolVersion = reader.ReadByte();
            handshakeV10.ServerVersion = reader.ReadNullTerminatedString();
            handshakeV10.ConnectionId = reader.ReadUInt32();
            handshakeV10.AuthPluginData = reader.ReadBytes(8);

            reader.ReadNext(1);

            var capabilityFlagsLow = reader.ReadUInt16();
            handshakeV10.CapabilityFlags = (CapabilitiesFlags)capabilityFlagsLow;

            if (reader.Remaining > 0)
            {
                handshakeV10.CharacterSet = (CharacterSet)reader.ReadByte();
                handshakeV10.StatusFlags = reader.ReadBytes(2);
                var capabilityFlagsHigh = reader.ReadUInt16();
                handshakeV10.CapabilityFlags |= (CapabilitiesFlags)(capabilityFlagsHigh << 16);

                int pluginDataLength = 0;
                if ((handshakeV10.CapabilityFlags & CapabilitiesFlags.PluginAuth) != 0)
                {
                    pluginDataLength = reader.ReadByte();
                }
                else
                {
                    reader.ReadNext(1);
                }

                reader.ReadNext(10);

                if ((handshakeV10.CapabilityFlags & CapabilitiesFlags.SecureConnection) != 0)
                {
                    var part2 = reader.ReadBytes(Math.Max(13, pluginDataLength - 8));
                    var temp = new byte[handshakeV10.AuthPluginData.Length + part2.Length];
                    Buffer.BlockCopy(handshakeV10.AuthPluginData, 0, temp, 0, handshakeV10.AuthPluginData.Length);
                    Buffer.BlockCopy(part2, 0, temp, handshakeV10.AuthPluginData.Length, part2.Length);
                    handshakeV10.AuthPluginData = temp;
                }

                if ((handshakeV10.CapabilityFlags & CapabilitiesFlags.PluginAuth) != 0)
                {
                    handshakeV10.AuthPluginName = reader.ReadNullTerminatedString();
                }
            }

            return handshakeV10;
        }
    }

    public static partial class ProtocolReader
    {
        public static HandshakeV10 ReadHandshakeV10(ref PacketReader reader)
        {
            return HandshakeV10.Parse(ref reader);
        }
    }
}