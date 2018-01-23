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
    public class QuotationGroupController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        RequirementService requirementService = new RequirementService();
        QuotationService quotationService = new QuotationService();
        //
        // GET: /DemandBookGroup/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuotationGroupSearch(string projectId)
        {
            List<QuotationGroupDto> list = quotationService.QuotationGroupSearch(projectId, UserInfo.UserId);
            return Json(new { List = list });
        }
        public ActionResult GroupDtl(string projectId, string groupId, string ModelType)
        {
            List<QuotationMstDto> list = quotationService.QuotationMstSearchByGroupId(projectId, groupId);
            ViewBag.ModelType = ModelType;
            return PartialView("_PartialGroupDtlQuotation", list);
        }
        public ActionResult Save(Quotation_Group group)
        {
            //if (group.Id == 0)
            //{
            //    group.IndateTime = DateTime.Now;
            //    group.InUserId = UserInfo.UserId;
            //    db.Quotation_Group.Add(group);
            //}
            //else
            //{
            //    group = db.Quotation_Group.Find(group.Id);
            //    TryUpdateModel<Quotation_Group>(group);
            //}
            //db.SaveChanges();
            string[] groupNameList = group.QuotationGroupName.Split('-');
            if (groupNameList.Length == 1)
            {
               group.QuotationGroupName =  group.QuotationGroupName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            quotationService.QuotationGroupSave(group.Id.ToString(), group.ProjectId.ToString(), group.QuotationGroupName, UserInfo.UserId,group.SettlementChk,group.RequirementGroupId.ToString());

            return Json(group);
        }
        public ActionResult ProjectCityIndex(string ModelType,string ProjectId,string GroupId)
        {
            MasterService service = new MasterService();

            List<ProjectDto> list = service.ProjectSearchById(ProjectId.ToString(), UserInfo.UserId);

            ProjectDto projects = new ProjectDto();
            if (list.Count > 0)
            {
                projects = list[0];
            }
            ViewBag.ProjectId = ProjectId;
            ViewBag.GroupId = GroupId;
            ViewBag.ModelType = ModelType;
            ViewBag.ApplyStautCode = projects.ApplyStatusCode;
            ViewBag.DepartmentCode = projects.DepartmentCode;
            return View("ProjectCityIndex");
        }
        #region 根据项目查询需求书
        //public ActionResult RequirementSearchByProjectId(string projectId, string province, string city)
        //{
        //    List<RequiremetMstDto> list = requirementService.RequirementMstByProjectIdSearch(projectId, province, city);
        //    return PartialView("_PartialDemandBookSelect", list);
        //}
        //public ActionResult RequirementSearchByProjectIdSearch(string projectId, string province, string city)
        //{
        //    List<RequiremetMstDto> list = requirementService.RequirementMstByProjectIdSearch(projectId, province, city);
        //    return Json(new { List = list });
        //}
        #endregion
        public ActionResult SettlementUpdate(Quotation_Group group)
        {
            quotationService.QuotationGroupSettlementUpdate(group.ProjectId.ToString(), group.Id.ToString(), group.SettlementChk.Value);
            return Json(group);
        }

        public ActionResult GetSettlementChk(Quotation_Group group)
        {
            group = db.Quotation_Group.Where(x => x.ProjectId == group.ProjectId && x.SettlementChk == group.SettlementChk).FirstOrDefault();
            return Json(new { Data=group});
        }
        public ActionResult RequirementGroupSelect(string projectId)
        {
            RequirementService requirementService = new RequirementService();
            List<RequirementGroupDto> lst = new List<RequirementGroupDto>();

            List<RequirementGroupDto> list = requirementService.RequirementGroupSearch(projectId, UserInfo.UserId);
            foreach (RequirementGroupDto dto in list)
            {
                if (dto.ApplyStatusCode == "完成")
                {
                    lst.Add(dto);
                }
            }

            return PartialView("_PartialDemandBookGroupSelect", lst);
            // return Json(new { List = list });
        }
        public ActionResult QuotationGroupDelete(string GroupId)
        {
            quotationService.QuotationGroupDelete(GroupId);

            return Json("");
        }

        public ActionResult QuotationGroupSelect(string ModelType, string projectName, string projectShortName)
        {
            List<QuotationGroupDto> list = quotationService.QuotationGroupByProjectNameSearch(ModelType, projectName, projectShortName, UserInfo.UserId);

            return PartialView("_PartialQuotationGroupSelect", list);
        } 

        public ActionResult QuotationGroupSearchByProjectName(string ModelType, string projectName, string projectShortName)
        {
            List<QuotationGroupDto> list = quotationService.QuotationGroupByProjectNameSearch(ModelType, projectName, projectShortName, UserInfo.UserId);

            return Json(new { List = list });
        }

        private int SaveQuotationMst(string QuotationType, string ProjectId, string SupplierId,string GroupId)
        {
            var mstDto = new QuotationMstDto()
            {
                QuotationId = "0",
                GroupId = GroupId,
                QuotationType = QuotationType,
                ProjectId = ProjectId,
                SupplierId = SupplierId
            };
            int QuotationId = quotationService.QuotationSave(mstDto, UserInfo.UserId);
            return QuotationId;
        }

        public ActionResult QuotationGroupCopy(string SourceGroupId, string TargetGroupId,string ProjectId)
        {
            int seqNo = 0;
            //Biancheng
            List<Quotation_BianChengDto> bianchengList = quotationService.Quotation_BianChengCopySearch(TargetGroupId, SourceGroupId);
            foreach(Quotation_BianChengDto dto in bianchengList){
                Quotation_Biancheng_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Biancheng_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                dtl.QuotationId = SaveQuotationMst("Biancheng", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Biancheng_Dtl.Add(dtl);
            }
            //ZhiXing
            seqNo = 0;
            List<Quotation_ZhiXingDto> zhixingList = quotationService.Quotation_ZhiXingCopySearch(TargetGroupId, SourceGroupId);
            foreach (Quotation_ZhiXingDto dto in zhixingList)
            {
                Quotation_Zhixing_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Zhixing_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                dtl.QuotationId = SaveQuotationMst("Zhixing", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Zhixing_Dtl.Add(dtl);
            }
            //Fuhe
            seqNo = 0;
            List<Quotation_FuHeDto> fuheList = quotationService.Quotation_FuHeCopySearch(TargetGroupId, SourceGroupId);
            foreach (Quotation_FuHeDto dto in fuheList)
            {
                Quotation_Fuhe_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Fuhe_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                dtl.QuotationId = SaveQuotationMst("Fuhe", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Fuhe_Dtl.Add(dtl);
            }
            //Yanjiu
            seqNo = 0;
            List<Quotation_YanJiuDto> yanjiuList = quotationService.Quotation_YanJiuCopySearch(TargetGroupId, SourceGroupId);
            foreach (Quotation_YanJiuDto dto in yanjiuList)
            {
                Quotation_Yanjiu_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Yanjiu_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                dtl.QuotationId = SaveQuotationMst("Yanjiu", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Yanjiu_Dtl.Add(dtl);
            }
            //Qita1
            seqNo = 0;
            List<Quotation_QiTa1Dto> qita1List = quotationService.Quotation_QiTa1CopySearch(TargetGroupId, SourceGroupId);
            foreach (Quotation_QiTa1Dto dto in qita1List)
            {
                Quotation_Qita1_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Qita1_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                dtl.QuotationId = SaveQuotationMst("Qita1", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Qita1_Dtl.Add(dtl);
            }
            //Qita2
            seqNo = 0;
            List<Quotation_QiTa2Dto> qita2List = quotationService.Quotation_QiTa2CopySearch(TargetGroupId, SourceGroupId);
            foreach (Quotation_QiTa2Dto dto in qita2List)
            {
                Quotation_Qita2_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Qita2_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                dtl.QuotationId = SaveQuotationMst("Qita2", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Qita2_Dtl.Add(dtl);
            }
            //Chezhan
            seqNo = 0;
            List<Quotation_CheZhanDto> chezhanList = quotationService.Quotation_ChezhanCopySearch(TargetGroupId, SourceGroupId);
            foreach (Quotation_CheZhanDto dto in chezhanList)
            {
                Quotation_Chezhan_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Chezhan_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.IndateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId;
                dtl.QuotationId = SaveQuotationMst("Chezhan", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Chezhan_Dtl.Add(dtl);
            }
            //Zhichi
            seqNo = 0;
            List<ZhiChi01Dto> zhichiList = quotationService.Quotation_ZhichiCopySearch(TargetGroupId, SourceGroupId);
            foreach (ZhiChi01Dto dto in zhichiList)
            {
                Quotation_Zhichi_Dtl dtl = AutoMapperHelper.MapTo<Quotation_Zhichi_Dtl>(dto);
                seqNo++;
                dtl.SeqNO = seqNo;
                dtl.InDateTime = DateTime.Now;
                dtl.InUserId = UserInfo.UserId; 
                dtl.QuotationId = SaveQuotationMst("Zhichi", ProjectId, dto.SupplierId, TargetGroupId);
                db.Quotation_Zhichi_Dtl.Add(dtl);
            }
            db.SaveChanges();

            return Json("");
        }
	
    }
}