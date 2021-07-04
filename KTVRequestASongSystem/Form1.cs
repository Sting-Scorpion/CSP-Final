using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KTVRequestASongSystem.Model;
using NeteaseCloudMusicApi;

namespace KTVRequestASongSystem
{
    //登录页
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Admin> _list = null;
        KTVDBEntities _KtvDB = new KTVDBEntities();
        CloudMusicApi api = new CloudMusicApi();


        //账号密码历史记录
        private void Form1_Load(object sender, EventArgs e)
        {

            _list = (from c in _KtvDB.Admin
                     select c).ToList();

            foreach (var item in _list)
            {
                if (!userName.Items.Contains(item.UserName))
                {
                    userName.Items.Add(item.UserName);
                }
            }
        }

        //登录
        private void button1_Click(object sender, EventArgs e)
        {
            string LoginContent = Tool.HttpTool.Get(string.Format($"/login/cellphone?phone={userName.Text.Trim()}&password={PassWord.Text.Trim()}"));
            if (LoginContent.Contains("密码错误"))
                MessageBox.Show("密码错误");
            else
            {
                LoginModel loginModel = BLL.LoginBLL.loginModelData(LoginContent);
                if (LoginContent.Contains("密码错误"))
                {
                    MessageBox.Show("密码错误");
                }
                else
                {
                    MessageBox.Show("登录成功");
                    Model.LoginDataModel.UserPhone = userName.Text.Trim();
                    Model.LoginDataModel.LoginName = loginModel.Nickname;
                    MainForm mainFrom = new MainForm();
                    mainFrom.Show();
                    this.Hide();

                    _list = (from c in _KtvDB.Admin
                             where c.UserName == userName.Text.Trim()
                             select c).ToList();
                    if (_list.Count == 0)
                    {
                        //当数据库中不包含该用户的时候添加用户
                        Admin admin = new Admin()
                        {
                            UserName = userName.Text.Trim(),
                            PassWord = PassWord.Text.Trim(),
                        };
                        _KtvDB.Admin.Add(admin);
                        _KtvDB.SaveChanges();
                    }
                    else //当用户存在但当密码被修改的时候修改数据库中的密码
                    {
                        if (_list[0].PassWord != PassWord.Text.Trim())
                        {
                            var user5 = _KtvDB.Admin.FirstOrDefault(p => p.ID == _list[0].ID);
                            if (user5 != null)
                            {
                                user5.PassWord = PassWord.Text.Trim();
                                _KtvDB.SaveChanges();
                            }
                            MessageBox.Show("修改成功");
                            this.Close();
                        }
                    }
                }
            }
        }


        private void userName_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var item in _list)
            {
                if (item.UserName == userName.Text.Trim())
                {
                    PassWord.Text = item.PassWord;
                }
            }
        }
    }
}
