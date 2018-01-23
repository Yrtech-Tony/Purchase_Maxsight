using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Purchase.DAL;
using Newtonsoft.Json;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Common;
using Purchase.Web.Common;

namespace Purchase.Web.Controllers
{
    public class ProjectsController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        MasterService service = new MasterService();

        // GET: Projects
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load(Projects condition, string userName, int pageNum, int? pageSize)
        {
            if (pageSize.HasValue)
            {
                _countPerPage = pageSize.Value;
            }
            //var Project = db.Projects.AsQueryable();
            //if (condition != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(condition.ProjectCode))
            //    {
            //        Project = Project.Where(x => x.ProjectCode.Contains(condition.ProjectCode));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.ProjectName))
            //    {
            //        Project = Project.Where(x => x.ProjectName.Contains(condition.ProjectName));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.ProjectShortName))
            //    {
            //        Project = Project.Where(x => x.ProjectShortName.Contains(condition.ProjectShortName));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.Step))
            //    {
            //        Project = Project.Where(x => x.ProjectShortName.Contains(condition.Step));
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.ModelType))
            //    {
            //        Project = Project.Where(x => x.ModelType == condition.ModelType);
            //    }
            //    if (!string.IsNullOrWhiteSpace(condition.DepartmentCode))
            //    {
            //        Project = Project.Where(x => x.DepartmentCode == condition.DepartmentCode);
            //    }
            //    //开始期间
            //    if (condition.StartDate.HasValue)
            //    {
            //        Project = Project.Where(x => x.StartDate >= condition.StartDate);
            //    }
            //    if (condition.EndDate.HasValue)
            //    {
            //        Project = Project.Where(x => x.StartDate <= condition.EndDate);
            //    }

            //}
            //string userId = (Session["LoginUser"] as Mst_UserInfo).UserId;
            //Project = Project.Where(x => x.InUserId == userId);

            //var query = from s in Project
            //        join apply in db.Apply on s.Id equals apply.ProjectId
            //            into joinAS2
            //            from apply in joinAS2.DefaultIfEmpty()
            //        join applyRecheckStatus in db.ApplyRecheckStatus on apply.Id equals applyRecheckStatus.ApplyId
            //        into joinAS3
            //        from applyRecheckStatus in joinAS3.DefaultIfEmpty()
            //            select new
            //            {
            //                Projects = s,
            //                applyRecheckStatus.RecheckStatusCode,
            //            };
            List<ProjectDto> list = service.ProjectSearch(condition.ServiceTrade, condition.ModelType, condition.DepartmentCode, condition.StartDate, condition.EndDate, condition.ProjectCode, condition.ProjectName, condition.ProjectShortName, condition.Step, (Session["LoginUser"] as Mst_UserInfo).UserId, condition.Year, userName);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();
            //var lst = query.OrderByDescending(x => x.Projects.InDateTime).Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projects projects)
        {
            if (ModelState.IsValid)
            {
                projects.InDateTime = DateTime.Now;
                projects.InUserId = UserInfo.UserId;
                if (string.IsNullOrEmpty(projects.Status))
                {
                    projects.StatusDate = DateTime.Now;
                }
                db.Projects.Add(projects);
                db.SaveChanges();

                try
                {
                    // 保存提醒的信息
                    MasterService mster = new MasterService();
                    mster.RemindCancelSave("项目结束提醒", projects.Id.ToString(), projects.InUserId, projects.EndDate.ToString());
                }
                catch (Exception)
                {
                }


                return RedirectToAction("Index", new { ModelType = modelTypes.IndexOf(projects.ModelType) });
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = new Projects() { Id = id.Value };
            ViewBag.queryParams = Request.QueryString["queryParams"];
            return View(projects);
        }
        public ActionResult FindOne(int id)
        {
            //Projects projects = db.Projects.Find(id);
            List<ProjectDto> list = service.ProjectSearchById(id.ToString(), UserInfo.UserId);

            ProjectDto projects = new ProjectDto();
            if (list.Count > 0)
            {
                projects = list[0];
            }
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProjectCitys(string projectId)
        {
            //MasterService masterService = new MasterService();
            var data = service.ProjectCitySearchByProjectId(projectId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProjectCitysByGroupId(string projectId, string groupId)
        {
            MasterService masterService = new MasterService();
            var data = masterService.ProjectCitySearchByProjectIdAndGroupId(projectId, groupId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProjectPersons(int projectId)
        {
            var lst = from a in db.ProjectPerson where a.ProjectId.Equals(projectId) select a;
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        // POST: Projects/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            Projects projects = new Projects();
            if (ModelState.IsValid)
            {
                projects = db.Projects.Find(id);
                TryUpdateModel<Projects>(projects);
                if (string.IsNullOrEmpty(projects.Status))
                {
                    projects.StatusDate = DateTime.Now;
                }

                db.SaveChanges();

                try
                {
                    // 保存提醒的信息
                    MasterService mster = new MasterService();
                    mster.RemindCancelSave("项目结束提醒", projects.Id.ToString(), projects.InUserId, projects.EndDate.ToString());
                }
                catch (Exception)
                {
                }
                string queryParams = Request.Form["queryParams"];
                return RedirectToAction("Index", new { ModelType = modelTypes.IndexOf(projects.ModelType), queryParams = queryParams });
            }
            return View(id);
        }

        [HttpPost]
        public ActionResult SaveCity(ProjectCity projectCity)
        {
            if (projectCity.Province == null)
                projectCity.Province = "";

            if (projectCity.City == null)
                projectCity.City = "";
            if (projectCity.Remark == null)
                projectCity.Remark = "";
            if (projectCity.SeqNO == 0)
            {//新增
                projectCity.SeqNO = 1;
                int maxSeq = db.ProjectCity.Where(x => x.ProjectId == projectCity.ProjectId).OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                if (maxSeq > 0)
                {
                    projectCity.SeqNO = maxSeq + 1;
                }
                projectCity.InDateTime = DateTime.Now;
                projectCity.InUserId = UserInfo.UserId;
                db.ProjectCity.Add(projectCity);
            }
            else
            {
                ProjectCity findOne = db.ProjectCity.Find(projectCity.ProjectId, projectCity.SeqNO);
                TryUpdateModel<ProjectCity>(findOne);
                if (findOne.Status == "取消")
                {
                    findOne.CancelDateTime = DateTime.Now;
                }
                else
                {
                    findOne.CancelDateTime = null;
                }
            }

            db.SaveChanges();
            return Json(new { Status = true, Data = projectCity });
        }

        [HttpPost]
        public ActionResult SavePerson(ProjectPerson person)
        {
            if (person.SeqNO == 0)
            {//新增
                int maxOne = db.ProjectPerson.Where(x => x.ProjectId == person.ProjectId).OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                person.SeqNO = maxOne + 1;
                person.InDateTime = DateTime.Now;
                person.InUserId = UserInfo.InUserId;
                db.ProjectPerson.Add(person);
            }
            else
            {
                db.Entry(person).State = EntityState.Modified;
            }
            db.SaveChanges();

            return Json(new { Status = true, Data = person });
        }

        public ActionResult DeleteProject(string ProjectId)
        {
            service.ProjectDelete(ProjectId);
            return Json("");
        }

        public ActionResult DeleteProjectCity(string ProjectId, string SeqNos)
        {
            foreach (string SeqNo in SeqNos.Split(','))
            {
                service.ProjectCityDelete(ProjectId, SeqNo);
            }

            return Json("");
        }
        public ActionResult DeleteProjectPerson(string ProjectId, string SeqNos)
        {
            foreach (string SeqNo in SeqNos.Split(','))
            {
                service.ProjectPersonDelete(ProjectId, SeqNo);
            }

            return Json("");
        }
        public ActionResult CopyProject(Projects projects, string copyProjectId)
        {
            projects.InDateTime = DateTime.Now;
            projects.InUserId = UserInfo.UserId;
            if (string.IsNullOrEmpty(projects.Status))
            {
                projects.StatusDate = DateTime.Now;
            }
            db.Projects.Add(projects);
            db.SaveChanges();
            //Copy project
            service.ProjectCopy(copyProjectId, projects.Id.ToString(), UserInfo.UserId);

            return RedirectToAction("Index", new { ModelType = modelTypes.IndexOf(projects.ModelType) });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #region 立项提交审核
        public ActionResult RecheckUserSelect()
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(ApplyType.立项.ToString(), UserInfo.UserId);

            return PartialView("_PartialRecheckUserSelect", lst);
        }
        public ActionResult ApplyCommit(string recheckUserId, Apply apply, string projectId, string applyIdexists, string seqNO, string attachArray)
        {
            ApplyService applayService = new ApplyService();
            apply.InDateTime = DateTime.Now;
            apply.ApplyUserId = UserInfo.UserId;
            apply.ApplyTypeId = ApplyType.立项.ToString();
            apply.ProjectId = Convert.ToInt32(projectId);
            int applyId = 0;
            if (Convert.ToInt32(applyIdexists) == 0)
            {
                applyId = applayService.ApplyCommit(apply, recheckUserId);
            }
            else
            {
                ApplyRecheckStatus status = new ApplyRecheckStatus();
                status.ApplyId = Convert.ToInt32(applyIdexists);
                status.RecheckUserId = ((Mst_UserInfo)Session["LoginUser"]).UserId;
                status.RecheckStatusCode = "申请";
                status.RecheckReason = apply.ApplyReason == null ? "" : apply.ApplyReason;
                status.SeqNO = Convert.ToInt32(seqNO);
                status.InDateTime = DateTime.Now;
                applayService.ApplyRecheckStatusUpdate(status, recheckUserId);
                applyId = Convert.ToInt32(applyIdexists);
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
        public ActionResult RecheckProcessExistChk(string recheckType)
        {
            ApplyService applayService = new ApplyService();
            string chk = applayService.RecheckProcessExistChk(recheckType, UserInfo.UserId);

            return Json(chk, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetAllProjects()
        {
            MasterService masterService = new MasterService();            
            List<ProjectDto> lst = masterService.ProjectSearchMaster("", "", "", "", "", UserInfo.UserId);

            return Json(lst);
        }
    }
}
