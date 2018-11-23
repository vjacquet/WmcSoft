using System;
using System.CodeDom;
using Xunit;

namespace WmcSoft.CodeDom.Tests
{
    public class CodeDomExtensionsTests
    {
        [Fact]
        public void CanCreateTryCatchFinallyBlocks()
        {
            var compileUnit = new CodeCompileUnit();
            var samples = new CodeNamespace("Samples");
            samples.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(samples);
            var class1 = new CodeTypeDeclaration("Class1");
            samples.Types.Add(class1);

            var start = new CodeEntryPointMethod();
            class1.Members.Add(start);

            var try1 = new CodeTryCatchFinallyStatement();
            var cs1 = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.Console"), "WriteLine", new CodePrimitiveExpression("Hello World!"));
            var result = try1.Try(new CodeExpressionStatement(cs1))
                .Catch<ArgumentException>("e");
            Assert.Equal(try1, result);
        }
    }
}
