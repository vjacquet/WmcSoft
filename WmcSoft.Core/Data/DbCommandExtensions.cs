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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace WmcSoft.Data
{
    public static class DbCommandExtensions
    {
        #region NameGenerators

        static Func<int, string> _defaultNameGenerator;
        public static Func<int, string> ParameterNameGenerator {
            get { return _defaultNameGenerator; }
            set { Interlocked.Exchange(ref _defaultNameGenerator, value); }
        }

        public static readonly Func<int, string> DefaultParameterNameGenerator = null;

        #endregion

        #region PreparedParameter class

        class PreparedParameter : IDbDataParameter
        {
            readonly IDbDataParameter _base;

            public PreparedParameter(IDbDataParameter parameter) {
                _base = parameter;
            }

            #region IDbDataParameter Members

            public byte Precision {
                get { return _base.Precision; }
                set { _base.Precision = value; }
            }

            public byte Scale {
                get { return _base.Scale; }
                set { _base.Scale = value; }
            }

            public int Size {
                get { return _base.Size; }
                set { _base.Size = value; }
            }

            #endregion

            #region IDataParameter Members

            public DbType DbType {
                get { return _base.DbType; }
                set { _base.DbType = value; }
            }

            public ParameterDirection Direction {
                get { return _base.Direction; }
                set { _base.Direction = value; }
            }

            public bool IsNullable {
                get { throw new NotImplementedException(); }
            }

            public string ParameterName {
                get { return _base.ParameterName; }
                set { _base.ParameterName = value; }
            }

            public string SourceColumn {
                get { return _base.SourceColumn; }
                set { _base.SourceColumn = value; }
            }

            public DataRowVersion SourceVersion {
                get { return _base.SourceVersion; }
                set { _base.SourceVersion = value; }
            }

            public object Value {
                get {
                    var value = _base.Value;
                    if (DBNull.Value.Equals(value))
                        return null;
                    return value;
                }
                set {
                    if (value == null)
                        _base.Value = DBNull.Value;
                    _base.Value = value;
                }
            }

            #endregion
        }

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
                command.PrepareParameter(name);
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

        public static IDbDataParameter PrepareParameter(this IDbCommand command) {
            var parameter = command.CreateParameter();
            command.Parameters.Add(parameter);
            return new PreparedParameter(parameter);
        }

        public static IDbDataParameter PrepareParameter(this IDbCommand command, string name) {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return new PreparedParameter(parameter);
        }

        public static IDbDataParameter[] PrepareParameters(this IDbCommand command, params string[] names) {
            var results = new IDbDataParameter[names.Length];
            for (int i = 0; i != names.Length; i++) {
                var parameter = command.CreateParameter();
                parameter.ParameterName = names[i];
                command.Parameters.Add(parameter);
                results[i] = new PreparedParameter(parameter);
            }
            return results;
        }

        public static IDbDataParameter PrepareParameter(this IDbCommand command, Func<int, string> nameGenerator) {
            if (nameGenerator == null)
                nameGenerator = ParameterNameGenerator;

            var parameter = command.CreateParameter();
            if (nameGenerator != null)
                parameter.ParameterName = nameGenerator(0);
            command.Parameters.Add(parameter);
            return new PreparedParameter(parameter);
        }

        public static IDbDataParameter[] PrepareParameters(this IDbCommand command, int count, Func<int, string> nameGenerator = null) {
            var results = new IDbDataParameter[count];

            if (nameGenerator == null)
                nameGenerator = ParameterNameGenerator;

            if (nameGenerator == null) {
                for (int i = 0; i != count; i++) {
                    var parameter = command.CreateParameter();
                    command.Parameters.Add(parameter);
                    results[i] = new PreparedParameter(parameter);
                }
            } else {
                for (int i = 0; i != count; i++) {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = nameGenerator(i);
                    command.Parameters.Add(parameter);
                    results[i] = new PreparedParameter(parameter);
                }
            }
            return results;
        }

        public static IDbDataParameter PrepareParameter<T>(this IDbCommand command, string name, T value = default(T)) {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);

            var prepared = new PreparedParameter(parameter);
            prepared.Value = value;
            return prepared;
        }

        #endregion

        #region ExecuteXXX

        public static T ExecuteScalar<T>(this IDbCommand command) {
            var result = command.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static T ExecuteScalarOrDefault<T>(this IDbCommand command) {
            var result = command.ExecuteScalar();
            if (result == null || DBNull.Value.Equals(result))
                return default(T);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static T? ExecuteNullableScalar<T>(this IDbCommand command) where T : struct {
            var result = command.ExecuteScalar();
            if (result == null || DBNull.Value.Equals(result))
                return null;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        #endregion

        #region ReadXXX

        public static IEnumerable<T> ReadAll<T>(this IDbCommand command, CommandBehavior behavior, Func<IDataRecord, T> materializer) {
            using (var reader = command.ExecuteReader(behavior)) {
                while (reader.Read()) {
                    yield return materializer(reader);
                }
            }
        }

        public static IEnumerable<T> ReadAll<T>(this IDbCommand command, Func<IDataRecord, T> materializer) {
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    yield return materializer(reader);
                }
            }
        }

        #endregion

        #region Materalizers (lab)

        private static readonly IDictionary<Type, MethodInfo> DataReaderAccessors;

        static DbCommandExtensions() {
            const BindingFlags bindingAttr = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance;
            var type = typeof(IDataRecord);
            var query = from m in type.GetMethods(bindingAttr)
                        where m.Name.StartsWith("Get") && m.Name != "GetDataTypeName" && m.Name != "GetName"
                        where m.ReturnType != typeof(object) && m.ReturnType != typeof(IDataReader) && m.ReturnType != typeof(Type)
                        let p = m.GetParameters()
                        where p.Length == 1 && p[0].ParameterType == typeof(int)
                        select m;
            DataReaderAccessors = query.ToDictionary(m => m.ReturnType);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<TResult>() {
            var reader = Expression.Parameter(typeof(IDataRecord), "reader");
            var bind = Expression.Call(reader, DataReaderAccessors[typeof(TResult)], Expression.Constant(0));
            var lamba = Expression.Lambda<Func<IDataReader, TResult>>(bind, reader);
            return lamba.Compile();
        }

        public static Func<IDataReader, TResult> MakeMaterializer<TResult>(MethodInfo method) {
            var reader = Expression.Parameter(typeof(IDataRecord), "reader");
            var calls = method.GetParameters().Select((t, i) => Expression.Call(reader, DataReaderAccessors[t.ParameterType], Expression.Constant(i)));
            var bind = Expression.Call(method, calls);
            var lamba = Expression.Lambda<Func<IDataReader, TResult>>(bind, reader);
            return lamba.Compile();
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T, TResult>(Func<T, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T1, T2, TResult>(Func<T1, T2, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        public static Func<IDataReader, TResult> MakeMaterializer<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) {
            return MakeMaterializer<TResult>(func.Method);
        }

        #endregion
    }
}