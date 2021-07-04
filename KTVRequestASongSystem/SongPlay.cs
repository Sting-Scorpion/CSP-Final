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
    public partial class SongPlay : Form
    {
        string url = "";
        string picUrl = "";
        public SongPlay(string _url,string _picUrl)
        {
            InitializeComponent();
            url = _url;
            picUrl = _picUrl;
        }

        public void state()
        {
            axWindowsMediaPlayer1.URL = url;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            pictureBox1.Image = Image.FromStream(System.Net.WebRequest.Create(picUrl).GetResponse().GetResponseStream()); 
        }
    }
}
