using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace TaskManager.Platform.Infrastructure.Models
{
    public class IsoDateTimeConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            if (value is DateTime dateTime)
            {
                return new Primitive(dateTime.ToString("o")); // ISO 8601 format
            }

            return new Primitive();
        }

        public object? FromEntry(DynamoDBEntry entry)
        {
            var primitive = entry as Primitive;
            if (primitive is null) return null;

            return DateTime.Parse(primitive.AsString());
        }
    }
}
