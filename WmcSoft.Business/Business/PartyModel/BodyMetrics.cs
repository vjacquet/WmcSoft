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

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Provides a way to store informations about the human body
    /// such as size, weight, hair color, eye color, measurements,
    /// and so on.
    /// </summary>
    public class BodyMetrics
    {
        #region Lifecycle

        BodyMetrics() {
        }

        #endregion

        #region Properties

        public string BloodType { get; set; }

        public string EyeColor { get; set; }

        public string Hair { get; set; }

        public string Complexion { get; set; }

        public string Weight { get; set; }

        public string Height { get; set; }

        public string ShoeSize { get; set; }

        #endregion
    }
}
