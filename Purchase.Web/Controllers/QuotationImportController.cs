using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XHX.Common;

namespace Purchase.Web.Controllers
{
    public class QuotationImportController : BaseController
    {
        public ActionResult Import()
        {
            string controllerName = RouteData.Values["controller"].ToString();
            string fileName = "";
            string fileFullName = Server.MapPath(baseUploadPath + controllerName);
            if (Request.Files != null)
            {
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase fileBase = Request.Files.Get(key);
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fileBase.FileName;
                    fileFullName = fileFullName + fileName;
                    fileBase.SaveAs(fileFullName);
                }
            }
            MSExcelUtil excelUtil = new MSExcelUtil();
            //Workbook workbook = excelUtil.OpenExcelByMSExcel(fileFullName);
            return View();
        }
    }
}