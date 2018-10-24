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

using System.Collections.Generic;
using System.IO;

namespace WmcSoft.IO
{
    /// <summary>
    /// Defines the extension methods to <see cref="TextReader"/> objects.
    /// This is a static class.
    /// </summary>
    public static class TextReaderExtensions
    {
        public static IEnumerable<string> ReadLines(this TextReader self)
        {
            string line;
            while ((line = self.ReadLine()) != null) {
                yield return line;
            }
        }
    }
}
