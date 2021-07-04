using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTVRequestASongSystem
{
    public partial class SongSingleForm : Form
    {
        List<SongSingleWatch> _list = null;
        KTVDBEntities _KtvDB = new KTVDBEntities();
        string SongName = "";
        int SongId = 0;
        string author = "";
        string NetWorkSite = "";
        string BackImage = "";

        public SongSingleForm(string _SongName, int _SongId, string _author, string _NetWorkSite, string _BackImage)
        {
            InitializeComponent();
            ini();
            SongName = _SongName;
            SongId = _SongId;
            author = _author;
            NetWorkSite = _NetWorkSite;
            BackImage = _BackImage;
        }

        void ini()
        {
            _list = (from c in _KtvDB.SongSingleWatch
                     where c.UserName == Model.LoginDataModel.UserPhone
                     select c).ToList();

            foreach (var item in _list)
            {
                if (!SongSingleName.Items.Contains(item.SongSingleName))
                {
                    SongSingleName.Items.Add(item.SongSingleName);
                }
            }

            SongSingleName.SelectedIndex = 0;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            List<SongSingleSongData> songSingleSongDatas = (from c in _KtvDB.SongSingleSongData
                                                            where c.UserName == Model.LoginDataModel.UserPhone && c.SongSingleName == SongSingleName.Text.Trim() && c.SongId == SongId
                                                            select c).ToList();
            if (songSingleSongDatas.Count == 0)
            {
                SongSingleSongData songSingleSongData = new SongSingleSongData()
                {
                    SongSingleName = SongSingleName.Text,
                    NetWorkSite = NetWorkSite,
                    author = author,
                    BackImage = BackImage,
                    SongId = SongId,
                    SongName = SongName,
                    UserName = Model.LoginDataModel.UserPhone
                };
                _KtvDB.SongSingleSongData.Add(songSingleSongData);
                _KtvDB.SaveChanges();
                MessageBox.Show("添加成功");
                this.Close();
            }
            else {
                MessageBox.Show("添加成功");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
