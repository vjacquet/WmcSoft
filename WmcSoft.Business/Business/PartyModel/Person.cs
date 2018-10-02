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

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents informations about a human being.
    /// </summary>
    [RM.DisplayName(nameof(Person))]
    [RM.Description(nameof(Person))]
    public class Person : Party
    {
        #region Lifecycle

        public Person()
        {
            OtherPersonNames = new List<PersonName>();
        }

        public Person(PersonName name)
            : this()
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            PersonName = name;
        }

        public Person(string prefix = null, string givenNames = null, string famillyName = null, string suffix = null, string preferredName = null)
            : this()
        {
            PersonName = new PersonName {
                Prefix = prefix,
                GivenNames = givenNames,
                FamilyName = famillyName
            };
        }

        public Person(string preferredName)
            : this(new PersonName { PreferredName = preferredName })
        {
        }

        #endregion

        #region Properties

        public virtual DateTime DateOfBirth { get; set; }
        public virtual DateTime? DateOfDeath { get; set; }
        public virtual PersonName PersonName { get; set; }
        public virtual ICollection<PersonName> OtherPersonNames { get; private set; }
        public virtual Gender Gender { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public virtual BodyMetrics BodyMetrics { get; set; }

        #endregion

        #region IFormattable overrides

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (PersonName == null)
                return Id.ToString();
            return PersonName.ToString(format, formatProvider);
        }

        #endregion
    }
}
