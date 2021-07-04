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
    public partial class ConfigurationFrom : Form
    {
        KTVDBEntities kTVDBEntities = new KTVDBEntities();

        public ConfigurationFrom()
        {
            InitializeComponent();
            BLL.ConfigPath.iniConfigPath();
            ini();
        }

        void ini()
        {
            textBox1.Text = BLL.ConfigPath.path["音乐保存路径"].ToString();
            textBox3.Text = BLL.ConfigPath.path["未收藏图标"].ToString();
            textBox4.Text = BLL.ConfigPath.path["收藏图标"].ToString();
            textBox5.Text = BLL.ConfigPath.path["喜欢图标"].ToString();
            textBox2.Text = BLL.ConfigPath.path["音乐图标"].ToString();
        }

        //保存
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            List<ConfigPath> configPaths = (from c in kTVDBEntities.ConfigPath
                                            select c).ToList();
            foreach (var item in configPaths)
            {
                if (item.PathName == "音乐保存路径")
                {
                    item.PathSite = textBox1.Text;
                }
                else if (item.PathName == "未收藏图标")
                {
                    item.PathSite = textBox3.Text;
                }
                else if (item.PathName == "收藏图标")
                {
                    item.PathSite = textBox4.Text;
                }
                else if (item.PathName == "喜欢图标")
                {
                    item.PathSite = textBox5.Text;
                }
                else if (item.PathName == "音乐图标")
                {
                    item.PathSite = textBox2.Text;
                }
            }
            foreach (var item in configPaths)
            {
                //2.标识为修改
                kTVDBEntities.Entry<ConfigPath>(item).State = System.Data.Entity.EntityState.Modified;
                //3.保存到数据库
                kTVDBEntities.SaveChanges();
            }

            MessageBox.Show("保存成功");
        }
    }
}
