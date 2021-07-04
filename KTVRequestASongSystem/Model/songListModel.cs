using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.Model
{
    /// <summary>
    /// 歌单
    /// </summary>
    public class songListModel
    {
        private string name;
        private int id;
        private List<arModel> arModels;
        private string picUrl;
   
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public List<arModel> ArModels { get => arModels; set => arModels = value; }
        public string PicUrl { get => picUrl; set => picUrl = value; }
    }
}
