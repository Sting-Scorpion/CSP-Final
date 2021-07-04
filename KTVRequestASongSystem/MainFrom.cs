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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            BLL.ConfigPath.iniConfigPath();
        }

        SongSingleManagementForm songSingleManagementForm = null;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SongSingleManagementForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            songSingleManagementForm.FormClosed -= SongSingleManagementForm_FormClosed;
            songSingleManagementForm = null;
        }

        //歌单
        private void panel1_Click(object sender, EventArgs e)
        {
            if (songSingleManagementForm == null)
            {
                songSingleManagementForm = new SongSingleManagementForm();
                songSingleManagementForm.Show();
                songSingleManagementForm.FormClosed += SongSingleManagementForm_FormClosed;
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }

        private void _searchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _searchForm.FormClosed -= _searchForm_FormClosed;
            _searchForm = null;
        }


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ConfigurationFrom_FormClosed(object sender, FormClosedEventArgs e)
        {
            configurationFrom.FormClosed -= ConfigurationFrom_FormClosed;
            configurationFrom = null;
        }

        //配置
        ConfigurationFrom configurationFrom = null;
        private void panel3_Click(object sender, EventArgs e)
        {
            if (configurationFrom == null)
            {
                configurationFrom = new ConfigurationFrom();
                configurationFrom.Show();
                configurationFrom.FormClosed += ConfigurationFrom_FormClosed;
            }
        }

        searchForm _searchForm = null;

        //搜索歌曲
        private void panel4_Click(object sender, EventArgs e)
        {
            
            if (_searchForm == null)
            {
                _searchForm = new searchForm();
                _searchForm.Show();
                _searchForm.FormClosed += _searchForm_FormClosed;
            }
        }
    }
}
