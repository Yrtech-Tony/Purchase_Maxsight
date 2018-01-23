using Microsoft.Office.Interop.Excel;
using Purchase.Service;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XHX.Common;

namespace Purchase.Web.Controllers
{

    public class PaymentController : BaseController
    {
        FlowOrderService service = new FlowOrderService();
        //
        // GET: /Payment/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FinanceIndex()
        {
            return View();
        }

        public ActionResult Load(string serviceTrade,string projectName, string projectShortName, string supplierName, DateTime? FactPaymentTimeStart, DateTime? FactPaymentTimeEnd,
            string FactPaymentAmtStart, string FactPaymentAmtEnd,string PayChk="否", int pageNum=1, int pageSize=10)
        {
            decimal? amtStart = string.IsNullOrEmpty(FactPaymentAmtStart) ? new Nullable<decimal>() : decimal.Parse(FactPaymentAmtStart);
            decimal? amtEnd = string.IsNullOrEmpty(FactPaymentAmtEnd) ? new Nullable<decimal>() : decimal.Parse(FactPaymentAmtEnd);
            List<FlowOrderDto> list = service.PaymentListSearch(serviceTrade,projectName, projectShortName, supplierName, FactPaymentTimeStart,
                FactPaymentTimeEnd, amtStart, amtEnd, PayChk);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();
           // ViewBag.UserId = UserInfo.UserId;
            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum,UserRoleType = UserInfo.RoleTypeCode });
        }

        public ActionResult Edit(string flowOrderId)
        {
            FlowOrderDto flowOrderDto = service.FlowOrderSearchById(flowOrderId, UserInfo.UserId).FirstOrDefault();

            return PartialView("_PartialEdit", flowOrderDto);
        }
        public ActionResult FinanceEdit(string flowOrderId)
        {
            FlowOrderDto flowOrderDto = service.FlowOrderSearchById(flowOrderId, UserInfo.UserId).FirstOrDefault();

            return PartialView("_PartialFinanceEdit", flowOrderDto);
        }

        [HttpPost]
        public ActionResult Save()
        {
            FlowOrderDto flowOrder = new FlowOrderDto()
            {
                PaymentRemark = "",
                Finance_PaymentStatus = "",
                Finance_PaymentAmt = "",
                Finance_NotPayReason = "",
                Finance_Constract = "",
                Finance_SettlementChk = "",
                Finance_InvoceAmt = "",
                Finance_InvoceAmtThis = ""
            };
            TryUpdateModel<FlowOrderDto>(flowOrder);
            service.PaymentListUpdate(flowOrder, UserInfo.UserId);

            return Json("");
        }
        public ActionResult FinanceSave()
        {
            FlowOrderDto flowOrder = new FlowOrderDto()
            {
                PreTaxAmt = "",
                InvoceAmtThis = "",
                ProjectType = "",
                ConstractChk = "",
                SettlementChk = "",
                InvoceChk = "",
                PaperChk = "",
                PaymentRemark = ""
            };
            TryUpdateModel<FlowOrderDto>(flowOrder);
            service.PaymentListUpdate(flowOrder, UserInfo.UserId);

            return Json("");
        }
        string tempPath = "~/CommonTemplate/";
        string basePath = "~/Export/Payment/";
        public ActionResult PaymentExport(string serviceTrade, string projectName, string projectShortName, string supplierName, DateTime? FactPaymentTimeStart, DateTime? FactPaymentTimeEnd,
            string FactPaymentAmtStart, string FactPaymentAmtEnd, string PayChk = "否")
        {
            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "对公付款汇总_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "对公付款汇总.xlsx");// 模板的路径
            System.IO.File.Copy(templateFile, path);

            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            decimal? amtStart = string.IsNullOrEmpty(FactPaymentAmtStart) ? new Nullable<decimal>() : decimal.Parse(FactPaymentAmtStart);
            decimal? amtEnd = string.IsNullOrEmpty(FactPaymentAmtEnd) ? new Nullable<decimal>() : decimal.Parse(FactPaymentAmtEnd);
            List<FlowOrderDto> list = service.PaymentListSearch(serviceTrade, projectName, projectShortName, supplierName, FactPaymentTimeStart,
                FactPaymentTimeEnd, amtStart, amtEnd, PayChk);
            int startIndex = 2;
            if (list.Count > 0)
            {
                foreach (FlowOrderDto dto in list)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, (startIndex-1).ToString());
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, dto.Year);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.FactPaymentTime);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.ModelType);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.ServiceTrade);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.ProjectName);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.ProjectShortName);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.ProjectCode);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.SupplierName);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.Payee);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, dto.PreTaxAmt);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.TaxAmt);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.FactPaymentAmt);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.InvoiceAmt);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.InvoceAmtThis);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, dto.ProjectType);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, dto.ExecuteCycleStartDate);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, dto.ExecuteCycleEndDate);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, dto.ConstractChk);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, dto.SettlementChk);
                    msExcelUtil.SetCellValue(sheet, 21, startIndex, dto.InvoceChk);
                    msExcelUtil.SetCellValue(sheet, 22, startIndex, dto.InvoiceNO);
                    msExcelUtil.SetCellValue(sheet, 23, startIndex, dto.PaperChk);
                    msExcelUtil.SetCellValue(sheet, 24, startIndex, dto.PaymentRemark);
                    msExcelUtil.SetCellValue(sheet, 25, startIndex, dto.Finance_PaymentStatus);
                    msExcelUtil.SetCellValue(sheet, 26, startIndex, dto.Finance_PaymentAmt);
                    msExcelUtil.SetCellValue(sheet, 27, startIndex, dto.Finance_NotPayReason);
                    msExcelUtil.SetCellValue(sheet, 28, startIndex, dto.Finance_Constract);
                    msExcelUtil.SetCellValue(sheet, 29, startIndex, dto.Finance_SettlementChk);
                    msExcelUtil.SetCellValue(sheet, 30, startIndex, dto.Finance_InvoceAmt);
                    msExcelUtil.SetCellValue(sheet, 31, startIndex, dto.Finance_InvoceAmtThis);
                    //msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }

            }
            workbook.Save();
             msExcelUtil.dispose();

            return Json(new { ExportPath = path });
        }
    }
}