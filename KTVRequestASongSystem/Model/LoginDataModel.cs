using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.Model
{
    public static class LoginDataModel
    {
        private static string userPhone;
        private static string loginName;

        public static string UserPhone { get => userPhone; set => userPhone = value; }
        public static string LoginName { get => loginName; set => loginName = value; }
    }
}
