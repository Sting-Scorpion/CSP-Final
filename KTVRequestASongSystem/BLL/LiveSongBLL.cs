using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.BLL
{
    public static class LiveSongBLL
    {
        /// <summary>
        /// 分析出该用户最喜欢的歌手
        /// </summary>
        public static string song()
        {
            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
            KTVDBEntities kTVDBEntities = new KTVDBEntities();
            List<SingerLoveWatch> singerLoveWatches = (from c in kTVDBEntities.SingerLoveWatch
                                                       where c.UserName == Model.LoginDataModel.UserPhone
                                                       select c).ToList();
            string SingerName = "周杰伦";
            //添加姓名
            foreach (var item in singerLoveWatches)
            {
                string[] name = item.Author.Trim().Split(' ');
                foreach (var itemName in name)
                {
                    if (!keyValuePairs.Keys.Contains(itemName))
                    {
                        keyValuePairs.Add(itemName, item.Number);
                    }
                    else
                    {
                        keyValuePairs[itemName] = keyValuePairs[itemName] + item.Number;
                    }
                }
            }
            
            //判断次数最多的歌手名
            foreach (var item in keyValuePairs)
            {
                if(item.Value == keyValuePairs.Values.Max())
                {
                    SingerName = item.Key;
                }
            }

            return SingerName;
        }
    }
}
