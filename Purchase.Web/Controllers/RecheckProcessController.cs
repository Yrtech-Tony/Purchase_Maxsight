using Purchase.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class RecheckProcessController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        //
        // GET: /RecheckProcess/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadRecheckProcess(string RecheckType)
        {
            var lst = db.RecheckProcess.Where(x => x.RecheckType == RecheckType);

            return Json(new { List = lst });
        }
        public ActionResult LoadRecheckProcessExcept(string RecheckType)
        {
            var query = from rpe in db.RecheckProcessExcept
                        where rpe.RecheckType == RecheckType
                        join user in db.Mst_UserInfo on rpe.ApplyUserId equals user.UserId
                        join user2 in db.Mst_UserInfo on rpe.RecheckUserId equals user2.UserId
                        select new
                        {
                            RecheckType  = rpe.RecheckType,
                            SeqNO = rpe.SeqNO,
                            ApplyUserId=rpe.ApplyUserId,
                            RecheckUserId = rpe.RecheckUserId,
                            UseChk = rpe.UseChk,
                            InUserId = rpe.InUserId,
                            InDateTime = rpe.InDateTime,
                            ApplyUserName = user.UserName,
                            RecheckUserName = user2.UserName,
                        };

            return Json(new { List = query.ToList() });
        }

        public ActionResult EditRecheckProcess(string RecheckType, int SeqNO = 0)
        {
            RecheckProcess findOne = new RecheckProcess();
            if (SeqNO > 0)
            {
                findOne = db.RecheckProcess.Find(RecheckType, SeqNO);
            }
            return PartialView("_PartialEditRecheckProcess", findOne);
        }

        public ActionResult SaveRecheckProcess(RecheckProcess recheckProcess)
        {
            if (recheckProcess.SeqNO == 0)
            {
                int maxSeq = db.RecheckProcess.Where(x => x.RecheckType == recheckProcess.RecheckType).OrderByDescending(x => x.SeqNO).Select(x=>x.SeqNO).FirstOrDefault();
                recheckProcess.SeqNO = maxSeq + 1;
                recheckProcess.InDateTime = DateTime.Now;
                recheckProcess.InUserId = UserInfo.UserId;
                db.RecheckProcess.Add(recheckProcess);
            }
            else
            {
                recheckProcess = db.RecheckProcess.Find(recheckProcess.RecheckType, recheckProcess.SeqNO);
                TryUpdateModel<RecheckProcess>(recheckProcess);
            }
            db.SaveChanges();

            return Json("");
        }


        public ActionResult EditRecheckProcessExcept(string RecheckType, int SeqNO = 0)
        {
            RecheckProcessExcept findOne = new RecheckProcessExcept();
            if (SeqNO > 0)
            {               
                findOne = db.RecheckProcessExcept.Find(RecheckType, SeqNO);
            }

            return PartialView("_PartialEditRecheckProcessExcept", findOne);
        }

        public ActionResult SaveRecheckProcessExcept(RecheckProcessExcept recheckProcessExcept)
        {
            if (recheckProcessExcept.SeqNO == 0)
            {
                int maxSeq = db.RecheckProcessExcept.Where(x => x.RecheckType == recheckProcessExcept.RecheckType).OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                recheckProcessExcept.SeqNO = maxSeq + 1;
                recheckProcessExcept.InDateTime = DateTime.Now;
                recheckProcessExcept.InUserId = UserInfo.UserId;
                db.RecheckProcessExcept.Add(recheckProcessExcept);
            }
            else
            {
                recheckProcessExcept = db.RecheckProcessExcept.Find(recheckProcessExcept.RecheckType, recheckProcessExcept.SeqNO);
                TryUpdateModel<RecheckProcessExcept>(recheckProcessExcept);
            }
            db.SaveChanges();

            return Json("");
        }
	}
}