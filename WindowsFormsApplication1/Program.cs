using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Quotation_BianChengDto dto = new Quotation_BianChengDto();
            dto.shuliang = "5.00";
            Debug.WriteLine(dto.shuliang);
            Application.Run(new Form1());
        }
    }
}
