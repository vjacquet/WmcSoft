using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Security
{
    [TestClass]
    public class SecurableTests
    {
        [TestMethod]
        public void CanAssignPermissions() {
            var everyone = Groups.Everyone;
            var user = new User("user");

            var read = Permissions.Read;
            var write = Permissions.Write;

            var todo = new Todo();
            todo.Grant(read, user);

            Assert.IsTrue(todo.AccessControl[user, read]);
            Assert.IsFalse(todo.AccessControl[user, write]);

            Assert.IsFalse(todo.AccessControl[everyone, read]);
        }

        [TestMethod]
        public void CanPropagatePermissions() {
            var user = new User("user");
            var everyone = Groups.Everyone;
            var group = new Group("France");
            group.Add(user);

            var read = Permissions.Read;
            var write = Permissions.Write;

            var todo = new Todo();
            todo.Grant(read, everyone);

            Assert.IsTrue(todo.AccessControl[everyone, read]);
            Assert.IsTrue(todo.AccessControl[user, read]);
            Assert.IsFalse(todo.AccessControl[everyone, write]);
        }

        [TestMethod]
        public void CanAssociatePermissions() {
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            var read = Permissions.Read;
            var write = Permissions.Write;
            var delete = Permissions.Delete;

            var todo1 = new Todo();
            todo1.Grant(read | write, everyone);
            Assert.IsTrue(todo1.AccessControl.IsUnique);

            var todo2 = new Todo();
            todo2.Grant(write, user1, user2);
            Assert.IsTrue(todo2.AccessControl.IsUnique);

            SecurityContext.Merge(todo1, todo2);

            Assert.IsFalse(todo1.AccessControl.IsUnique);
            Assert.IsFalse(todo2.AccessControl.IsUnique);

            Assert.IsTrue(todo2.AccessControl[everyone,read]);
            Assert.IsTrue(todo1.AccessControl[user1, write]);
            Assert.IsTrue(todo1.AccessControl[user2, write]);
       }

        [TestMethod]
        public void CanDisassociatePermissions() {
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            var read = Permissions.Read;
            var write = Permissions.Write;
            var delete = Permissions.Delete;

            var todo1 = new Todo();
            todo1.Grant(read, everyone);
            Assert.IsTrue(todo1.AccessControl.IsUnique);

            var todo2 = new Todo();
            todo2.Grant(write, user1, user2);
            Assert.IsTrue(todo2.AccessControl.IsUnique);

            SecurityContext.Merge(todo2, todo1);

            Assert.IsFalse(todo1.AccessControl.IsUnique);
            Assert.IsFalse(todo2.AccessControl.IsUnique);

            Assert.IsTrue(todo2.AccessControl[everyone, read]);
            Assert.IsTrue(todo1.AccessControl[user1, write]);
            Assert.IsTrue(todo1.AccessControl[user2, write]);

            SecurityContext.Unbind(todo2);
            todo1.AccessControl.Revoke(write, user1);

            Assert.IsTrue(todo2.AccessControl.IsUnique);
            Assert.IsTrue(todo2.AccessControl[user1, write]);
            Assert.IsTrue(todo2.AccessControl[user2, write]);
            Assert.IsFalse(todo1.AccessControl[user1, write]);
            Assert.IsTrue(todo1.AccessControl[user2, write]);
        }
    }
}
