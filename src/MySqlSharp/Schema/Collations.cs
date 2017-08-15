using MySqlSharp.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Schema
{
    // https://dev.mysql.com/doc/refman/5.6/ja/information-schema.html

    public class Collations
    {
        public string COLLATION_NAME { get; private set; }
        public string CHARACTER_SET_NAME { get; private set; }
        public long ID { get; private set; }

        [ValueConverter(typeof(string), typeof(YesNoStringToBooleanConverter))]
        public bool IS_DEFAULT { get; private set; }

        [ValueConverter(typeof(string), typeof(YesNoStringToBooleanConverter))]
        public bool IS_COMPILED { get; private set; }

        public long SORTLEN { get; private set; }
    }

    public class YesNoStringToBooleanConverter : IValueConverter<string, bool>
    {
        public bool Convert(string value)
        {
            return string.Equals(value, "YES", StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class InformationSchemaQuery
    {
        /// <summary>SELECT * from COLLATIONS</summary>
        public static Collations Collations(MySqlDriver driver)
        {
            var query = driver.Query("SELECT * from COLLATIONS");
            throw new NotImplementedException("TODO:Mapping");
        }
    }
}
