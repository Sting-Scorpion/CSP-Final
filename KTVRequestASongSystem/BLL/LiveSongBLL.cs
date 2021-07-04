using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.BLL
{
    public static class LiveSongBLL
    {
        public static string findFavorite(out string SingerName2nd, out string SingerName3rd)
        {
            Dictionary<string, int> favorSingerNames = new Dictionary<string, int>();
            KTVDBEntities kTVDBEntities = new KTVDBEntities();
            List<SingerLoveWatch> singerLoveWatches = (from c in kTVDBEntities.SingerLoveWatch
                                                       where c.UserName == Model.LoginDataModel.UserPhone
                                                       select c).ToList();

            //默认
            string SingerName1st = "周杰伦";
            //string SingerName2nd = "";

            //添加歌手名 与对应歌曲播放的次数
            foreach (var item in singerLoveWatches)
            {
                string[] name = item.Author.Trim().Split(' ');
                foreach (var itemName in name)
                {
                    if (!favorSingerNames.Keys.Contains(itemName))
                    {
                        favorSingerNames.Add(itemName, item.Number);
                    }
                    else
                    {
                        favorSingerNames[itemName] = favorSingerNames[itemName] + item.Number;
                    }
                }
            }

            /*
            //判断次数最多和次多的歌手名
            foreach (var item in favorSingerNames)
            {
                if (item.Value == favorSingerNames.Values.Max())
                {
                    SingerName = item.Key;
                }
            }
            */

            //按播放次数排序各个歌手
            var orderedSinger = from p in favorSingerNames orderby p.Value descending select p;
            SingerName1st = orderedSinger.First().Key;
            SingerName2nd = orderedSinger.ElementAt(1).Key;
            SingerName3rd = orderedSinger.ElementAt(2).Key;

            return SingerName1st;
        }
    }
}
