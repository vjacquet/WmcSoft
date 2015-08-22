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

        interface ISerializationStrategy
        {
            T Deserialize(XmlReader reader);
            void Serialize(XmlWriter writer, T value);
        }

        class DataContractStrategy : ISerializationStrategy
        {
            private readonly DataContractSerializer _serializer;

            public DataContractStrategy() {
                _serializer = new DataContractSerializer(typeof(T));
            }

            public T Deserialize(XmlReader reader) {
                return (T)_serializer.ReadObject(reader);
            }

            public void Serialize(XmlWriter writer, T value) {
                _serializer.WriteObject(writer, value);
            }
        }

        class XmlStrategy : ISerializationStrategy
        {
            private readonly XmlSerializer _serializer;

            public XmlStrategy() {
                _serializer = new XmlSerializer(typeof(T));
            }

            public T Deserialize(XmlReader reader) {
                return (T)_serializer.Deserialize(reader);
            }

            public void Serialize(XmlWriter writer, T value) {
                _serializer.Serialize(writer, value);
            }
        }

        #endregion

        #region Fields

        private readonly ISerializationStrategy _strategy;
        private readonly XmlReaderSettings _readerSettings;
        private readonly XmlWriterSettings _writerSettings;

        #endregion

        #region Lifecycle

        public XmlSerializer() {
            if (typeof(T).IsDefined(typeof(XmlTypeAttribute), false))
                _strategy = new XmlStrategy();
            else
                _strategy = new DataContractStrategy();

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

        #endregion

        #region Methods

        protected virtual T DoDeserialize(XmlReader reader) {
            return _strategy.Deserialize(reader);
        }

        public T Deserialize(Stream stream) {
            using (var reader = XmlReader.Create(stream, _readerSettings)) {
                return DoDeserialize(reader);
            }
        }

        public T Deserialize(TextReader textReader) {
            using (var reader = XmlReader.Create(textReader, _readerSettings)) {
                return DoDeserialize(reader);
            }
        }

        public T Deserialize(XmlReader xmlReader) {
            using (var reader = XmlReader.Create(xmlReader, _readerSettings)) {
                return DoDeserialize(reader);
            }
        }

        protected virtual void DoSerialize(XmlWriter writer, T value) {
            _strategy.Serialize(writer, value);
        }

        public void Serialize(Stream stream, T value) {
            using (var writer = XmlWriter.Create(stream, _writerSettings)) {
                DoSerialize(writer, value);
            }
        }

        public void Serialize(TextWriter textWriter, T value) {
            using (var writer = XmlWriter.Create(textWriter, _writerSettings)) {
                DoSerialize(writer, value);
            }
        }

        public void Serialize(XmlWriter xmlWriter, T value) {
            using (var writer = XmlWriter.Create(xmlWriter, _writerSettings)) {
                DoSerialize(writer, value);
            }
        }

        #endregion
    }
}
