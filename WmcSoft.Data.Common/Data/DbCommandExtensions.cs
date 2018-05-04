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
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace WmcSoft.Data
{
    /// <summary>
    /// Defines extension methods to the <see cref="IDbCommand"/> interface. This is a static class. 
    /// </summary>
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

            public PreparedParameter(IDbDataParameter parameter)
            {
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
            where TCommand : IDbCommand
        {
            if (parameters != null) {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(parameters)) {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = descriptor.Name;
                    parameter.Value = descriptor.GetValue(parameters) ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        public static TCommand WithParameters<TCommand>(this TCommand command, params string[] names)
            where TCommand : IDbCommand
        {
            foreach (var name in names) {
                command.PrepareParameter(name);
            }
            return command;
        }

        public static TCommand WithParameters<TCommand>(this TCommand command, int count, Func<int, string> nameGenerator = null)
            where TCommand : IDbCommand
        {
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

        public static IDbDataParameter PrepareParameter(this IDbCommand command)
        {
            var parameter = command.CreateParameter();
            command.Parameters.Add(parameter);
            return new PreparedParameter(parameter);
        }

        public static IDbDataParameter PrepareParameter(this IDbCommand command, string name)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);
            return new PreparedParameter(parameter);
        }

        public static IDbDataParameter[] PrepareParameters(this IDbCommand command, params string[] names)
        {
            var results = new IDbDataParameter[names.Length];
            for (int i = 0; i != names.Length; i++) {
                var parameter = command.CreateParameter();
                parameter.ParameterName = names[i];
                command.Parameters.Add(parameter);
                results[i] = new PreparedParameter(parameter);
            }
            return results;
        }

        public static IDbDataParameter PrepareParameter(this IDbCommand command, Func<int, string> nameGenerator)
        {
            if (nameGenerator == null)
                nameGenerator = ParameterNameGenerator;

            var parameter = command.CreateParameter();
            if (nameGenerator != null)
                parameter.ParameterName = nameGenerator(0);
            command.Parameters.Add(parameter);
            return new PreparedParameter(parameter);
        }

        public static IDbDataParameter[] PrepareParameters(this IDbCommand command, int count, Func<int, string> nameGenerator = null)
        {
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

        public static IDbDataParameter PrepareParameter<T>(this IDbCommand command, string name, T value = default(T))
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            command.Parameters.Add(parameter);

            var prepared = new PreparedParameter(parameter);
            prepared.Value = value;
            return prepared;
        }

        #endregion

        #region WithinTransaction

        public static TCommand WithinTransaction<TCommand>(this TCommand command, IDbTransaction transaction)
            where TCommand : IDbCommand
        {
            command.Transaction = transaction;
            return command;
        }

        #endregion

        #region ExecuteXXX

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        public static T ExecuteScalar<T>(this IDbCommand command)
        {
            var result = command.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static T ExecuteScalarOrDefault<T>(this IDbCommand command, T defaultValue = default(T))
        {
            var result = command.ExecuteScalar();
            if (result == null || DBNull.Value.Equals(result))
                return defaultValue;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static T? ExecuteNullableScalar<T>(this IDbCommand command) where T : struct
        {
            var result = command.ExecuteScalar();
            if (result == null || DBNull.Value.Equals(result))
                return null;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static object ExecuteStoredProcedure(this IDbCommand command)
        {
            Debug.Assert(command.CommandType == CommandType.StoredProcedure);

            var result = command.Parameters.Cast<IDbDataParameter>().FirstOrDefault(p => p.Direction == ParameterDirection.ReturnValue);
            if (result == null) {
                result = command.CreateParameter();
                result.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(result);
            }
            command.ExecuteNonQuery();
            return result.Value;
        }

        public static T ExecuteStoredProcedure<T>(this IDbCommand command)
        {
            var result = command.ExecuteStoredProcedure();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static T ExecuteStoredProcedureOrDefault<T>(this IDbCommand command, T defaultValue = default(T))
        {
            var result = command.ExecuteStoredProcedure();
            if (result == null || DBNull.Value.Equals(result))
                return defaultValue;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static T? ExecuteNullableStoredProcedure<T>(this IDbCommand command) where T : struct
        {
            var result = command.ExecuteStoredProcedure();
            if (result == null || DBNull.Value.Equals(result))
                return null;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        #endregion

        #region ReadXXX

        public static IEnumerable<T> ReadAll<T>(this IDbCommand command, CommandBehavior behavior, Func<IDataRecord, T> materializer)
        {
            using (var reader = command.ExecuteReader(behavior)) {
                while (reader.Read()) {
                    yield return materializer(reader);
                }
            }
        }

        public static IEnumerable<T> ReadAll<T>(this IDbCommand command, Func<IDataRecord, T> materializer)
        {
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    yield return materializer(reader);
                }
            }
        }

        #endregion

        #region Materalizers (lab)

        private static readonly IDictionary<Type, MethodInfo> DataRecordAccessors;

        static DbCommandExtensions()
        {
            const BindingFlags bindingAttr = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance;
            var type = typeof(IDataRecord);
            var query = from m in type.GetMethods(bindingAttr)
                        where m.Name.StartsWith("Get") && m.Name != "GetDataTypeName" && m.Name != "GetName"
                        where m.ReturnType != typeof(object) && m.ReturnType != typeof(IDataReader) && m.ReturnType != typeof(Type)
                        let p = m.GetParameters()
                        where p.Length == 1 && p[0].ParameterType == typeof(int)
                        select m;
            DataRecordAccessors = query.ToDictionary(m => m.ReturnType);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<TResult>()
        {
            var reader = Expression.Parameter(typeof(IDataRecord), "reader");
            var bind = Expression.Call(reader, DataRecordAccessors[typeof(TResult)], Expression.Constant(0));
            var lamba = Expression.Lambda<Func<IDataRecord, TResult>>(bind, reader);
            return lamba.Compile();
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<TResult>(MethodInfo method, int offset)
        {
            Debug.Assert(method.IsStatic);
            var reader = Expression.Parameter(typeof(IDataRecord), "reader");
            var calls = method.GetParameters()
                .Select((t, i) => Expression.Call(reader, DataRecordAccessors[t.ParameterType], Expression.Constant(offset + i)));
            var bind = method.IsStatic
                ? Expression.Call(method, calls)
                : Expression.Call(Expression.Constant(null, method.DeclaringType), method, calls);
            var lamba = Expression.Lambda<Func<IDataRecord, TResult>>(bind, reader);
            return lamba.Compile();
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<TResult>(object instance, MethodInfo method, int offset = 0)
        {
            var reader = Expression.Parameter(typeof(IDataRecord), "reader");
            var calls = method.GetParameters()
                .Select((t, i) => Expression.Call(reader, DataRecordAccessors[t.ParameterType], Expression.Constant(offset + i)));
            var bind = method.IsStatic
                ? Expression.Call(method, calls)
                : Expression.Call(Expression.Constant(instance, method.DeclaringType), method, calls);
            var lamba = Expression.Lambda<Func<IDataRecord, TResult>>(bind, reader);
            return lamba.Compile();
        }

        /// <summary>
        /// Binds a factory function for the type <c>TResult</c> to a function taking a <see cref="IDataRecord"/> as parameter;
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <typeparam name="TResult">The type of the entity to materialize.</typeparam>
        /// <param name="func">The factory function for the entity.</param>
        /// <param name="offset">The columns number where in the data record. Defaults to 0.</param>
        /// <returns>The materializer.</returns>
        public static Func<IDataRecord, TResult> MakeMaterializer<T, TResult>(Func<T, TResult> func, int offset = 0)
        {
            // Calling MakeMaterializer on [T f(int)] returns [(IDataRecord r) => f(r.GetInt32(0))];
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<T1, T2, TResult>(Func<T1, T2, TResult> func, int offset = 0)
        {
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, int offset = 0)
        {
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, int offset = 0)
        {
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, int offset = 0)
        {
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, int offset = 0)
        {
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, int offset = 0)
        {
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        public static Func<IDataRecord, TResult> MakeMaterializer<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, int offset = 0)
        {
            return MakeMaterializer<TResult>(func.Target, func.Method, offset);
        }

        #endregion
    }
}
