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
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Configuration;

namespace WmcSoft.CodeBuilders
{
    public abstract class TerminalCodeBuilder : CodeBuilder
    {
        public sealed override void Parse(XmlReader reader, CodeBuilderContext context) {
            IDictionary<string, string> attributes = CodeBuilder.ReadAttributes(reader);
            DoParse(attributes, context);

            if (attributes.Count > 0)
                throw new CodeBuilderException("Unrecognize attribute.");
        }

        protected abstract void DoParse(IDictionary<string, string> attributes, CodeBuilderContext context);
    }

}
