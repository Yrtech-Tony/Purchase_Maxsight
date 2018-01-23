using Newtonsoft.Json;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class SupplierEvaController : BaseController
    {
        PurchaseEntities db = new PurchaseEntities();
        SupplierEvaService service = new SupplierEvaService();
        //
        // GET: /AccruedCharges/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string SubjectType,string ProjectId,string SupplierName)
        {
            List<SupplierEvaDto> lst = service.SupplierEvaAnwserSearch(ProjectId,SubjectType,SupplierName);
            return PartialView("_PartialCreate", lst);
        }

        public ActionResult Load(string SubjectType, string projectName, string projectShortName, string supplierName, int pageNum, int? pageSize)
        {
            List<SupplierEvaDto> list = service.SupplierEvaAnwserAnaSearch(SubjectType, projectName, projectShortName, supplierName);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });          
        }

        [HttpPost]
        public ActionResult Save(int ProjectId, string jsonArr)
        {
            List<SupplierEvaAnswer> SupplierEvaAnswers = JsonConvert.DeserializeObject<List<SupplierEvaAnswer>>(jsonArr);
            foreach (SupplierEvaAnswer answer in SupplierEvaAnswers)
            {
                if (!answer.SupplierScore.HasValue) continue;
               
                if (answer.Id > 0)
                {
                    answer.ModifyDateTime = DateTime.Now;
                    answer.ModifyUserId = UserInfo.UserId;
                    db.Entry<SupplierEvaAnswer>(answer).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    answer.ProjectId = ProjectId;
                    answer.InDateTime = DateTime.Now;
                    answer.InUserId = UserInfo.UserId;
                    db.SupplierEvaAnswer.Add(answer);
                }                
            }
            db.SaveChanges();

            return Json("");
        }

        public ActionResult Delete(int AnswerId)
        {
            SupplierEvaAnswer supplierAnswer = db.SupplierEvaAnswer.Find(AnswerId);
            if (supplierAnswer != null)
            {
                db.SupplierEvaAnswer.Remove(supplierAnswer);
                db.SaveChanges();
            }            

            return Json("");
        }
    }
}