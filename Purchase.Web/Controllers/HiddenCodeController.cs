using Purchase.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class HiddenCodeController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        // GET: HiddenCode
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Load(string type, string name, int pageNum = 1, int pageSize = 10)
        {
            var query = db.HiddenCode.AsQueryable();
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(x => x.Type == type);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name == name);
            }
            query = query.OrderBy(x => x.Name);
            int total = query.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            var list = query.Skip(start).Take(_countPerPage).ToList();

            HiddenCodeType codeType = db.HiddenCodeType.Where(x => x.TypName == type).FirstOrDefault();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum, ChildChk = codeType != null ? codeType.ChildChk : false });
        }
        public ActionResult Edit(string type, string code)
        {
            HiddenCode findOne = new HiddenCode();
            if (!string.IsNullOrEmpty(type))
            {
                 findOne = db.HiddenCode.Find(type, code);
            }

            return PartialView("_PartialEdit", findOne);
        }
        public ActionResult EditChild(string type, string code)
        {
            HiddenCode findOne = new HiddenCode();
            if (!string.IsNullOrEmpty(type))
            {
                findOne = db.HiddenCode.Find(type, code);
            }

            return PartialView("_PartialChildEdit", findOne);
        }
        [HttpPost]
        public ActionResult Save(string type, string code)
        {
            HiddenCode findOne = db.HiddenCode.Find(type, code);

            if (findOne == null)
            {//add
                findOne = new HiddenCode();
                findOne.Name = code;
                TryUpdateModel<HiddenCode>(findOne);
                if (code == "不涉及" || code == "不限制")
                {
                    findOne.InDateTime = DateTime.MaxValue;
                }
                else
                {
                    findOne.InDateTime = DateTime.Now;
                }               
                findOne.InUserId = UserInfo.UserId;
                db.HiddenCode.Add(findOne);
            }
            else
            {
                findOne.Name = code;
                TryUpdateModel<HiddenCode>(findOne);
            }
            db.SaveChanges();

            return Json(findOne);
        }

    }
}