using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Web.Common;

namespace Purchase.Web.Controllers
{
    public class ConstractTemplateController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        private MasterService service = new MasterService();

        // GET: ConstractTemplate
        public ActionResult Index()
        {
            ViewBag.UserRoleType = UserInfo.RoleTypeCode;
            return View();
        }

        public ActionResult Load(string templateName, int? pageSize, int pageNum)
        {
            List<ConstractTemplate> list = service.ConstractTemplateSearch(templateName,"");
            int total = list.Count();
            int pageCount = CalcPages(total,pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });

            //var query = db.ConstractTemplate.AsQueryable();
            //if (!string.IsNullOrWhiteSpace(templateName))
            //{
            //    query = query.Where(x => x.TemplateName.Contains(templateName));
            //}
            //int total = query.Count();
            //int pageCount = CalcPages(total,pageSize);
            //int start = CalcStartIndex(pageNum);
            //List<ConstractTemplate> lst = query.OrderByDescending(x => x.InDateTime).Skip(start).Take(_countPerPage).ToList();

            //return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        // GET: ConstractTemplate/Create
        public ActionResult Create()
        {
            return View(new ConstractTemplate() { UseChk = true });
        }
        public ActionResult ShowTem()
        {
            return View("ConstractTemplate.html");
        }
        // POST: ConstractTemplate/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConstractTemplate constractTemplate, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                constractTemplate.InDateTime = DateTime.Now;
                constractTemplate.InUserId = UserInfo.UserId;
                db.ConstractTemplate.Add(constractTemplate);
                db.SaveChanges();
                return RedirectToAction("Edit/" + constractTemplate.Id);
            }

            return View(constractTemplate);
        }

        public ActionResult UploadFile(HttpPostedFileBase file, string TemplateCode)
        {
            DeleteFile(TemplateCode);
            return Json(SaveFile(file, TemplateCode), JsonRequestBehavior.AllowGet);
        }

        // GET: ConstractTemplate/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.UserRoleType = UserInfo.RoleTypeCode;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstractTemplate constractTemplate = db.ConstractTemplate.Find(id);
            if (constractTemplate == null)
            {
                return HttpNotFound();
            }
            ViewBag.queryParams = Request.QueryString["queryParams"];
            return View(constractTemplate);
        }

        // POST: ConstractTemplate/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConstractTemplate constractTemplate)
        {
            if (ModelState.IsValid)
            {
                constractTemplate = db.ConstractTemplate.Find(constractTemplate.Id);
                TryUpdateModel<ConstractTemplate>(constractTemplate);

                db.SaveChanges();
                string query = Request.Form["queryParams"];
                return RedirectToAction("Index", new { query = query });
            }
            return View(constractTemplate);
        }

        public ActionResult CopyTemplate(int Id, string TemplateName)
        {
            ConstractTemplate copyOne = new ConstractTemplate()
            {
                TemplateName = TemplateName,
                UseChk = true
            };
            db.ConstractTemplate.Add(copyOne);
            db.SaveChanges();
            //复制模板详细
            foreach (ConstractTemplateDtl ctd in db.ConstractTemplateDtl.Where(x => x.ConstractTemplateId == Id))
            {
                ConstractTemplateDtl copyDtl = new DAL.ConstractTemplateDtl()
                {
                    ConstractTemplateId = copyOne.Id,
                    ContentType = ctd.ContentType,
                    ContentType2 = ctd.ContentType2,
                    SeqNO = ctd.SeqNO,
                    OrderNO = ctd.OrderNO,
                    TemplateContent = ctd.TemplateContent,
                };
                db.ConstractTemplateDtl.Add(copyDtl);
            }
            db.SaveChanges();

            return Json(copyOne);
        }

        // GET: ConstractTemplate/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConstractTemplate constractTemplate = db.ConstractTemplate.Find(id);
            if (constractTemplate == null)
            {
                return HttpNotFound();
            }
            return View(constractTemplate);
        }

        // POST: ConstractTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ConstractTemplate constractTemplate = db.ConstractTemplate.Find(id);
            db.ConstractTemplate.Remove(constractTemplate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region ========= Dtl =======
        public ActionResult ConstractTemplateDtl(string templateId, string seqNO)
        {
            List<ConstractTemplateDtl> list = service.ConstractTemplateDtlSearch(templateId, seqNO);
            return PartialView("_PartialConstractTemplateDetail", list);
        }

        public ActionResult GetConstractTemplateDtls(string Id)
        {
            List<ConstractTemplateDtl> list = service.ConstractTemplateDtlSearch(Id, "");
            //List<ConstractTemplate> Title = service.ConstractTemplateSearch()
            return Json(new { List = list });
        }

        public ActionResult GetConstractTemplateDtlsForFixContent(string Id)
        {
            List<ConstractTemplateDtl> list = service.ConstractTemplateDtlSearch(Id, "");
            //List<ConstractTemplate> Title = service.ConstractTemplateSearch()
            return Json(new { List = list.Where(x=> !string.IsNullOrEmpty(x.ContentType2)) });
        }

        public ActionResult DeleteConstractTemplateDtl(ConstractTemplateDtl dtl)
        {
            service.ConstractTemplateDtlDelete(dtl);
            return Json("");
        }
        public ActionResult SaveConstractTemplateDtl(ConstractTemplateDtl dtl, int isInsert)
        {
            dtl.InDateTime = DateTime.Now;
            dtl.InUserId = UserInfo.UserId;
            if (dtl.ContentType == "图片")
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    HttpPostedFileBase fileBase = Request.Files.Get(0);
                    if (fileBase.ContentLength > 0)
                    {
                        string fileName = SaveFile(fileBase, dtl.ConstractTemplateId + "_" + dtl.SeqNO);
                        dtl.TemplateContent = fileName;
                    }
                }
            }
            if (isInsert == 1)
            {
                service.ConstractTemplateDtlInsert(dtl);
            }
            else
            {
                service.ConstractTemplateDtlSave(dtl);
            }

            return RedirectToAction("Edit/" + dtl.ConstractTemplateId);
        }

        public ActionResult ExportConstractTemplate(string ConstractTemplateId)
        {
            if (Request.Files != null)
            {
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase fileBase = Request.Files.Get(key);
                    string fileName = SaveFile(fileBase, ConstractTemplateId);
                    ConstractTemplateExport.Export(ConstractTemplateId, GetFullFileName(fileName), UserInfo.UserId);

                }
            }

            return RedirectToAction("Edit/" + ConstractTemplateId);
        }
        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
