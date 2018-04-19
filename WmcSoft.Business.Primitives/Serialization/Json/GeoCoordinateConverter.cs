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
using Newtonsoft.Json;

namespace WmcSoft.Serialization.Json
{
    /// <summary>
    /// Converts a <see cref="Latitude"/> or <see cref="Longitude"/> to and from a decimal value. 
    /// </summary>
    public class GeoCoordinateConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value) {
            case null:
                writer.WriteNull();
                break;
            case Latitude latitude:
                writer.WriteValue(latitude);
                break;
            case Longitude longitude:
                writer.WriteValue(longitude);
                break;
            default:
                throw new JsonSerializationException("Expected Latitude or Longitude object value");
            }
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
            if (reader.TokenType == JsonToken.Float) {
                try {
                    if(objectType == typeof(Latitude)) {
                        return new Latitude(Convert.ToDecimal(reader.Value));
                    } else if (objectType == typeof(Longitude)) {
                        return new Longitude(Convert.ToDecimal(reader.Value));
                    }
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
            return objectType == typeof(Latitude) || objectType == typeof(Longitude);
        }
    }
}
