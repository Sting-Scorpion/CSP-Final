using KTVRequestASongSystem.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.BLL
{
    /// <summary>
    /// 获取歌曲列表
    /// </summary>
    public static class SongListBLL
    {
        public static List<songListModel> songList(string JsonData)
        {
            List<songListModel> songListModels = new List<songListModel>();
            JObject jArray = JObject.Parse(JsonData);
            foreach (var item in jArray["result"]["songs"])
            {
                songListModels.Add(new songListModel()
                {
                    Name = item["name"].ToString(),
                    Id = int.Parse(item["id"].ToString()),
                    ArModels = ArModels(item),
                    PicUrl = item["al"]["picUrl"].ToString()
                });
            }

            return songListModels;
        }

        public static List<arModel> ArModels(JToken jToken)
        {
            List<arModel> arModels = new List<arModel>();
            foreach (var item in jToken["ar"])
            {
                arModels.Add(new arModel()
                {
                    Id = int.Parse(item["id"].ToString()),
                    Name = item["name"].ToString()
                });
            }
            return arModels;
        }
    }
}
