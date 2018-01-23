using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class QuotationController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        QuotationService quotationService = new QuotationService();
        //
        // GET: /Quotation/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QueryQuotationTypeByProject(string modelType, string ProjectId)
        {
            var list = quotationService.QuotationTypeByProject(ProjectId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult QuotationForSupplier()
        {
            return View();
        }

        public ActionResult LoadQuotationForSupplier(string modelType, string projectCode, string projectName, string projectShortName,
           string supplierCode, string supplierName, string supplierShortName)
        {
            var lst = quotationService.QuotationSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, (Session["LoginUser"] as Mst_UserInfo).UserId);

            return Json(new { List = lst });
        }

        // GET: Projects/Create
        public ActionResult Create(int Id = 0)
        {
            ViewBag.QuotationId = Id;
            ViewBag.SeqNO = Request.QueryString["SeqNo"];
            ViewBag.QuotationType = Request.QueryString["QuotationType"];
            ViewBag.GroupId = Request.QueryString["GroupId"];
            return View();
        }
        public ActionResult FindQuotationMst(int QuotationId)
        {
            //if (QuotationId > 0)
            //{
            //    Quotation_QuotationMst dtl = db.Quotation_QuotationMst.Find(QuotationId);
            //    return Json(dtl, JsonRequestBehavior.AllowGet);
            //}
            QuotationMstDto mst = quotationService.QuotationSearchById(QuotationId.ToString(), UserInfo.UserId).FirstOrDefault();
            return Json(mst, JsonRequestBehavior.AllowGet);
        }
        private int SaveQuotationMst(string QuotationType, int ProjectId, int SupplierId)
        {
            int QuotationId = 0;
            ////查询mst中 某供应商，某项目，此确认单类型是否已保存过
            //var mst = db.Quotation_QuotationMst.Where(x => x.QuotationType == QuotationType && x.ProjectId == ProjectId && x.SupplierId == SupplierId && x.Status != "取消").FirstOrDefault();
            //if (mst != null)
            //{
            //    QuotationId = mst.Id;
            //}
            //else
            {
                var mstDto = new QuotationMstDto();
                TryUpdateModel<QuotationMstDto>(mstDto);
                QuotationId = quotationService.QuotationSave(mstDto, UserInfo.UserId);
            }
            return QuotationId;
        }
        public ActionResult LoadDetailByQuotationId(int QuotationId, string QuotationType)
        {
            if (QuotationType == "Biancheng")
            {
                var lst = db.Quotation_Biancheng_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Zhixing")
            {
                var lst = db.Quotation_Zhixing_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Fuhe")
            {
                var lst = db.Quotation_Fuhe_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Yanjiu")
            {
                var lst = db.Quotation_Yanjiu_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Qita1")
            {
                var lst = db.Quotation_Qita1_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Qita2")
            {
                var lst = db.Quotation_Qita2_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Chezhan")
            {
                var lst = db.Quotation_Chezhan_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Zhichi")
            {
                var lst = db.Quotation_Zhichi_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            return Json(null);
        }
        #region ======编程======
        public ActionResult LoadBiancheng(string modelType, string projectCode, string projectName, string projectShortName,
           string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_BianChengSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }
        public ActionResult FindBiancheng(int QuotationId, int? SeqNo)
        {
            Quotation_Biancheng_Dtl dtl = new Quotation_Biancheng_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Biancheng_Dtl.Find(QuotationId, SeqNo);

            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBiancheng(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                //获取seqno
                int seqNo = 1;
                var maxOne = db.Quotation_Biancheng_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Biancheng_Dtl dtl = new Quotation_Biancheng_Dtl();
                TryUpdateModel<Quotation_Biancheng_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Biancheng_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Biancheng_Dtl dtl = new Quotation_Biancheng_Dtl();
                dtl = db.Quotation_Biancheng_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Biancheng_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId });
        }

        #endregion
        #region ======执行======
        public ActionResult LoadZhixing(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_ZhiXingSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }
        public ActionResult FindZhixing(int QuotationId, int? SeqNo)
        {
            Quotation_Zhixing_Dtl dtl = new Quotation_Zhixing_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Zhixing_Dtl.Find(QuotationId, SeqNo);
            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveZhixing(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                int seqNo = 1;
                var maxOne = db.Quotation_Zhixing_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Zhixing_Dtl dtl = new Quotation_Zhixing_Dtl();
                TryUpdateModel<Quotation_Zhixing_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Zhixing_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Zhixing_Dtl dtl = new Quotation_Zhixing_Dtl();
                dtl = db.Quotation_Zhixing_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Zhixing_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId }); ;
        }

        #endregion
        #region ======复核======
        public ActionResult LoadFuhe(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_FuHeSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }
        public ActionResult FindFuhe(int QuotationId, int? SeqNo)
        {
            Quotation_Fuhe_Dtl dtl = new Quotation_Fuhe_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Fuhe_Dtl.Find(QuotationId, SeqNo);
            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveFuhe(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                int seqNo = 1;
                var maxOne = db.Quotation_Fuhe_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Fuhe_Dtl dtl = new Quotation_Fuhe_Dtl();
                TryUpdateModel<Quotation_Fuhe_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Fuhe_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Fuhe_Dtl dtl = new Quotation_Fuhe_Dtl();
                dtl = db.Quotation_Fuhe_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Fuhe_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId });
        }
        #endregion
        #region ======研究======
        public ActionResult LoadYanjiu(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_YanJiuSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }

        public ActionResult FindYanjiu(int QuotationId, int? SeqNo)
        {
            Quotation_Yanjiu_Dtl dtl = new Quotation_Yanjiu_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Yanjiu_Dtl.Find(QuotationId, SeqNo);
            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveYanjiu(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                int seqNo = 1;
                var maxOne = db.Quotation_Yanjiu_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Yanjiu_Dtl dtl = new Quotation_Yanjiu_Dtl();
                TryUpdateModel<Quotation_Yanjiu_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Yanjiu_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Yanjiu_Dtl dtl = new Quotation_Yanjiu_Dtl();
                dtl = db.Quotation_Yanjiu_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Yanjiu_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId }); ;
        }

        #endregion
        #region ======其他1======
        public ActionResult LoadQita1(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_QiTa1Search(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }

        public ActionResult FindQita1(int QuotationId, int? SeqNo)
        {
            Quotation_Qita1_Dtl dtl = new Quotation_Qita1_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Qita1_Dtl.Find(QuotationId, SeqNo);
            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveQita1(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                int seqNo = 1;
                var maxOne = db.Quotation_Qita1_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Qita1_Dtl dtl = new Quotation_Qita1_Dtl();
                TryUpdateModel<Quotation_Qita1_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Qita1_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Qita1_Dtl dtl = new Quotation_Qita1_Dtl();
                dtl = db.Quotation_Qita1_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Qita1_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId }); ;
        }

        #endregion
        #region ======其他2======
        public ActionResult LoadQita2(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_QiTa2Search(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }
        public ActionResult FindQita2(int QuotationId, int? SeqNo)
        {
            Quotation_Qita2_Dtl dtl = new Quotation_Qita2_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Qita2_Dtl.Find(QuotationId, SeqNo);
            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveQita2(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                int seqNo = 1;
                var maxOne = db.Quotation_Qita2_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Qita2_Dtl dtl = new Quotation_Qita2_Dtl();
                TryUpdateModel<Quotation_Qita2_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Qita2_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Qita2_Dtl dtl = new Quotation_Qita2_Dtl();
                dtl = db.Quotation_Qita2_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Qita2_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId }); ;
        }

        #endregion
        #region ======支持======
        public ActionResult LoadZhichi(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_ZhichiSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }
        public ActionResult FindZhichi(int QuotationId, int? SeqNo)
        {
            Quotation_Zhichi_Dtl dtl = new Quotation_Zhichi_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Zhichi_Dtl.Find(QuotationId, SeqNo);
            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveZhichi(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                int seqNo = 1;
                var maxOne = db.Quotation_Zhichi_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Zhichi_Dtl dtl = new Quotation_Zhichi_Dtl();
                TryUpdateModel<Quotation_Zhichi_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Zhichi_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Zhichi_Dtl dtl = new Quotation_Zhichi_Dtl();
                dtl = db.Quotation_Zhichi_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Zhichi_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId }); ;
        }

        #endregion
        #region ======车展======
        public ActionResult LoadChezhan(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_ChezhanSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }
        public ActionResult FindChezhan(int QuotationId, int? SeqNo)
        {
            Quotation_Chezhan_Dtl dtl = new Quotation_Chezhan_Dtl();
            if (QuotationId > 0)
            {
                dtl = db.Quotation_Chezhan_Dtl.Find(QuotationId, SeqNo);
            }
            return Json(dtl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveChezhan(int QuotationId, int? SeqNo, string QuotationType, int ProjectId, int SupplierId)
        {
            if (QuotationId == 0)
            {//新增确认单
                QuotationId = SaveQuotationMst(QuotationType, ProjectId, SupplierId);
                int seqNo = 1;
                var maxOne = db.Quotation_Chezhan_Dtl.Where(x => x.QuotationId == QuotationId)
                    .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxOne > 0)
                {
                    seqNo = maxOne + 1;
                }
                Quotation_Chezhan_Dtl dtl = new Quotation_Chezhan_Dtl();
                TryUpdateModel<Quotation_Chezhan_Dtl>(dtl);
                dtl.SeqNO = seqNo;
                dtl.QuotationId = QuotationId;
                dtl.IndateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                db.Quotation_Chezhan_Dtl.Add(dtl);
            }
            else
            {
                Quotation_Chezhan_Dtl dtl = new Quotation_Chezhan_Dtl();
                dtl = db.Quotation_Chezhan_Dtl.Find(QuotationId, SeqNo);
                TryUpdateModel<Quotation_Chezhan_Dtl>(dtl);
            }

            db.SaveChanges();
            return Json(new { QuotationId = QuotationId }); ;
        }

        #endregion
        public ActionResult ApplyCommit(string recheckUserId, Apply apply, string projectId, string groupId, string applyIdexists, string seqNO, string attachArray)
        {
            //apply.InDateTime = DateTime.Now;
            //apply.ApplyUserId = UserInfo.UserId;
            //apply.ApplyTypeId = ApplyType.确认单.ToString();
            //apply.ProjectId = Convert.ToInt32(projectId);
            //int applyId = applayService.ApplyCommit(apply, recheckUserId);
            ApplyService applayService = new ApplyService();
            apply.InDateTime = DateTime.Now;
            apply.ApplyUserId = UserInfo.UserId;
            apply.ApplyTypeId = ApplyType.确认单.ToString();
            apply.ProjectId = Convert.ToInt32(groupId);// 确认单申请时ProjectId 保存为groupId
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
            applayService.ApplyDtlDelete(new ApplyDtl { ApplyId = applyId });
            List<QuotationMstDto> list = quotationService.QuotationMstSearchByGroupId(projectId, groupId);
            foreach (QuotationMstDto item in list)
            {
                ApplyDtl applyDtl = new ApplyDtl()
                {
                    ApplyId = applyId,
                    ApplyContentId = Convert.ToInt32(item.QuotationId),
                    ApplyTypeId = ApplyType.确认单.ToString(),
                    InDateTime = DateTime.Now,
                    InUserId = UserInfo.UserId
                };
                applayService.ApplyDtlSave(applyDtl);
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
        public ActionResult RecheckUserSelect()
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(ApplyType.确认单.ToString(), UserInfo.UserId);

            return PartialView("_PartialRecheckUserSelect", lst);
        }
        #region 需求书提交审核验证
        public ActionResult CommitCheckSearch(string projectId)
        {
            CommitCheckDto commitCheck = quotationService.CommitCheckSearch(projectId, UserInfo.UserId).FirstOrDefault();
            return Json(commitCheck, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 确认单选择操作

        public ActionResult QuotationMstUpdate(string id, bool selectChk)
        {
            quotationService.Quotation_QuotationMstUpdate(id, selectChk, UserInfo.UserId);
            return Json("");
        }
        #endregion
        #region 根据ProjectId查询Project
        public ActionResult ProjectById(string projectId)
        {
            MasterService masterService = new MasterService();

            ProjectDto lst = masterService.ProjectSearchById(projectId, UserInfo.UserId).FirstOrDefault();
            return Json(new { List = lst });
        }
        #endregion

        public ActionResult GroupSelect(int projectId)
        {
            var lst = db.Quotation_Group.Where(x => x.ProjectId == projectId);
            return PartialView("_PartialQuotationGroupSelect", lst);
        }

        string suffix = ".xlsx";
        string tempPath = "~/QuotationTemplate/";
        string basePath = "~/Export/Quotation/";
        string tempPathHor = "~/QuotationTemplate/横向确认单/";

        /// <summary>
        /// 查询导出
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="projectCode"></param>
        /// <param name="projectName"></param>
        /// <param name="projectShortName"></param>
        /// <param name="supplierCode"></param>
        /// <param name="supplierName"></param>
        /// <param name="supplierShortName"></param>
        /// <param name="lastChk"></param>
        /// <param name="QuotationGroupId"></param>
        /// <param name="quotationType"></param>
        /// <returns></returns>        
        public ActionResult ExportQuotation(string modelType, string projectCode, string projectName, string projectShortName,
           string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId, string quotationTypes)
        {
            string zip = "";
            string absPath = Server.MapPath(basePath);
            string createFloader = absPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\\";
            if (!Directory.Exists(createFloader))
            {
                Directory.CreateDirectory(createFloader);
            }
            foreach (string type in quotationTypes.Split(','))
            {
                string quotationType = type;
                List<QuotationDto> lst = new List<QuotationDto>();
                #region ==================竖向的确认单模板 先注释===============================
                //if (quotationType == "Zhixing")
                //{
                //    lst.AddRange(quotationService.Quotation_ZhiXingSearch(modelType, projectCode, projectName, projectShortName,
                //      supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                //else if (quotationType == "Fuhe")
                //{
                //    lst.AddRange(quotationService.Quotation_FuHeSearch(modelType, projectCode, projectName, projectShortName,
                //      supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                //else if (quotationType == "Biancheng")
                //{
                //    lst.AddRange(quotationService.Quotation_BianChengSearch(modelType, projectCode, projectName, projectShortName,
                //        supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                //else if (quotationType == "Yanjiu")
                //{
                //    lst.AddRange(quotationService.Quotation_YanJiuSearch(modelType, projectCode, projectName, projectShortName,
                //        supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                //else if (quotationType == "Zhichi")
                //{
                //    lst.AddRange(quotationService.Quotation_ZhichiSearch(modelType, projectCode, projectName, projectShortName,
                //        supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                //else if (quotationType == "Qita1")
                //{
                //    lst.AddRange(quotationService.Quotation_QiTa1Search(modelType, projectCode, projectName, projectShortName,
                //        supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                //else if (quotationType == "Qita2")
                //{
                //    lst.AddRange(quotationService.Quotation_QiTa2Search(modelType, projectCode, projectName, projectShortName,
                //        supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                //else if (quotationType == "Chezhan")
                //{
                //    lst.AddRange(quotationService.Quotation_ChezhanSearch(modelType, projectCode, projectName, projectShortName,
                //        supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                //}
                #endregion

                #region ========横向确认单========
                string typeText = "";
                QuotationTypeAndText.TryGetValue(quotationType, out typeText);
                string createFileName = projectShortName+"_"+ typeText  + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_确认单" + ".xlsx";
                string path = createFloader + createFileName;
                string templateFile = "";
                if (modelType == "业务" && (quotationType == "Qita1" || quotationType == "Qita2"))
                {
                    //横向确认单
                    List<QuotationExport_Data_Dto> DataDtoList = quotationService.QuotationExport_DataSearch(modelType, projectCode, projectName, projectShortName,
                           supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId, quotationType);
                    List<QuotationExport_Head_Dto> HeaderDtoList = quotationService.QuotationExport_HeadSearch(modelType, projectCode, projectName, projectShortName,
                             supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId, quotationType);
                    List<QuotationExport_Left_Dto> LeftDtoList = quotationService.QuotationExport_LeftSearch(modelType, projectCode, projectName, projectShortName,
                             supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId, quotationType);

                    templateFile = Server.MapPath(tempPathHor + quotationType + suffix);
                    System.IO.File.Copy(templateFile, path);

                    QuotationHorExport export = new QuotationHorExport();
                    export.ExportQuotationHorQita(path, projectName, projectShortName, projectCode, HeaderDtoList, LeftDtoList, DataDtoList);
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
                            List<QuotationExport_Data_Dto> DataDtoList = quotationService.QuotationExport_DataSearch(modelType, projectCode, projectName, projectShortName,
                                   supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId, quotationType);
                            List<QuotationExport_Head_Dto> HeaderDtoList = quotationService.QuotationExport_HeadSearch(modelType, projectCode, projectName, projectShortName,
                                     supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId, quotationType);
                            List<QuotationExport_Left_Dto> LeftDtoList = quotationService.QuotationExport_LeftSearch(modelType, projectCode, projectName, projectShortName,
                                     supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId, quotationType);

                            templateFile = Server.MapPath(tempPathHor + quotationType + suffix);
                            System.IO.File.Copy(templateFile, path);

                            if (quotationType == "Qita1" || quotationType == "Qita2")
                            {
                                QuotationHorExport export = new QuotationHorExport();
                                export.ExportQuotationHor(path, projectName, projectShortName, projectCode, HeaderDtoList, LeftDtoList, DataDtoList);
                            }
                            else
                            {
                                QuotationHorExport export = new QuotationHorExport();
                                export.ExportQuotationHor(path, projectName, projectShortName, projectCode, HeaderDtoList, LeftDtoList, DataDtoList);
                            }
                            break;
                        case "Zhichi":
                        case "Chezhan":
                        case "Qita1":
                        case "Qita2":
                            if (quotationType == "Zhichi")
                            {
                                lst.AddRange(quotationService.Quotation_ZhichiSearch(modelType, projectCode, projectName, projectShortName,
                                    supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                            }
                            else if (quotationType == "Qita1")
                            {
                                lst.AddRange(quotationService.Quotation_QiTa1Search(modelType, projectCode, projectName, projectShortName,
                                    supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                            }
                            else if (quotationType == "Qita2")
                            {
                                lst.AddRange(quotationService.Quotation_QiTa2Search(modelType, projectCode, projectName, projectShortName,
                                    supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                            }
                            else if (quotationType == "Chezhan")
                            {
                                lst.AddRange(quotationService.Quotation_ChezhanSearch(modelType, projectCode, projectName, projectShortName,
                                    supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId));
                            }

                            if (modelType != "业务")
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


        public ActionResult PerQuotationTypeSumSearch(string projectId, string supplierCode, string supplierShortName, string supplierName, string quotationGroupId)
        {
            List<QuotationPerQuotationTypeSum_Head_Dto> HeaderDtoList = quotationService.Quotation_PerQuotationTypeSumSearch_Head(projectId, supplierCode, supplierShortName, supplierName, "false", quotationGroupId);
            List<QuotationPerQuotationTypeSum_Left_Dto> LeftDtoList = quotationService.Quotation_PerQuotationTypeSumSearch_Left(projectId, supplierCode, supplierShortName, supplierName, "false", quotationGroupId);
            List<QuotationPerQuotationTypeSum_Data_Dto> DataDtoList = quotationService.Quotation_PerQuotationTypeSumSearch_Data(projectId, supplierCode, supplierShortName, supplierName, "false", quotationGroupId);

            List<QuotationPerQuotationTypeSum_Head_Dto> LastHeaderDtoList = quotationService.Quotation_PerQuotationTypeSumSearch_Head(projectId, supplierCode, supplierShortName, supplierName, "true", quotationGroupId);
            List<QuotationPerQuotationTypeSum_Left_Dto> LastLeftDtoList = quotationService.Quotation_PerQuotationTypeSumSearch_Left(projectId, supplierCode, supplierShortName, supplierName, "true", quotationGroupId);
            List<QuotationPerQuotationTypeSum_Data_Dto> LastDataDtoList = quotationService.Quotation_PerQuotationTypeSumSearch_Data(projectId, supplierCode, supplierShortName, supplierName, "true", quotationGroupId);


            return Json(new
            {
                PerQuotationTypeSumList = new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList },
                LastPerQuotationTypeSumList = new { DataDtoList = LastDataDtoList, HeaderDtoList = LastHeaderDtoList, LeftDtoList = LastLeftDtoList }
            });
        }
    }
}