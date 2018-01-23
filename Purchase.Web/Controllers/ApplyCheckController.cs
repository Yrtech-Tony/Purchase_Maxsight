using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class ApplyCheckController : BaseController
    {
        private ApplyService service = new ApplyService();
        QuotationService quotationService = new QuotationService();
        private PurchaseEntities db = new PurchaseEntities();
        // GET: MyRecheckSearch
        public ActionResult MyApplySearch()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MyApplySearch(string ApplyId, string ApplyTypeId, string startDate, string endDate, string statusCode, string projectName, string projectShortName, string supplierName, string supplierShortName,string RecheckStatusCode,int pageNum, int? pageSize)
        {
            DateTime startTime = Utils.String2Time(startDate, DateTime.MinValue);
            DateTime endTime = Utils.String2Time(endDate, DateTime.MaxValue);

            if (pageSize.HasValue)
            {
                _countPerPage = pageSize.Value;
            }
            List<MyApplyDto> lst = service.MyApplySearch(ApplyId, UserInfo.UserId, ApplyTypeId, startTime, endTime, statusCode, projectName, projectShortName, supplierName, supplierShortName,RecheckStatusCode);

            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }
        // GET: MyRecheckSearch
        public ActionResult MyRecheckSearch()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MyRecheckSearch(string ApplyId, string ApplyUserId, string ApplyTypeId, string startDate, string endDate, string recheckStatusCode, string projectName, string projectShortName, string userId, int pageNum, int? pageSize)
        {
            DateTime startTime = Utils.String2Time(startDate, DateTime.MinValue);
            DateTime endTime = Utils.String2Time(endDate, DateTime.MaxValue);

            if (pageSize.HasValue)
            {
                _countPerPage = pageSize.Value;
            }
            List<MyApplyDto> lst = service.MyRecheckSearch(ApplyId, ApplyUserId, ApplyTypeId, startTime, endTime, recheckStatusCode, UserInfo.UserId, projectName, projectShortName, ((Mst_UserInfo)Session["LoginUser"]).UserId);

            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }
        public ActionResult MyNeedRecheckSearch()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MyNeedRecheckSearch(string ApplyId, string ApplyUserId, string ApplyTypeId, string startDate, string endDate, int pageNum, int? pageSize)
        {
            DateTime startTime = Utils.String2Time(startDate, DateTime.MinValue.AddDays(1));
            DateTime endTime = Utils.String2Time(endDate, DateTime.MaxValue.AddDays(-1));

            if (pageSize.HasValue)
            {
                _countPerPage = pageSize.Value;
            }
            List<MyApplyDto> lst = service.MyNeedRecheckSearch(ApplyId, ApplyUserId, ApplyTypeId, startTime, endTime, ((Mst_UserInfo)Session["LoginUser"]).UserId);

            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }
        [HttpPost]
        public ActionResult RecheckStatusDetail(string ApplyId)
        {
            List<RecheckStatusDtlDto> list = service.RecheckDtlSearch(ApplyId);

            return PartialView("_PartialApplyCheckRecheckStatusDetail", list);

        }

        public ActionResult ApplyDetail(string ApplyId, string ApplyTypeId)
        {
            ViewBag.UserId = UserInfo.UserId;
            ViewBag.UserRoleType = UserInfo.RoleTypeCode;
            ViewBag.ApplyId = ApplyId;
            if (ApplyTypeId == "供应商")
            {
                List<SupplierDto> list = service.ApplyDtl_SupplierSearch(ApplyId);
                return PartialView("_PartialApplyCheckApplyDtlSupplier", list);
            }
            else if (ApplyTypeId == "合同")
            {
                List<ConstractDto> list = service.ApplyDtl_ConstractSearch(ApplyId);
                return PartialView("_PartialApplyCheckApplyDtlConstract", list);
            }
            else if (ApplyTypeId == "立项")
            {
                List<ProjectDto> list = service.ApplyDtl_ProjectSearch(ApplyId);
                return PartialView("_PartialApplyCheckApplyDtlProject", list);
            }
            else if (ApplyTypeId == "需求书")
            {
                List<RequiremetMstDto> list = service.ApplyDtl_RequirementSearch(ApplyId);
                return PartialView("_PartialApplyCheckApplyDtlRequirement", list);
            }
            else if (ApplyTypeId == "确认单")
            {
                List<QuotationMstDto> list = service.ApplyDtl_QuotationTypeSearch(ApplyId);
                //List<QuotationMstDto> list = service.ApplyDtl_QuotationSearch(ApplyId);
                return PartialView("_PartialApplyCheckApplyDtlQuotation", list);
            }
            else if (ApplyTypeId == "结算单")
            {
                List<SettlementMstDto> list = service.ApplyDtl_SettlementSearch(ApplyId);
                return PartialView("_PartialApplyCheckApplyDtlSettlement", list);
            }
            else if (ApplyTypeId == "流转单")
            {
                List<FlowOrderDto> list = service.ApplyDtl_FlowOrderSearch(ApplyId);
                return PartialView("_PartialApplyCheckApplyDtlFlowOrder", list);
            }
            return PartialView("_PartialApplyCheckRecheckStatusDetail", null);
        }
        #region 验证是否勾选过确认单
        public int QuotationSelectCountById(string ApplyId)
        {
            int SelectCount = 0;
            List<QuotationMstDto> list = service.ApplyDtl_QuotationTypeSearch(ApplyId);
            foreach (QuotationMstDto dto in list)
            {
                if (dto.QuotationType == "Biancheng")
                {
                    SelectCount += service.QuotationApplySearchBiancheng(ApplyId).Where(x => x.SelectedChk).Count();
                }
                if (dto.QuotationType == "Chezhan")
                {
                    SelectCount += service.QuotationApplySearchChezhan(ApplyId).Where(x => x.SelectedChk).Count();
                }
                if (dto.QuotationType == "Fuhe")
                {
                    SelectCount += service.QuotationApplySearchFuhe(ApplyId).Where(x => x.SelectedChk).Count();
                }
                if (dto.QuotationType == "Qita1")
                {
                    SelectCount += service.QuotationApplySearchQitao1(ApplyId).Where(x => x.SelectedChk).Count();
                }
                if (dto.QuotationType == "Qita2")
                {
                    SelectCount += service.QuotationApplySearchQitao2(ApplyId).Where(x => x.SelectedChk).Count();
                }
                if (dto.QuotationType == "Yanjiu")
                {
                    SelectCount += service.QuotationApplySearchYanjiu(ApplyId).Where(x => x.SelectedChk).Count();
                }
                if (dto.QuotationType == "Zhichi")
                {
                    SelectCount += service.QuotationApplySearchZhichi(ApplyId).Where(x => x.SelectedChk).Count();
                }
                if (dto.QuotationType == "Zhixing")
                {
                    SelectCount += service.QuotationApplySearchZhixing(ApplyId).Where(x => x.SelectedChk).Count();
                }

            }
            return SelectCount;
        }
        public ActionResult ApplyDetail_Quotation_CheckSelect(string ApplyId)
        {
            int SelectCount = QuotationSelectCountById(ApplyId);
            return Json(SelectCount.ToString(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApplyDetail_QuotationList_CheckSelect(string ApplyIdList)
        {
            bool selectChk = true;
            string[] applyList = new string[] { };
            if (ApplyIdList.Length > 0)
            {
                applyList = ApplyIdList.Split(';');
            }
            foreach (string applyId in applyList)
            {
                if (QuotationSelectCountById(applyId.Split(',')[0]) == 0)
                {
                    selectChk = false;
                }
            }
            return Json(selectChk, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult ApplyDetail_Quotation(string ApplyId, string QuotationType)
        {
            if (QuotationType == "Biancheng")
            {
                List<Quotation_BianChengDto> list = service.QuotationApplySearchBiancheng(ApplyId);
                return PartialView("_PartialApplyCheckLoadBiancheng", list);
            }
            else if (QuotationType == "Chezhan")
            {
                List<Quotation_CheZhanDto> list = service.QuotationApplySearchChezhan(ApplyId);
                return PartialView("_PartialApplyCheckLoadChezhan", list);
            }
            else if (QuotationType == "Fuhe")
            {
                List<Quotation_FuHeDto> list = service.QuotationApplySearchFuhe(ApplyId);
                return PartialView("_PartialApplyCheckLoadFuhe", list);
            }
            else if (QuotationType == "Qita1")
            {
                List<Quotation_QiTa1Dto> list = service.QuotationApplySearchQitao1(ApplyId);
                return PartialView("_PartialApplyCheckLoadQita1", list);
            }
            else if (QuotationType == "Qita2")
            {
                List<Quotation_QiTa2Dto> list = service.QuotationApplySearchQitao2(ApplyId);
                return PartialView("_PartialApplyCheckLoadQita2", list);
            }
            else if (QuotationType == "Yanjiu")
            {
                List<Quotation_YanJiuDto> list = service.QuotationApplySearchYanjiu(ApplyId);
                return PartialView("_PartialApplyCheckLoadYanjiu", list);
            }
            else if (QuotationType == "Zhichi")
            {
                List<ZhiChi01Dto> list = service.QuotationApplySearchZhichi(ApplyId);
                return PartialView("_PartialApplyCheckLoadZhichi", list);
            }
            else if (QuotationType == "Zhixing")
            {
                List<Quotation_ZhiXingDto> list = service.QuotationApplySearchZhixing(ApplyId);
                return PartialView("_PartialApplyCheckLoadZhixing", list);
            }

            return PartialView("_PartialApplyCheckLoadBiancheng", null);
        }
        public ActionResult ApplyDetail_QuotationSelect(string resultList)
        {
            string[] list = resultList.Split(';');
            foreach (string str in list)
            {
                string[] result = str.Split(',');
                bool selectChk = false;
                if (result[3] == "true")
                {
                    selectChk = true;
                }
                service.QuotationSelectSave(result[0], result[1], result[2], selectChk);

            }
            return Json("");
        }
        public ActionResult ApplyDetail_SettlementDtl(int applyId, int settlementId,int quotationGroupId, int projectId, int supplierId)
        {
            SettlementService settlementService = new SettlementService();
            // 查询当前项目下供应商执行的基本信息
            SettlementMstDto settlementMstDto = settlementService.SettleDtlMainSearch(projectId.ToString(), supplierId.ToString(),settlementId.ToString(), quotationGroupId.ToString(), UserInfo.UserId).FirstOrDefault();
            // 查询当前项目下的预算和结算信息
            SettlementBugetAmtDto bugetAmtDto = service.ApplyDtl_SettlementBugetAmtSearch(projectId.ToString(), applyId.ToString()).FirstOrDefault();

            if (settlementMstDto.ExecuteCycle == null)
            {
                settlementMstDto.ExecuteCycle = "";
            }
            if (settlementMstDto.zhixingchengshi == "全国-")
            {
                settlementMstDto.zhixingchengshi = "全国";
            }

            // 查询供应商结算的详细信息
            List<SettlementDtlDto> lst = service.ApplyDtl_SettlementDtlByApplyIdAndSettlementIdSearch(applyId.ToString(), settlementId.ToString());
            lst.AddRange(service.ApplyDtl_SettlementDtlByApplyIdAndSettlementIdSearch_shoudong(applyId.ToString(), settlementId.ToString()));

            return Json(new { List = lst, MainObject = settlementMstDto, BugetAmtDto = bugetAmtDto });
        }
        public ActionResult ApplySettlementZhuijia(string applyId)
        {
            var lst = service.ApplyDtl_SettlementDtlZhuijiaByApplyIdSearch(applyId);

            return Json(new { List = lst });
        }
        // 供应商详细查看
        public ActionResult _PartialApplyCheckApplyDtlSupplierEdit(int applyId, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierDto suppliermng = new SupplierDto() { SupplierId = id.ToString(), ApplyId = applyId.ToString() };
            return View(suppliermng);
        }
        public ActionResult ApplyDetail_SupplierDtl(int applyId, int supplierId)
        {
            //SupplierMng suppliermng = db.SupplierMng.Find(id);
            SupplierDto suppliermng = service.ApplyDtl_SupplierByApplyIdAndSupplierId(applyId.ToString(), supplierId.ToString()).FirstOrDefault();
            return Json(suppliermng, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApplyDetail_SupplierDtl_Pngs(int applyId, int SupplierId)
        {
            List<Object> retData = new List<object>();
            var attachs = db.ApplyDtl_SupplierMngAttachmentFile.Where(x => x.ApplyId == applyId && x.ApplyContentId == SupplierId);
            foreach (var attach in attachs)
            {
                retData.Add(new { fileName = attach.FileName, fileFullName = GetFullFileName(attach.FileName) });
            }
            return Json(retData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApplyDetail_FlowOrderBy(string applyId, string id)
        {
            //FlowOrderService flowerService = new FlowOrderService();
            //  FlowOrderDto flowOrderDto = flowerService.FlowOrderSearchById(id, UserInfo.UserId).FirstOrDefault();
            FlowOrderDto flowOrderDto = service.ApplyDtl_FlowOrderByApplyIdAndFlowOrderId(applyId, id).FirstOrDefault();
            if (flowOrderDto == null)
            {
                flowOrderDto = new FlowOrderDto();
            }

            return PartialView("_PartialApplyCheckApplyDtlFlowOrderDtlEdit", flowOrderDto);
        }
        public ActionResult ApplyDetail_FlowOrderBugetSearch(string applyId)
        {
            FlowOrderBugetDto flowOrderBugetDto = service.ApplyDtl_FlowOrderBugetSearch(applyId).FirstOrDefault();
            if (flowOrderBugetDto == null)
            {
                return Json(false);
            }
            return Json(flowOrderBugetDto);
        }
        

        public ActionResult RecheckSelect(string recheckType)
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(recheckType, UserInfo.UserId);

            return PartialView("_PartialRecheck", lst);
        }
        public ActionResult RecheckListSelect(string recheckType)
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(recheckType, UserInfo.UserId);

            return PartialView("_PartialRecheckList", lst);
        }
        public ActionResult RecheckCommit(ApplyRecheckStatus applyStatus, string nextRecheckUserId)
        {
            ApplyService applayService = new ApplyService();
            // 查询当前提交审核的申请，审核人的审核信息
            RecheckStatusDtlDto recheckStatus = applayService.RecheckDtlSearch(applyStatus.ApplyId.ToString()).Where(x => x.SeqNO == applyStatus.SeqNO.ToString()).FirstOrDefault();

            // 只有当审核人还没有审核的时候执行下面的操作，如果审核人已经审核过的话，不需要再执行
            //正常情况下不会出现这样的数据，为了解决特定网络情况下导致的重复数据，所以添加了这个判断
            if (string.IsNullOrEmpty(recheckStatus.RecheckStatusCode))
            {


                MyApplyDto applyInfo = applayService.ApplySearchById(applyStatus.ApplyId.ToString()).FirstOrDefault();
                if (applyStatus.RecheckStatusCode == "拒绝")
                {
                    nextRecheckUserId = applayService.PreRecheckUserSearch(applyStatus.ApplyId.ToString(), UserInfo.UserId);
                }
                applyStatus.RecheckUserId = UserInfo.UserId;
                applyStatus.InDateTime = DateTime.Now;
                if (nextRecheckUserId == null)
                {
                    nextRecheckUserId = "";
                }
                applayService.ApplyRecheckStatusUpdate(applyStatus, nextRecheckUserId);
                if (applyStatus.RecheckStatusCode == "拒绝")
                {
                    ApplyEmailSend(applyInfo, nextRecheckUserId, "拒绝");
                }
                else
                {
                    ApplyEmailSend(applyInfo, nextRecheckUserId, "");
                }
                // 状态更新后再查询一遍，如果是完成的话则发送邮件给申请人，邮件通知已经审核完成
                MyApplyDto afterUpdate = applayService.ApplySearchById(applyStatus.ApplyId.ToString()).FirstOrDefault();
                if (afterUpdate.ApplyStatusCode == "完成")
                {
                    ApplyEmailSend(applyInfo, nextRecheckUserId, "完成");
                }
            }

            return Json("");
        }
        public ActionResult RecheckListCommit(string applyStatusList, string recheckReason, string nextRecheckUserId)
        {
            ApplyService applayService = new ApplyService();

            string[] applyList = new string[] { };
            if (applyStatusList.Length > 0)
            {
                applyList = applyStatusList.Split(';');
            }
            foreach (string item in applyList)
            {
                // 查询当前提交审核的申请，审核人的审核信息
                RecheckStatusDtlDto recheckStatus = applayService.RecheckDtlSearch(item.Split(',')[0].ToString()).Where(x => x.SeqNO == item.Split(',')[1].ToString()).FirstOrDefault();
                // 只有当审核人还没有审核的时候执行下面的操作，如果审核人已经审核过的话，不需要再执行
                //正常情况下不会出现这样的数据，为了解决特定网络情况下导致的重复数据，所以添加了这个判断
                if (string.IsNullOrEmpty(recheckStatus.RecheckStatusCode))
                {
                    ApplyRecheckStatus applyStatus = new ApplyRecheckStatus();
                    applyStatus.RecheckUserId = UserInfo.UserId;
                    applyStatus.InDateTime = DateTime.Now;
                    applyStatus.ApplyId = Convert.ToInt32(item.Split(',')[0]);
                    applyStatus.SeqNO = Convert.ToInt32(item.Split(',')[1]);
                    applyStatus.RecheckStatusCode = "同意";
                    applyStatus.RecheckReason = recheckReason;
                    applayService.ApplyRecheckStatusUpdate(applyStatus, nextRecheckUserId);
                    MyApplyDto applyInfo = applayService.ApplySearchById(applyStatus.ApplyId.ToString()).FirstOrDefault();
                    ApplyEmailSend(applyInfo, nextRecheckUserId, "");
                    // 状态更新后再查询一遍，如果是完成的话则发送邮件给申请人，邮件通知已经审核完成
                    MyApplyDto afterUpdate = applayService.ApplySearchById(applyStatus.ApplyId.ToString()).FirstOrDefault();
                    if (afterUpdate.ApplyStatusCode == "完成")
                    {
                        ApplyEmailSend(applyInfo, nextRecheckUserId, "完成");
                    }
                }

            }
            return Json("");
        }
        private void ApplyEmailSend(MyApplyDto apply, string recheckUserId, string recheckStatusCode)
        {
            try
            {
                MasterService mstService = new MasterService();
                UserInfoDto userinfo_Apply = new UserInfoDto();//申请人的信息
                UserInfoDto userinfo_Recheck = new UserInfoDto();//下一个审核人员的信息
                UserInfoDto userinfo_From = new UserInfoDto();//当前登陆人员的信息
                string content = "";
                string subject = "";
                string[] mailTo = null;
                if (recheckStatusCode == "拒绝")
                {
                    userinfo_Apply = mstService.UserInfoSearchByUserId(apply.ApplyUserId).FirstOrDefault();
                    userinfo_Recheck = mstService.UserInfoSearchByUserId(recheckUserId).FirstOrDefault();
                    userinfo_From = mstService.UserInfoSearchByUserId(UserInfo.UserId).FirstOrDefault();
                    subject = userinfo_Apply.UserName + "的" + apply.ApplyTypeId + "申请被拒绝";
                    content = "您好,以下是" + userinfo_Apply.UserName + "的申请，请及时在系统进行处理" + "<br>";
                    content += "申请Id:" + apply.ApplyId.ToString() + "<br>";
                    content += "申请人:" + userinfo_Apply.UserName + "<br>";
                    content += "申请类型:" + apply.ApplyTypeId + "<br>";
                    content += "申请理由:" + apply.ApplyReason + "<br>";
                    content += "项目全称:" + apply.ProjectName + "<br>";
                    content += "项目简称:" + apply.ProjectShortName + "<br>";
                    content += "供应商全称:" + apply.SupplierName + "<br>";

                }
                else if (recheckStatusCode == "完成")
                {
                    // 当审核完成的时候没有下一个审核人发送邮件给申请人，告知申请已经批准完成
                    userinfo_Apply = mstService.UserInfoSearchByUserId(apply.ApplyUserId).FirstOrDefault();
                    userinfo_Recheck = userinfo_Apply;
                    subject = "申请审核完成通知";
                    content = "您好,你的申请已经审核完成" + "<br>";
                    content += "申请Id:" + apply.ApplyId.ToString() + "<br>";
                    content += "申请人:" + userinfo_Apply.UserName + "<br>";
                    content += "申请类型:" + apply.ApplyTypeId + "<br>";
                    content += "申请理由:" + apply.ApplyReason + "<br>";
                    content += "项目全称:" + apply.ProjectName + "<br>";
                    content += "项目简称:" + apply.ProjectShortName + "<br>";
                    content += "供应商全称:" + apply.SupplierName + "<br>";
                }
                else
                {
                    userinfo_Apply = mstService.UserInfoSearchByUserId(apply.ApplyUserId).FirstOrDefault();
                    userinfo_Recheck = mstService.UserInfoSearchByUserId(recheckUserId).FirstOrDefault();
                    userinfo_From = mstService.UserInfoSearchByUserId(apply.ApplyUserId).FirstOrDefault();
                    subject = userinfo_Apply.UserName + "的" + apply.ApplyTypeId + "申请";
                    content = "您好,以下是" + userinfo_Apply.UserName + "的申请，请及时在系统进行处理" + "<br>";
                    content += "申请Id:" + apply.ApplyId.ToString() + "<br>";
                    content += "申请人:" + userinfo_Apply.UserName + "<br>";
                    content += "申请类型:" + apply.ApplyTypeId + "<br>";
                    content += "申请理由:" + apply.ApplyReason + "<br>";
                    content += "项目全称:" + apply.ProjectName + "<br>";
                    content += "项目简称:" + apply.ProjectShortName + "<br>";
                    content += "供应商全称:" + apply.SupplierName + "<br>";

                }
                string[] mailCC = null;
                if (userinfo_Recheck == null)
                {
                   // mailTo = "".Split(',');
                }
                else
                {
                    mailTo = userinfo_Recheck.Email.Split(',');
                }


                List<string> attachPaths = new List<string>();

                //个人签名 图片形式
                string sign = "";
                if (!string.IsNullOrEmpty(userinfo_From.EmailFooter))
                {
                    string emailSignFile = Server.MapPath("~/EmailSign/" + userinfo_From.EmailFooter);
                    sign = emailSignFile;
                }
                //准备发送邮件对象
                ISendMail sendMail = new UseNetMail();
                sendMail.CreateHost(new ConfigHost()
                {
                    EnableSsl = false,
                    Username = userinfo_From.Email,
                    Password = userinfo_From.EmailPassword,
                });
                sendMail.CreateMultiMail(new ConfigMail()
                {
                    From = userinfo_From.Email,
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

        string suffix = ".xlsx";
        string tempPath = "~/QuotationTemplate/";
        string basePath = "~/Export/Quotation/";
        string tempPathHor = "~/QuotationTemplate/横向确认单/";

        public ActionResult ExportApplyDetailQuotation(string ApplyId)
        {           
            string zip = "";
            string absPath = Server.MapPath(basePath);
            string createFloader = absPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\\";
            if (!Directory.Exists(createFloader))
            {
                Directory.CreateDirectory(createFloader);
            }
            List<QuotationMstDto> list = service.ApplyDtl_QuotationTypeSearch(ApplyId);
            string projectShortName = "";
            foreach (QuotationMstDto qMstDto in list)
            {
                string quotationType = qMstDto.QuotationType;
                List<QuotationDto> lst = new List<QuotationDto>();
                projectShortName = qMstDto.ProjectShortName;
                #region ==================竖向的确认单模板 先注释===============================
                //if (QuotationType == "Biancheng")
                //{
                //    list.AddRange(service.QuotationApplySearchBiancheng(ApplyId));
                //}
                //else if (QuotationType == "Chezhan")
                //{
                //    list.AddRange(service.QuotationApplySearchChezhan(ApplyId));
                //}
                //else if (QuotationType == "Fuhe")
                //{
                //    list.AddRange(service.QuotationApplySearchFuhe(ApplyId));
                //}
                //else if (QuotationType == "Qita1")
                //{
                //    list.AddRange(service.QuotationApplySearchQitao1(ApplyId));
                //}
                //else if (QuotationType == "Qita2")
                //{
                //    list.AddRange(service.QuotationApplySearchQitao2(ApplyId));
                //}
                //else if (QuotationType == "Yanjiu")
                //{
                //    list.AddRange(service.QuotationApplySearchYanjiu(ApplyId));
                //}
                //else if (QuotationType == "Zhichi")
                //{
                //    list.AddRange(service.QuotationApplySearchZhichi(ApplyId));
                //}
                //else if (QuotationType == "Zhixing")
                //{
                //    list.AddRange(service.QuotationApplySearchZhixing(ApplyId));
                //}
                #endregion
                #region ========横向确认单========
                string typeText = "";
                QuotationTypeAndText.TryGetValue(quotationType, out typeText);
                string createFileName = projectShortName + "_" + typeText + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_确认单" + ".xlsx";
                string path = createFloader + createFileName;
                string templateFile = "";
                if (qMstDto.ModelType == "业务" && (quotationType == "Qita1" || quotationType == "Qita2"))
                {
                    //横向确认单
                    List<QuotationExport_Data_Dto> DataDtoList = quotationService.QuotationExport_DataSearch(qMstDto.ModelType, qMstDto.ProjectCode,
                                 qMstDto.ProjectName, projectShortName, qMstDto.SupplierCode, qMstDto.SupplierName, qMstDto.SupplierShortName, "初版",
                                 qMstDto.GroupId, quotationType);
                    List<QuotationExport_Head_Dto> HeaderDtoList = quotationService.QuotationExport_HeadSearch(qMstDto.ModelType, qMstDto.ProjectCode,
                        qMstDto.ProjectName, projectShortName, qMstDto.SupplierCode, qMstDto.SupplierName, qMstDto.SupplierShortName, "初版",
                        qMstDto.GroupId, quotationType);
                    List<QuotationExport_Left_Dto> LeftDtoList = quotationService.QuotationExport_LeftSearch(qMstDto.ModelType, qMstDto.ProjectCode,
                        qMstDto.ProjectName, projectShortName, qMstDto.SupplierCode, qMstDto.SupplierName, qMstDto.SupplierShortName, "初版",
                        qMstDto.GroupId, quotationType);

                    templateFile = Server.MapPath(tempPathHor + quotationType + suffix);
                    System.IO.File.Copy(templateFile, path);

                    QuotationHorExport export = new QuotationHorExport();
                    export.ExportQuotationHorQita(path, qMstDto.ProjectName, projectShortName, qMstDto.ProjectCode, HeaderDtoList, LeftDtoList, DataDtoList);
                }
                else
                {
                    switch (quotationType)
                    {
                        case "Biancheng":
                        case "Zhixing":
                        case "Fuhe":
                        case "Yanjiu":
                            //横向确认单
                            List<QuotationExport_Data_Dto> DataDtoList = quotationService.QuotationExport_DataSearch(qMstDto.ModelType, qMstDto.ProjectCode,
                                qMstDto.ProjectName, projectShortName, qMstDto.SupplierCode, qMstDto.SupplierName, qMstDto.SupplierShortName, "初版",
                                qMstDto.GroupId, quotationType);
                            List<QuotationExport_Head_Dto> HeaderDtoList = quotationService.QuotationExport_HeadSearch(qMstDto.ModelType, qMstDto.ProjectCode,
                                qMstDto.ProjectName, projectShortName, qMstDto.SupplierCode, qMstDto.SupplierName, qMstDto.SupplierShortName, "初版",
                                qMstDto.GroupId, quotationType);
                            List<QuotationExport_Left_Dto> LeftDtoList = quotationService.QuotationExport_LeftSearch(qMstDto.ModelType, qMstDto.ProjectCode,
                                qMstDto.ProjectName, projectShortName, qMstDto.SupplierCode, qMstDto.SupplierName, qMstDto.SupplierShortName, "初版",
                                qMstDto.GroupId, quotationType);

                            templateFile = Server.MapPath(tempPathHor + quotationType + suffix);
                            System.IO.File.Copy(templateFile, path);

                            QuotationHorExport export = new QuotationHorExport();
                            export.ExportQuotationHor(path, qMstDto.ProjectName, projectShortName, qMstDto.ProjectCode, HeaderDtoList, LeftDtoList, DataDtoList);
                            break;
                        case "Zhichi":
                        case "Chezhan":
                        case "Qita1":
                        case "Qita2":
                            if (quotationType == "Zhichi")
                            {
                                lst.AddRange(service.QuotationApplySearchZhichi(ApplyId));
                            }
                            else if (quotationType == "Qita1")
                            {
                                lst.AddRange(service.QuotationApplySearchQitao1(ApplyId));
                            }
                            else if (quotationType == "Qita2")
                            {
                                lst.AddRange(service.QuotationApplySearchQitao2(ApplyId));
                            }
                            else if (quotationType == "Chezhan")
                            {
                                lst.AddRange(service.QuotationApplySearchChezhan(ApplyId));
                            }

                            if (qMstDto.ModelType != "业务")
                            {//内部采购  无形商品(其他2)=>wuxingshangpin 有形商品(其他1)=>youxingshangpin
                                if (quotationType == "Qita1")
                                {
                                    quotationType = "Youxingshangpincaigou";
                                }
                                if (quotationType == "Qita2")
                                {
                                    quotationType = "Wuxingshangpincaigou";
                                }
                            }
                            templateFile = Server.MapPath(tempPath + quotationType + suffix);
                            System.IO.File.Copy(templateFile, path);

                            QuotationQueryExport export2 = new QuotationQueryExport();
                            MethodInfo method = null;
                            method = export2.GetType().GetMethod("Export" + quotationType);
                            if (method != null)
                            {
                                method.Invoke(export2, new object[] { path, lst, new ProjectDto { ProjectShortName = projectShortName } });
                            }
                            break;
                        default:
                            break;

                    }
                }
                
                #endregion
            }

            string zipName = absPath + projectShortName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_确认单" + ".zip";
            List<FileInfo> fileList = new List<FileInfo>();

            foreach (string file in Directory.GetFiles(createFloader))
            {
                fileList.Add(new FileInfo(file));
            }
            Compress(fileList, zipName, 9, 100);
            zip = zipName;

            return Json(new { ExportPath = zip });
            
        }

        //提交审核添加附件
        public ActionResult AttachSave(string ApplyId)
        {
            string fileName = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                string key = Request.Files.GetKey(i);
                HttpPostedFileBase file = Request.Files.Get(key);
                fileName= SaveFile(file, DateTime.Now.ToString("yyyyMMddHHmmss"));
            }

            return Json(new { fileName = fileName, fileFullName = GetFullFileName(fileName) },JsonRequestBehavior.AllowGet);
        }

        public ActionResult AttachDelete(string fileFullName)
        {
            FileInfo fileInfo = new FileInfo(fileFullName);
            fileInfo.Delete();

            return Json(fileFullName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AttachShow(int applyId = 0)
        {
            var lst = db.ApplyFile.Where(x=>x.ApplyId== applyId);

            return PartialView("_PartialAttachShow", lst);
        }

        public ActionResult DownloadAttachFile(string fileName)
        {
            return DownloadFile(GetRelativeFileName(fileName));
        }
    }
}