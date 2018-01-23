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
    public class CapitalRequirementController : BaseController
    {
        CapitalRequirementService service = new CapitalRequirementService();
        //
        // GET: /AccruedCharges/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load(string serviceTrade,DateTime startDate, DateTime endDate, string projectName="", string projectShortName="")
        {
            DateTime firstDay = service.GetWeekFirstDay(startDate);
            DateTime lastDay = service.GetWeekLastDay(endDate);
            List<HeaderDto> HeaderDtoList = service.GetHeadList(startDate, endDate);
            List<LeftDto> LeftDtoList = service.GetLeftList(serviceTrade,startDate, endDate, projectName, projectShortName);
            List<DataDto> DataDtoList = service.GetDataList(serviceTrade,startDate, endDate, projectName, projectShortName, HeaderDtoList, LeftDtoList);

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList });
        }


        [HttpPost]
        public ActionResult Save()
        {
            CapitalRequiremenDto CapitalRequiremenDto = new CapitalRequiremenDto();
            TryUpdateModel<CapitalRequiremenDto>(CapitalRequiremenDto);
            service.CapitalrequirementSave(CapitalRequiremenDto, UserInfo.UserId);

            return Json("");
        }
        string tempPath = "~/CommonTemplate/";
        string basePath = "~/Export/CaptialRequirement/";
        public ActionResult ExportCapitalRequirement(string serviceTrade, DateTime startDate, DateTime endDate, string projectName = "", string projectShortName = "")
        {
            DateTime firstDay = service.GetWeekFirstDay(startDate);
            DateTime lastDay = service.GetWeekLastDay(endDate);
            List<HeaderDto> HeaderDtoList = service.GetHeadList(startDate, endDate);
            List<LeftDto> LeftDtoList = service.GetLeftList(serviceTrade, startDate, endDate, projectName, projectShortName);
            List<DataDto> DataDtoList = service.GetDataList(serviceTrade, startDate, endDate, projectName, projectShortName, HeaderDtoList, LeftDtoList);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "资金需求书表" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "资金需求表.xlsx");
            System.IO.File.Copy(templateFile, path);
            CapitalRequirementExport export = new CapitalRequirementExport();
            export.ExportCapitalRequirement(path, startDate, endDate, HeaderDtoList, LeftDtoList, DataDtoList);

            return Json(new { ExportPath = path });
        }
	}
}