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



            var stream = client.GetStream();

            var buffer = new byte[1024];
            var count = stream.Read(buffer, 0, buffer.Length);

            var p = ProtocolReader.ReadHandshakeV10(buffer, 0, out var readSize);

            // TODO:SSL



        }
    }
}
