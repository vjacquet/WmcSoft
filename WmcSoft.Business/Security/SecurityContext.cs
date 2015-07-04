using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    public abstract class SecurityContext
    {
        public static SecurityContext Current { get; set; }

        public abstract void RunWithElevatedPrivileges(Action action);

        public static void Merge(params ISecurable[] securables) {
            if (securables == null || securables.Length < 2)
                return;

            var primary = securables[0];
            for (int i =1; i < securables.Length; i++) {
                primary.AccessControl.MergeWith(securables[i].AccessControl);
            }
        }

        //public static void Inherit(ISecurable parent, params ISecurable[] securables) {
        //    throw new NotImplementedException();
        //}

        public static void Unbind(ISecurable securable) {
            securable.AccessControl.MakeUnique();
        }

        //public object Audit(Expression<Func<ISecurable, bool>> predicate) {
        //    throw new NotImplementedException();
        //}
    }
}
