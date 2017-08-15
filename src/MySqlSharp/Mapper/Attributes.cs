using System;

namespace MySqlSharp.Mapper
{
    public interface IValueConverter<TFrom, TTo>
    {
        TTo Convert(TFrom value);
    }

    public class ValueConverterAttribute : Attribute
    {
        public Type ColumnType { get; }
        public Type ValueConverterType { get; }

        public ValueConverterAttribute(Type columnType, Type valueConverterType)
        {
            this.ColumnType = columnType;
            this.ValueConverterType = valueConverterType;
        }
    }

    public class ColumnNameAttribute : Attribute
    {
        public string ColumnName { get; }

        public ColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
