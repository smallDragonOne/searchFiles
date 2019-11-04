using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SearchFile
{
    public partial class Form1 : Form
    {
        private readonly string FilePath = "file.txt";
        public Form1()
        {
            InitializeComponent();
            this.textBox2.Text = "1024";
            this.textBox3.Text = "*";
            this.button2.Hide();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button2.Hide();
            var dirPath = this.textBox1.Text;
            if (string.IsNullOrWhiteSpace(this.textBox2.Text))
            {
                MessageBox.Show("大小不能为空！");
                return;
            }
            decimal fileSize = 0;
            try
            {
                fileSize = Convert.ToDecimal(this.textBox2.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("请填写正确的数字！");
                return;
            }

            if (!Directory.Exists(dirPath))
            {
                MessageBox.Show("无效路径！");
                return;
            }

            this.textBox3.Text = this.textBox3.Text == "*" ? "" : this.textBox3.Text;

            string[] files = new string[] { };
            try
            {
                files = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            var fileQuery = from f in files.AsParallel()
                            let fileInfo = new FileInfo(f)
                            where fileInfo.Length >= fileSize * 1024 * 1024 && fileInfo.Extension.ToLower().Contains("." + this.textBox3.Text.ToLower())
                            select f;
            var fileList = fileQuery.ToList();
            if (fileList.Count == 0)
            {
                MessageBox.Show("没有找到符合的文件");
            }
            else
            {
                using (var w = new StreamWriter(FilePath))
                {
                    foreach (var item in fileList)
                    {
                        w.WriteLine(item);
                    }
                }
                this.button2.Show();
                MessageBox.Show("查询成功！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(FilePath))
            {
                MessageBox.Show("当前结果不存在！");
            }
            System.Diagnostics.Process.Start("notepad.exe", FilePath);
        }
    }
}
