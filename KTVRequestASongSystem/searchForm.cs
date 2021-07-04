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
        int state = 0; //当前状态 ：0 搜索 1 收藏
        int locationX = 0;
        int locationY = 30;
        //int sizeX = 316;
        //int sizeY = 60;

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
            this.panel_song.AutoScroll = true;

            //搜索页面下
            if (state == 0)
            {
                string keyword1st = "";
                string keyword2nd = "";
                string keyword3rd = "";

                string content = "";
                string content2nd = "";
                string content3rd = "";

                if (searchTxt.Text.Trim() == "")
                {
                    keyword1st = BLL.LiveSongBLL.findFavorite(out keyword2nd,out keyword3rd);

                    //根据收藏歌单判断最喜爱的歌手，并推荐该歌手的歌曲
                    content = Tool.HttpTool.Get($"/cloudsearch?keywords={keyword1st}&limit=30");

                    content2nd = Tool.HttpTool.Get($"/cloudsearch?keywords={keyword2nd}&limit=30");

                    content3rd = Tool.HttpTool.Get($"/cloudsearch?keywords={keyword3rd}&limit=30");

                    //调用静态类SongListBLL的方法，将数据转化为song对象数组，并绘制界面
                    List<songListModel> songListModels = SongListBLL.songList(content, 5);
                    List<songListModel> songListModels2nd = SongListBLL.songList(content2nd, 4);
                    List<songListModel> songListModels3rd = SongListBLL.songList(content3rd, 3);

                    songListModels.AddRange(songListModels2nd);
                    songListModels.AddRange(songListModels3rd);

                    showSongLists(songListModels);

                }
                else
                {
                    content = Tool.HttpTool.Get($"/cloudsearch?keywords={searchTxt.Text.Trim()}&limit=30");

                    //调用静态类SongListBLL的方法，将数据转化为song对象数组，并绘制界面
                    List<songListModel> songListModels = SongListBLL.songList(content,-1);
                    showSongLists(songListModels);
                }

                
            }
            //收藏页面下
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
        /// 绘制收藏界面
        /// </summary> 
        public void coolectsSong()
        {
            locationX = 0;
            locationY = 30;
            panel_song.Controls.Clear();
            label3.Text = "";
            foreach (var item in coolects)
            {
                string name = item.SongName + "_" + item.SongId;
                Label labelName = new Label();
                labelName.Text += item.author;
                labelName.Location = new Point(locationX + 40, locationY + 24);
                panel_song.Controls.Add(labelName);

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
                string urlsc = Application.StartupPath + "\\image\\" + "icon_收藏.png";
                if (count == 1)
                {
                    urlsc = Application.StartupPath + "\\image\\" + "icon_收藏（高亮）.png";
                }

                pictureBoxsc.Image = Image.FromFile(urlsc);
                panel_song.Controls.Add(pictureBoxsc);

                PictureBox pictureBox = new PictureBox();
                pictureBox.Click += PictureBox_Click;
                pictureBox.Name = name;
                pictureBox.Location = new Point(locationX + 250, locationY);
                pictureBox.Size = new Size(40, 30);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                //pictureBox.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + @"\image\音乐.png");
                pictureBox.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_播放.png");
                panel_song.Controls.Add(pictureBox);

                //歌名
                Label label = new Label();
                label.Text = item.SongName;
                label.Location = new Point(locationX + 40, locationY);
                label.Size = new Size(150, 20);
                panel_song.Controls.Add(label);

                //画线条
                Label labelx = new Label();
                labelx.ForeColor = Color.FromArgb(255, 186, 185, 185);
                labelx.Size = new Size(325, 30);
                labelx.Text = "__________________________________________________";
                labelx.Location = new Point(10, locationY + 40);
                panel_song.Controls.Add(labelx);

                locationY += 70;
            }
        }

        //绘制搜索界面
        public void showSongLists(List<songListModel> songListModels)
        {
            locationX = 0;
            locationY = 30;
            panel_song.Controls.Clear();
            label3.Text = "";

            foreach (var item in songListModels)
            {
                string name = item.Name + "_" + item.Id;

                //添加歌手名
                Label artistName = new Label();
                foreach (var item1 in item.ArModels)
                {
                    artistName.Text += item1.Name + " ";//添加歌手名
                }
                artistName.Location = new Point(locationX + 40, locationY + 24);
                artistName.Size = new Size(120, 20);
                panel_song.Controls.Add(artistName);

                name += "_" + artistName.Text + "_" + item.PicUrl;

                PictureBox pictureBoxsc = new PictureBox();
                pictureBoxsc.Name = name;//name由歌曲名 id 歌手名 和图片url组成
                pictureBoxsc.Click += PictureBoxsc_Click;
                pictureBoxsc.Location = new Point(locationX + 10, locationY);
                pictureBoxsc.Size = new Size(30, 20);
                pictureBoxsc.SizeMode = PictureBoxSizeMode.Zoom;

                //判断是否为已收藏歌曲
                int count = 0;
                foreach (var item1 in coolects)
                {
                    if (item1.SongId == item.Id)
                    {
                        count = 1;
                    }
                }

                //string urlsc = System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["未收藏图标"];
                string urlsc = Application.StartupPath + "\\image\\" + "icon_收藏.png";
                if (count == 1)
                {
                    //urlsc = System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["收藏图标"];
                    urlsc = Application.StartupPath + "\\image\\" + "icon_收藏（高亮）.png";
                }

                pictureBoxsc.Image = Image.FromFile(urlsc);
                panel_song.Controls.Add(pictureBoxsc);

                //添加歌名
                Label label = new Label();
                label.Text = item.Name;
                label.Location = new Point(locationX + 40, locationY);
                label.Size = new Size(150, 20);
                panel_song.Controls.Add(label);

                //添加播放音乐按钮
                PictureBox pictureBox = new PictureBox();
                pictureBox.Click += PictureBox_Click;
                pictureBox.Name = name;
                pictureBox.Location = new Point(locationX + 250, locationY);
                pictureBox.Size = new Size(40, 30);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                //pictureBox.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["音乐图标"]);
                pictureBox.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_播放.png");
                panel_song.Controls.Add(pictureBox);

                Label labelx = new Label();
                labelx.ForeColor = Color.FromArgb(255, 186, 185, 185);
                labelx.Size = new Size(325, 30);
                labelx.Text = "__________________________________________________";
                labelx.Location = new Point(10, locationY + 40);
                panel_song.Controls.Add(labelx);

                locationY += 70;


            }
        }

        /// <summary>
        /// 收藏（小星星）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBoxsc_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            string[] cont = pic.Name.Split('_');
            //合并了从cont[3]开始的后几项
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

            //判断是否收藏
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
                //pic.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["收藏图标"]);
                pic.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_收藏（高亮）.png");
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
                //将已收藏歌曲添加到数据库
                kTVDBEntities.Coolect.Add(coolect);
                kTVDBEntities.SaveChanges();

            }
            //取消收藏
            else
            {
                //pic.Image = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + BLL.ConfigPath.path["未收藏图标"]);
                pic.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_收藏.png");
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
                //Remove()将当前对象标记为可删除状态
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

        //点击发现tab
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            state = 0;
            label3.Text = "试试搜索你喜欢的歌曲和歌手吧~";
            panel_song.Controls.Clear();

            //“发现”二字的颜色高亮表示
            label1.ForeColor = Color.FromArgb(89, 136, 239);
            label2.ForeColor = Color.White;

            this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_发现（高亮）.png");
            this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_收藏.png");

        }

        //点击收藏tab
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            searchTxt.Text = "";
            panel_song.Controls.Clear();
            state = 1;
            //“收藏”二字的颜色高亮表示
            label2.ForeColor = Color.FromArgb(89, 136, 239);
            label1.ForeColor = Color.White;
            button1_Click(null, null);

            this.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_发现.png");
            this.pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\image\\" + "icon_收藏（高亮）.png");
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

        //添加到歌单
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

        //在搜索框内提示可能想搜索的歌曲 每一秒检测一次当前输入框内的内容
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (state == 0)
            {
                searchTxt.Items.Clear();
                string content = "";
                if (searchTxt.Text.Trim() != "")
                {
                    content = Tool.HttpTool.Get($"/cloudsearch?keywords={searchTxt.Text.Trim()}&limit=10");
                    List<songListModel> songListModels = SongListBLL.songList(content,-1);
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

        //返回开始界面
        private void pictureBox_return_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
