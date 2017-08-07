MySqlSharp
===
Extremely Fast MySQL Driver for C#, work in progress.

Async does not mean fast, I thought database driver is the serialization problem over TCP. I've released two fastest serializers [ZeroFormatter](https://github.com/neuecc/ZeroFormatter) and [MessagePack for C#](https://github.com/neuecc/MessagePack-CSharp), this MySQL driver is using there optimization techiniques.

![image](https://user-images.githubusercontent.com/46207/29018376-ce3ecba0-7b95-11e7-90f0-d32d7c80b04b.png)

It shows 1/50 memory usage decrease(not optimized yet, I should reduce means perfs)

Currently this driver is under development and is not filled with alpha version. However, I am glad if there is your attention and opinion.

How achive performance?
---
* Async over Sync, Sync over Async, both slows. We should implementes optimized for both.
* Use Mutable Struct Reader, it enables reduce memory allocation.
* Direct deserialize from binary to number.
* Avoid ADO.NET abstrtaction, expose primitive api and ADO.NET should be built on top of it.
* Micro ORM(like Dapper) built on top of primitive MySQL API.
* Use prepared-statement cache like [Npgsql approach](http://www.roji.org/prepared-statements-in-npgsql-3-2).

MySQL [X-Protocol](https://dev.mysql.com/doc/internals/en/x-protocol.html) increase server-client performance but [Amazon Aurora](https://aws.amazon.com/rds/aurora/details/) or others can not use X-Protocol yet.

Mutable Struct Reader
---
MySQL protocol stream has many chunked packets. If you make(or as MySQLConnecto does) packet reader, requires many reader allocation.

![image](https://user-images.githubusercontent.com/46207/29018333-a8902444-7b95-11e7-8215-d4e0000e0fac.png)

I've implements struct packet reader and static helper methods.

```csharp
// Standard API, I don't use it
using (var packetReader = new PacketReader())
using (var protocolReader = new ProtocolReader(packetReader))
{
    protocolReader.ReadTextResultSet();
}

// Mutable Struct Reader
var reader = new PacketReader(); // struct but mutable, has reading(offset) state
ProtocolReader.ReadTextResultSet(ref reader); // (ref PacketReader)
```

It reduce many memory allocations.

Direct deserialize from binary to number
---
MySQL row data is normaly text protocol. If retrieve integer, requires string encoding and parsing.

```csharp
// string allocation and parsing cost
int.Parse(Encoding.UTF8.GetString(binary));
```

I've introduced [NumberConverter](https://github.com/neuecc/MySqlSharp/blob/master/src/MySqlSharp/Internal/NumberConverter.cs) it enables direct convert. 

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

var reader = driver.Query("selct 1"); // COM_QUERY
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