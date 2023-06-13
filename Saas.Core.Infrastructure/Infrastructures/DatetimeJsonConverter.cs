﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saas.Core.Infrastructure.Infrastructures
{
    /// <summary>
    /// json.net 日期格式化转换器
    /// </summary>
    public class DatetimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String) return reader.GetDateTime();
            return DateTime.TryParse(reader.GetString(), out var date) ? date : reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
