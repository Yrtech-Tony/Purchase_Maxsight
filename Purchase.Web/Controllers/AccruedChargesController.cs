using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class AccruedChargesController : BaseController
    {
        AccruedChargeService service = new AccruedChargeService();
        //
        // GET: /AccruedCharges/
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load(string ServiceTrade,string ProjectName, string ProjectShortName,string SupplierName, int pageNum, int? pageSize)
        {
            List<AccruedChargesDto> list = service.AccruedChargesSearch(ServiceTrade,ProjectName, ProjectShortName, SupplierName);
            int total = list.Count();
            int pageCount = CalcPages(total,pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }


        [HttpPost]
        public ActionResult Save()
        {
            AccruedChargesDto accruedChargesDto = new AccruedChargesDto();
            TryUpdateModel<AccruedChargesDto>(accruedChargesDto);
            service.AccruedChargesSave(accruedChargesDto, UserInfo.UserId);

            return Json("");
        }

        string tempPath = "~/CommonTemplate/";
        string basePath = "~/Export/AccruedCharges/";
        public ActionResult ExportAccruedCharges(string ServiceTrade, string ProjectName, string ProjectShortName, string SupplierName)
        {
            List<AccruedChargesDto> list = service.AccruedChargesSearch(ServiceTrade, ProjectName, ProjectShortName, SupplierName);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "资金计提_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "资金计提.xlsx");
            System.IO.File.Copy(templateFile, path);
            AccruedChargesExport export = new AccruedChargesExport();
            export.ExportAccruedCharges(path, list);

            return Json(new { ExportPath = path });
        }
	}
}