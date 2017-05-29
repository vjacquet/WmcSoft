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

namespace WmcSoft.Runtime.Serialization
{
    public class BinarySerializer<T>
    {
        #region Fields

        private readonly System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _serializer;

        #endregion

        #region Lifecycle

        public BinarySerializer(ISurrogateSelector selector, StreamingContext context)
        {
            _serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(selector, context);
        }

        public BinarySerializer()
        {
            _serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        }

        #endregion

        #region Methods

        protected virtual T DoDeserialize(Stream stream)
        {
            return (T)_serializer.Deserialize(stream);
        }

        /// <summary>
        /// Deserializes an instance of <typeparamref name="T"/> from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream from which to deserialize the instance.</param>
        /// <returns>The instance.</returns>
        public T Deserialize(Stream stream)
        {
            return DoDeserialize(stream);
        }

        protected virtual void DoSerialize(Stream stream, T value)
        {
            _serializer.Serialize(stream, value);
        }

        /// <summary>
        ///  Serializes the instance to the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to which the <paramref name="instance"/> is to be serialized.</param>
        /// <param name="instance">The value to serialize.</param>
        public void Serialize(Stream stream, T instance)
        {
            DoSerialize(stream, instance);
        }

        #endregion
    }
}
