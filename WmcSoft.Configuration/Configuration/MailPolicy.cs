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

        [ConfigurationProperty("sender", IsRequired = false, DefaultValue = null)]
        [TypeConverter(typeof(MailAddressConverter))]
        public MailAddress Sender {
            get { return (MailAddress)this["sender"]; }
            set { this["sender"] = value; }
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

        static IEnumerable<MailAddress> Forward(MailAddress address)
        {
            yield return address;
        }

        /// <summary>
        /// Creates a <see cref="MailMessage"/> using the configured elements.
        /// </summary>
        /// <returns>The mail message.</returns>
        /// <remarks>Duplicates are removed per properties and across properties 
        /// (i.e. an email address in <see cref="MailMessage.To"/> will be removed from <see cref="MailMessage.CC"/> and <see cref="MailMessage.Bcc"/></remarks>
        public MailMessage CreateMessage()
        {
            return CreateMessage(Forward);
        }

        /// <summary>
        /// Creates a <see cref="MailMessage"/> using the configured elements, expanding fake email addresses into real email addresses.
        /// </summary>
        /// <param name="interpreter">The interpreter function to expand the email addresses.</param>
        /// <returns>The mail message.</returns>
        /// <remarks>Duplicates are removed per properties and across properties 
        /// (i.e. an email address in <see cref="MailMessage.To"/> will be removed from <see cref="MailMessage.CC"/> and <see cref="MailMessage.Bcc"/></remarks>
        public MailMessage CreateMessage(Func<MailAddress, IEnumerable<MailAddress>> interpreter)
        {
            var m = new MailMessage {
                Subject = Subject ?? ""
            };

            var to = To ?? new MailAddressCollection();
            var cc = Cc ?? new MailAddressCollection();
            var bcc = Bcc ?? new MailAddressCollection();
            var replyTo = ReplyTo ?? new MailAddressCollection();

            if (Sender != null)
                m.Sender = Sender;
            m.To.AddRange(to.SelectMany(interpreter).Distinct());
            m.CC.AddRange(cc.SelectMany(interpreter).Except(to).Distinct());
            m.Bcc.AddRange(bcc.SelectMany(interpreter).Except(to).Except(cc).Distinct());
            m.ReplyToList.AddRange(replyTo.SelectMany(interpreter));

            return m;
        }
    }
}
