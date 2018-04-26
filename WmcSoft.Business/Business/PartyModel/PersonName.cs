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

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents the name for a Person.
    /// </summary>
    public class PersonName : DomainObject<TKey>, ITemporal
    {
        #region Lifecycle

        public PersonName()
        {
        }

        public PersonName(string preferredName)
        {
            PreferredName = preferredName;
        }

        public PersonName(string prefix = null, string givenNames = null, string famillyName = null, string suffix = null, string preferredName = null)
        {
            Prefix = prefix;
            GivenNames = givenNames;
            FamilyName = famillyName;
            Suffix = suffix;
            PreferredName = preferredName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An honorific such as Mr., Miss, Dr., Rev., and so on
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The first name in Western countries—this can include 
        /// hyphenated names (it.g., Jean-Paul) and names including 
        /// more than one word (it.g., Kwai Lin)
        /// </summary>
        public string GivenName {
            get {
                var name = GivenNames;
                if (name == null)
                    return null;
                var index = name.IndexOf(',');
                if (index < 0)
                    return null;
                return name.Substring(0, index);
            }
        }

        /// <summary>
        /// Any name other than the family name
        /// </summary>
        public string GivenNames { get; set; }

        /// <summary>
        /// The last name in Western countries—this is the only 
        /// mandatory component of the name and the only one 
        /// used if a Person has but a single name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// A name that the person is commonly known by—this is 
        /// often a contraction of one of the other names 
        /// (it.g., "Jim" is short for "James," "Bill" is short 
        /// for "William")
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Each suffix may be a generational label (it.g., Jr., III),
        /// a qualification (it.g., BSc., bachelor of sciences; 
        /// Ph.D., doctor of philosophy), a title (it.g., FRSC, 
        /// Fellow of the Royal Society of Chemistry; 
        /// Bart, Baronet; KG, Knight of the Garter)
        /// </summary>
        public string Suffix { get; set; }

        public PersonNameUse Use { get; set; }

        #endregion

        #region ITemporal Members

        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }

        #endregion
    }
}
