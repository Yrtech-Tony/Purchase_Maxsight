using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Attributes
{
    public class RequirementSaveFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.RequestContext.RouteData.Values["action"].ToString();
            if (!string.IsNullOrEmpty(actionName) && (actionName.StartsWith("Save") || actionName == "DeleteDtl"))
            {
                string RequirementGroupId = filterContext.RequestContext.HttpContext.Request.Form["GroupId"];
                if (string.IsNullOrEmpty(RequirementGroupId))
                {
                    RequirementGroupId = filterContext.RequestContext.HttpContext.Request.Form["RequirementGroupId"];
                }
                string ProjectId = filterContext.RequestContext.HttpContext.Request.Form["ProjectId"];
                if (!string.IsNullOrEmpty(RequirementGroupId))
                {
                    RequirementService requirementService = new RequirementService();
                    string username = ((Mst_UserInfo)filterContext.RequestContext.HttpContext.Session["LoginUser"]).UserId;
                    RequirementGroupDto group = requirementService.RequirementGroupSearchById(ProjectId, RequirementGroupId, username == null ? "" : username).FirstOrDefault();
                    //if (group !=null && !string.IsNullOrEmpty(group.ApplyStatusCode))
                    //{
                    //    throw new Exception("已提交审核或者非本人添加的项目");
                    //} 

                    // 需求书不能修改的条件由提交审核后不能修改变更为 生成确认单后不能修改
                    // 只有没有生成确认单可以随时修改
                    if (group != null && !group.UserChk)
                    {
                        // 临时开放权限
                       // throw new Exception("该需求书非本人填写，不能修改");
                    
                    }
                    if (group != null && group.QuotationGroupId!=0)
                    {
                        // 临时开放权限
                       // throw new Exception("该需求书已经生成确认单不能进行修改");
                    } 
                }
            }
        }
    }
}