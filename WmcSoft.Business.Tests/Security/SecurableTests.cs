using Xunit;

namespace WmcSoft.Security
{
    public class SecurableTests
    {
        [Fact]
        public void CanAssignPermissions()
        {
            var everyone = Groups.Everyone;
            var user = new User("user");

            var read = Permissions.Read;
            var write = Permissions.Write;

            var todo = new Todo();
            todo.Grant(read, user);

            Assert.True(todo.AccessControl[user, read]);
            Assert.False(todo.AccessControl[user, write]);

            Assert.False(todo.AccessControl[everyone, read]);
        }

        [Fact]
        public void CanPropagatePermissions()
        {
            var user = new User("user");
            var everyone = Groups.Everyone;
            var group = new Group("France");
            group.Add(user);

            var read = Permissions.Read;
            var write = Permissions.Write;

            var todo = new Todo();
            todo.Grant(read, everyone);

            Assert.True(todo.AccessControl[everyone, read]);
            Assert.True(todo.AccessControl[user, read]);
            Assert.False(todo.AccessControl[everyone, write]);
        }

        [Fact]
        public void CanAssociatePermissions()
        {
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            var read = Permissions.Read;
            var write = Permissions.Write;
            var delete = Permissions.Delete;

            var todo1 = new Todo();
            todo1.Grant(read | write, everyone);
            Assert.True(todo1.AccessControl.IsUnique);

            var todo2 = new Todo();
            todo2.Grant(write, user1, user2);
            Assert.True(todo2.AccessControl.IsUnique);

            SecurityContext.Merge(todo1, todo2);

            Assert.False(todo1.AccessControl.IsUnique);
            Assert.False(todo2.AccessControl.IsUnique);

            Assert.True(todo2.AccessControl[everyone, read]);
            Assert.True(todo1.AccessControl[user1, write]);
            Assert.True(todo1.AccessControl[user2, write]);
        }

        [Fact]
        public void CanDisassociatePermissions()
        {
            var everyone = Groups.Everyone;
            var user1 = new User("user1");
            var user2 = new User("user2");

            var read = Permissions.Read;
            var write = Permissions.Write;
            var delete = Permissions.Delete;

            var todo1 = new Todo();
            todo1.Grant(read, everyone);
            Assert.True(todo1.AccessControl.IsUnique);

            var todo2 = new Todo();
            todo2.Grant(write, user1, user2);
            Assert.True(todo2.AccessControl.IsUnique);

            SecurityContext.Merge(todo2, todo1);

            Assert.False(todo1.AccessControl.IsUnique);
            Assert.False(todo2.AccessControl.IsUnique);

            Assert.True(todo2.AccessControl[everyone, read]);
            Assert.True(todo1.AccessControl[user1, write]);
            Assert.True(todo1.AccessControl[user2, write]);

            SecurityContext.Unbind(todo2);
            todo1.AccessControl.Revoke(write, user1);

            Assert.True(todo2.AccessControl.IsUnique);
            Assert.True(todo2.AccessControl[user1, write]);
            Assert.True(todo2.AccessControl[user2, write]);
            Assert.False(todo1.AccessControl[user1, write]);
            Assert.True(todo1.AccessControl[user2, write]);
        }
    }
}
