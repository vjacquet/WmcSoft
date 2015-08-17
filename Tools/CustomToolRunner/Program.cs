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
using System.Collections;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
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
