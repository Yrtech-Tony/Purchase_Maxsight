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
    public class DemandBookGroupController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        RequirementService requirementService = new RequirementService();
        MasterService masterService = new MasterService();
        //
        // GET: /DemandBookGroup/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RequirementGroupSearch(string projectId)
        {
            List<RequirementGroupDto> list = requirementService.RequirementGroupSearch(projectId,UserInfo.UserId);
            return Json(new { List = list });
        }
        public ActionResult GroupDtl(string ModelType,string projectId, string groupId)
        {
            List<RequiremetMstDto> list = requirementService.RequirementMstSearchByGroupId(projectId, groupId);
            ViewBag.ModelType = ModelType;
            return PartialView("_PartialGroupDtlRequirement", list);
        }
        public ActionResult Save(Requirement_Group group)
        {

            string[] groupNameList =group.RequirementGroupName.Split('-');
            if (group.Id == 0)
            {
                group.RequirementGroupName = group.RequirementGroupName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                group.IndateTime = DateTime.Now;
                group.InUserId = UserInfo.UserId;
                db.Requirement_Group.Add(group);
            }
            else
            {
                group = db.Requirement_Group.Find(group.Id);
                TryUpdateModel<Requirement_Group>(group);
                if (groupNameList.Length==1)
                {
                    group.RequirementGroupName = group.RequirementGroupName + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }
            }
            db.SaveChanges();

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
        public ActionResult RequirementSearchByProjectId(string projectId, string province, string city,string groupId)
        {
            List<RequiremetMstDto> list = requirementService.RequirementMstByProjectIdSearch(projectId, province, city,groupId);
            return PartialView("_PartialDemandBookSelect", list);
        }
        public ActionResult RequirementSearchByProjectIdSearch(string projectId, string province, string city,string groupId)
        {
            List<RequiremetMstDto> list = requirementService.RequirementMstByProjectIdSearch(projectId, province, city,groupId);
            return Json(new { List = list });
        }
        #endregion

        public ActionResult DemandBookGroupCopy(string ModelType)
        {
            ViewBag.ModelType = ModelType;
            return PartialView("_PartialDemandBookGroupCopy");
        }

        public ActionResult SearchDemandBookByGroup(string projectId, string groupId,string ModelType)
        {
            ViewBag.ModelType = ModelType;
            List<ProjectCityDto> list = masterService.ProjectCitySearchByProjectIdAndGroupId(projectId, groupId);
            return PartialView("_PartialDemandBooksByGroup", list);
        }
        public ActionResult SearchDemandBookByGroupCurrent(string projectId, string oldGroupId,string newGroupId, string ModelType)
        {
            ViewBag.ModelType = ModelType;
            List<ProjectCityDto> list = requirementService.RequirementGroupCopyProjectCitySearch(projectId, oldGroupId, newGroupId, UserInfo.UserId);
            return PartialView("_PartialDemandBooksByGroupCurrent", list);
        }
        public ActionResult CopyDemandBookBatch(string FromGroupId,string TargetGroupId, string jsonArr)
        {
            List<string[]> lst = JsonConvert.DeserializeObject<List<string[]>>(jsonArr);

            foreach (string[] values in lst)
            {
                requirementService.RequirementCopy(values[1], FromGroupId, values[0], TargetGroupId, UserInfo.UserId);
            }

            return Json("");
        }
        public ActionResult RequirementGroupDelete(string GroupId)
        {
            requirementService.RequirementGroupDelete(GroupId);

            return Json("");
        }

        public ActionResult ProjectSelect(string modelType, string projectCode, string projectName, string projectShortName,string serviceTrade)
        {
            MasterService masterService = new MasterService();
            if (modelType != null && modelType.Length == 1)
            {
                modelType = modelTypes[Int32.Parse(modelType)];
            }
            List<ProjectDto> lst = masterService.ProjectSearchMaster(modelType, projectCode, projectName, projectShortName, serviceTrade,"");

            return PartialView("_PartialDemandBookProjectSelect", lst);
        }

        public ActionResult RequirementGroupSelect(string ModelType,string projectName, string projectShortName)
        {
            List<RequirementGroupDto> list = requirementService.RequirementGroupSearchByProjectName(ModelType,projectName, projectShortName, UserInfo.UserId);

            return PartialView("_PartialDemandBookGroupSelect", list);
        }
        public ActionResult RequirementGroupSearchByProjectName(string ModelType, string projectName, string projectShortName)
        {
            List<RequirementGroupDto> list = requirementService.RequirementGroupSearchByProjectName(ModelType,projectName, projectShortName, UserInfo.UserId);

            return Json(new { List = list });
        }
	}
}