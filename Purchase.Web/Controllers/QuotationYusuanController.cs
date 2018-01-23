using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class QuotationYusuanController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        QuotationService quotationService = new QuotationService();
        //
        // GET: /Quotation/
        public ActionResult Index()
        {
            return View();
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
            else if (QuotationType == "Zhichi")
            {
                var lst = db.Quotation_Zhichi_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
                return Json(lst);
            }
            else if (QuotationType == "Chezhan")
            {
                var lst = db.Quotation_Chezhan_Dtl.Where(x => x.QuotationId == QuotationId).ToList();
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

        #endregion
        #region ======执行======
        public ActionResult LoadZhixing(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_ZhiXingSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
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
        #endregion
        #region ======研究======
        public ActionResult LoadYanjiu(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_YanJiuSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
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
        #endregion
        #region ======其他2======
        public ActionResult LoadQita2(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_QiTa2Search(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
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
        #endregion
        #region ======车展======
        public ActionResult LoadChezhan(string modelType, string projectCode, string projectName, string projectShortName,
          string supplierCode, string supplierName, string supplierShortName, string lastChk, string QuotationGroupId)
        {
            var lst = quotationService.Quotation_ChezhanSearch(modelType, projectCode, projectName, projectShortName,
               supplierCode, supplierName, supplierShortName, lastChk, QuotationGroupId);

            return Json(new { List = lst });
        }
        #endregion
        public ActionResult UpdateYusuanList(string id, string seqNo, string quotationType, string yusuandanjia)
        {
            if (quotationType == "Youxingshangpincaigou")
            {
                quotationType = "Qita1";
            }
            if (quotationType == "Wuxingshangpincaigou")
            {
                quotationType = "Qita2";
            }
            string sql = string.Format("update dbo.Quotation_{0}_Dtl set yusuandanjia={1} where QuotationId={2} and SeqNO={3}",
                    quotationType, yusuandanjia, id, seqNo);
            db.Database.ExecuteSqlCommand(sql);

            return Json("");
        }
        public ActionResult UpdateYusuanAndContingency(string jsonArr, string ProjectId, string ContingencyFee)
        {
            SaveContingency(ProjectId, ContingencyFee);
            List<QuotationYusan> lst = JsonConvert.DeserializeObject<List<QuotationYusan>>(jsonArr);
            foreach (QuotationYusan yusuan in lst)
            {
                if (yusuan.QuotationType == "Youxingshangpincaigou")
                {
                    yusuan.QuotationType = "Qita1";
                }
                if (yusuan.QuotationType == "Wuxingshangpincaigou")
                {
                    yusuan.QuotationType = "Qita2";
                }
                string sql = string.Format("update dbo.Quotation_{0}_Dtl set yusuandanjia={1} where QuotationId={2} and SeqNO={3}",
                        yusuan.QuotationType, yusuan.yusuandanjia, yusuan.Id, yusuan.SeqNo);
                db.Database.ExecuteSqlCommand(sql);
            }

            try
            {
                // 预算确认单填写完毕之后，提醒应付流转单
                MasterService master = new MasterService();
                master.RemindCancelSave("流转单填写提醒", ProjectId, this.UserInfo.UserId, DateTime.Now.ToString());
            }
            catch (Exception)
            {
            }
            return Json("");
        }

        public ActionResult FindContingency(string ProjectId)
        {
            Contingency findOne = db.Contingency.Find(ProjectId);
            if (findOne == null)
            {
                findOne = new Contingency();
            }
            return Json(findOne);
        }
        public void SaveContingency(string ProjectId, string ContingencyFee)
        {
            decimal? feeNullable = string.IsNullOrEmpty(ContingencyFee) ? new Nullable<decimal>() : new Decimal(Double.Parse(ContingencyFee));

            Contingency findOne = db.Contingency.Find(ProjectId);
            if (findOne != null)
            {
                findOne.ContingencyFee = feeNullable;
            }
            else
            {
                findOne = new Contingency()
                {
                    ProjectId = ProjectId,
                    ContingencyFee = feeNullable,
                    InDateTime = DateTime.Now,
                    InUserId = UserInfo.UserId
                };
                db.Contingency.Add(findOne);
            }
            db.SaveChanges();
        }

        public ActionResult CheckYusuanInsert(string ProjectId, string GroupId, string QuotationType)
        {
            bool hasData = quotationService.QuotationYusuanSaveCheck(ProjectId, GroupId, QuotationType) > 0;
            return Json(hasData);
        }
    }
}