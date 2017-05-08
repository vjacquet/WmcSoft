using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Security
{
    [TestClass]
    public class PermissionTests
    {
        [TestMethod]
        public void CanCreatePermissions() {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var set = read | write;

            Assert.IsTrue(set.Contains(read));
            Assert.IsTrue(set.Contains(write));
        }

        [TestMethod]
        public void CanUnionPermissionSets() {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var execute = Permissions.Execute;
            var set1 = read | write;
            var set2 = read | execute;

            var set3 = set1 | set2;

            Assert.IsTrue(set3.Contains(read));
            Assert.IsTrue(set3.Contains(write));
            Assert.IsTrue(set3.Contains(execute));
        }

        [TestMethod]
        public void CanIntersectPermissionSets() {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var execute = Permissions.Execute;
            var set1 = read | write;
            var set2 = read | execute;

            var set3 = set1 & set2;

            Assert.IsTrue(set3.Contains(read));
            Assert.IsFalse(set3.Contains(write));
            Assert.IsFalse(set3.Contains(execute));
        }

        [TestMethod]
        public void CanGrantPermissions() {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var execute = Permissions.Execute;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();
            securable.Grant(read | write, user1);

            Assert.IsTrue(securable.AccessControl[user1, read]);
            Assert.IsFalse(securable.AccessControl[user1, execute]);
            Assert.IsFalse(securable.AccessControl[user2, read]);
            Assert.IsFalse(securable.AccessControl[user2, execute]);
        }

        [TestMethod]
        public void CanDenyPermissions() {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var user1 = new User("user1");

            ISecurable securable = new Command();
            securable.Grant(read | write, user1);

            Assert.IsTrue(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user1, write]);

            securable.Deny(write, user1);
            Assert.IsTrue(securable.AccessControl[user1, read]);
            Assert.IsFalse(securable.AccessControl[user1, write]);
        }

        [TestMethod]
        public void CanCreateComplexPermissions() {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();

            securable.Grant(read, everyone);
            Assert.IsTrue(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user2, read]);

            securable.Deny(read, user1);
            Assert.IsFalse(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user2, read]);
        }

        [TestMethod]
        public void CanRevokeGrantPermissions() {
            var read = Permissions.Read;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();

            securable.Grant(read, user1);
            securable.Grant(read, user2);
            Assert.IsTrue(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user2, read]);

            securable.Revoke(read, user1);
            Assert.IsFalse(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user2, read]);
        }

        [TestMethod]
        public void CanRevokeDenyPermissions() {
            var read = Permissions.Read;
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();

            securable.Grant(read, everyone);
            Assert.IsTrue(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user2, read]);

            securable.Deny(read, user1);
            Assert.IsFalse(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user2, read]);

            securable.Revoke(read, user1);
            Assert.IsTrue(securable.AccessControl[user1, read]);
            Assert.IsTrue(securable.AccessControl[user2, read]);
        }

        [TestMethod]
        public void CannotCompareAuthorizeLevelPermissionAndRequiredLevelPermission()
        {
            var required = new RequiredLevelPermission("permission", 1);
            var authorized = new AutorizedLevelPermission("permission", 1);

            Assert.AreNotEqual(required, authorized);
        }
    }
}
