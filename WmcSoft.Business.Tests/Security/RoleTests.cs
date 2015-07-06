using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Security
{
    [TestClass]
    public class RoleTests
    {
        [TestMethod]
        public void CanAssignPermissions() {
            var read = Permissions.Read;
            var write = Permissions.Write;

            var everyone = Groups.Everyone;
            var storage = new Role("storage", CommandPermissions.FileOpen, CommandPermissions.FileSave);

            var fileOpen = new FileOpenCommand();
            fileOpen.Grant(storage, everyone);

            Assert.IsTrue(fileOpen.AccessControl[everyone, CommandPermissions.FileOpen]);
            Assert.IsTrue(fileOpen.AccessControl[everyone, CommandPermissions.FileSave]);
            Assert.IsFalse(fileOpen.AccessControl[everyone, read]);
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
