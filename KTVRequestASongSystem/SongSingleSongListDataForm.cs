using KTVRequestASongSystem.BLL;
using KTVRequestASongSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTVRequestASongSystem
{
    public partial class SongSingleSongListDataForm : Form
    {
        int locationX = 0;
        int locationY = 30;
        string SongSingleName = "";
        KTVDBEntities kTVDBEntities = new KTVDBEntities();
        List<SongSingleSongData> coolects = null;
        songUrlDataModel songUrlDataModels = null;
        string backImage = "";
        SongPlay songPlay = null;

        public SongSingleSongListDataForm(string _SongSingleName)
        {
            InitializeComponent();
            SongSingleName = _SongSingleName;
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (searchTxt.Text.Trim() == "")
            {
                coolects = (from c in kTVDBEntities.SongSingleSongData
                            where c.UserName == Model.LoginDataModel.UserPhone && c.SongSingleName == SongSingleName 
                            select c).ToList();
                coolectsSong();
            }
            else
            {
                coolects = (from c in kTVDBEntities.SongSingleSongData
                            where c.UserName == Model.LoginDataModel.UserPhone && c.SongSingleName == SongSingleName && (c.SongName.Contains(searchTxt.Text) || c.author.Contains(searchTxt.Text))
                            select c).ToList();
                coolectsSong();
            }
        }

        /// <summary>
        /// 收藏画界面
        /// </summary> 
        public void coolectsSong()
        {
            locationX = 0;
            locationY = 30;
            panel3.Controls.Clear();
            foreach (var item in coolects)
            {
                string name = item.SongName + "_" + item.SongId;
                Label labelName = new Label();
                labelName.Text += item.author;
                labelName.Location = new Point(locationX + 40, locationY + 24);
                panel3.Controls.Add(labelName);

                name += "_" + labelName.Text + "_" + item.BackImage;

                PictureBox pictureBoxsc = new PictureBox();
                pictureBoxsc.Name = name;
                pictureBoxsc.Click += PictureBoxsc_Click1; ;
                pictureBoxsc.Location = new Point(locationX + 10, locationY);
                pictureBoxsc.Size = new Size(30, 20);
                pictureBoxsc.SizeMode = PictureBoxSizeMode.Zoom;

                string urlsc = System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["喜欢图标"];

                pictureBoxsc.Image = Image.FromFile(urlsc);
                panel3.Controls.Add(pictureBoxsc);

                PictureBox pictureBox = new PictureBox();
                pictureBox.Click += PictureBoxsc_Click;
                pictureBox.Name = name;
                pictureBox.Location = new Point(locationX + 250, locationY);
                pictureBox.Size = new Size(40, 30);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["音乐图标"]);
                panel3.Controls.Add(pictureBox);

                //歌名
                Label label = new Label();
                label.Text = item.SongName;
                label.Location = new Point(locationX + 40, locationY);
                panel3.Controls.Add(label);

                Label labelx = new Label();
                labelx.Size = new Size(325, 30);
                labelx.Text = "____________________________________________________";
                labelx.Location = new Point(0, locationY + 40);
                panel3.Controls.Add(labelx);

                locationY += 70;
            }
        }

        private void PictureBoxsc_Click1(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            //判断是否收藏
            string[] cont = pic.Name.Split('_');

            int id = int.Parse(cont[1]);
            List<SongSingleSongData> songSingleSongDatas = (from c in kTVDBEntities.SongSingleSongData
                                                            where c.UserName == Model.LoginDataModel.UserPhone && c.SongSingleName == SongSingleName && c.SongId == id
                                                            select c).ToList();
            kTVDBEntities.SongSingleSongData.Attach(songSingleSongDatas[0]);
            //Remove()起到了标记当前对象为删除状态，可以删除
            kTVDBEntities.SongSingleSongData.Remove(songSingleSongDatas[0]);
            kTVDBEntities.SaveChanges();
            button1_Click(null, null);
        }

        private void PictureBoxsc_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            string[] cont = pic.Name.Split('_');
            gm.Text = cont[0];
            gs.Text = cont[2];
            if (cont.Length > 4)
            {
                string urlAdd = "";
                for (int i = 3; i < cont.Length; i++)
                {
                    if (i == cont.Length - 1)
                    {
                        urlAdd += cont[i];
                    }
                    else
                    {
                        urlAdd += cont[i] + "_";
                    }
                }
                cont[3] = urlAdd;
            }

            string songData = Tool.HttpTool.Get($"/song/url?id={cont[1]}");
            songUrlDataModels = songUrlDataBLL.songUrlData(songData);
            songPlay = new SongPlay(songUrlDataModels.Url, cont[3]);
            backImage = cont[3];
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                string savePath = BLL.ConfigPath.path["音乐保存路径"] + gm.Text + "_" + gs.Text + "." + Path.GetFileName(songUrlDataModels.Url).Split('.')[1];
                bool x = Tool.HttpTool.HttpDownload(songUrlDataModels.Url, savePath);
                if (x == true)
                {
                    MessageBox.Show("下载成功");
                    LocalSavePathWatch localSavePathWatch = new LocalSavePathWatch()
                    {
                        Author = gs.Text,
                        LocalSavePath = savePath,
                        SongID = songUrlDataModels.Id.ToString(),
                        SongName = gm.Text,
                        BackImage = backImage
                    };
                    kTVDBEntities.LocalSavePathWatch.Add(localSavePathWatch);
                    kTVDBEntities.SaveChanges();
                }
                else
                {
                    MessageBox.Show("下载失败");
                }
            }
            catch
            {
                
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try
            {
                songPlay.Show();
                songPlay.state();
                SingerLoveWatchBLL singerLoveWatch = new SingerLoveWatchBLL(songUrlDataModels.Id.ToString(), gm.Text, gs.Text);
            }
            catch
            {

            }
        }
    }
}
