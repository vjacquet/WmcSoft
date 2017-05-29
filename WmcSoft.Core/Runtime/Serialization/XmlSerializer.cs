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

using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace WmcSoft.Runtime.Serialization
{
    public class XmlSerializer<T>
    {
        #region Strategies

        class DataContractStrategy : IXmlSerializationStrategy<T>
        {
            private readonly DataContractSerializer _serializer;

            public DataContractStrategy()
            {
                _serializer = new DataContractSerializer(typeof(T));
            }

            public T Deserialize(XmlReader reader)
            {
                return (T)_serializer.ReadObject(reader);
            }

            public void Serialize(XmlWriter writer, T value)
            {
                _serializer.WriteObject(writer, value);
            }
        }

        class XmlStrategy : IXmlSerializationStrategy<T>
        {
            private readonly XmlSerializer _serializer;

            public XmlStrategy()
            {
                _serializer = new XmlSerializer(typeof(T));
            }

            public T Deserialize(XmlReader reader)
            {
                return (T)_serializer.Deserialize(reader);
            }

            public void Serialize(XmlWriter writer, T value)
            {
                _serializer.Serialize(writer, value);
            }
        }

        #endregion

        #region Fields

        private readonly IXmlSerializationStrategy<T> _strategy;
        private readonly XmlReaderSettings _readerSettings;
        private readonly XmlWriterSettings _writerSettings;

        #endregion

        #region Lifecycle

        static IXmlSerializationStrategy<T> GetStrategy()
        {
            if (typeof(T).IsDefined(typeof(XmlTypeAttribute), false) || typeof(T).IsDefined(typeof(XmlRootAttribute), false))
                return new XmlStrategy();
            else
                return new DataContractStrategy();
        }

        protected XmlSerializer(IXmlSerializationStrategy<T> strategy)
        {
            _strategy = strategy;
            _readerSettings = new XmlReaderSettings {
                CloseInput = false,
                IgnoreComments = true,
                IgnoreWhitespace = true,
                ConformanceLevel = ConformanceLevel.Auto,
            };
            _writerSettings = new XmlWriterSettings {
                CloseOutput = false,
                Indent = true,
                IndentChars = "  ",
            };
        }

        public XmlSerializer() : this(GetStrategy())
        {
        }

        #endregion

        #region Methods

        protected virtual T DoDeserialize(XmlReader reader)
        {
            return _strategy.Deserialize(reader);
        }

        /// <summary>
        /// Deserializes an instance of <typeparamref name="T"/> from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream from which to deserialize the instance.</param>
        /// <returns>The instance.</returns>
        public T Deserialize(Stream stream)
        {
            using (var r = XmlReader.Create(stream, _readerSettings)) {
                return DoDeserialize(r);
            }
        }

        /// <summary>
        /// Deserializes an instance of <typeparamref name="T"/> from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader from which to deserialize the instance.</param>
        /// <returns>The instance.</returns>
        public T Deserialize(TextReader reader)
        {
            using (var r = XmlReader.Create(reader, _readerSettings)) {
                return DoDeserialize(r);
            }
        }

        /// <summary>
        /// Deserializes an instance of <typeparamref name="T"/> from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader from which to deserialize the instance.</param>
        /// <returns>The instance.</returns>
        public T Deserialize(XmlReader reader)
        {
            using (var r = XmlReader.Create(reader, _readerSettings)) {
                return DoDeserialize(r);
            }
        }

        protected virtual void DoSerialize(XmlWriter writer, T value)
        {
            _strategy.Serialize(writer, value);
        }

        /// <summary>
        ///  Serializes the instance to the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to which the <paramref name="instance"/> is to be serialized.</param>
        /// <param name="instance">The value to serialize.</param>
        public void Serialize(Stream stream, T value)
        {
            using (var w = XmlWriter.Create(stream, _writerSettings)) {
                DoSerialize(w, value);
            }
        }

        /// <summary>
        ///  Serializes the instance to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to which the <paramref name="instance"/> is to be serialized.</param>
        /// <param name="instance">The value to serialize.</param>
        public void Serialize(TextWriter writer, T value)
        {
            using (var w = XmlWriter.Create(writer, _writerSettings)) {
                DoSerialize(w, value);
            }
        }

        /// <summary>
        ///  Serializes the instance to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to which the <paramref name="instance"/> is to be serialized.</param>
        /// <param name="instance">The value to serialize.</param>
        public void Serialize(XmlWriter writer, T value)
        {
            using (var w = XmlWriter.Create(writer, _writerSettings)) {
                DoSerialize(w, value);
            }
        }

        #endregion
    }
}
