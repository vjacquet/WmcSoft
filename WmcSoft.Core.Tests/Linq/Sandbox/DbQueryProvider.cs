using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Linq.Sandbox
{
    public class DbQueryProvider : QueryProviderBase
    {
        IDbConnection _connection;

        public DbQueryProvider(IDbConnection connection) {
            _connection = connection;
        }

        public override string GetQueryText(Expression expression) {
            return Translate(expression);
        }

        public override object Execute(Expression expression) {
            var elementType = GetElementType(expression);

            var objectReaderType = typeof(ObjectReader<>).MakeGenericType(elementType);
            var commandText = Translate(expression);
            var cmd = _connection.CreateCommand();
            cmd.CommandText = commandText;
            var reader = cmd.ExecuteReader();
            return Activator.CreateInstance(objectReaderType, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { reader }, null);
        }

        private string Translate(Expression expression) {
            var evaluated = Evaluator.PartialEval(expression);
            return new QueryTranslator().Translate(evaluated);
        }
    }
}