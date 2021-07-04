using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTVRequestASongSystem.Model
{
    public class songUrlDataModel
    {
        private int id;
        private string url;
        private string md5;
        private string encodeType;

        public int Id { get => id; set => id = value; }
        public string Url { get => url; set => url = value; }
        public string Md5 { get => md5; set => md5 = value; }
        public string EncodeType { get => encodeType; set => encodeType = value; }
    }
}
