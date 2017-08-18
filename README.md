MySqlSharp
===
Extremely Fast MySQL Driver for C#, work in progress.

Async does not make code magically fast, rather it is a nice and performant tool to handle concurrency.
One of the biggest performance sinks in database drivers is serialization.
I've released the two fastest serializers [ZeroFormatter](https://github.com/neuecc/ZeroFormatter) and [MessagePack for C#](https://github.com/neuecc/MessagePack-CSharp) for .NET, this MySQL driver is using the same optimization techniques.

| Method              |     Mean | Error |  Gen 0 | Allocated |
|---------------------|---------:|------:|-------:|----------:|
| MySqlSharp          | 893.2 us |    NA |      - |     968 B |
| MySqlConnector      | 863.0 us |    NA | 9.7656 |   69153 B |
| AsyncMysqlConnector | 843.8 us |    NA | 5.8594 |   47000 B |

The graph shows that this library uses 50 times less memory than other similar libraries (runtime speed has not been optimized yet)

Currently this driver is under development and has not reached alpha status yet.
However, I will be glad if it receives attention and I would like to hear your opinion.

Performance enhancements
---
* Synchronous and Asynchronous code doesn't make the code fast, optimizations should be done for both.
* Extensive use of Value Types, usage of structs for parser state allows for reduction of memory allocations.
* Directly deserialize from binary to number.
* Avoid ADO.NET abstraction, expose primitive API and ADO.NET should be built on top of it.
* Micro ORM(like Dapper) built on top of primitive MySQL API.
* Fast query text parsing with `ref char[]` buffer(avoiding usage of `StringBuilder` and `String`) - [FastQueryParser](https://github.com/neuecc/MySqlSharp/blob/master/src/MySqlSharp/Internal/FastQueryParser.cs).
* Use prepared-statement cache like [Npgsql approach](http://www.roji.org/prepared-statements-in-npgsql-3-2).

MySQL [X-Protocol](https://dev.mysql.com/doc/internals/en/x-protocol.html) increases server-client performance but [Amazon Aurora](https://aws.amazon.com/rds/aurora/details/) and other services do not implement X-Protocol yet.

Struct Packet Reader
---
The MySQL protocol stream has many chunked packets.
If a packet reader class is instantiated for every packet (as MySQLConnector does), it will result in many allocations.

![image](https://user-images.githubusercontent.com/46207/29018333-a8902444-7b95-11e7-8215-d4e0000e0fac.png)

This library tries to avoid allocations by saving the state of the packet reader in a struct and utilizing static helper methods.

```csharp
// Standard API, used by conventional libraries
using (var packetReader = new PacketReader())
using (var protocolReader = new ProtocolReader(packetReader))
{
    protocolReader.ReadTextResultSet();
}

// Struct Packet Reader
var reader = new PacketReader(); // has reading(offset) state
ProtocolReader.ReadTextResultSet(ref reader); // (ref PacketReader) modifies the struct
```

Using structs instead of instantiating objects reduces memory allocation a lot.

Direct deserialization from binary to number
---
MySQL row data is normally represented in text when being sent from the server to the client.
An integer for example is transferred literally as a string of ASCII symbols.
If the server wants to send the integer 10 to the client, it sends "10", or [49, 48] if it is represented as a byte sequence.
This normally requires string encoding and parsing, like in the following example:

```csharp
// has an additional string allocation and parsing cost
int.Parse(Encoding.UTF8.GetString(binary));
```

However MySqlSharp uses a [NumberConverter](https://github.com/neuecc/MySqlSharp/blob/master/src/MySqlSharp/Internal/NumberConverter.cs) to directly convert it to a number.

```csharp
public static Int32 ToInt32(byte[] bytes, int offset, int count)
{
    // Min: -2147483648
    // Max: 2147483647
    // Digits: 10

    if (bytes[offset] != Minus)
    {
        switch (count)
        {
            case 1:
                return (System.Int32)(((Int32)(bytes[offset] - Zero)));
            case 2:
                return (System.Int32)(((Int32)(bytes[offset] - Zero) * 10) + ((Int32)(bytes[offset + 1] - Zero)));
            case 3:
                return (System.Int32)(((Int32)(bytes[offset] - Zero) * 100) + ((Int32)(bytes[offset + 1] - Zero) * 10) + ((Int32)(bytes[offset + 2] - Zero)));
            // snip case 4..9
            case 10:
                return (System.Int32)(((Int32)(bytes[offset] - Zero) * 1000000000) + ((Int32)(bytes[offset + 1] - Zero) * 100000000) + ((Int32)(bytes[offset + 2] - Zero) * 10000000) + ((Int32)(bytes[offset + 3] - Zero) * 1000000) + ((Int32)(bytes[offset + 4] - Zero) * 100000) + ((Int32)(bytes[offset + 5] - Zero) * 10000) + ((Int32)(bytes[offset + 6] - Zero) * 1000) + ((Int32)(bytes[offset + 7] - Zero) * 100) + ((Int32)(bytes[offset + 8] - Zero) * 10) + ((Int32)(bytes[offset + 9] - Zero)));
            default:
                throw new ArgumentException("Int32 out of range count");
        }
    }
    else
    {
        // snip... * -1
    }
}
```

Primitive API for MySQL
---
Expose [MySQL Protocol](http://imysql.com/mysql-internal-manual/text-protocol.html)(`COM_QUIT`, `COM_QUERY`, `COM_PING`, etc...) directly.

```csharp
// Driver Direct
var driver = new MySqlDriver(option);
driver.Open();

var reader = driver.Query("select 1"); // COM_QUERY
while (reader.Read())
{
    var v = reader.GetInt32(0);
}

// you can use other native APIs
driver.Ping(); // COM_PING
driver.Statistics(); // COM_STATISTICS
```

Of course, you can also use ADO.NET.

```csharp
// ADO.NET Wrapper
var conn = new MySqlConnection("connStr");
conn.Open();

var cmd = conn.CreateCommand();
cmd.CommandText = "select 1";

var reader = cmd.ExecuteReader();
while (reader.Read())
{
    var v = reader.GetInt32(0);
}
```
