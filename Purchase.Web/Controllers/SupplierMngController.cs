using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Purchase.DAL;
using Purchase.Common;
using System.IO;
using System.Data.Entity.Validation;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using Newtonsoft.Json;

namespace Purchase.Web.Controllers
{
    public class SupplierMngController : BaseController
    {
        string defalutCode = "9999";
        private PurchaseEntities db = new PurchaseEntities();
        private MasterService service = new MasterService();

        // GET: /SupplierMng/
        public ActionResult Index(string query)
        {
            ViewBag.queryParams = query;
            ViewBag.UserRoleType = UserInfo.RoleTypeCode;
            return View();
        }
        public ActionResult BankInfo()
        {
            return View();
        }
        public ActionResult LoadBank(SupplierMng condition, int pageNum, int? pageSize)
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss";
            if (pageSize.HasValue)
            {
                _countPerPage = pageSize.Value;
            }
            //var query = db.SupplierMng.OrderByDescending(x => x.BankModifyDateTime).Select(s => new
            //    {
            //        s.Id,
            //        s.SupplierCode,
            //        s.SupplierType,
            //        s.SupplierName,
            //        s.SupplierShortName,
            //        s.RecommendDepartment,
            //        s.CooperationState,
            //        s.AccountName,
            //        s.AccountBankFullName,
            //        s.AccountBankNo,
            //        s.VATRate,
            //        s.ServiceTrade,
            //        s.Remark,
            //        s.PurchaseType,
            //        s.Province,
            //        s.City,
            //        s.InDateTime,
            //        s.InUserId,
            //        s.BankModifyColumn,
            //        s.BankModifyDateTime
            //    }).AsQueryable();

            //#region ====查询条件============
            //if (condition != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(condition.SupplierCode))
            //    {
            //        query = query.Where(x => x.SupplierCode.Contains(condition.SupplierCode));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.SupplierName))
            //    {
            //        query = query.Where(x => x.SupplierName.Contains(condition.SupplierName));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.SupplierShortName))
            //    {
            //        query = query.Where(x => x.SupplierShortName.Contains(condition.SupplierShortName));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.CooperationState))
            //    {
            //        query = query.Where(x => x.CooperationState.Contains(condition.CooperationState));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.ServiceTrade))
            //    {
            //        query = query.Where(x => x.ServiceTrade.Contains(condition.ServiceTrade));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.PurchaseType))
            //    {
            //        query = query.Where(x => x.PurchaseType.Contains(condition.PurchaseType));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.Province))
            //    {
            //        query = query.Where(x => x.Province.Contains(condition.Province));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.City))
            //    {
            //        query = query.Where(x => x.City.Contains(condition.City));
            //    }
            //}
            //#endregion
            //int total = query.Count();
            //int pageCount = CalcPages(total,pageSize);
            //int start = CalcStartIndex(pageNum);
            //var lst = query.OrderByDescending(x => x.InDateTime).Skip(start).Take(_countPerPage).ToList();
            List<SupplierDto> list = service.SupplierBankInfoSearch(condition, ((Mst_UserInfo)Session["LoginUser"]).UserId);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });

            //return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }
        public ActionResult Load(SupplierMng condition, string applyStatusCode, int pageNum, int? pageSize)
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss";
            if (pageSize.HasValue)
            {
                _countPerPage = pageSize.Value;
            }
            List<SupplierDto> list = service.SupplierSearch(condition, ((Mst_UserInfo)Session["LoginUser"]).UserId, applyStatusCode);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }
        // GET: ConstractTemplate/Create
        public ActionResult Create()
        {
            ViewBag.UserRoleType = UserInfo.RoleTypeCode;
            return View(new SupplierMng());
        }
        public ActionResult SupplierSearchByName(string supplierName, string serviceTrade)
        {
            List<SupplierDto> list = service.SupplierSearchBySupplierName(supplierName, serviceTrade);
            return Json(new { List = list });
        }
        private string GetSupplierCode(string City)
        {
            if (City == null) return defalutCode;
            string sCode = "";
            //生成供应商编号逻辑：
            //1:根据city找城市区号
            //2:如果找不到区号使用默认的区号9999
            //3:去数据库中查询供应商编号以这个区号开头的有几条记录，
            //供应商编号 = 城市区号+（记录数+1）
            sCode = CityCodes.Where(x => City.Contains(x.Name)).Select(x => x.Zip).FirstOrDefault();
            if (string.IsNullOrEmpty(sCode))
            {
                sCode = defalutCode;
            }

            int count = db.SupplierMng.Where(x => x.SupplierCode.StartsWith(sCode)).Count();
            sCode += (count + 1).ToString().PadLeft(4, '0');
            return sCode;
        }
        // POST: ConstractTemplate/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierMng supplierMng)
        {
            try
            {
                //if (ModelState.IsValid)
                //{

                List<SupplierDto> list = service.SupplierSearchBySupplierName(supplierMng.SupplierName, supplierMng.ServiceTrade);
                if (list.Count == 0)
                {

                    supplierMng.SupplierCode = GetSupplierCode(supplierMng.City);
                    db.SupplierMng.Add(supplierMng);
                    supplierMng.InDateTime = DateTime.Now;
                    supplierMng.InUserId = UserInfo.UserId;
                    supplierMng.UseChk = true;
                    supplierMng.ModifyUserId = UserInfo.UserId;
                    supplierMng.ModifyDateTime = DateTime.Now;

                    db.SaveChanges();
                    service.SupplierModifySave(supplierMng.Id.ToString(), UserInfo.UserId, "", true); // 新增的时候对财务人员保存一条提醒记录
                    SavePng(supplierMng);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                //}
                //else
                //{ 

                //}
                return View(supplierMng);
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
        }

        private void SavePng(SupplierMng supplierMng)
        {
            supplierMng.SupplierMngAttachmentFile.Clear();
            string[] attachs = Request.Form["AttachPngs"].Split(',');
            for (int i = 0; i < attachs.Length; i++)
            {
                int seqNo = i + 1;
                SupplierMngAttachmentFile attach = new SupplierMngAttachmentFile();
                attach.FileName = attachs[i];
                attach.SeqNO = seqNo;
                attach.SupplierId = supplierMng.Id;
                attach.SupplierMng = supplierMng;
                attach.UploadChk = !string.IsNullOrWhiteSpace(attach.FileName);
                attach.InDateTime = DateTime.Now;
                attach.InUserId = UserInfo.InUserId;

                supplierMng.SupplierMngAttachmentFile.Add(attach);
            }
        }

        public ActionResult SaveAttachPng()
        {
            string fileName = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                string key = Request.Files.GetKey(i);
                HttpPostedFileBase file = Request.Files.Get(key);
                fileName = SaveFile(file, DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            return Json(new { fileName = fileName, fileFullName = GetFullFileName(fileName) });
        }

        // GET: /SupplierMng/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.queryParams = Request.QueryString["queryParams"];
            ViewBag.UserRoleType = UserInfo.RoleTypeCode;
            SupplierMng suppliermng = new SupplierMng() { Id = id.Value };
            return View(suppliermng);
        }
        public ActionResult FindOne(int id)
        {
            //SupplierMng suppliermng = db.SupplierMng.Find(id);
            List<SupplierDto> list = service.SupplierSearchById(id.ToString(), UserInfo.UserId);
            SupplierDto suppliermng = new SupplierDto();
            if (list.Count > 0)
            {
                suppliermng = list[0];
            }
            return Json(suppliermng, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FindOnePngs(int id)
        {
            List<Object> retData = new List<object>();
            var attachs = db.SupplierMngAttachmentFile.Where(x => x.SupplierId == id);
            foreach (var attach in attachs)
            {
                retData.Add(new { fileName = attach.FileName, fileFullName = GetFullFileName(attach.FileName) });
            }
            return Json(retData, JsonRequestBehavior.AllowGet);
        }
        // POST: /SupplierMng/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                SupplierMng supplierMng = new SupplierMng();
                //if (ModelState.IsValid)
                //{
                supplierMng = db.SupplierMng.Find(id);

                // 不是临时保存的时候，如果变更了银行信息则需要保留一条提醒，如果是临时保存不需要进操作
                if (supplierMng.TempChk != "Y")
                {
                    string changePros = "";
                    bool isChange = false;
                    string[] propertys = { "SupplierName", "AccountName", "AccountBankFullName", "AccountBankNo", "AccountBankAddress" };
                    foreach (string pro in propertys)
                    {
                        var value = supplierMng.GetType().GetProperty(pro).GetValue(supplierMng);
                        string valueString = value == null ? "" : value.ToString();
                        if (collection.Get(pro) != valueString)
                        {
                            changePros += pro + ",";
                            isChange = true;
                        }
                    }
                    if (isChange)
                    {
                        changePros = changePros.Substring(0, changePros.Length - 1);
                        //supplierMng.BankModifyDateTime = DateTime.Now;
                        //supplierMng.BankModifyColumn = changePros;
                        service.SupplierModifySave(supplierMng.Id.ToString(), UserInfo.UserId, changePros, false); // 修改的时候对财务人员保存一条提醒记录
                    }
                }
                supplierMng.ModifyDateTime = DateTime.Now;
                supplierMng.ModifyUserId = UserInfo.UserId;
                TryUpdateModel<SupplierMng>(supplierMng);
                // 如果供应商代码为9999，有可能是没有选择城市，所有重新给供应商编码赋值。
                if (supplierMng.SupplierCode == "9999")
                {
                    supplierMng.SupplierCode = GetSupplierCode(supplierMng.City);
                }

                SavePng(supplierMng);
                db.SaveChanges();

                string queryParams = Request.Form["queryParams"];
                return RedirectToAction("Index", new { queryParams = queryParams });

                //}
                return View(supplierMng);
            }
            catch (Exception e)
            {
                return View(e.Message.ToString());
            }

        }
        public ActionResult ClearBankModifyColumn(int id)
        {
            if (!UserInfo.RoleTypeCode.Contains("财务基本权限"))
            {
                throw new Exception("财务人员才可以操作");
            }
            service.SupplierModifyAlterDelete(id.ToString(), UserInfo.UserId);
            //SupplierMng supplierMng = db.SupplierMng.Find(id);
            //if (supplierMng != null)
            //{
            //    supplierMng.BankModifyColumn = "";
            //}
            //db.SaveChanges();

            return Json("");
        }
        public ActionResult GetPng(string fileName)
        {
            return File(GetFullFileName(fileName), "image/jpeg");
        }

        public ActionResult UploadFile(HttpPostedFileBase file, string SupplierCode)
        {
            return Json(SaveFile(file, SupplierCode), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAttachs(int id)
        {
            List<SupplierMngAttachmentFile> attachs = db.SupplierMngAttachmentFile.Where(x => x.SupplierId == id).ToList();
            return Json(attachs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAttach(int id, int seqNo)
        {
            SupplierMngAttachmentFile attach = db.SupplierMngAttachmentFile.Where(x => x.SupplierId == id && x.SeqNO == seqNo).FirstOrDefault();
            db.SupplierMngAttachmentFile.Remove(attach);
            db.SaveChanges();
            // DeleteFile(id + "_" + seqNo);
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult RecheckUserSelect()
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(ApplyType.供应商.ToString(), UserInfo.UserId);

            return PartialView("_PartialRecheckUserSelect", lst);
        }

        public ActionResult ApplyCommit(string recheckUserId, Apply apply, int SupplierMngId, string applyIdexists, string seqNO, string attachArray)
        {
            ApplyService applayService = new ApplyService();
            apply.InDateTime = DateTime.Now;
            apply.ApplyUserId = UserInfo.UserId;
            apply.ApplyTypeId = ApplyType.供应商.ToString();
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
            applayService.ApplyDtlDelete(new ApplyDtl { ApplyId = applyId, ApplyTypeId = "供应商" });
            ApplyDtl applyDtl = new ApplyDtl()
            {
                ApplyId = applyId,
                ApplyContentId = SupplierMngId,
                ApplyTypeId = ApplyType.供应商.ToString(),
                InDateTime = DateTime.Now,
                InUserId = UserInfo.UserId
            };
            applayService.ApplyDtlSave(applyDtl);
            applayService.ApplyDtlSave_SupplierMng(applyDtl);

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
        // POST: /SupplierMng/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupplierMng suppliermng = db.SupplierMng.Find(id);
            db.SupplierMng.Remove(suppliermng);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult LoadSuppplierService(string serviceTrade, string supplierType, string departmentCode)
        {
            string type = "供应商-" + serviceTrade + "-" + supplierType + "-提供服务";
            if (supplierType != "业务采购")
            {
                if (supplierType.StartsWith("其他"))
                {
                    supplierType = "其他";
                }
                type = "供应商-" + serviceTrade + "-" + departmentCode + "-提供服务" + "-" + supplierType;
            }
            ViewBag.Data = SelectListTool.HiddenCodeList(type).Where(x => x.Text != "").Select(x => x.Value).ToList();
            return PartialView("_PartialProvideServiceSelect", "");
        }

        public ActionResult SupplierMngCopy(string supplierId)
        {
            service.SupplierMngCopy(supplierId, UserInfo.UserId);
            return Json("");
        }

        string tempPath = "~/CommonTemplate/";
        string basePath = "~/Export/Supplier/";
        public ActionResult ExportSupplier(SupplierMng condition, string applyStatusCode)
        {
            List<SupplierDto> list = service.SupplierSearch(condition, ((Mst_UserInfo)Session["LoginUser"]).UserId, applyStatusCode);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "供应商_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "供应商.xlsx");
            System.IO.File.Copy(templateFile, path);
            SupplierMngExport export = new SupplierMngExport();
            export.ExportSupplier(path, list);

            return Json(new { ExportPath = path });
        }

        public ActionResult ExportSupplierBankInfo(SupplierMng condition)
        {
            List<SupplierDto> list = service.SupplierBankInfoSearch(condition, ((Mst_UserInfo)Session["LoginUser"]).UserId);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "供应商银行信息_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "供应商银行信息.xlsx");
            System.IO.File.Copy(templateFile, path);
            SupplierMngExport export = new SupplierMngExport();
            export.ExportBankInfo(path, list);

            return Json(new { ExportPath = path });
        }

        public ActionResult GetAllSuppliers()
        {
            MasterService masterService = new MasterService();
            List<SupplierDto> lst = masterService.SupplierPopupSearch("", "", "", "", "");

            return Json(lst);
        }
    }
}
