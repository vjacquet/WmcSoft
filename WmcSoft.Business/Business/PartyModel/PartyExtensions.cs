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
using System.Linq;

namespace WmcSoft.Business.PartyModel
{
    public static class PartyExtensions
    {
        #region Roles

        public static bool CanPlayRole<R>(this Party party) where R : PartyRole
        {
            bool isTypeCompatible = PartyRole.GetConstraintsOf<R>().Any(r => r.CanPlayRole(party));
            //if (isTypeCompatible && party is ISupportCapabilities) {
            //    // The capabilities is a RuleContext so use this to apply rules.
            //    var capabilities = ((ISupportCapabilities)party).Capabilities;
            //}
            return isTypeCompatible;
        }

        public static R AddRole<R>(this Party party) where R : PartyRole
        {
            var role = (R)Activator.CreateInstance(typeof(R), party);
            role.Id = Guid.NewGuid();
            return role;
        }

        public static R EnsureRole<R>(this Party party) where R : PartyRole
        {
            var role = party.Roles.OfType<R>().FirstOrDefault();
            if (role == null)
                role = party.AddRole<R>();
            return role;
        }

        public static bool HasRole<R>(this Party party) where R : PartyRole
        {
            var hasRole = party.Roles.OfType<R>().Any();
            return hasRole;
        }

        public static bool HasRole<R>(this Party party, Party other) where R : PartyRole
        {
            var hasRole = party.Roles.OfType<R>().Any(r => r.Relationships.Any(rs => rs.Client.Party == other));
            return hasRole;
        }

        #endregion

        #region Builders

        public static Person WithAlias(this Person person, PersonName name)
        {
            person.OtherPersonNames.Add(name);
            return person;
        }

        public static Person WithAlias(this Person person, string prefix = null, string givenNames = null, string famillyName = null, string suffix = null, string preferredName = null)
        {
            return WithAlias(person, new PersonName {
                Use = PersonNameUse.Alias,
                Prefix = prefix,
                GivenNames = givenNames,
                FamilyName = famillyName,
                Suffix = suffix,
                PreferredName = preferredName,
            });
        }

        public static TOrganization TradingAs<TOrganization>(this TOrganization organization, string name)
            where TOrganization : Organization
        {
            organization.OtherOrganizationNames.Add(new OrganizationName(name, OrganizationNameUse.TradingName));
            return organization;
        }

        public static TParty WithAddress<TParty>(this TParty party, AddressBase address)
            where TParty : Party
        {
            party.Addresses.Add(address);
            return party;
        }

        public static TParty WithAddress<TParty>(this TParty party, string use, AddressBase address)
            where TParty : Party
        {
            party.AddressProperties.Add(new AddressProperties { Use = use, Party = party, Address = address });
            return party;
        }

        public static TParty WithEmail<TParty>(this TParty party, string email, string use = null)
            where TParty : Party
        {
            return WithAddress(party, use, new EmailAddress(email));
        }

        public static TParty WithWebPage<TParty>(this TParty party, string url, string use = null)
            where TParty : Party
        {
            return WithAddress(party, use, new WebPageAddress(new Uri(url)));
        }

        public static TParty WithWebPage<TParty>(this TParty party, Uri url, string use = null)
            where TParty : Party
        {
            return WithAddress(party, use, new WebPageAddress(url));
        }

        public static TParty WithPhone<TParty>(this TParty party, string number, string use = null)
            where TParty : Party
        {
            return WithAddress(party, use, new TelecomAddress(number, TelecomPhysicalType.Phone));
        }

        public static TParty WithFax<TParty>(this TParty party, string number, string use = null)
            where TParty : Party
        {
            return WithAddress(party, use, new TelecomAddress(number, TelecomPhysicalType.Fax));
        }

        public static TParty WithCellphone<TParty>(this TParty party, string number, string use = null)
            where TParty : Party
        {
            return WithAddress(party, use, new TelecomAddress(number, TelecomPhysicalType.Cell));
        }

        public static TParty WithPager<TParty>(this TParty party, string number, string use = null)
            where TParty : Party
        {
            return WithAddress(party, use, new TelecomAddress(number, TelecomPhysicalType.Pager));
        }

        #endregion
    }
}
