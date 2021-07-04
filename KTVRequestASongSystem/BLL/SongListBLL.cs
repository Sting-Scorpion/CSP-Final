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
    /// 获取歌单
    /// </summary>
    public static class SongListBLL
    {
        public static List<songListModel> songList(string JsonData,int showCount)
        {
            //将json数据转化为song对象
            List<songListModel> songListModels = new List<songListModel>();
            JObject jArray = JObject.Parse(JsonData);
            int temp = 0;

            foreach (var item in jArray["result"]["songs"])
            {
                //利用showCount参数，实现解析指定个数的对象
                if(showCount == -1 || temp < showCount)
                {
                    songListModels.Add(new songListModel()
                    {
                        Name = item["name"].ToString(),
                        Id = int.Parse(item["id"].ToString()),
                        ArModels = ArModels(item),
                        PicUrl = item["al"]["picUrl"].ToString()
                    });
                    temp++;
                }
                else
                {
                    //结束
                    break;
                }
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
