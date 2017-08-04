using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public static void WriteHandshakeResponse41(ref PacketWriter writer, MySqlConnectionOptions options, HandshakeV10 initialHandshake)
        {
            // writer.WriteInt32((int)CapabilitiesFlags.Protocol41); // TODO:other flags



            writer.WriteInt32((int)(
                CapabilitiesFlags.Protocol41 |
                CapabilitiesFlags.LongPassword |
                CapabilitiesFlags.SecureConnection |
                (initialHandshake.CapabilityFlags & CapabilitiesFlags.PluginAuth) |
                (initialHandshake.CapabilityFlags & CapabilitiesFlags.PluginAuthLenencClientData) |
                CapabilitiesFlags.MultiStatements |
                CapabilitiesFlags.MultiResults |
                CapabilitiesFlags.PSMultiResults |
                CapabilitiesFlags.LocalFiles |
                (string.IsNullOrWhiteSpace(options.Database) ? 0 : CapabilitiesFlags.ConnectWithDB)// |
                                                                                                   //(cs.UseAffectedRows ? 0 : CapabilitiesFlags.FoundRows) |
                                                                                                   //(cs.UseCompression ? CapabilitiesFlags.Compress : CapabilitiesFlags.None) |
                                                                                                   //additionalCapabilities
                ));





            writer.WriteInt32(0x40000000); // TODO:max packet size
            writer.WriteByte((byte)CharacterSet.utf8mb4_bin);
            writer.WriteBytes(Empty23Bytes);
            writer.WriteNullTerminatedString(options.UserId);

            if (initialHandshake.CapabilityFlags.HasFlag(CapabilitiesFlags.PluginAuthLenencClientData))
            {
                var auth = CraeateNative41AuthResponse(initialHandshake.AuthPluginData, options.Password);
                writer.WriteByte((byte)auth.Length);
                writer.WriteBytes(auth);
            }
            else
            {
                writer.WriteByte(1);
            }

            if (initialHandshake.CapabilityFlags.HasFlag(CapabilitiesFlags.ConnectWithDB))
            {
                writer.WriteNullTerminatedString(options.Database);
            }

            if (initialHandshake.CapabilityFlags.HasFlag(CapabilitiesFlags.PluginAuth))
            {
                writer.WriteNullTerminatedString(initialHandshake.AuthPluginName);
            }

            if (initialHandshake.CapabilityFlags.HasFlag(CapabilitiesFlags.ConnectAttrs))
            {
                // TODO:other attributes...
            }
        }

        static byte[] CraeateNative41AuthResponse(byte[] randomChallenge, string password)
        {
            // SHA1( password ) XOR SHA1( "20-bytes random data from server" <concat> SHA1( SHA1( password ) ) )
            using (var sha1 = SHA1.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                var left = sha1.ComputeHash(passwordBytes);

                var rightSeed = new byte[40];
                Buffer.BlockCopy(randomChallenge, 0, rightSeed, 0, 20);
                Buffer.BlockCopy(sha1.ComputeHash(sha1.ComputeHash(passwordBytes)), 0, rightSeed, 20, 20);

                var right = sha1.ComputeHash(rightSeed);

                for (int i = 0; i < left.Length; i++)
                {
                    left[i] ^= right[i];
                }

                return left;
            }
        }
    }
}