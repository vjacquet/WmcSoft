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

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents informations about a human being.
    /// </summary>
    public class Person : Party
    {
        #region Lifecycle

        public Person() {
            OtherPersonNames = new List<PersonName>();
        }

        public Person(PersonName name)
            : this() {
            PersonName = name;
        }

        public Person(string preferredName)
            : this(new PersonName { PreferredName = preferredName }) {
        }

        #endregion

        #region Properties

        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public PersonName PersonName { get; set; }
        public ICollection<PersonName> OtherPersonNames { get; private set; }
        public Gender Gender { get; set; }
        public Ethnicity Ethnicity { get; set; }
        public BodyMetrics BodyMetrics { get; set; }

        #endregion
    }
}
