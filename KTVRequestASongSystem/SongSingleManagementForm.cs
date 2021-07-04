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
    public partial class SongSingleManagementForm : Form
    {
        List<SongSingleWatch> _list = null;
        KTVDBEntities _KtvDB = new KTVDBEntities();
        public SongSingleManagementForm()
        {
            InitializeComponent();
            ini();
        }

        void ini()
        {
            _list = (from c in _KtvDB.SongSingleWatch
                     where c.UserName == Model.LoginDataModel.UserPhone
                     select c).ToList();
            dataGridView1.DataSource = _list;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SongSingleWatch songSingleWatch = new SongSingleWatch()
            {
                SongSingleName = AddSongSingleText.Text.Trim(),
                UserName = Model.LoginDataModel.UserPhone
            };
            _KtvDB.SongSingleWatch.Add(songSingleWatch);
            _KtvDB.SaveChanges();
            MessageBox.Show("添加成功");
            ini();
            AddSongSingleText.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            var user1 = _KtvDB.SongSingleWatch.Find(id);
            _KtvDB.SongSingleWatch.Remove(user1);
            _KtvDB.SaveChanges();
            MessageBox.Show("删除成功");
            ini();
        }

        SongSingleSongListDataForm songSingleSongListDataForm = null;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (songSingleSongListDataForm == null)
            {
                songSingleSongListDataForm = new SongSingleSongListDataForm(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                songSingleSongListDataForm.Show();
                songSingleSongListDataForm.FormClosed += SongSingleSongListDataForm_FormClosed;
            }
        }

        private void SongSingleSongListDataForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            songSingleSongListDataForm.FormClosed -= SongSingleSongListDataForm_FormClosed;
            songSingleSongListDataForm = null;
        }
    }
}
