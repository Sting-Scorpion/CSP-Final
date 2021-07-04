using KTVRequestASongSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KTVRequestASongSystem.BLL
{
    public static class LoginBLL
    {
        public static LoginModel loginModelData(string JsonData)
        {
            JObject jArray = JObject.Parse(JsonData);
            LoginModel loginModel = new LoginModel()
            {
                Id = jArray["account"]["id"].ToString(),
                UserName = jArray["account"]["userName"].ToString(),
                Cookie = jArray["cookie"].ToString(),
                Token = jArray["token"].ToString(),
                Nickname = jArray["profile"]["nickname"].ToString()
            };

            return loginModel;
        }
    }
}
