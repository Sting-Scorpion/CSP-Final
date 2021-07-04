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
        string url = "";//音乐url
        string picUrl = "";//音乐图片url
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

            //picturnBox信息
            //pictureBox.Location = new Point(locationX + 250, locationY);
            pictureBox1.Location = new Point(24, 60);
            //图片大小
            pictureBox1.Size = new Size(290, 290);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBox1.Image = Image.FromStream(System.Net.WebRequest.Create(picUrl).GetResponse().GetResponseStream());

        }
    }
}
