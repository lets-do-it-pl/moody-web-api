using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LetsDoIt.Moody.Infrastructure.ValueTypes;

namespace LetsDoIt.Moody.Web
{
    public class EmailJsonConverter : JsonConverter<Email>
    {
        public override Email Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            Email.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Email email,
            JsonSerializerOptions options) =>
            writer.WriteStringValue(email.ToString());
    }
}