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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTVRequestASongSystem
{
    public partial class searchForm : Form
    {
        int state = 0; //当前状态 0 搜索 1 收藏
        int locationX = 0;
        int locationY = 30;
        int sizeX = 316;
        int sizeY = 60;

        KTVDBEntities kTVDBEntities = new KTVDBEntities();
        List<Coolect> coolects = null;
        songUrlDataModel songUrlDataModels = null;
        string backImage = "";
        SongPlay songPlay = null;

        public searchForm()
        {
            InitializeComponent();
            coolects = (from c in kTVDBEntities.Coolect
                        where c.UserName == Model.LoginDataModel.UserPhone
                        select c).ToList();

            button1_Click(null, null);
        }

        //搜索
        private void button1_Click(object sender, EventArgs e)
        {
            this.panel3.AutoScroll = true;
            if (state == 0)
            {
                string content = "";
                if (searchTxt.Text.Trim() == "")
                {
                    content = Tool.HttpTool.Get($"/cloudsearch?keywords={BLL.LiveSongBLL.song()}&limit=30");
                }
                else
                {
                    content = Tool.HttpTool.Get($"/cloudsearch?keywords={searchTxt.Text.Trim()}&limit=30");
                }

                List<songListModel> songListModels = SongListBLL.songList(content);
                interfaceSong(songListModels);
            }
            else
            {
                if (searchTxt.Text.Trim() == "")
                {
                    coolects = (from c in kTVDBEntities.Coolect
                                where c.UserName == Model.LoginDataModel.UserPhone
                                select c).ToList();
                    coolectsSong();
                }
                else
                {
                    coolects = (from c in kTVDBEntities.Coolect
                                where c.UserName == Model.LoginDataModel.UserPhone && (c.SongName.Contains(searchTxt.Text) || c.author.Contains(searchTxt.Text))
                                select c).ToList();
                    coolectsSong();
                }
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
            label3.Text = "";
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
                pictureBoxsc.Click += PictureBoxsc_Click;
                pictureBoxsc.Location = new Point(locationX + 10, locationY);
                pictureBoxsc.Size = new Size(30, 20);
                pictureBoxsc.SizeMode = PictureBoxSizeMode.Zoom;
                int count = 0;
                foreach (var item1 in coolects)
                {
                    if (item1.SongId == item.SongId)
                    {
                        count = 1;
                    }
                }
                string urlsc = System.IO.Directory.GetCurrentDirectory() + @"\image\收藏_线.png";
                if (count == 1)
                {
                    urlsc = System.IO.Directory.GetCurrentDirectory() + @"\image\收藏_面.png";
                }

                pictureBoxsc.Image = Image.FromFile(urlsc);
                panel3.Controls.Add(pictureBoxsc);

                PictureBox pictureBox = new PictureBox();
                pictureBox.Click += PictureBox_Click;
                pictureBox.Name = name;
                pictureBox.Location = new Point(locationX + 250, locationY);
                pictureBox.Size = new Size(40, 30);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + @"\image\音乐.png");
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

        /// <summary>
        /// 画界面
        /// </summary>
        /// <param name="songListModels"></param>
        public void interfaceSong(List<songListModel> songListModels)
        {
            locationX = 0;
            locationY = 30;
            panel3.Controls.Clear();
            label3.Text = "";
            foreach (var item in songListModels)
            {
                string name = item.Name + "_" + item.Id;
                Label labelName = new Label();
                foreach (var item1 in item.ArModels)
                {
                    labelName.Text += item1.Name + " ";
                }
                labelName.Location = new Point(locationX + 40, locationY + 24);
                panel3.Controls.Add(labelName);

                name += "_" + labelName.Text + "_" + item.PicUrl;

                PictureBox pictureBoxsc = new PictureBox();
                pictureBoxsc.Name = name;
                pictureBoxsc.Click += PictureBoxsc_Click;
                pictureBoxsc.Location = new Point(locationX + 10, locationY);
                pictureBoxsc.Size = new Size(30, 20);
                pictureBoxsc.SizeMode = PictureBoxSizeMode.Zoom;
                int count = 0;
                foreach (var item1 in coolects)
                {
                    if (item1.SongId == item.Id)
                    {
                        count = 1;
                    }
                }

                string urlsc = System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["未收藏图标"];
                if (count == 1)
                {
                    urlsc = System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["收藏图标"];
                }

                pictureBoxsc.Image = Image.FromFile(urlsc);
                panel3.Controls.Add(pictureBoxsc);

                Label label = new Label();
                label.Text = item.Name;
                label.Location = new Point(locationX + 40, locationY);
                panel3.Controls.Add(label);

                PictureBox pictureBox = new PictureBox();
                pictureBox.Click += PictureBox_Click;
                pictureBox.Name = name;
                pictureBox.Location = new Point(locationX + 250, locationY);
                pictureBox.Size = new Size(40, 30);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                pictureBox.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["音乐图标"]);
                panel3.Controls.Add(pictureBox);

                Label labelx = new Label();
                labelx.Size = new Size(325, 30);
                labelx.Text = "____________________________________________________";
                labelx.Location = new Point(0, locationY + 40);
                panel3.Controls.Add(labelx);

                locationY += 70;
            }
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBoxsc_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            //判断是否收藏
            string[] cont = pic.Name.Split('_');
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

            int count = 0;
            foreach (var item1 in coolects)
            {
                if (item1.SongId == int.Parse(cont[1]))
                {
                    count = 1;
                }
            }

            if (count == 0)
            {
                pic.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["收藏图标"]);
                Coolect coolect = new Coolect()
                {
                    SongId = int.Parse(cont[1]),
                    SongName = cont[0],
                    LocalitySite = "",
                    NetWorkSite = $"https://autumnfish.cn//song/url?id={cont[1]}",
                    author = cont[2],
                    BackImage = cont[3],
                    UserName = Model.LoginDataModel.UserPhone
                };
                kTVDBEntities.Coolect.Add(coolect);
                kTVDBEntities.SaveChanges();
            }
            else
            {
                pic.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["未收藏图标"]);
                Coolect user = null;
                foreach (var item in coolects)
                {
                    if (item.SongId == int.Parse(cont[1]))
                    {
                        user = item;
                    }
                }
                //将要删除的对象附加到EF容器中
                kTVDBEntities.Coolect.Attach(user);
                //Remove()起到了标记当前对象为删除状态，可以删除
                kTVDBEntities.Coolect.Remove(user);
                kTVDBEntities.SaveChanges();
            }

            //刷新
            coolects = (from c in kTVDBEntities.Coolect
                        where c.UserName == Model.LoginDataModel.UserPhone
                        select c).ToList();

            if (state == 1)
            {
                button1_Click(null, null);
            }
        }

        /// <summary>
        /// 点击播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_Click(object sender, EventArgs e)
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

        //发现
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            state = 0;
            label3.Text = "试试搜索你喜欢的歌曲和歌手吧~";
            panel3.Controls.Clear();
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.White;
        }

        //收藏
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            searchTxt.Text = "";
            panel3.Controls.Clear();
            state = 1;
            label2.ForeColor = Color.Black;
            label1.ForeColor = Color.White;
            button1_Click(null, null);
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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        SongSingleForm songSingleForm = null;
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            try
            {
                if (songSingleForm == null)
                {
                    songSingleForm = new SongSingleForm(gm.Text, songUrlDataModels.Id, gs.Text, songUrlDataModels.Url, backImage);
                    songSingleForm.Show();
                    songSingleForm.FormClosed += SongSingleForm_FormClosed;
                }
            }
            catch
            {

            }
        }

        private void SongSingleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            songSingleForm.FormClosed -= SongSingleForm_FormClosed;
            songSingleForm = null;
        }
        

        private void searchTxt_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (state == 0)
            {
                searchTxt.Items.Clear();
                string content = "";
                if (searchTxt.Text.Trim() != "")
                {
                    content = Tool.HttpTool.Get($"/cloudsearch?keywords={searchTxt.Text.Trim()}&limit=10");
                    List<songListModel> songListModels = SongListBLL.songList(content);
                    foreach (var item in songListModels)
                    {
                        if (!searchTxt.Items.Contains(item.Name))
                        {
                            searchTxt.Items.Add(item.Name);
                        }
                    }
                }
                searchTxt.SelectionStart = searchTxt.Text.Length;
            }
        }

        private void pictureBox_return_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
