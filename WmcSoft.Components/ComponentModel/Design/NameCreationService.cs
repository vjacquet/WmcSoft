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
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace WmcSoft.ComponentModel.Design
{
    /// <summary>
    /// Provides a service that can generate unique names for objects.
    /// </summary>
    public class NameCreationService : INameCreationService
    {
        #region Fields

        readonly IEqualityComparer<string> _comparer;

        #endregion

        #region Lifecycle

        public NameCreationService(IEqualityComparer<string> comparer)
        {
            _comparer = comparer;
        }

        public NameCreationService() : this(StringComparer.InvariantCulture)
        {

        }

        #endregion

        #region INameCreationService Membres

        /// <summary>
        /// Creates a new name that is unique to all components 
        /// in the specified container.
        /// </summary>
        /// <param name="container">The container where the new object is added.</param>
        /// <param name="dataType">The data type of the object that receives the name.</param>
        /// <returns>A unique name for the data type.</returns>
        public virtual string CreateName(IContainer container, Type dataType)
        {
            string baseName = dataType.Name.Substring(0, 1).ToLower() + dataType.Name.Substring(1);
            string candidateName;
            int uniqueID = 1;

            // Continue to increment uniqueID numeral until a 
            // unique ID is located.
            bool unique = false;
            do {
                candidateName = baseName + uniqueID;
                unique = true;

                for (int i = 0; i < container.Components.Count; i++) {
                    if (_comparer.Equals(candidateName, container.Components[i].Site.Name)) {
                        unique = false;
                        uniqueID++;
                        break;
                    }
                }
            }
            while (!unique);

            return candidateName;
        }

        /// <summary>
        /// Gets a value indicating whether the specified name is valid.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <returns>true if the name is valid; otherwise, false.</returns>
        public bool IsValidName(string name)
        {
            return DoValidateName(name) == null;
        }

        /// <summary>
        /// Throws an exception if the specified name does not contain 
        /// all valid character types.
        /// </summary>
        /// <param name="name">The name to validate. </param>
        public void ValidateName(string name)
        {
            var exception = DoValidateName(name);
            if (exception != null)
                throw exception;
        }

        #endregion

        #region Overridables

        protected virtual Exception DoValidateName(string name)
        {
            for (int i = 0; i < name.Length; i++) {
                var uc = Char.GetUnicodeCategory(name, i);
                switch (uc) {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    break;
                default:
                    return new ArgumentException(String.Format(WmcSoft.Properties.Resources.InvalidNameArgumentException, name));
                }
            }
            return null;
        }

        #endregion
    }
}
