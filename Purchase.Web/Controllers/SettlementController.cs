using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class SettlementController : BaseController
    {
        PurchaseEntities db = new PurchaseEntities();
        SettlementService service = new SettlementService();
        QuotationService quotationService = new QuotationService();
        ApplyService applyService = new ApplyService();
        //
        // GET: /Payment/
        public ActionResult SettlementMst()
        {
            return View();
        }
        public ActionResult SettlementDtl()
        {
            return View();
        }
        public ActionResult LoadSettlementMsts(string projectId, string supplierId,string delChk)
        {
            List<SettlementMstDto> lst = service.SettlementMstSearch(projectId, supplierId, UserInfo.UserId, delChk);
            return Json(new { List = lst });
        }
        public ActionResult LoadBugetAmt(string projectId)
        {
            SettlementBugetAmtDto BugetAmtDto = service.SettlementBugetAmtSearch(projectId).FirstOrDefault();
            return Json(new { BugetAmtDto = BugetAmtDto });
        }

        public ActionResult SaveSettlementMsts(string jsonArr)
        {
            MasterService master = new MasterService();
            List<SettlementMstDto> lst = JsonConvert.DeserializeObject<List<SettlementMstDto>>(jsonArr);
            foreach (SettlementMstDto dto in lst)
            {
                service.SettlementMstUpdate(dto.SettlementId, dto.SettleChk, dto.DelChk, UserInfo.UserId);
            }
            return Json("");
        }

        public ActionResult LoadSettlementDtlsForAdd(string projectId, string supplierId, string quotationGroupId)
        {
            SettlementMstDto settlementMstDto = service.SettleDtlMainSearch(projectId, supplierId, "0",quotationGroupId, UserInfo.UserId).FirstOrDefault();
            if (settlementMstDto.ExecuteCycle == null)
            {
                settlementMstDto.ExecuteCycle = "";
            }
            if (settlementMstDto.zhixingchengshi == "全国-")
            {
                settlementMstDto.zhixingchengshi = "全国";
            }
            if (settlementMstDto == null)
            {
                settlementMstDto = new SettlementMstDto();
            }

            List<SettlementDtlDto> lst = new List<SettlementDtlDto>();
            List<SettlementDtlDto> zhuijiaShoudong = new List<SettlementDtlDto>();
            lst = service.SettlementDtlSearch_Before(projectId, supplierId, quotationGroupId, UserInfo.UserId);
            List<SettlementDtlDto> lst2 = new List<SettlementDtlDto>();
            List<SettlementDtlDto> lst3 = new List<SettlementDtlDto>();
            foreach (SettlementDtlDto dto in lst)
            {
                dto.SettlementType = "1";
                lst2.Add(new SettlementDtlDto()
                {
                    ProjectId = dto.ProjectId,
                    SupplierId = dto.SupplierId,
                    Quotationid = dto.Quotationid,
                    SeqNO = dto.SeqNO,
                    danjia = "",
                    FeeContent = dto.FeeContent,
                    SettleCount = dto.SettleCount,
                    Remark = dto.Remark,
                    SettlementType = "2",

                });
                lst3.Add(new SettlementDtlDto()
                {
                    ProjectId = dto.ProjectId,
                    SupplierId = dto.SupplierId,
                    Quotationid = dto.Quotationid,
                    SeqNO = dto.SeqNO,
                    danjia = "",
                    FeeContent = dto.FeeContent,
                    SettleCount = dto.SettleCount,
                    Remark = dto.Remark,
                    SettlementType = "3",

                });
            }
            lst.AddRange(lst2);
            lst.AddRange(lst3);

            //做分次结算以后  每个供应商有多个结算记录，单个结算登记时不需要结算金额
            //SettlementBugetAmtDto bugetAmtDto = service.SettlementBugetAmtSearch(projectId.ToString()).FirstOrDefault();

            return Json(new
            {
                List = lst,
                MainObject = settlementMstDto,
                Exist = false,
                //BugetAmtDto = bugetAmtDto,
                ZuijiaShoudongList = zhuijiaShoudong
            });
        }

        public ActionResult LoadSettlementDtlsForEdit(string projectId, string supplierId, string quotationGroupId, string settlementId)
        {
            SettlementMstDto settlementMstDto = service.SettleDtlMainSearch(projectId, supplierId, settlementId,quotationGroupId, UserInfo.UserId).FirstOrDefault();
            if (settlementMstDto.ExecuteCycle == null)
            {
                settlementMstDto.ExecuteCycle = "";
            }
            if (settlementMstDto.zhixingchengshi == "全国-")
            {
                settlementMstDto.zhixingchengshi = "全国";
            }
            if (settlementMstDto == null)
            {
                settlementMstDto = new SettlementMstDto();
            }

            List<SettlementDtlDto> lst = service.SettlementDtlSearch_After(settlementId, UserInfo.UserId);
            List<SettlementDtlDto> zhuijiaShoudong = service.SettlementDtlSearch_After_shoudong(settlementId, UserInfo.UserId);

            return Json(new
            {
                List = lst,
                MainObject = settlementMstDto,
                Exist = false,
                //BugetAmtDto = bugetAmtDto,
                ZuijiaShoudongList = zhuijiaShoudong
            });
        }

        public ActionResult SettleDtlMainSearch(string projectId, string supplierId, string quotationGroupId, string settlementId, bool settleChk, bool? delChk)
        {
            bool delChk_temp = true ;
            if (delChk == null || delChk == false)
            {
                delChk_temp = false;
            }
            else {
                delChk_temp = true;
            }
            service.SettlementMstUpdate(settlementId, settleChk, delChk_temp, UserInfo.UserId);
            SettlementMstDto settlementMstDto = service.SettleDtlMainSearch(projectId, supplierId, settlementId, quotationGroupId, UserInfo.UserId).FirstOrDefault();
            if (settlementMstDto == null) settlementMstDto = new SettlementMstDto();
            return Json(settlementMstDto);
        }
        public ActionResult SaveSettlementDtls(string jsonArr, string shoudongArr, string ProjectId, string SupplierId, string QuotationGroupId, string SettlementId, string SeqNO_Supplier)
        {
            int mstId = int.Parse(SettlementId);
            if (mstId == 0)
            {
                SettlementMstDto mstDto = new SettlementMstDto()
                {
                    SettlementId = SettlementId,
                    ProjectId = ProjectId,
                    SupplierId = SupplierId,
                    QuotationGroupId = QuotationGroupId
                };
                mstId = service.SettlementMstSave(mstDto, UserInfo.UserId);
            }

            List<SettlementDtlDto> lst = JsonConvert.DeserializeObject<List<SettlementDtlDto>>(jsonArr);
            foreach (SettlementDtlDto dto in lst)
            {
                dto.SettlementId = mstId.ToString();
                service.SettlementDtlSave(dto, UserInfo.UserId);
            }
            List<SettlementDtlDto> shoudongLst = JsonConvert.DeserializeObject<List<SettlementDtlDto>>(shoudongArr);
            foreach (SettlementDtlDto dto in shoudongLst)
            {
                dto.SettlementId = mstId.ToString();
                dto.settlementSeqNO = dto.settlementSeqNO == null ? "0" : dto.settlementSeqNO;
                dto.Quotationid = "0";
                dto.SeqNO = "0";
                dto.SettlementType = "3";
                service.SettlementDtlSave_shoudong(dto, UserInfo.UserId);
            }

            return Json(new { SettlementId=mstId });
        }
        #region SettlementGroup
        public ActionResult GetProjectAmt(string projectId)
        {
            //SettlementBugetAmtDto BugetAmtDto = applyService.ApplyDtl_SettlementBugetAmtSearch(projectId, "").FirstOrDefault();
            SettlementBugetAmtDto BugetAmtDto = service.SettlementBugetAmtSearch(projectId).FirstOrDefault();
            return Json(BugetAmtDto);
        }

        public ActionResult SettlementGroup()
        {
            return View();
        }
        public ActionResult SettlementGroupSearch(string projectId)
        {
            List<SettlementGroupDto> list = service.SettlementGroupSearch(projectId, UserInfo.UserId);

            return Json(new { List = list });
        }
        public ActionResult SettlementGroupSave(string SettlementGroupId, string SettlementGroupName, string projectId)
        {
            if (SettlementGroupId == null)
                SettlementGroupId = "";

            SettlementGroupDto group = new SettlementGroupDto();
            group.GroupId = SettlementGroupId;
            group.GroupName = SettlementGroupName;
            group.ProjectId = projectId;
            service.SettlementGroupSave(group, UserInfo.UserId);

            return Json("");
        }
        public ActionResult SettlementMstByGroupIdSearch(string projectId, string groupId)
        {
            List<SettlementGroupDtlDto> list = service.SettlementMstSearchByGroupId(projectId, groupId);
            return Json(new { List = list });
        }
        public ActionResult SettlementGroupDtlSave(string settlementgroupId, string strselect)
        {
            // 删除当前组下的全部数据
            service.SettlementGroupDtlDelete(settlementgroupId);
            //  添加新的数据
            string[] selectList = strselect.Split(';');
            foreach (string select in selectList)
            {
                if (!string.IsNullOrEmpty(select))
                {
                    SettlementGroupDtlDto dto = new SettlementGroupDtlDto();
                    dto.GroupId = settlementgroupId;
                    dto.SettlementId = select;
                    service.SettlementGroupDtlSave(dto, UserInfo.UserId);
                }
            }
            return Json("");
        }
        public ActionResult RecheckUserSelect()
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(ApplyType.结算单.ToString(), UserInfo.UserId);

            return PartialView("_PartialRecheckUserSelect", lst);
        }
        public ActionResult ApplyCommit(string recheckUserId, Apply apply, string projectId, string groupId, string applyIdexists, string seqNO, string attachArray)
        {
            ApplyService applayService = new ApplyService();
            apply.InDateTime = DateTime.Now;
            apply.ApplyUserId = UserInfo.UserId;
            apply.ApplyTypeId = ApplyType.结算单.ToString();
            apply.ProjectId = Convert.ToInt32(groupId);// 结算单申请时ProjectId 保存为groupId
            int applyId = 0;
            if (Convert.ToInt32(applyIdexists) == 0)
            {
                applyId = applayService.ApplyCommit(apply, recheckUserId);
            }
            else
            {
                applyId = Convert.ToInt32(applyIdexists);
                ApplyRecheckStatus status = new ApplyRecheckStatus();
                status.ApplyId = Convert.ToInt32(applyIdexists);
                status.RecheckUserId = ((Mst_UserInfo)Session["LoginUser"]).UserId;
                status.RecheckStatusCode = "申请";
                status.RecheckReason = apply.ApplyReason == null ? "" : apply.ApplyReason;
                status.SeqNO = Convert.ToInt32(seqNO);
                status.InDateTime = DateTime.Now;
                applayService.ApplyRecheckStatusUpdate(status, recheckUserId);

            }
            applayService.ApplyDtlDelete(new ApplyDtl { ApplyId = applyId, ApplyTypeId = "结算单" });
            List<SettlementGroupDtlDto> list = service.SettlementMstSearchByGroupId(projectId, groupId.ToString()).Where(x=>x.SelectedChk==true).ToList();
            foreach (SettlementGroupDtlDto item in list)
            {
                ApplyDtl applyDtl = new ApplyDtl()
                {
                    ApplyId = applyId,
                    ApplyContentId = Convert.ToInt32(item.SettlementId),
                    ApplyTypeId = ApplyType.结算单.ToString(),
                    InDateTime = DateTime.Now,
                    InUserId = UserInfo.UserId
                };
                // applayService.ApplyDtlSave(applyDtl);
                applyService.ApplyDtlSave_Settlement(applyDtl);
            }
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
                    //mailTo = "".Split(',');
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
        #endregion

        /// <summary>
        /// 清除结算单来源
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearSettlementChk(string projectId, string groupId)
        {
            quotationService.QuotationGroupSettlementUpdate(projectId, groupId, false);
            service.SettlementMstDelete(projectId);
            return Json("");
        }

        public ActionResult EmailSend(int projectId, int supplierId, string SettlementId, string QuotationGroupId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.SupplierId = supplierId;
            Projects Projects = db.Projects.Find(projectId);
            ViewBag.ProjectName = "";
            ViewBag.ProjectShortName = "";
            if (Projects != null)
            {
                ViewBag.ProjectName = Projects.ProjectName;
                ViewBag.ProjectShortName = Projects.ProjectShortName;
                ViewBag.ProjectCode = Projects.ProjectCode;
                ViewBag.ModelType = Projects.ModelType;
            }
            SupplierMng SupplierMng = db.SupplierMng.Find(supplierId);
            ViewBag.SupplierName = "";
            ViewBag.SupplierShortName = "";
            ViewBag.Email = "";
            if (SupplierMng != null)
            {
                ViewBag.SupplierName = SupplierMng.SupplierName;
                ViewBag.SupplierShortName = SupplierMng.SupplierShortName;
                ViewBag.Email = SupplierMng.BussinessMainEmail + "," + SupplierMng.BussinessSecondEmail;
            }
            ViewBag.SettlementId = SettlementId;
            ViewBag.QuotationGroupId = QuotationGroupId;

            return View("EmailSend");
        }


        public ActionResult SaveEmail(SupplierRequirementEmail email, int SettlementId, string attachs, string otherAttachs)
        {
            MasterService mstService = new MasterService();
            UserInfoDto userinfo = mstService.UserInfoSearchByUserId(UserInfo.UserId).FirstOrDefault();

            var findOne = db.SettlementMst.Find(SettlementId);
            if (findOne != null)
            {
                findOne.EmailSendDateTime = DateTime.Now;
                findOne.EmailSendUser = UserInfo.UserId;
            }
            db.SaveChanges();
            try
            {


                //发送邮件
                string[] mailTo = email.Recipients.Split(',');
                string[] mailCC = new string[] { userinfo.Email };
                if (!string.IsNullOrEmpty(email.CCPerson))
                {
                    mailCC = mailCC.Union(email.CCPerson.Split(',')).ToArray();
                }
                string subject = email.Title;
                string content = email.EmailContent == null ? "" : email.EmailContent.Replace("\n", "<br>");

                List<string> attachPaths = new List<string>();
                if (attachs != null)
                {
                    foreach (string attach in attachs.Split(';'))
                    {
                        attachPaths.Add(Server.MapPath("~/Export/Settlement/" + attach));
                    }
                    if (otherAttachs != null)
                    {
                        foreach (string attach in otherAttachs.Split(';'))
                        {
                            attachPaths.Add(Server.MapPath(EmailAttachs + attach));
                        }
                    }
                }

                //个人签名 图片形式
                string sign = "";
                if (!string.IsNullOrEmpty(UserInfo.EmailFooter))
                {
                    string emailSignFile = Server.MapPath("~/EmailSign/" + UserInfo.EmailFooter);
                    sign = emailSignFile;
                }
                //准备发送邮件对象
                ISendMail sendMail = new UseNetMail();
                sendMail.CreateHost(new ConfigHost()
                {
                    EnableSsl = false,
                    Username = userinfo.Email,
                    Password = userinfo.EmailPassword,
                });
                sendMail.CreateMultiMail(new ConfigMail()
                {
                    From = userinfo.Email,
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

            // 发完邮件之后保存供应商评分提醒
            try
            {
                MasterService master = new MasterService();
                master.RemindCancelSave("供应商评分填写提醒", findOne.ProjectId.ToString() + "&" + findOne.SupplierId.ToString(), UserInfo.UserId, DateTime.Now.ToString());
            }
            catch (Exception)
            {
            }


            return Json("");
        }

        string EmailAttachs = "~/EmailAttachs/";
        public ActionResult SaveOtherAttachs()
        {
            List<Object> retFiles = new List<Object>();
            string path = Server.MapPath(EmailAttachs);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if (Request.Files != null)
            {
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase fileBase = Request.Files.Get(key);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fileBase.FileName;
                    string fileFullName = path + fileName;
                    fileBase.SaveAs(fileFullName);
                    retFiles.Add(new { fileName = fileName, fileFullName = fileFullName });
                }
            }

            return Json(retFiles);
        }

        public ActionResult CheckProjectQuotationYusuan(string ProjectId, string QuotationGroupId)
        {
            bool hasData = true;
            List<QuotationTypeDto> lst = quotationService.QuotationTypeSearchByProjectIdAndGroupId(ProjectId, QuotationGroupId);
            foreach (QuotationTypeDto dto in lst)
            {
                hasData = quotationService.QuotationYusuanSaveCheck(ProjectId, QuotationGroupId, dto.QuotationType) > 0;
                if (!hasData) break;
            }
            return Json(hasData);
        }

        public ActionResult QuotationGroupSelect(string projectId)
        {
            var lst = quotationService.QuotationGroupSearch(projectId, UserInfo.UserId).Where(x => x.ApplyStatusCode == "完成").ToList();
            return PartialView("_PartialQuotationGroupSelect", lst);
        }

        public ActionResult SettlementGroupDelete(string GroupId)
        {
            service.SettlementGroupDelete(GroupId);

            return Json("");
        }

        public ActionResult ZhuijiaIndex()
        {
            return View();
        }

        public ActionResult Zhuijia(string ProjectId)
        {
            var lst = service.SettlementDtlSearch_zhuijia(ProjectId);

            return Json(new { List = lst });
        }

        //分配流转单
        public ActionResult LoadFlowOrder(string projectId, string supplierId,string settlementId,bool settleChk)
        {
            List<FlowOrderDto> lst = service.SettlementFlowOrderSearch(projectId, supplierId, settlementId, settleChk, UserInfo.UserId);
            return PartialView("_PartialFlowOrderBySupplier", lst); 
        }

        public ActionResult LoadFlowOrderBySettle(string projectId, string supplierId, string settlementId, bool delChk,bool settleChk)
        {
            List<FlowOrderDto> lst = service.SettlementFlowOrderSearch(projectId, supplierId, settlementId,settleChk, UserInfo.UserId);
            return Json(lst); 
        }
        public ActionResult SettlementMstUpldate(string settlementId, bool delChk, bool settleChk)
        {
            service.SettlementMstUpdate(settlementId, settleChk, delChk, UserInfo.UserId);

            return Json("");
        }
        public ActionResult AllotFlowOrderSave( string settlementId, bool settleChk,bool delChk,string jsonArr)
        {
            // 修改为在勾选的时候进行更新
            //更新最后一次结算和废弃
            //service.SettlementMstUpdate(settlementId,settleChk,delChk,UserInfo.UserId);

            List<string[]> lst = JsonConvert.DeserializeObject<List<string[]>>(jsonArr);
            foreach (string[] arr in lst)
            {
                if (arr.Length < 4) continue;
                string fid = arr[0];
                string factTime = arr[1] == null ? "" : arr[1];
                string amt = arr[2] == null ? "" : arr[2];
                string sid = arr[3];
                service.SettlementFlowOrderUpdate(fid, factTime, amt, sid, UserInfo.UserId);
            }

            return Json("");
        }
    }
}