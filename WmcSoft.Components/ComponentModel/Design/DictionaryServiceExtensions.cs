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

using System.Collections;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel.Design
{
    public static class DictionaryServiceExtensions
    {
        /// <summary>
        /// Decorates a dictionary as a dictionary service
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <returns>The dictionary service</returns>
        public static IDictionaryService AsDictionaryService(this IDictionary dictionary)
        {
            return new DictionaryService(dictionary);
        }

        /// <summary>
        /// Creates a dictionary service and initialize it with the values of the dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <returns>The dictionary service</returns>
        public static IDictionaryService ToDictionaryService(this IDictionary dictionary)
        {
            return new DictionaryService(new Hashtable(dictionary));
        }

        /// <summary>
        /// Removes a value from the dictionary service.
        /// </summary>
        /// <param name="svc">The dictionary service.</param>
        /// <param name="key">The key.</param>
        /// <returns>true if the value was removed, false if it was missing.</returns>
        public static bool RemoveValue(this IDictionaryService svc, object key)
        {
            if (svc.GetValue(key) != null) {
                svc.SetValue(key, null);
                return true;
            }
            return false;
        }
    }
}
