using KTVRequestASongSystem.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.BLL
{
    public static class songUrlDataBLL
    {

        public static songUrlDataModel songUrlData(string JsonData)
        {
            List<songUrlDataModel> songUrlDataModels = new List<songUrlDataModel>();
            JObject jArray = JObject.Parse(JsonData);
            foreach (var item in jArray["data"])
            {
                songUrlDataModels.Add(new songUrlDataModel()
                {
                    Id = int.Parse(item["id"].ToString()),
                    Url = item["url"].ToString(),
                    Md5 = item["md5"].ToString(),
                    EncodeType = item["encodeType"].ToString()
                });
            }

            return songUrlDataModels[0];
        }
    }
}
