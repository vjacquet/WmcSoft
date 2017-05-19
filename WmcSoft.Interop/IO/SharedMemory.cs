using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using WmcSoft.Interop;

namespace WmcSoft.IO
{
    /// <summary>
    /// This class wraps memory that can be simultaneously 
    /// shared by multiple AppDomains and Processes.
    /// </summary>
    [Serializable]
    public sealed class SharedMemory : ISerializable, IDisposable
    {
        const StreamingContextStates InvalidContexts =
                  StreamingContextStates.CrossMachine |
                  StreamingContextStates.File |
                  StreamingContextStates.Other |
                  StreamingContextStates.Persistence |
                  StreamingContextStates.Remoting;

        // The handle and string that identify 
        // the Windows file-mapping object.
        private IntPtr m_hFileMap = IntPtr.Zero;
        private String m_name;

        // The address of the memory-mapped file-mapping object.
        private IntPtr m_address;

        public unsafe Byte* Address {
            get { return (Byte*)m_address; }
        }

        // The constructors.
        public SharedMemory(Int32 size) : this(size, null) { }

        public SharedMemory(Int32 size, String name)
        {
            m_hFileMap = Kernel.CreateFileMapping(Kernel.InvalidHandleValue,
                IntPtr.Zero, Kernel.PAGE_READWRITE,
                0, unchecked((UInt32)size), name);
            if (m_hFileMap == IntPtr.Zero)
                throw new Exception("Could not create memory-mapped file.");
            m_name = name;
            m_address = Kernel.MapViewOfFile(m_hFileMap, Kernel.FILE_MAP_WRITE,
                0, 0, IntPtr.Zero);
        }

        // The cleanup methods.
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        private void Dispose(Boolean disposing)
        {
            Kernel.UnmapViewOfFile(m_address);
            Kernel.CloseHandle(m_hFileMap);
            m_address = IntPtr.Zero;
            m_hFileMap = IntPtr.Zero;
        }

        ~SharedMemory()
        {
            Dispose(false);
        }

        // Private helper methods.
        private static Boolean AllFlagsSet(Int32 flags, Int32 flagsToTest)
        {
            return (flags & flagsToTest) == flagsToTest;
        }

        private static Boolean AnyFlagsSet(Int32 flags, Int32 flagsToTest)
        {
            return (flags & flagsToTest) != 0;
        }

        // The security attribute demands that code that calls  
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // The context's State member indicates
            // where the object will be deserialized.

            // A SharedMemory object cannot be serialized 
            // to any of the following destinations.
            if (AnyFlagsSet((Int32)context.State, (Int32)InvalidContexts))
                throw new SerializationException("The SharedMemory object " +
                    "cannot be serialized to any of the following streaming contexts: " +
                    InvalidContexts);

            const StreamingContextStates DeserializableByHandle =
                      StreamingContextStates.Clone |
                      // The same process.
                      StreamingContextStates.CrossAppDomain;
            if (AnyFlagsSet((Int32)context.State, (Int32)DeserializableByHandle))
                info.AddValue("hFileMap", m_hFileMap);

            const StreamingContextStates DeserializableByName =
                      // The same computer.
                      StreamingContextStates.CrossProcess;
            if (AnyFlagsSet((Int32)context.State, (Int32)DeserializableByName)) {
                if (m_name == null)
                    throw new SerializationException("The SharedMemory object " +
                        "cannot be serialized CrossProcess because it was not constructed " +
                        "with a String name.");
                info.AddValue("name", m_name);
            }
        }

        // The security attribute demands that code that calls  
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        private SharedMemory(SerializationInfo info, StreamingContext context)
        {
            // The context's State member indicates 
            // where the object was serialized from.

            if (AnyFlagsSet((Int32)context.State, (Int32)InvalidContexts))
                throw new SerializationException("The SharedMemory object " +
                    "cannot be deserialized from any of the following stream contexts: " +
                    InvalidContexts);

            const StreamingContextStates SerializedByHandle =
                      StreamingContextStates.Clone |
                      // The same process.
                      StreamingContextStates.CrossAppDomain;
            if (AnyFlagsSet((Int32)context.State, (Int32)SerializedByHandle)) {
                try {
                    Kernel.DuplicateHandle(Kernel.GetCurrentProcess(),
                        (IntPtr)info.GetValue("hFileMap", typeof(IntPtr)),
                        Kernel.GetCurrentProcess(), ref m_hFileMap, 0, false,
                        Kernel.DUPLICATE_SAME_ACCESS);
                } catch (SerializationException) {
                    throw new SerializationException("A SharedMemory was not serialized " +
                        "using any of the following streaming contexts: " +
                        SerializedByHandle);
                }
            }

            const StreamingContextStates SerializedByName =
                      // The same computer.
                      StreamingContextStates.CrossProcess;
            if (AnyFlagsSet((Int32)context.State, (Int32)SerializedByName)) {
                try {
                    m_name = info.GetString("name");
                } catch (SerializationException) {
                    throw new SerializationException("A SharedMemory object was not " +
                        "serialized using any of the following streaming contexts: " +
                        SerializedByName);
                }
                m_hFileMap = Kernel.OpenFileMapping(Kernel.FILE_MAP_WRITE, false, m_name);
            }
            if (m_hFileMap != IntPtr.Zero) {
                m_address = Kernel.MapViewOfFile(m_hFileMap, Kernel.FILE_MAP_WRITE,
                    0, 0, IntPtr.Zero);
            } else {
                throw new SerializationException("A SharedMemory object could not " +
                    "be deserialized.");
            }
        }
    }

}
