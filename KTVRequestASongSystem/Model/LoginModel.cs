using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.Model
{
    public class LoginModel
    {

        private string cookie;
        private string id;
        private string userName;
        private string nickname; //用户名
        private string token; 
        
        public string Cookie{ get => cookie; set => cookie = value;}
        public string Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string Token { get => token; set => token = value; }
    }
}
