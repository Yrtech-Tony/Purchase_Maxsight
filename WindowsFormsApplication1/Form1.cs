using Microsoft.Office.Interop.Excel;
using Purchase.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XHX.Common;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Host = ConfigurationSettings.AppSettings["EmailHost"];
            int Port = int.Parse(ConfigurationSettings.AppSettings["EmailPort"]);
            string UserName = ConfigurationSettings.AppSettings["EmailUserName"];
            string Password = ConfigurationSettings.AppSettings["EmailPassword"];
            string MailFrom = ConfigurationSettings.AppSettings["EmailMailFrom"];

            UseNetMail netMail = new UseNetMail();
            netMail.CreateHost(new ConfigHost()
            {
                EnableSsl = false,
                Username = UserName,
                Password = Password,
                Port = 587,
                Server = Host,
            });
            netMail.CreateMultiMail(new ConfigMail()
            {
                From = MailFrom,
                To = "506011382@qq.com".Split(','),
                Subject = "暗访需求书",
                Body = "需求书<br>需求书",
                Resources = @"gaolei-emailSign.png".Split(','),
                Attachments = null
            });

            netMail.SendMail();
            MessageBox.Show("发送成功！");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\Users\huohaitao\Desktop\导入需求书";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var db = new PurchaseEntities())
                {
                    foreach (string filename in openFileDialog1.FileNames)
                    {
                        textBox1.Text = filename;
                        MSExcelUtil msExcelUtil = new MSExcelUtil();
                        Workbook workbook = msExcelUtil.OpenExcelByMSExcel(filename);
                        Worksheet sheet = workbook.ActiveSheet;
                        int rowIndex = 2;
                        string type = null;
                        do
                        {
                            type = msExcelUtil.GetCellValue(sheet, 1, rowIndex);
                            string content = msExcelUtil.GetCellValue(sheet, 2, rowIndex);
                            string childChk = msExcelUtil.GetCellValue(sheet, 3, rowIndex);
                            if (string.IsNullOrEmpty(content)) continue;

                            string[] items = content.Split('/');
                            foreach (string item in items)
                            {
                                HiddenCode code = new HiddenCode();
                                code.Type = type.Trim();
                                code.Code = item.Trim();
                                code.Name = item.Trim();
                                if (item == "不限制" || item == "不涉及")
                                {
                                    code.InDateTime = DateTime.MaxValue;
                                }
                                else
                                {
                                    code.InDateTime = DateTime.Now;
                                }
                                code.InUserId = "sysadmin";
                                code.UseChk = true;

                                if (db.HiddenCode.Find(type, item) == null)
                                {
                                    db.HiddenCode.Add(code);
                                }
                                if (db.HiddenCodeType.Where(x => x.TypName == code.Type).Count() == 0)
                                {
                                    db.HiddenCodeType.Add(new HiddenCodeType()
                                    {
                                        InDateTime = DateTime.Now,
                                        InUserId = "sysadmin",
                                        TypName = code.Type,
                                        ChildChk = string.IsNullOrEmpty(childChk) ? false : Convert.ToBoolean(childChk),
                                    });
                                    db.SaveChanges();
                                }
                            }

                            rowIndex++;
                        } while (!string.IsNullOrEmpty(type));
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException dex)
                        {
                            MessageBox.Show(dex.Message);
                        }

                        MessageBox.Show("导入完成");
                    }
                }

            }
        }
    }
}
