using Microsoft.Office.Interop.Excel;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XHX.Common;

namespace Purchase.Web.Common
{
    public class ConstractTemplateExport
    {
        private static MasterService service = new MasterService();

        static string[] fixContentArray = { "项目名称", "执行区域", "项目时间", "复核比例", "样本量配额", "确认单", "项目周期", "布展时间", "撤展时间", "运输路线", "服务内容", "协议有效时间", "展览展示委托内容", "运输车辆信息", "平台名称", "最终交付日期", "交货日期" };
        public static void Export(string id, string path,string userid)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            try
            {
                service.ConstractTemplateDtlDelete(new ConstractTemplateDtl() { ConstractTemplateId = int.Parse(id) });
                int maxRow = 100000;
                int startRowIndex = 2;
                while (true)
                {
                    string type = msExcelUtil.GetCellValue(sheet, 1, startRowIndex);
                    string content = msExcelUtil.GetCellValue(sheet, 2, startRowIndex);
                    if (string.IsNullOrWhiteSpace(type))
                    {
                        break;
                    }
                    type = type.Trim();
                    ConstractTemplateDtl constractTemplateDtl = new ConstractTemplateDtl();

                    constractTemplateDtl.ConstractTemplateId = int.Parse(id);
                    constractTemplateDtl.InDateTime = DateTime.Now;
                    constractTemplateDtl.InUserId = userid;
                    constractTemplateDtl.OrderNO = 0;
                    if (fixContentArray.Contains(type))
                    {
                        constractTemplateDtl.ContentType = "固定内容";
                        constractTemplateDtl.ContentType2 = type;
                    }
                    else
                    {
                        constractTemplateDtl.ContentType = type;
                        constractTemplateDtl.TemplateContent = content;
                    }                   

                    service.ConstractTemplateDtlSave(constractTemplateDtl);

                    startRowIndex++;
                    if (startRowIndex > maxRow)
                    {
                        break;
                    }
                } 
            }
            catch (Exception ex)
            {
                Utils.log(ex.ToString());
            }                    
           
             msExcelUtil.dispose();
        }
    }
}