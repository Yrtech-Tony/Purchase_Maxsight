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
    public class SupplierEvaOldController : BaseController
    {
        PurchaseEntities db = new PurchaseEntities();
        SupplierEvaService service = new SupplierEvaService();
        //
        // GET: /AccruedCharges/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SupplierEvaAna()
        {
            return View();
        }


        public ActionResult LoadSupplierEvaAnwserAna(SupplierEvaAnaDto supplierEvaAnaDto)
        {
            List<SupplierEvaDto> list = service.SupplierEvaAnwserAnaSearch(supplierEvaAnaDto.SubjectType,
                supplierEvaAnaDto.ProjectName,supplierEvaAnaDto.ProjectShortName,supplierEvaAnaDto.SupplierName);

            return Json(new { List = list });
        }
        public ActionResult Load(string projectName, string projectShortName, string supplierName, int pageNum, int? pageSize)
        {
            List<SupplierEvaDto> list = service.SupplierEvaAnwserSearch(projectName, projectShortName, supplierName);
            if (string.IsNullOrEmpty(supplierName))
            {
                int total = list.Count();
                int pageCount = CalcPages(total,pageSize);
                int start = CalcStartIndex(pageNum);
                list = list.Skip(start).Take(_countPerPage).ToList();

                return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
            }
            else
            {
                return Json(new { List = list });
            }           
        }

        [HttpPost]
        public ActionResult Save(string jsonArr)
        {
            List<SupplierEvaAnswer> SupplierEvaAnswers = JsonConvert.DeserializeObject<List<SupplierEvaAnswer>>(jsonArr);
            foreach (SupplierEvaAnswer answer in SupplierEvaAnswers)
            {
                if (!answer.SupplierScore.HasValue) continue;
                service.SupplierEvaAnswerSave(answer, UserInfo.UserId);
            }

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