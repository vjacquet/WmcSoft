using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using WmcSoft.CustomTools.Design;
using WmcSoft.VisualStudio;

namespace WmcSoft.CustomToolRunner
{
    class Program
    {
        static void Main(string[] args) {
            Hashtable customTools = (Hashtable)ConfigurationManager.GetSection("customTools");
            string typeName = (string)customTools[args[0]];
            Type type = Type.GetType(typeName);
            var provider = new CSharpCodeProvider();
            var tool = (CustomToolBase)Activator.CreateInstance(type, provider);

            string inputFileName = args[1];
            string outputFileName = Path.ChangeExtension(inputFileName, tool.GetDefaultExtension());
            TextReader reader = new StreamReader(inputFileName);

            IntPtr ptr;
            int count;
            tool.Generate(inputFileName, reader.ReadToEnd(), "", out ptr, out count, new ConsoleProgress(inputFileName));

            byte[] buffer = new byte[count];
            Marshal.Copy(ptr, buffer, 0, count);

            using (var os = new FileStream(outputFileName, FileMode.Create, FileAccess.Write)) {
                os.Write(buffer, 0, count);
            }
        }
    }

    class ConsoleProgress : IVsGeneratorProgress
    {
        string inputFileName;

        public ConsoleProgress(string inputFileName) {
            this.inputFileName = inputFileName;
        }

        #region IVsGeneratorProgress Members

        public void GeneratorError(bool fWarning, int dwLevel, string bstrError, int dwLine, int dwColumn) {
            if (fWarning) {
                Console.Error.WriteLine("{0}({1}) : warning #{2}: {3}", inputFileName, dwLine, dwLevel, bstrError);
            } else {
                Console.Error.WriteLine("{0}({1}) : error #{2}: {3}", inputFileName, dwLine, dwLevel, bstrError);
            }
        }

        public void Progress(int nComplete, int nTotal) {
            if (nComplete == nTotal)
                Console.Out.WriteLine("Done.");
        }

        #endregion
    }

}
