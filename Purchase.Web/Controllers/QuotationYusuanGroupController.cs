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
    public class QuotationYusuanGroupController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        QuotationService quotationService = new QuotationService();
        //
        // GET: /QuotationYusuanGroup/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QuotationYusuanGroupSearch(string projectId)
        {
            List<Group_YusuanDto> list = quotationService.QuotationGroup_Yusuan(projectId, UserInfo.UserId);
            return Json(new { List = list });
        }
        
        public ActionResult Save(Group_YusuanDto group)
        {
            quotationService.QuotationGrouupYusuanSave(group.Id.ToString(), group.QuotationYusuanGroupName, group.ProjectId.ToString(), group.LastChk, UserInfo.UserId);

            return Json(group);
        }

        public ActionResult QuotationYusuanGroupDelete(string GroupId)
        {
            quotationService.QuotationGrouupYusuanDelete(GroupId);

            return Json("");
        }
	}
}