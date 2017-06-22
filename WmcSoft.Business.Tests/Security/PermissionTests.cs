using Xunit;

namespace WmcSoft.Security
{
    public class PermissionTests
    {
        [Fact]
        public void CanCreatePermissions()
        {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var set = read | write;

            Assert.True(set.Contains(read));
            Assert.True(set.Contains(write));
        }

        [Fact]
        public void CanUnionPermissionSets()
        {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var execute = Permissions.Execute;
            var set1 = read | write;
            var set2 = read | execute;

            var set3 = set1 | set2;

            Assert.True(set3.Contains(read));
            Assert.True(set3.Contains(write));
            Assert.True(set3.Contains(execute));
        }

        [Fact]
        public void CanIntersectPermissionSets()
        {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var execute = Permissions.Execute;
            var set1 = read | write;
            var set2 = read | execute;

            var set3 = set1 & set2;

            Assert.True(set3.Contains(read));
            Assert.False(set3.Contains(write));
            Assert.False(set3.Contains(execute));
        }

        [Fact]
        public void CanGrantPermissions()
        {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var execute = Permissions.Execute;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();
            securable.Grant(read | write, user1);

            Assert.True(securable.AccessControl[user1, read]);
            Assert.False(securable.AccessControl[user1, execute]);
            Assert.False(securable.AccessControl[user2, read]);
            Assert.False(securable.AccessControl[user2, execute]);
        }

        [Fact]
        public void CanDenyPermissions()
        {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var user1 = new User("user1");

            ISecurable securable = new Command();
            securable.Grant(read | write, user1);

            Assert.True(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user1, write]);

            securable.Deny(write, user1);
            Assert.True(securable.AccessControl[user1, read]);
            Assert.False(securable.AccessControl[user1, write]);
        }

        [Fact]
        public void CanCreateComplexPermissions()
        {
            var read = Permissions.Read;
            var write = Permissions.Write;
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();

            securable.Grant(read, everyone);
            Assert.True(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user2, read]);

            securable.Deny(read, user1);
            Assert.False(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user2, read]);
        }

        [Fact]
        public void CanRevokeGrantPermissions()
        {
            var read = Permissions.Read;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();

            securable.Grant(read, user1);
            securable.Grant(read, user2);
            Assert.True(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user2, read]);

            securable.Revoke(read, user1);
            Assert.False(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user2, read]);
        }

        [Fact]
        public void CanRevokeDenyPermissions()
        {
            var read = Permissions.Read;
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            ISecurable securable = new Command();

            securable.Grant(read, everyone);
            Assert.True(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user2, read]);

            securable.Deny(read, user1);
            Assert.False(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user2, read]);

            securable.Revoke(read, user1);
            Assert.True(securable.AccessControl[user1, read]);
            Assert.True(securable.AccessControl[user2, read]);
        }

        [Fact]
        public void CannotCompareAuthorizeAndRequiredLevelPermission()
        {
            var required = new RequiredLevelPermission("permission", 1);
            var authorized = new AutorizedLevelPermission("permission", 1);

            Assert.False(required.Equals(authorized));
            Assert.False(authorized.Equals(required));
            Assert.NotEqual<Permission>(authorized, required);
        }
    }
}