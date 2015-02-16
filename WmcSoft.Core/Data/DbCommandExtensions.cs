using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Data
{
    public static class DbCommandExtensions
    {
        public static void AddReflectedParameters(this IDbCommand command, object parameters) {
            if (parameters == null)
                return;

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(parameters)) {
                var name = descriptor.Name;
                var value = descriptor.GetValue(parameters);
                var parameter = command.CreateParameter();
                parameter.ParameterName = name;
                parameter.Value = value;
                command.Parameters.Add(parameter);
            }
        }

        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            var command = connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.Transaction = transaction;
            command.AddReflectedParameters(parameters);
            if (timeout != null) {
                command.CommandTimeout = (int)Math.Max(timeout.GetValueOrDefault().TotalSeconds, 1d);
            }
            return command;
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
