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
using System.Runtime.Serialization;

namespace WmcSoft.Units
{
    /// <summary>
    /// Description r�sum�e de IncompatibleMetricException.
    /// </summary>
    public class IncompatibleMetricException : MetricException
    {
        public IncompatibleMetricException()
            : base(DefaultMessage) {
        }

        public IncompatibleMetricException(string message)
            : base(message) {
        }

        protected IncompatibleMetricException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        public IncompatibleMetricException(string message, Exception innerException)
            : base(message, innerException) {
        }

        #region DefaultMessage
        public static string DefaultMessage {
            get {
                if (defaultMessage == null) {
                    defaultMessage = RM.GetString(RM.IncompatibleMetricException);
                }
                return defaultMessage;
            }
        } static string defaultMessage;
        #endregion
    }
}