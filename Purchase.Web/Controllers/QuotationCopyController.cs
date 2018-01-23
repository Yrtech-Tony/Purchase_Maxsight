using Newtonsoft.Json;
using Purchase.Common;
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
    public class QuotationCopyController : BaseController
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
        #region ======编程======
        public ActionResult LoadBiancheng(string SourceGroupId, string TargetGroupId)
        {
             List<Quotation_BianChengDto> lst = quotationService.Quotation_BianChengCopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveBiancheng(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            //List<Quotation_Biancheng_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Biancheng_Dtl>>(jsonArr);
            List<Quotation_BianChengDto> lstDto = JsonConvert.DeserializeObject<List<Quotation_BianChengDto>>(jsonArr);

            foreach (Quotation_BianChengDto dtl_dto in lstDto)
            {
                Quotation_Biancheng_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Biancheng_Dtl>(dtl_dto);
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Biancheng");
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
        public ActionResult LoadZhixing(string SourceGroupId, string TargetGroupId)
        {
            List<Quotation_ZhiXingDto> lst = quotationService.Quotation_ZhiXingCopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveZhixing(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            List<Quotation_ZhiXingDto> lstDto = JsonConvert.DeserializeObject<List<Quotation_ZhiXingDto>>(jsonArr);

            foreach (Quotation_ZhiXingDto dtl_dto in lstDto)
            {
                Quotation_Zhixing_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Zhixing_Dtl>(dtl_dto); ;
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Zhixing");
                if (Convert.ToInt32(dtl_dto.QuotationId) == 0 && Convert.ToInt32(dtl_dto.SeqNO) == 0)
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
        public ActionResult LoadFuhe(string SourceGroupId, string TargetGroupId)
        {
            List<Quotation_FuHeDto> lst = quotationService.Quotation_FuHeCopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveFuhe(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            //List<Quotation_Fuhe_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Fuhe_Dtl>>(jsonArr);
            List<Quotation_FuHeDto> lstDto = JsonConvert.DeserializeObject<List<Quotation_FuHeDto>>(jsonArr);


            foreach (Quotation_FuHeDto dtl_dto in lstDto)
            {
                Quotation_Fuhe_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Fuhe_Dtl>(dtl_dto);
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Fuhe");
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
        public ActionResult LoadYanjiu(string SourceGroupId, string TargetGroupId)
        {
            List<Quotation_YanJiuDto> lst = quotationService.Quotation_YanJiuCopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveYanjiu(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
           // List<Quotation_Yanjiu_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Yanjiu_Dtl>>(jsonArr);
            List<Quotation_YanJiuDto> lstDto = JsonConvert.DeserializeObject<List<Quotation_YanJiuDto>>(jsonArr);


            foreach (Quotation_YanJiuDto dtl_dto in lstDto)
            {
                Quotation_Yanjiu_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Yanjiu_Dtl>(dtl_dto);
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Yanjiu");
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
        public ActionResult LoadQita1(string SourceGroupId, string TargetGroupId)
        {
            List<Quotation_QiTa1Dto> lst = quotationService.Quotation_QiTa1CopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveQita1(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            //List<Quotation_Qita1_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Qita1_Dtl>>(jsonArr);
            List<Quotation_QiTa1Dto> lstDto = JsonConvert.DeserializeObject<List<Quotation_QiTa1Dto>>(jsonArr);
            foreach (Quotation_QiTa1Dto dtl_dto in lstDto)
            {
                Quotation_Qita1_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Qita1_Dtl>(dtl_dto);
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Qita1");
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
        public ActionResult LoadQita2(string SourceGroupId, string TargetGroupId)
        {
          List<Quotation_QiTa2Dto> lst = quotationService.Quotation_QiTa2CopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveQita2(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
          //  List<Quotation_Qita2_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Qita2_Dtl>>(jsonArr);
            List<Quotation_QiTa2Dto> lstDto = JsonConvert.DeserializeObject<List<Quotation_QiTa2Dto>>(jsonArr);

            foreach (Quotation_QiTa2Dto dtl_dto in lstDto)
            {
                Quotation_Qita2_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Qita2_Dtl>(dtl_dto);
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Qita2");
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
        public ActionResult LoadZhichi(string SourceGroupId, string TargetGroupId)
        {
           List<ZhiChi01Dto> lst = quotationService.Quotation_ZhichiCopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveZhichi(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            //List<Quotation_Zhichi_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Zhichi_Dtl>>(jsonArr);
            List<ZhiChi01Dto> lstDto = JsonConvert.DeserializeObject<List<ZhiChi01Dto>>(jsonArr);


            foreach (ZhiChi01Dto dtl_dto in lstDto)
            {
                Quotation_Zhichi_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Zhichi_Dtl>(dtl_dto);
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Zhichi");
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
        public ActionResult LoadChezhan(string SourceGroupId, string TargetGroupId)
        {
          List<Quotation_CheZhanDto> lst = quotationService.Quotation_ChezhanCopySearch(TargetGroupId, SourceGroupId);
            return Json(new { List = lst });
        }
        public ActionResult SaveChezhan(string jsonArr, string ProjectId, string SupplierId, string GroupId)
        {
            //List<Quotation_Chezhan_Dtl> lst = JsonConvert.DeserializeObject<List<Quotation_Chezhan_Dtl>>(jsonArr);
            List<Quotation_CheZhanDto> lstDto = JsonConvert.DeserializeObject<List<Quotation_CheZhanDto>>(jsonArr);


            foreach (Quotation_CheZhanDto dtl_dto in lstDto)
            {
                Quotation_Chezhan_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Chezhan_Dtl>(dtl_dto);
                int QuotationId = SaveQuotationMst(GroupId, ProjectId, dtl_dto.SupplierId, "Chezhan");
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