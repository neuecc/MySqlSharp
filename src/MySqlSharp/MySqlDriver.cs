using MySqlSharp.Internal;
using MySqlSharp.Protocol;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MySqlSharp
{
    // TODO:API...?

    public class MySqlDriver : IDisposable
    {
        MySqlConnectionOptions options;

        TcpClient client;

        public MySqlDriver(MySqlConnectionOptions options)
        {
            this.options = options;
        }

        public async Task ConnectAsync()
        {
            var ipAddresses = await Dns.GetHostAddressesAsync(options.Server);
            var client = new TcpClient(AddressFamily.InterNetwork);
            client.ReceiveTimeout = 99999;
            client.SendTimeout = 99999;

            await client.ConnectAsync(ipAddresses, options.Port);

            // TODO:use raw socket.

            var stream = client.GetStream();
            stream.WriteTimeout = 99999;
            stream.ReadTimeout = 99999;

            var buffer = new byte[1024];
            var count = stream.Read(buffer, 0, buffer.Length);

            var reader = PacketReader.Create(buffer, 0, count);
            var p = ProtocolReader.ReadHandshakeV10(ref reader);

            // TODO:SSL

            var writer = PacketWriter.Create();

            ProtocolWriter.WriteHandshakeResponse41(ref writer, options, p);


            var writeBuffer = writer.GetBuffer(1);

            // TODO:
            // int<3 > payload_length 
            //int<1 > sequence_id Sequence ID
            // string<var> payload[len = payload_length] payload of the packet

            var okurumono = new byte[4 + writeBuffer.Count];


            stream.Write(writeBuffer.Array, writeBuffer.Offset, writeBuffer.Count);

            count = stream.Read(buffer, 0, buffer.Length);





            var reader2 = PacketReader.Create(buffer, 0, count);
            var response = ProtocolReader.ReadResponsePacket(ref reader2);

            response.ThrowIfError();

            //writer = PacketWriter.Create();

            // PING!
            //writer.WriteByte((byte)DBCmd.PING);
            //writer.WritePacketHeader(0);


            //ProtocolWriter.WriteQuery(ref writer, "SELECT version(), version()");

            //writeBuffer = writer.GetBuffer(0);
            //stream.Write(writeBuffer.Array, writeBuffer.Offset, writeBuffer.Count);

            //count = stream.Read(buffer, 0, buffer.Length);

            //var reader3 = PacketReader.Create(buffer, 0, count);
            //var response3 = ProtocolReader.ReadResponsePacket(ref reader3);

            ////response3.ThrowIfError();

            //// ProtocolReader.


            //var columnCount = (int)reader3.ReadLengthEncodedInteger();

            //for (int i = 0; i < columnCount; i++)
            //{
            //    // read again?
            //    // var header1 = ProtocolReader.ReadPacketHeader(ref reader3);

            //    //count = stream.Read(buffer, 0, buffer.Length);
            //}



            this.client = client;
        }

        // Command

        PacketReader SyncWriteAndRead(ref PacketWriter writer, int sequenceId, Stream stream)
        {
            var segment = writer.GetBuffer(sequenceId);

            stream.Write(segment.Array, segment.Offset, segment.Count);

            // reuse write buffer
            var buffer = segment.Array;
            var readCount = stream.Read(buffer, 0, buffer.Length);

            return PacketReader.Create(buffer, 0, readCount);
        }

        public OkPacket Ping()
        {
            using (var stream = client.GetStream())
            {
                var readWriteBuffer = InternalMemoryPool.GetBuffer();
                var writer = PacketWriter.Create(readWriteBuffer);
                ProtocolWriter.WritePing(ref writer);

                var reader = SyncWriteAndRead(ref writer, 0, stream);

                var response = ProtocolReader.ReadResponsePacket(ref reader);
                response.ThrowIfError();

                return (OkPacket)response;
            }
        }

        public TextResultSet Query(string query)
        {
            var stream = client.GetStream();

            var readWriteBuffer = InternalMemoryPool.GetBuffer();
            var writer = PacketWriter.Create(readWriteBuffer);
            ProtocolWriter.WriteQuery(ref writer, query);

            var reader = SyncWriteAndRead(ref writer, 0, stream);

            // TODO: Ok or ResultSet?
            var set = ProtocolReader.ReadTextResultSet(ref reader);

            return set;
        }

        public void Prepare(string query)
        {
            var stream = client.GetStream();

            var readWriteBuffer = InternalMemoryPool.GetBuffer();
            var writer = PacketWriter.Create(readWriteBuffer);
            ProtocolWriter.WriteQuery(ref writer, query);

            var reader = SyncWriteAndRead(ref writer, 0, stream);

            // COM_STMT_PREPARE_OK on success, ERR_Packet otherwise
            if (reader.IsErrorPacket())
            {
                throw ErrorPacket.Parse(ref reader).ToMySqlException();
            }

            // reader.

        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
