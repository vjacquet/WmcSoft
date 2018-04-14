#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Globalization;
using Newtonsoft.Json;
using WmcSoft.Time;

namespace WmcSoft.Serialization.Json
{
    /// <summary>
    /// Converts a <see cref="TimeOfDay"/> to and from a string value. 
    /// </summary>
    public class TimeOfDayConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) {
                writer.WriteNull();
                return;
            }
            if (value.GetType() == typeof(TimeOfDay)) {
                writer.WriteValue(value.ToString());
                return;
            }
            throw new JsonSerializationException("Expected TimeOfDay object value");
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing property value of the JSON that is being converted.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }
            if (reader.TokenType == JsonToken.String) {
                try {
                    var timespan = TimeSpan.ParseExact((string)reader.Value, "hh\\:mm", CultureInfo.InvariantCulture);
                    return new TimeOfDay(timespan.Hours, timespan.Minutes);
                } catch (Exception e) {
                    throw new JsonSerializationException("Unexpected error.", e);
                }
            }
            throw new JsonSerializationException($"Unexpected token or value when parsing version. Token: {reader.TokenType}, Value: {reader.Value}");
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeOfDay);
        }
    }
}
