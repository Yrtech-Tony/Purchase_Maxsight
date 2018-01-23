using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XHX.Common;

namespace Purchase.Web.Controllers
{
    public class FlowOrderController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        FlowOrderService service = new FlowOrderService();
        // GET: FlowOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ApplyPayIndex()
        {
            return View();
        }

        public ActionResult LoadApplyPay(string projectName, string projectShortName, string supplierName, string serviceTrade, int pageNum, int? pageSize)
        {
            List<FlowOrderDto> list = service.FlowOrderSearch1(projectName, projectShortName, supplierName, serviceTrade, UserInfo.UserId);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult Load(string projectId, string supplierId, int pageNum, int? pageSize)
        {
            List<FlowOrderDto> list = service.FlowOrderSearch(projectId, supplierId, UserInfo.UserId);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }

        // GET: Edit
        public ActionResult Edit(string id, string ProjectId, string SupplierId)
        {
            FlowOrderDto flowOrderDto = service.FlowOrderSearchById(id, UserInfo.UserId).FirstOrDefault();
            if (flowOrderDto == null)
            {
                flowOrderDto = new FlowOrderDto();
            }

            return PartialView("_PartialEdit", flowOrderDto);
        }

        [HttpPost]
        public ActionResult Save()
        {
            FlowOrderDto flowOrder = new FlowOrderDto();
            TryUpdateModel<FlowOrderDto>(flowOrder);
            service.FlowOrderSave(flowOrder, UserInfo.UserId);
            try
            {
                MasterService master = new MasterService();
                master.RemindCancelSave("计提填写提醒", flowOrder.ProjectId.ToString(), UserInfo.UserId, DateTime.Now.ToString());
                master.RemindCancelSave("资金需求填写提醒", flowOrder.ProjectId.ToString(), UserInfo.UserId, DateTime.Now.ToString());
                // 对公付款填写提醒拥有对公付款保存权限的人员
                List<UserInfoDto> list = master.UserInfoSearch("", "", "对公付款保存");
                foreach (UserInfoDto user in list)
                {
                    master.RemindCancelSave("对公付款填写提醒", flowOrder.ProjectId.ToString(), user.UserId, DateTime.Now.ToString());
                }
            }
            catch (Exception)
            {


            }

            return Json("");
        }
        public ActionResult ApplyCommit(string recheckUserId, Apply apply, int FlowOrderId, string applyIdexists, string seqNO, string attachArray)
        {
            ApplyService applayService = new ApplyService();
            apply.InDateTime = DateTime.Now;
            apply.ApplyUserId = UserInfo.UserId;
            apply.ApplyTypeId = ApplyType.流转单.ToString();
            apply.ProjectId = apply.ProjectId.HasValue ? apply.ProjectId.Value : 0;
            int applyId = 0;
            if (Convert.ToInt32(applyIdexists) == 0)
            {
                applyId = applayService.ApplyCommit(apply, recheckUserId);
            }
            else
            {
                ApplyRecheckStatus status = new ApplyRecheckStatus();
                status.ApplyId = Convert.ToInt32(applyIdexists);
                status.RecheckUserId = ((Mst_UserInfo)Session["LoginUser"]).UserId;
                status.RecheckStatusCode = "申请";
                status.RecheckReason = apply.ApplyReason == null ? "" : apply.ApplyReason;
                status.SeqNO = Convert.ToInt32(seqNO);
                status.InDateTime = DateTime.Now;
                applayService.ApplyRecheckStatusUpdate(status, recheckUserId);
                applyId = Convert.ToInt32(applyIdexists);
            }
            applayService.ApplyDtlDelete(new ApplyDtl { ApplyId = applyId, ApplyTypeId = "流转单" });
            ApplyDtl applyDtl = new ApplyDtl()
            {
                ApplyId = applyId,
                ApplyContentId = FlowOrderId,
                ApplyTypeId = ApplyType.流转单.ToString(),
                InDateTime = DateTime.Now,
                InUserId = UserInfo.UserId
            };
            //applayService.ApplyDtlSave(applyDtl);
            applayService.ApplyDtlSave_FlowOrder(applyDtl);
            MyApplyDto applyInfo = applayService.ApplySearchById(applyId.ToString()).FirstOrDefault();
            ApplyEmailSend(applyInfo, recheckUserId);

            //保存附件
            List<string> attachList = JsonConvert.DeserializeObject<List<string>>(attachArray);
            int fileSeqNO = 1;
            attachList.ForEach((string attach) =>
            {
                db.ApplyFile.Add(new ApplyFile()
                {
                    ApplyId = applyId,
                    FileName = attach,
                    InDateTime = DateTime.Now,
                    InUserId = UserInfo.UserId,
                    SeqNO = fileSeqNO++
                });
            });
            db.SaveChanges();

            return Json(applyId, JsonRequestBehavior.AllowGet);
        }
        private void ApplyEmailSend(MyApplyDto apply, string recheckUserId)
        {
            try
            {


                MasterService mstService = new MasterService();
                UserInfoDto userinfo_Apply = mstService.UserInfoSearchByUserId(apply.ApplyUserId).FirstOrDefault();
                UserInfoDto userinfo_Recheck = mstService.UserInfoSearchByUserId(recheckUserId).FirstOrDefault();
                string[] mailTo = null;
                if (userinfo_Recheck == null)
                {
                    // mailTo = "".Split(',');
                }
                else
                {
                    mailTo = userinfo_Recheck.Email.Split(',');
                }
                string[] mailCC = null;
                string subject = userinfo_Apply.UserName + "的" + apply.ApplyTypeId + "申请";
                string content = "您好,以下是" + userinfo_Apply.UserName + "的申请，请及时在系统进行处理" + "<br>";
                content += "申请Id:" + apply.ApplyId.ToString() + "<br>";
                content += "申请人:" + userinfo_Apply.UserName + "<br>";
                content += "申请类型:" + apply.ApplyTypeId + "<br>";
                content += "申请理由:" + apply.ApplyReason + "<br>";
                content += "项目全称:" + apply.ProjectName + "<br>";
                content += "项目简称:" + apply.ProjectShortName + "<br>";
                content += "供应商全称:" + apply.SupplierName + "<br>";

                List<string> attachPaths = new List<string>();

                //个人签名 图片形式
                string sign = "";
                if (!string.IsNullOrEmpty(userinfo_Apply.EmailFooter))
                {
                    string emailSignFile = Server.MapPath("~/EmailSign/" + userinfo_Apply.EmailFooter);
                    sign = emailSignFile;
                }
                //准备发送邮件对象
                ISendMail sendMail = new UseNetMail();
                sendMail.CreateHost(new ConfigHost()
                {
                    EnableSsl = false,
                    Username = userinfo_Apply.Email,
                    Password = userinfo_Apply.EmailPassword,
                });
                sendMail.CreateMultiMail(new ConfigMail()
                {
                    From = userinfo_Apply.Email,
                    To = mailTo,
                    CC = mailCC,
                    Subject = subject,
                    Body = content,
                    Resources = sign.Split(','),
                    Attachments = attachPaths.ToArray()
                });

                sendMail.SendMail();
            }
            catch (Exception)
            {


            }
        }
        public ActionResult RecheckUserSelect()
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(ApplyType.流转单.ToString(), UserInfo.UserId);

            return PartialView("_PartialRecheckUserSelect", lst);
        }
        public ActionResult ApplyPayUpdate(string jsonArr)
        {
            List<string> lst = JsonConvert.DeserializeObject<List<string>>(jsonArr);
            foreach (string id in lst)
            {
                service.ApplyPayUpdate(id, UserInfo.UserId);
            }

            return Json("");
        }

        public ActionResult BugetSearch(string projectId)
        {
            FlowOrderBugetDto dto = service.FlowOrderBugetSearch(projectId).FirstOrDefault();
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InvoiceAmtUpdate(string jsonArr)
        {
            List<FlowOrderDto> lst = JsonConvert.DeserializeObject<List<FlowOrderDto>>(jsonArr);
            foreach (FlowOrderDto dto in lst)
            {
                service.InvoiceAmtUpdate(dto.FlowOrderId, dto.InvoiceAmt);
            }

            return Json("");
        }

        string tempPath = "~/CommonTemplate/";
        string basePath = "~/Export/FlowOrder/";
        public ActionResult FlowOrderExport(string projectId, string supplierId)
        {
            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "应付流转单_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "应付流转单.xlsx");// 模板的路径
            System.IO.File.Copy(templateFile, path);

            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            FlowOrderBugetDto dto_flow = service.FlowOrderBugetSearch(projectId).FirstOrDefault();
            if (string.IsNullOrEmpty(dto_flow.FactSum)) dto_flow.FactSum = "0.00";
            msExcelUtil.SetCellValue(sheet, 2, 2, dto_flow.Contingency);
            msExcelUtil.SetCellValue(sheet, 4, 2, dto_flow.BugetSum);
            msExcelUtil.SetCellValue(sheet, 6, 2, dto_flow.FactSum);


            List<FlowOrderDto> list = service.FlowOrderSearch(projectId, supplierId, UserInfo.UserId);
            int startIndex = 4;
            if (list.Count > 0)
            {
                foreach (FlowOrderDto dto in list)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, dto.ServiceTrade);
                   // msExcelUtil.SetCellValue(sheet, 2, startIndex, dto.DepartmentCode);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, dto.CostDepartment);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.ProjectName);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.ProjectShortName);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.ProjectCode);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.ExecuteCycleStartDate);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.ExecuteCycleEndDate);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.SupplierName);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.BudgetAmt);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.BudgetLeftAmt);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, dto.ExpendType);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.ExpendPattern);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.PaymentType);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.PaymentCompany);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.EstimatedPaymentTime);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, dto.EstimatePaymentAmt);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, dto.FactPaymentTime);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, dto.FactPaymentAmt);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, dto.InvoiceAmt);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, dto.Payee);
                    msExcelUtil.SetCellValue(sheet, 21, startIndex, dto.Remark);
                    msExcelUtil.SetCellValue(sheet, 22, startIndex, dto.Finance_PaymentStatus);
                    if (list.IndexOf(dto) < list.Count - 1)
                    {
                        msExcelUtil.CopyRow(sheet, startIndex);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }

            }
            workbook.Save();
            msExcelUtil.dispose();

            return Json(new { ExportPath = path });
        }

        public ActionResult ApplyPayExport(string projectName, string projectShortName, string supplierName, string serviceTrade)
        {
            List<FlowOrderDto> list = service.FlowOrderSearch1(projectName, projectShortName, supplierName, serviceTrade, UserInfo.UserId);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "应付流转单付款申请_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "应付流转单付款申请.xlsx");// 模板的路径
            System.IO.File.Copy(templateFile, path);

            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 2;
            if (list.Count > 0)
            {
                foreach (FlowOrderDto dto in list)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, dto.ServiceTrade);
                   // msExcelUtil.SetCellValue(sheet, 2, startIndex, dto.DepartmentCode);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, dto.CostDepartment);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.ProjectName);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.ProjectShortName);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.ProjectCode);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.ExecuteCycleStartDate.HasValue ? dto.ExecuteCycleStartDate.Value.ToString("yyyy-MM-dd") : "");
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.ExecuteCycleEndDate.HasValue ? dto.ExecuteCycleEndDate.Value.ToString("yyyy-MM-dd") : "");
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.SupplierName);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.BudgetAmt);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.BudgetLeftAmt);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, dto.ExpendType);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.ExpendPattern);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.PaymentType);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.PaymentCompany);
                    //msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.EstimatedPaymentTime.HasValue ? dto.EstimatedPaymentTime.Value.ToString("yyyy-MM-dd") : "");
                    //msExcelUtil.SetCellValue(sheet, 16, startIndex, dto.EstimatePaymentAmt);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.FactPaymentTime.HasValue ? dto.FactPaymentTime.Value.ToString("yyyy-MM-dd") : "");
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, dto.FactPaymentAmt);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, dto.InvoiceAmt);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, dto.Payee);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, dto.Remark);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, dto.Finance_PaymentStatus);
                    if (list.IndexOf(dto) < list.Count - 1)
                    {
                        msExcelUtil.CopyRow(sheet, startIndex);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }

            }
            workbook.Save();
            msExcelUtil.dispose();

            return Json(new { ExportPath = path });
        }
    }
}