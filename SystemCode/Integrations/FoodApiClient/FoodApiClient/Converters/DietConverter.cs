using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Converters
{
    public class DietConverter : JsonConverter<CustomTypes.Diet>
    {
        public override CustomTypes.Diet Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return (CustomTypes.Diet)Enum.Parse(typeof(CustomTypes.Diet), reader.GetString(), true);
        }

        public override void Write(Utf8JsonWriter writer, CustomTypes.Diet dietTypeValue,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(dietTypeValue.ToString());
        }
    }
}
