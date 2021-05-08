using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Microsoft.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace WmcSoft.CodeDom.Tests
{
    public class CodeDomExtensionsTests
    {
        private readonly ITestOutputHelper _output;

        public CodeDomExtensionsTests(ITestOutputHelper output)
        {
            _output = output;
        }

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

            var provider = new CSharpCodeProvider();
            var sb = new StringBuilder();
            using (var writer = new IndentedTextWriter(new StringWriter(sb), "    "))
                provider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions { });
            _output.WriteLine(sb.ToString());
        }

        [Fact]
        public void CanCreatePartialMethod()
        {
            var compileUnit = new CodeCompileUnit();
            var samples = new CodeNamespace("Samples");
            samples.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(samples);
            var c = new CodeTypeDeclaration("C") { IsPartial = true };
            samples.Types.Add(c);

            var m = new CodeMemberMethod {
                Attributes = MemberAttributes.Abstract,
                Name = "M",
                ReturnType = new CodeTypeReference()
            };

            var provider = new CSharpCodeProvider();

            var sb = new StringBuilder();
            using (var writer = new IndentedTextWriter(new StringWriter(sb), "    "))
                provider.GenerateCodeFromMember(m, writer, new CodeGeneratorOptions { });
            var text = "        " + string.Join("partial", sb.ToString().Trim().Split(new[] { "abstract" }, 2, StringSplitOptions.None));
            var snippet = new CodeSnippetTypeMember(text);
            c.Members.Add(snippet);

            sb.Clear();
            using (var writer = new IndentedTextWriter(new StringWriter(sb), "    "))
                provider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions { });
            var result = sb.ToString();
            Assert.NotNull(result);
            _output.WriteLine(result);
        }
    }
}
