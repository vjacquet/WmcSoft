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
using System.Threading;

namespace WmcSoft.Data
{
    public static class DbCommandExtensions
    {
        #region NameGenerators

        static Func<int, string> _defaultNameGenerator;
        public static Func<int, string> ParameterNameGenerator {
            get {
                return _defaultNameGenerator;
            }
            set {
                Interlocked.Exchange(ref _defaultNameGenerator, value);
            }
        }

        public static readonly Func<int, string> DefaultParameterNameGenerator = null;

        #endregion

        #region WithParameters

        public static TCommand WithParameters<TCommand>(this TCommand command, object parameters)
            where TCommand : IDbCommand {
            if (parameters != null) {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(parameters)) {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = descriptor.Name;
                    parameter.Value = descriptor.GetValue(parameters);
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        public static TCommand WithParameters<TCommand>(this TCommand command, params string[] names)
            where TCommand : IDbCommand {
            foreach (var name in names) {
                command.AddParameter(name);
            }
            return command;
        }

        public static TCommand WithParameters<TCommand>(this TCommand command, int count, Func<int, string> nameGenerator = null)
            where TCommand : IDbCommand {
            if (nameGenerator == null)
                nameGenerator = ParameterNameGenerator;

            if (nameGenerator == null) {
                for (int i = 0; i != count; i++) {
                    var parameter = command.CreateParameter();
                    command.Parameters.Add(parameter);
                }
            } else {
                for (int i = 0; i != count; i++) {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = nameGenerator(i);
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        #endregion

        #region AddParameter(s)

        public static IDbDataParameter AddParameter(this IDbCommand command) {
            var parameter = command.CreateParameter();
            command.Parameters.Add(parameter);
            return parameter;
        }

        public static IDbDataParameter AddParameter(this IDbCommand command, string name) {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return parameter;
        }

        public static IDbDataParameter[] AddParameters(this IDbCommand command, params string[] names) {
            var results = new IDbDataParameter[names.Length];
            for (int i = 0; i != names.Length; i++) {
                var parameter = command.CreateParameter();
                parameter.ParameterName = names[i];
                command.Parameters.Add(parameter);
                results[i] = parameter;
            }
            return results;
        }

        public static IDbDataParameter AddParameter(this IDbCommand command, Func<int, string> nameGenerator) {
            if (nameGenerator == null)
                nameGenerator = ParameterNameGenerator;

            var parameter = command.CreateParameter();
            if (nameGenerator != null)
                parameter.ParameterName = nameGenerator(0);
            command.Parameters.Add(parameter);
            return parameter;
        }

        public static IDbDataParameter[] AddParameters(this IDbCommand command, int count, Func<int, string> nameGenerator = null) {
            var results = new IDbDataParameter[count];

            if (nameGenerator == null)
                nameGenerator = ParameterNameGenerator;

            if (nameGenerator == null) {
                for (int i = 0; i != count; i++) {
                    var parameter = command.CreateParameter();
                    command.Parameters.Add(parameter);
                    results[i] = parameter;
                }
            } else {
                for (int i = 0; i != count; i++) {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = nameGenerator(i);
                    command.Parameters.Add(parameter);
                    results[i] = parameter;
                }
            }
            return results;
        }

        public static IDbDataParameter AddParameter<T>(this IDbCommand command, string name, T value = default(T)) {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
            return parameter;
        }

        #endregion

        #region ReadXXX

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

        #endregion
    }
}
