using System;
using System.IO;
using Microsoft.Win32.SafeHandles;
using Xunit;

using static WmcSoft.IO.NativeMethods;

namespace WmcSoft.IO
{
    public class NativeFileStreamTests
    {
        [Fact]
        public void CanCreateNativeFileStream()
        {
            const string path = "fs-data.txt";
            const string streamName = ":stream1:$DATA";

            File.Delete(path);
            using (var fs = File.CreateText(path)) {
                fs.WriteLine("Line #1");
            }

            var ptr = CreateFile(path + streamName, GENERIC_WRITE, 0, IntPtr.Zero, OPEN_ALWAYS, 0, IntPtr.Zero);
            using (var h = new SafeFileHandle(ptr, true))
            using (var fs = new FileStream(h, FileAccess.Write))
            using (var writer = new StreamWriter(fs)) {
                writer.WriteLine("Line in stream");
            }

            var fi = new FileInfo(path);
            Assert.Contains(fi.EnumerateFileStreamsInfo(), i => i.Name == streamName);
        }
    }
}
