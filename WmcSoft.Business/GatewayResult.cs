#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft
{
    [DebuggerDisplay("{ToString(),nq}")]
    [Serializable]
    public sealed class GatewayResult : ISerializable
    {
        private static GatewayError[] None = new GatewayError[0];
        private static GatewayError[] Undefined = new GatewayError[0];

        public static GatewayResult Success = new GatewayResult(None);

        private readonly GatewayError[] _errors;

        private GatewayResult(GatewayError[] errors)
        {
            _errors = errors;
        }

        public bool Succeeded {
            get { return ReferenceEquals(_errors, None); }
        }

        public ReadOnlyCollection<GatewayError> Errors {
            get { return new ReadOnlyCollection<GatewayError>(_errors); }
        }

        public static GatewayResult Failed(params GatewayError[] errors)
        {
            if (errors == null || errors.Length == 0)
                return new GatewayResult(Undefined);
            return new GatewayResult((GatewayError[])errors.Clone());
        }

        public override string ToString()
        {
            if (Succeeded)
                return "Succeeded";
            if (_errors.Length == 0)
                return "Failed";
            return "Failed: " + string.Join(", ", Array.ConvertAll(_errors, e => e.Code));
        }

        #region Serialization

        private GatewayResult(SerializationInfo info, StreamingContext context)
        {
            var succeeded = info.GetBoolean("Succeeded");
            _errors = succeeded ? None : info.GetValue<GatewayError[]>("Errors");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Succeeded) {
                info.AddValue("Succeeded", true);
            } else {
                info.AddValue("Succeeded", false);
                info.AddValue("Errors", _errors);
            }
        }

        #endregion
    }
}