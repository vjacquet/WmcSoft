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

namespace WmcSoft.Security
{
    public abstract class SecurityContext
    {
        public static SecurityContext Current { get; set; }

        public abstract void RunWithElevatedPrivileges(Action action);

        public static void Merge(params ISecurable[] securables)
        {
            if (securables == null || securables.Length < 2)
                return;

            var primary = securables[0];
            for (int i = 1; i < securables.Length; i++) {
                primary.AccessControl.MergeWith(securables[i].AccessControl);
            }
        }

        //public static void Inherit(ISecurable parent, params ISecurable[] securables) {
        //    throw new NotImplementedException();
        //}

        public static void Unbind(ISecurable securable)
        {
            securable.AccessControl.MakeUnique();
        }

        //public object Audit(Expression<Func<ISecurable, bool>> predicate) {
        //    throw new NotImplementedException();
        //}
    }
}
