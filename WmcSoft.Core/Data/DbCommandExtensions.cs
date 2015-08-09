#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace WmcSoft.Data
{
    public static class DbCommandExtensions
    {
        public static void AddReflectedParameters(this IDbCommand command, object parameters) {
            if (parameters == null)
                return;

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(parameters)) {
                var parameter = command.CreateParameter();
                parameter.ParameterName = descriptor.Name;
                parameter.Value = descriptor.GetValue(parameters);
                command.Parameters.Add(parameter);
            }
        }

        public static IEnumerable<T> ReadAll<T>(this IDbCommand command, CommandBehavior behavior, Func<IDataRecord, T> materializer) {
            using (command)
            using (var reader = command.ExecuteReader(behavior)) {
                while (reader.Read()) {
                    yield return materializer(reader);
                }
            }
        }

        public static IEnumerable<T> ReadAll<T>(this IDbCommand command, Func<IDataRecord, T> materializer) {
            using (command)
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    yield return materializer(reader);
                }
            }
        }

        public static T ReadScalar<T>(this IDbCommand command) {
            using (command) {
                var result = command.ExecuteScalar();
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public static T ReadScalarOrDefault<T>(this IDbCommand command) {
            using (command) {
                var result = command.ExecuteScalar();
                if (result == null || DBNull.Value.Equals(result))
                    return default(T);
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public static T? ReadNullableScalar<T>(this IDbCommand command) where T : struct {
            using (command) {
                var result = command.ExecuteScalar();
                if (result == null || DBNull.Value.Equals(result))
                    return null;
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
    }
}
