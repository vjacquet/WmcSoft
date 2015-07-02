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
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using WmcSoft.Net.Mail;

namespace WmcSoft.Configuration
{
    /// <summary>
    /// Represents a policy to create MailMessage based on the configuration.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class MailPolicy : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true)]
        public string Name {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("subject", IsRequired = false, DefaultValue = null)]
        public string Subject {
            get { return (string)this["subject"]; }
            set { this["subject"] = value; }
        }

        [ConfigurationProperty("to", IsRequired = false, DefaultValue = null)]
        [TypeConverter(typeof(MailAddressCollectionConverter))]
        public MailAddressCollection To {
            get { return (MailAddressCollection)this["to"]; }
            set { this["to"] = value; }
        }

        [ConfigurationProperty("cc", IsRequired = false, DefaultValue = null)]
        [TypeConverter(typeof(MailAddressCollectionConverter))]
        public MailAddressCollection Cc {
            get { return (MailAddressCollection)this["cc"]; }
            set { this["cc"] = value; }
        }

        [ConfigurationProperty("bcc", IsRequired = false, DefaultValue = null)]
        [TypeConverter(typeof(MailAddressCollectionConverter))]
        public MailAddressCollection Bcc {
            get { return (MailAddressCollection)this["bcc"]; }
            set { this["bcc"] = value; }
        }

        [ConfigurationProperty("replyTo", IsRequired = false, DefaultValue = null)]
        [TypeConverter(typeof(MailAddressCollectionConverter))]
        public MailAddressCollection ReplyTo {
            get { return (MailAddressCollection)this["replyTo"]; }
            set { this["replyTo"] = value; }
        }

        static IEnumerable<MailAddress> Forward(MailAddress address) {
            yield return address;
        }

        public MailMessage CreateMessage() {
            return CreateMessage(Forward);
        }

        public MailMessage CreateMessage(Func<MailAddress, IEnumerable<MailAddress>> interpreter) {
            var m = new MailMessage();

            m.Subject = this.Subject ?? "";

            var to = this.To ?? new MailAddressCollection();
            var cc = this.Cc ?? new MailAddressCollection();
            var bcc = this.Bcc ?? new MailAddressCollection();
            var replyTo = this.ReplyTo ?? new MailAddressCollection();

            m.To.AddRange(to.SelectMany(a => interpreter(a)));
            m.CC.AddRange(cc.SelectMany(a => interpreter(a)).Except(to));
            m.Bcc.AddRange(bcc.SelectMany(a => interpreter(a)).Except(to).Except(cc));
            m.ReplyToList.AddRange(replyTo.SelectMany(a => interpreter(a)));

            return m;
        }
    }
}