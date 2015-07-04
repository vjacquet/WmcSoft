using WmcSoft.Security;

namespace WmcSoft.Security
{
    /// <summary>
    /// Registry of permissions.
    /// </summary>
    /// <remarks>The class is not static so it is possible to inherit from it.</remarks>
    public class Permissions
    {
        private Permissions() { }

        public static readonly Permission Read = new Permission("read");
        public static readonly Permission Write = new Permission("write");
        public static readonly Permission Delete = new Permission("delete");

        public static readonly Permission Delegate = new Permission("delegate");

        public static readonly Permission Execute = new Permission("execute");
        public static readonly Permission Abort = new Permission("abort");
    }
}
