using System;

namespace MySqlSharp.Protocol
{
    // https://dev.mysql.com/doc/dev/mysql-server/8.0.0/group__group__cs__capabilities__flags.html
    [Flags]
    public enum CapabilitiesFlags : ulong
    {
        None = 0,

        /// <summary>Use the improved version of Old Password Authentication.</summary>
        LongPassword = 1,
        /// <summary>Send found rows instead of affected rows in EOF_Packet.</summary>
        FoundRows = 2,
        /// <summary>Get all column flags.</summary>
        LongFlag = 4,
        /// <summary>Database (schema) name can be specified on connect in Handshake Response Packet.</summary>
        ConnectWithDB = 8,
        /// <summary>Don't allow database.table.column.</summary>
        NoSchema = 16,
        /// <summary>Compression protocol supported.</summary>
        Compress = 32,
        /// <summary>Special handling of ODBC behavior.</summary>
        Odbc = 64,
        /// <summary>Can use LOAD DATA LOCAL.</summary>
        LocalFiles = 128,
        /// <summary>Ignore spaces before '('.</summary>
        IgnoreSpace = 256,
        /// <summary>New 4.1 protocol.</summary>
        Protocol41 = 512,
        /// <summary>This is an interactive client.</summary>
        Interactive = 1024,
        /// <summary>Use SSL encryption for the session.</summary>
        Ssl = 2048,
        /// <summary>Client only flag.</summary>
        IgnoreSigpipe = 4096,
        /// <summary>Client knows about transactions.</summary>
        Transactions = 8192,
        /// <summary>DEPRECATED: Old flag for 4.1 protocol.</summary>
        Reserved = 16384,
        /// <summary>DEPRECATED: Old flag for 4.1 authentication.</summary>
        SecureConnection = 32768,
        /// <summary>Enable/disable multi-stmt support.</summary>
        MultiStatements = (1UL << 16),
        /// <summary>Enable/disable multi-results.</summary>
        MultiResults = (1UL << 17),
        /// <summary>Multi-results and OUT parameters in PS-protocol.</summary>
        PSMultiResults = (1UL << 18), // PreparedStatementMultiResults
        /// <summary>Client supports plugin authentication.</summary>
        PluginAuth = (1UL << 19),
        /// <summary>Client supports connection attributes.</summary>
        ConnectAttrs = (1UL << 20),
        /// <summary>Enable authentication response packet to be larger than 255 bytes.</summary>
        PluginAuthLenencClientData = (1UL << 21), // PluginAuthLengthEncodedClientData
        /// <summary>Don't close the connection for a user account with expired password.</summary>
        CanHandleExpiredPasswords = (1UL << 22),
        /// <summary>Capable of handling server state change information.</summary>
        SessionTrack = (1UL << 23),
        /// <summary>Client no longer needs EOF_Packet and will use OK_Packet instead.</summary>
        DepreacateEof = (1UL << 24),
        /// <summary>Verify server certificate.</summary>
        SslVerifyServerCert = (1UL << 30),
        /// <summary>Don't reset the options after an unsuccessful connect.
        RememberOptions = (1UL << 31),
    }

    public static class CapabilitiesFlagsExtensions
    {
        public static bool HasFlag(this CapabilitiesFlags capabilities, CapabilitiesFlags flag)
        {
            return (capabilities & flag) != 0;
        }
    }
}