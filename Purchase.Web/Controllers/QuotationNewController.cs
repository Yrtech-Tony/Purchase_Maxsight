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
    public class QuotationNewController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        QuotationService quotationService = new QuotationService();
        private RequirementService requirementService = new RequirementService();
        // GET: QuotationNew
        public ActionResult Index()
        {
            return View();
        }

        private int SaveQuotationMst(string GroupId, string ProjectId, string SupplierId, string QuotationType)
        {
            var mstDto = new QuotationMstDto();
            mstDto.QuotationId = "";
            mstDto.GroupId = GroupId;
            mstDto.ProjectId = ProjectId;
            mstDto.SupplierId = SupplierId;
            mstDto.QuotationType = QuotationType;
            int QuotationId = quotationService.QuotationSave(mstDto, UserInfo.UserId);
            return QuotationId;
        }

        public ActionResult RequirementSelect(string projectId, string province, string city,string groupId,string quotationType)
        {
            List<RequiremetMstDto> list = requirementService.RequirementMstByProjectIdSearch(projectId, province, city, groupId);

            return PartialView("_PartialDemandBookSelect", GetRequirementMstDtoByQuotationType(quotationType,list));
        }
        #region ======编程======
        public ActionResult LoadBiancheng(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<Quotation_BianChengDto> lst = new List<Quotation_BianChengDto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach (string id in RequirementId.Split(','))
                {
                    lst.AddRange(quotationService.QuotationRegSearch_Biancheng(projectId, supplierId, GroupId, RequirementGroupId, Province, City, id));
                }
            }     

            return Json(new { List = lst });
        }
        public ActionResult SaveBiancheng(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Biancheng_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Biancheng_Dtl>>(jsonArr);

            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Biancheng");

            foreach (Quotation_Biancheng_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 1;
                    var maxOne = db.Quotation_Biancheng_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 1;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.InDateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Biancheng_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Biancheng_Dtl findOne = db.Quotation_Biancheng_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.hesuandanwei = dtl.hesuandanwei;
                        findOne.shuliang = dtl.shuliang;
                        findOne.danjia = dtl.danjia;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        }
        #endregion

        #region ======执行======
        public ActionResult LoadZhixing(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<Quotation_ZhiXingDto> lst = new List<Quotation_ZhiXingDto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach(string id in  RequirementId.Split(',')){
                    lst.AddRange(quotationService.QuotationRegSearch_Zhixing(projectId, supplierId, GroupId, RequirementGroupId, Province, City,id));
                }
            }
            return Json(new { List = lst });
        }
        public ActionResult SaveZhixing(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Zhixing_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Zhixing_Dtl>>(jsonArr);

            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Zhixing");

            foreach (Quotation_Zhixing_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 1;
                    var maxOne = db.Quotation_Zhixing_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 1;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.InDateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Zhixing_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Zhixing_Dtl findOne = db.Quotation_Zhixing_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.AccountUnit = dtl.AccountUnit;
                        findOne.Count = dtl.Count;
                        findOne.UnitPrice = dtl.UnitPrice;
                        findOne.Remark = dtl.Remark;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        } 
        #endregion

        #region ======复核======
        public ActionResult LoadFuhe(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<Quotation_FuHeDto> lst = new List<Quotation_FuHeDto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach (string id in RequirementId.Split(','))
                {
                    lst.AddRange(quotationService.QuotationRegSearch_Fuhe(projectId, supplierId, GroupId, RequirementGroupId, Province, City, id));
                }
            } 
           
            return Json(new { List = lst });
        }
        public ActionResult SaveFuhe(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Fuhe_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Fuhe_Dtl>>(jsonArr);

            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Fuhe");
            foreach (Quotation_Fuhe_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 1;
                    var maxOne = db.Quotation_Fuhe_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 1;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.InDateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Fuhe_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Fuhe_Dtl findOne = db.Quotation_Fuhe_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.hesuandanwei = dtl.hesuandanwei;
                        findOne.shuliang = dtl.shuliang;
                        findOne.danjia = dtl.danjia;
                        findOne.beizhu = dtl.beizhu;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        }
        #endregion

        #region ======研究======
        public ActionResult LoadYanjiu(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<Quotation_YanJiuDto> lst = new List<Quotation_YanJiuDto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach (string id in RequirementId.Split(','))
                {
                    lst.AddRange(quotationService.QuotationRegSearch_Yanjiu(projectId, supplierId, GroupId, RequirementGroupId, Province, City, id));
                }
            } 
           
            return Json(new { List = lst });
        }
        public ActionResult SaveYanjiu(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Yanjiu_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Yanjiu_Dtl>>(jsonArr);

            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Yanjiu");

            foreach (Quotation_Yanjiu_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 1;
                    var maxOne = db.Quotation_Yanjiu_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 1;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.InDateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Yanjiu_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Yanjiu_Dtl findOne = db.Quotation_Yanjiu_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.hesuandanwei = dtl.hesuandanwei;
                        findOne.shuliang = dtl.shuliang;
                        findOne.danjia = dtl.danjia;
                        findOne.beizhu = dtl.beizhu;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        }
        #endregion

        #region ======其他1======
        public ActionResult LoadQita1(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<Quotation_QiTa1Dto> lst = new List<Quotation_QiTa1Dto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach (string id in RequirementId.Split(','))
                {
                    lst.AddRange(quotationService.QuotationRegSearch_Qita1(projectId, supplierId, GroupId, RequirementGroupId, Province, City, id));
                }
            } 
           
            return Json(new { List = lst });
        }
        public ActionResult SaveQita1(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Qita1_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Qita1_Dtl>>(jsonArr);

            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Qita1");

            foreach (Quotation_Qita1_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 1;
                    var maxOne = db.Quotation_Qita1_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 1;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.InDateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Qita1_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Qita1_Dtl findOne = db.Quotation_Qita1_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.shuliang = dtl.shuliang;
                        findOne.danjia = dtl.danjia;
                        findOne.beizhu = dtl.beizhu;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        }
        #endregion

        #region ======其他2======
        public ActionResult LoadQita2(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<Quotation_QiTa2Dto> lst = new List<Quotation_QiTa2Dto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach (string id in RequirementId.Split(','))
                {
                    lst.AddRange(quotationService.QuotationRegSearch_Qita2(projectId, supplierId, GroupId, RequirementGroupId, Province, City, id));
                }
            } 
           
            return Json(new { List = lst });
        }
        public ActionResult SaveQita2(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Qita2_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Qita2_Dtl>>(jsonArr);

            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Qita2");

            foreach (Quotation_Qita2_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 2;
                    var maxOne = db.Quotation_Qita2_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 2;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.InDateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Qita2_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Qita2_Dtl findOne = db.Quotation_Qita2_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.shuliang = dtl.shuliang;
                        findOne.danjia = dtl.danjia;
                        findOne.beizhu = dtl.beizhu;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        }
        #endregion

        #region ======支持======
        public ActionResult LoadZhichi(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<ZhiChi01Dto> lst = new List<ZhiChi01Dto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach (string id in RequirementId.Split(','))
                {
                    lst.AddRange(quotationService.QuotationRegSearch_Zhichi(projectId, supplierId, GroupId, RequirementGroupId, Province, City, id));
                }
            } 
           
            return Json(new { List = lst });
        }
        public ActionResult SaveZhichi(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Zhichi_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Zhichi_Dtl>>(jsonArr);
           
            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Zhichi");

            foreach (Quotation_Zhichi_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 1;
                    var maxOne = db.Quotation_Zhichi_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 1;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.InDateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Zhichi_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Zhichi_Dtl findOne = db.Quotation_Zhichi_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.hesuandanwei = dtl.hesuandanwei;
                        findOne.shuliang = dtl.shuliang;
                        findOne.danjia = dtl.danjia;
                        findOne.beizhu = dtl.beizhu;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        }
        #endregion

        #region ======车展======
        public ActionResult LoadChezhan(string projectId, string supplierId, string GroupId, string RequirementGroupId, string Province, string City, string RequirementId)
        {
            List<Quotation_CheZhanDto> lst = new List<Quotation_CheZhanDto>();
            if (!string.IsNullOrEmpty(RequirementId))
            {
                var ids = RequirementId.Split(',');
                foreach (string id in RequirementId.Split(','))
                {
                    lst.AddRange(quotationService.QuotationRegSearch_Chezhan(projectId, supplierId, GroupId, RequirementGroupId, Province, City, id));
                }
            } 
           
            return Json(new { List = lst });
        }
        public ActionResult SaveChezhan(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_Chezhan_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Chezhan_Dtl>>(jsonArr);

            int QuotationId = SaveQuotationMst(GroupId, ProjectId, SupplierId, "Chezhan");

            foreach (Quotation_Chezhan_Dtl dtl in lst)
            {
                if (dtl.QuotationId == 0 && dtl.SeqNO == 0)
                {
                    int seqNo = 1;
                    var maxOne = db.Quotation_Chezhan_Dtl.Where(x => x.QuotationId == QuotationId)
                        .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNo = maxOne + 1;
                    }

                    dtl.QuotationId = QuotationId;
                    dtl.SeqNO = seqNo;
                    dtl.IndateTime = DateTime.Now;
                    dtl.InUserId = UserInfo.UserId;
                    db.Quotation_Chezhan_Dtl.Add(dtl);

                    db.SaveChanges();
                }
                else
                {
                    Quotation_Chezhan_Dtl findOne = db.Quotation_Chezhan_Dtl.Find(dtl.QuotationId, dtl.SeqNO);
                    if (findOne != null)
                    {
                        findOne.hesuandanwei = dtl.hesuandanwei;
                        findOne.shuliang = dtl.shuliang;
                        findOne.danjia = dtl.danjia;
                        findOne.beizhu = dtl.beizhu;
                    }
                    db.SaveChanges();
                }
            }

            return Json("");
        }
        #endregion

    }
}