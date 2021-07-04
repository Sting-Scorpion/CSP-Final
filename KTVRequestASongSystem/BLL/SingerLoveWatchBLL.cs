using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.BLL
{
    public class SingerLoveWatchBLL
    {
        List<SingerLoveWatch> singerLoveWatches = null;
        KTVDBEntities kTVDBEntities = new KTVDBEntities();

        public SingerLoveWatchBLL(string songId, string SongName, string Author)
        {
            singerLoveWatches = (from c in kTVDBEntities.SingerLoveWatch
                                 where c.UserName == Model.LoginDataModel.UserPhone && c.SongId == songId
                                 select c).ToList();
            if (singerLoveWatches.Count == 0)
            {
                SingerLoveWatch singerLoveWatch = new SingerLoveWatch()
                {
                    Author = Author,
                    UserName = Model.LoginDataModel.UserPhone,
                    Number = 1,
                    SongId = songId,
                    SongName = SongName
                };
                kTVDBEntities.SingerLoveWatch.Add(singerLoveWatch);
                kTVDBEntities.SaveChanges();
            }
            else
            {
                SingerLoveWatch singerLoveWatch = singerLoveWatches[0];
                singerLoveWatch.Number += 1;
                kTVDBEntities.Entry<SingerLoveWatch>(singerLoveWatch).State = EntityState.Modified;
                kTVDBEntities.SaveChanges();
            }
        }
    }
}
