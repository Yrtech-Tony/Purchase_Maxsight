using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using Purchase.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class ConstractController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        private MasterService service = new MasterService();
        // GET: Constract
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Load(string serviceTrade, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string constractName, int? pageSize, int pageNum)
        {
            List<ConstractDto> list = service.ConstractSearch(serviceTrade, projectCode, projectName, projectShortName, "", constractName, supplierCode, supplierName, supplierShortName, UserInfo.UserId);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }
        public ActionResult Create()
        {
            return View(new ConstractDto());
        }
        // POST: Constract/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Constract constract)
        {
            constract.InDateTime = DateTime.Now;
            constract.InUserId = UserInfo.UserId;
            db.Constract.Add(constract);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.queryParams = Request.QueryString["queryParams"];
            ConstractDto constract = new ConstractDto() { ConstractId = id.Value.ToString() };
            return View(constract);
        }
        // POST: Constract/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            Constract constract = db.Constract.Find(id);
            TryUpdateModel<Constract>(constract);
            db.SaveChanges();
            string queryParams = Request.Form["queryParams"];
            return RedirectToAction("Index", new { queryParams = queryParams });
        }

        public ActionResult FindOne(int id)
        {
            ConstractDto constract = service.ConstractByIdSearch(id.ToString(), UserInfo.UserId).FirstOrDefault();
            return Json(constract, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SupplierMngSelect(string SupplierCode, string SupplierName, string SupplierShortName, string province, string city)
        {
            MasterService masterService = new MasterService();
            List<SupplierDto> lst = masterService.SupplierPopupSearch(SupplierCode, SupplierName, SupplierShortName, province, city);

            return PartialView("_PartialSupplierMngSelect", lst);
        }
        public ActionResult SupplierSelectSearch(string SupplierCode, string SupplierName, string SupplierShortName, string province, string city)
        {
            MasterService masterService = new MasterService();
            List<SupplierDto> lst = masterService.SupplierPopupSearch(SupplierCode, SupplierName, SupplierShortName, province, city);

            return Json(new { List = lst });
        }

        public ActionResult RecheckUserSelect()
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(ApplyType.合同.ToString(), UserInfo.UserId);

            return PartialView("_PartialRecheckUserSelect", lst);
        }
        public ActionResult ApplyCommit(string recheckUserId, Apply apply, int ConstractId, string applyIdexists, string seqNO, string attachArray)
        {            
            ApplyService applayService = new ApplyService();
            apply.InDateTime = DateTime.Now;
            apply.ApplyUserId = UserInfo.UserId;
            apply.ApplyTypeId = ApplyType.合同.ToString();
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
                status.RecheckUserId = UserInfo.UserId;
                status.RecheckStatusCode = "申请";
                status.RecheckReason = apply.ApplyReason == null ? "" : apply.ApplyReason;
                status.SeqNO = Convert.ToInt32(seqNO);
                status.InDateTime = DateTime.Now;
                applayService.ApplyRecheckStatusUpdate(status, recheckUserId);
                applyId = Convert.ToInt32(applyIdexists);
            }
            applayService.ApplyDtlDelete(new ApplyDtl { ApplyId = applyId });
            ApplyDtl applyDtl = new ApplyDtl()
            {
                ApplyId = applyId,
                ApplyContentId = ConstractId,
                ApplyTypeId = ApplyType.合同.ToString(),
                InDateTime = DateTime.Now,
                InUserId = UserInfo.UserId
            };
            applayService.ApplyDtlSave(applyDtl);
            MyApplyDto applyInfo = applayService.ApplySearchById(applyId.ToString()).FirstOrDefault();
            ApplyEmailSend(applyInfo, recheckUserId);

            //保存附件
            List<string> attachList = JsonConvert.DeserializeObject<List<string>>(attachArray);
            int fileSeqNO = 1;
            attachList.ForEach((string attach)=>{
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
        public ActionResult TemplateSelect(string TemplateName, string TemplateType)
        {
            MasterService masterService = new MasterService();
            List<ConstractTemplate> lst = masterService.ConstractTemplateSearch(TemplateName, TemplateType);

            return PartialView("_PartialTemplateSelect", lst);
        }
        public ActionResult TemplateSearch(string TemplateName, string TemplateType)
        {
            MasterService masterService = new MasterService();
            List<ConstractTemplate> lst = masterService.ConstractTemplateSearch(TemplateName, TemplateType);
            return Json(new { List = lst });
        }

        public ActionResult ConstractDtlEdit(int? id, int TemplateId)
        {
            ConstractDtl constractDtl = new ConstractDtl();
            if (id.HasValue)
            {
                constractDtl = db.ConstractDtl.Find(id.Value);
            }
            ViewBag.TableIds = new SelectList(SearchConstractTemplateTables(TemplateId));
            return PartialView("_PartialConstractDtl", constractDtl);
        }
        public ActionResult GetConstractTemplateTables(int TemplateId)
        {
            return Json(SearchConstractTemplateTables(TemplateId));
        }

        private List<string> SearchConstractTemplateTables(int templateId)
        {
            var lst = db.ConstractTemplateDtl.Where(x => x.ConstractTemplateId == templateId && x.ContentType == "表格")
                .Select(x => "table-" + x.OrderNO).ToList();

            return lst;
        }

        public ActionResult ConstractDtlSave(int? id)
        {
            ConstractDtl constractDtl = new ConstractDtl();
            if (id.HasValue)
            {
                constractDtl = db.ConstractDtl.Find(id.Value);
                TryUpdateModel<ConstractDtl>(constractDtl);
            }
            else
            {
                TryUpdateModel<ConstractDtl>(constractDtl);
                constractDtl.InDateTime = DateTime.Now;
                constractDtl.InUserId = UserInfo.UserId;
                db.ConstractDtl.Add(constractDtl);
            }
            db.SaveChanges();
            return Json("");
        }
        private string[] SplitString(string value, int length)
        {
            string[] retArr = new string[length];
            if (value == null)
            {
                return retArr;
            }
            value.Split(';').CopyTo(retArr, 0);
            return retArr;
        }
        public ActionResult ConstractDtlSaveBatch(ConstractDtl constractDtl)
        {
            string[] tableIds = constractDtl.TableId.Split(';');
            string[] column1s = SplitString(constractDtl.Column1, tableIds.Length);
            string[] column2s = SplitString(constractDtl.Column2, tableIds.Length);
            string[] column3s = SplitString(constractDtl.Column3, tableIds.Length);
            string[] column4s = SplitString(constractDtl.Column4, tableIds.Length);
            string[] column5s = SplitString(constractDtl.Column5, tableIds.Length);
            string[] column6s = SplitString(constractDtl.Column6, tableIds.Length);
            string[] column7s = SplitString(constractDtl.Column7, tableIds.Length);
            string[] column8s = SplitString(constractDtl.Column8, tableIds.Length);
            string[] column9s = SplitString(constractDtl.Column9, tableIds.Length);
            string[] column10s = SplitString(constractDtl.Column10, tableIds.Length);

            db.Database.ExecuteSqlCommand("delete from ConstractDtl where ConstractId=" + constractDtl.ConstractId);

            for (int i = 0; i < tableIds.Length; i++)
            {
                ConstractDtl item = new ConstractDtl();
                item.ConstractId = constractDtl.ConstractId;
                item.TableId = tableIds[i];
                item.Column1 = column1s[i];
                item.Column2 = column2s[i];
                item.Column3 = column3s[i];
                item.Column4 = column4s[i];
                item.Column5 = column5s[i];
                item.Column6 = column6s[i];
                item.Column7 = column7s[i];
                item.Column8 = column8s[i];
                item.Column9 = column9s[i];
                item.Column10 = column10s[i];
                item.InDateTime = DateTime.Now;
                item.InUserId = UserInfo.UserId;

                db.ConstractDtl.Add(item);
            }
            db.SaveChanges();

            return Json("");
        }

        public ActionResult ConstractDtlSearch(int ConstractId)
        {
            var lst = db.ConstractDtl.Where(x => x.ConstractId == ConstractId).OrderBy(x => x.TableId).ToList();

            return Json(lst);
        }
        public ActionResult GetConstractQuotations(string QuotationIdList)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            string[] quotations = QuotationIdList.Split(';');
            List<QuotationDtlBase> mstDtos = new List<QuotationDtlBase>();
            foreach (string quotation in quotations)
            {
                if (quotation.Contains(","))
                {
                    string[] items = quotation.Split(',');
                    QuotationDtlBase mstDto = new QuotationDtlBase()
                    {
                        QuotationId = items[0],
                        QuotationType = items[1],
                        SeqNO = int.Parse(items[2])
                    };
                    mstDtos.Add(mstDto);
                }
            }
            List<QuotationDtlBase> mstDtos2 = mstDtos.GroupBy(x => new { x.QuotationId, x.QuotationType }).Select(g => new QuotationDtlBase
            {
                QuotationId = g.Key.QuotationId,
                QuotationType = g.Key.QuotationType,
                SeqNOs = g.Select(item => item.SeqNO)
            }).ToList();
            foreach (QuotationDtlBase qbase in mstDtos2)
            {
                Object model = null;
                int QuotationId = int.Parse(qbase.QuotationId);
                if (qbase.QuotationType == "Biancheng")
                {
                    model = db.Quotation_Biancheng_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }
                else if (qbase.QuotationType == "Zhixing")
                {
                    model = db.Quotation_Zhixing_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }
                else if (qbase.QuotationType == "Fuhe")
                {
                    model = db.Quotation_Fuhe_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }
                else if (qbase.QuotationType == "Yanjiu")
                {
                    model = db.Quotation_Yanjiu_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }
                else if (qbase.QuotationType == "Qita1")
                {
                    model = db.Quotation_Qita1_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }
                else if (qbase.QuotationType == "Qita2")
                {
                    model = db.Quotation_Qita2_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }
                else if (qbase.QuotationType == "Chezhan")
                {
                    model = db.Quotation_Chezhan_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }
                else if (qbase.QuotationType == "Zhichi")
                {
                    model = db.Quotation_Zhichi_Dtl.Where(x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)).ToList();
                }

                dic.Add(qbase.QuotationType, model);
            }


            return PartialView("_PartialConstractQuotations", dic);
        }


        [ValidateInput(false)]
        public ActionResult ChangeToPdf(string html, string constractName)
        {
            string fileName = constractName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            string path = Request.MapPath("~/ConstractPdf/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            HtmlToPDFTool.StringToPDF(html, path + fileName);
            return Json(new { Status = true, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmailSend(string id)
        {
            ConstractDto dto = service.ConstractByIdSearch1(id).FirstOrDefault();
            return View(dto);
        }
        string EmailAttachs = "~/EmailAttachs/";
        public ActionResult SaveEmail(SupplierRequirementEmail email, string constractId, string attachs, string otherAttachs)
        {
            MasterService mstService = new MasterService();
            UserInfoDto userinfo = mstService.UserInfoSearchByUserId(UserInfo.UserId).FirstOrDefault();

            // 保存合同发送的信息
            service.ConstractEmailSendSave(constractId, UserInfo.UserId);

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
                    attachPaths.Add(Server.MapPath("~/ConstractPdf/" + attach));
                }
            }
            if (otherAttachs != null)
            {
                foreach (string attach in otherAttachs.Split(';'))
                {
                    attachPaths.Add(Server.MapPath(EmailAttachs + attach));
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

            return Json("");
        }

        public ActionResult LoadConstractDtlCommonText(string ConstractId, string ConstractTemplateId)
        {
            List<ConstractCommonTextDto> lst = service.ConstractCommonTextSearch(ConstractId, ConstractTemplateId);
            return Json(lst);
        }

        public ActionResult SaveConstractDtlCommonText(string jsonArr)
        {
            List<ConstractCommonTextDto> lst = JsonConvert.DeserializeObject<List<ConstractCommonTextDto>>(jsonArr);

            if (lst != null)
            {
                foreach (ConstractCommonTextDto item in lst)
                {
                    service.ConstractCommonTextSave(item.CommonTextId, item.ConstractId, item.ConstractTemplateId, item.SeqNO, item.ContentTxt, item.TemplateContent, UserInfo.UserId);
                }
            }
            return Json(lst);
        }

        public ActionResult ConstractViewDtlSearch(string ConstractId, string ConstractTemplateId)
        {
            ConstractTemplate ct = db.ConstractTemplate.Find(int.Parse(ConstractTemplateId));
            List<ConstractTemplateDtl> lst = service.ConstractViewDtlSearch(ConstractTemplateId, ConstractId);
            return Json(new { ConstractTemplate = ct, TemplateDtls = lst });
        }

        public ActionResult QuotationSelect(string projectId, string supplierId)
        {
            List<QuotationMstDto> lst = service.Constract_QuotationTypeSearch(projectId, supplierId);
            return PartialView("_PartialQuotationSelect", lst);
        }

        public ActionResult LoadQuotation(int QuotationId, string QuotationType)
        {
            Object model = null;
            if (QuotationType == "Biancheng")
            {
                model = db.Quotation_Biancheng_Dtl.Where(x => x.QuotationId == QuotationId && x.danjia != null && x.shuliang != null).ToList();
                // x => x.QuotationId == QuotationId && qbase.SeqNOs.Contains(x.SeqNO)
            }
            else if (QuotationType == "Zhixing")
            {
                model = db.Quotation_Zhixing_Dtl.Where(x => x.QuotationId == QuotationId && x.UnitPrice != null && x.Count != null).ToList();
            }
            else if (QuotationType == "Fuhe")
            {
                model = db.Quotation_Fuhe_Dtl.Where(x => x.QuotationId == QuotationId && x.danjia != null && x.shuliang != null).ToList();
            }
            else if (QuotationType == "Yanjiu")
            {
                model = db.Quotation_Yanjiu_Dtl.Where(x => x.QuotationId == QuotationId && x.danjia != null && x.shuliang != null).ToList();
            }
            else if (QuotationType == "Qita1")
            {
                model = db.Quotation_Qita1_Dtl.Where(x => x.QuotationId == QuotationId && x.danjia != null && x.shuliang != null).ToList();
            }
            else if (QuotationType == "Qita2")
            {
                model = db.Quotation_Qita2_Dtl.Where(x => x.QuotationId == QuotationId && x.danjia != null && x.shuliang != null).ToList();
            }
            else if (QuotationType == "Chezhan")
            {
                model = db.Quotation_Chezhan_Dtl.Where(x => x.QuotationId == QuotationId && x.danjia != null && x.shuliang != null).ToList();
            }
            else if (QuotationType == "Zhichi")
            {
                model = db.Quotation_Zhichi_Dtl.Where(x => x.QuotationId == QuotationId && x.danjia != null && x.shuliang != null).ToList();
            }

            return PartialView("PartialQuotation/_PartialLoad" + QuotationType, model);
        }

        public ActionResult DeleteConstract(string ConstractId)
        {
            MasterService masterService = new MasterService();
            masterService.ConstractDelete(ConstractId);

            return Json("");
        }

        public ActionResult CopyConstract(string ConstractId)
        {
            MasterService masterService = new MasterService();
            masterService.ConstractCopy(ConstractId, UserInfo.UserId);

            return Json("");
        }
    }
}