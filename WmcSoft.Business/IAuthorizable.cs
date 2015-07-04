using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public interface IAuthorizable
    {
        string AuthorizedBy { get; set; }
        DateTime? AuthorizedOn { get; set; }

        bool CanAuthorize();
        void Authorize();
        bool CanRevoke();
        void Revoke();
    }
}
