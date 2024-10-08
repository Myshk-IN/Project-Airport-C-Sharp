using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.UserDataTools
{
    public enum LoginState
    {
        Blocked,
        Connected,
        NotExists,
        BadPassword,
        Exists,
        Error
    }
}
