using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.BLL
{
    public static class ConfigPath
    {
        public static Dictionary<string, string> path = new Dictionary<string, string>();

        public static void iniConfigPath()
        {
            path.Clear();
            KTVDBEntities kTVDBEntities = new KTVDBEntities();
            var configPaths = (from c in kTVDBEntities.ConfigPath
                               select c).ToList();
            foreach (var item in configPaths)
            {
                path.Add(item.PathName, item.PathSite);
            }
        }
    }
}
