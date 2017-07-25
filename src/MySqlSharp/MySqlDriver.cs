using MySqlSharp.Protocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MySqlSharp
{
    // TODO:API...?

    public class MySqlDriver
    {
        MySqlConnectionOptions options;

        public MySqlDriver(MySqlConnectionOptions options)
        {
            this.options = options;
        }

        public async Task ConnectAsync()
        {
            var ipAddresses = await Dns.GetHostAddressesAsync(options.Server);
            var client = new TcpClient(AddressFamily.InterNetwork);

            await client.ConnectAsync(ipAddresses, options.Port);

            // TODO:use raw socket.

            var stream = client.GetStream();

            var buffer = new byte[1024];
            var count = stream.Read(buffer, 0, buffer.Length);

            var reader = new BufferReader(buffer, 0, count);
            var p = ProtocolReader.ReadHandshakeV10(ref reader);

            // TODO:SSL

            var writer = new BufferWriter();

            ProtocolWriter.WriteHandshakeResponse41(ref writer, options, p);

            var writeBuffer = writer.GetBuffer();
            stream.Write(writeBuffer.Array, writeBuffer.Offset, writeBuffer.Count);

            count = stream.Read(buffer, 0, buffer.Length);


            var reader2 = new BufferReader(buffer, 0, count);
            var error = ProtocolReader.ReadErrorPacket(ref reader2);

        }
    }
}
