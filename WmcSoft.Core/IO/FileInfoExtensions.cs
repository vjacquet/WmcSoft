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
using System.IO;
using System.Runtime.InteropServices;

namespace WmcSoft.IO
{
    public static class FileInfoExtensions
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Win32StreamID
        {
            public int dwStreamId;
            public int dwStreamAttributes;
            public long Size;
            public int dwStreamNameSize;
        }

        public static IEnumerable<FileStreamInfo> EnumerateFileStreamsInfo(this FileInfo file) {
            const int bufferSize = 4096;
            using (FileStream fs = file.OpenRead()) {
                IntPtr context = IntPtr.Zero;
                IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
                try {
                    while (true) {
                        uint numRead;
                        if (!NativeMethods.BackupRead(fs.SafeFileHandle, buffer,
                           (uint)Marshal.SizeOf(typeof(Win32StreamID)), out numRead,
                           false, true, ref context))
                            throw new Win32Exception();

                        if (numRead > 0) {
                            Win32StreamID streamID = (Win32StreamID)
                              Marshal.PtrToStructure(buffer, typeof(Win32StreamID));
                            string name = null;
                            if (streamID.dwStreamNameSize > 0) {
                                if (!NativeMethods.BackupRead(fs.SafeFileHandle, buffer,
                                     (uint)System.Math.Min(bufferSize, streamID.dwStreamNameSize),
                                     out numRead, false, true, ref context))
                                    throw new Win32Exception();
                                name = Marshal.PtrToStringUni(buffer, (int)numRead / 2);
                            }

                            yield return new FileStreamInfo(name, (FileStreamType)streamID.dwStreamId, streamID.Size);

                            if (streamID.Size > 0) {
                                uint lo, hi;
                                NativeMethods.BackupSeek(fs.SafeFileHandle, uint.MaxValue,
                                  int.MaxValue, out lo, out hi, ref context);
                            }
                        } else
                            break;
                    }
                }
                finally {
                    Marshal.FreeHGlobal(buffer);
                    uint numRead;
                    if (!NativeMethods.BackupRead(fs.SafeFileHandle, IntPtr.Zero, 0, out numRead,
                         true, false, ref context))
                        throw new Win32Exception();
                }
            }
        }
    }

    public enum FileStreamType
    {
        Data = 1,
        ExternalData = 2,
        SecurityData = 3,
        AlternateData = 4,
        Link = 5,
        PropertyData = 6,
        ObjectID = 7,
        ReparseData = 8,
        SparseDock = 9
    }

    public class FileStreamInfo
    {
        public FileStreamInfo(string name, FileStreamType streamType, long size) {
            Name = name;
            StreamType = streamType;
            Size = size;
        }
        public string Name { get; private set; }
        public FileStreamType StreamType { get; private set; }
        public long Size { get; private set; }
    }

}