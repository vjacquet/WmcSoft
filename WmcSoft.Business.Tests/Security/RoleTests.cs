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

using Xunit;

namespace WmcSoft.Security
{
    public class RoleTests
    {
        [Fact]
        public void CanAssignPermissions()
        {
            var read = Permissions.Read;
            var write = Permissions.Write;

            var everyone = Groups.Everyone;
            var storage = new Role("storage", CommandPermissions.FileOpen, CommandPermissions.FileSave);

            var fileOpen = new FileOpenCommand();
            fileOpen.Grant(storage, everyone);

            Assert.True(fileOpen.AccessControl[everyone, CommandPermissions.FileOpen]);
            Assert.True(fileOpen.AccessControl[everyone, CommandPermissions.FileSave]);
            Assert.False(fileOpen.AccessControl[everyone, read]);
        }
    }

    public class FileOpenCommand : Command
    {
        public override PermissionSet SupportedPermissions {
            get {
                return CommandPermissions.FileOpen;
            }
        }
    }

    public class FileSaveCommand : Command
    {
        public override PermissionSet SupportedPermissions {
            get {
                return CommandPermissions.FileSave;
            }
        }
    }

    public class CommandPermissions : Permissions
    {
        protected CommandPermissions() { }

        public static readonly Permission FileOpen = new Permission("File.Open");
        public static readonly Permission FileSave = new Permission("File.Save");
    }
}