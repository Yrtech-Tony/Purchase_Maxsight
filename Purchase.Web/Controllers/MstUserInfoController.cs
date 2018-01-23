using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class MstUserInfoController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        // GET: MstUserInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load(Mst_UserInfo condition, int pageNum, int? pageSize)
        {
            var query = db.Mst_UserInfo.AsQueryable();
            if (!string.IsNullOrEmpty(condition.UserId))
            {
                query = query.Where(x => x.UserId.Contains(condition.UserId));
            }
            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query = query.Where(x => x.UserName.Contains(condition.UserName));
            }
            query = query.OrderByDescending(x => x.InDateTime);
            int total = query.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            var list = query.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }
        public ActionResult Edit(string userId)
        {
            Mst_UserInfo findOne = db.Mst_UserInfo.Find(userId);
            if (findOne == null)
            {
                findOne = new Mst_UserInfo();
            }
            return PartialView("_PartialEdit", findOne);
        }

        public ActionResult Save(string userid, string Email, string EmailPassword)
        {
            Mst_UserInfo findOne = db.Mst_UserInfo.Find(userid);

            if (findOne == null)
            {
                findOne = new Mst_UserInfo();
                TryUpdateModel<Mst_UserInfo>(findOne);
                findOne.InDateTime = DateTime.Now;
                findOne.InUserId = UserInfo.UserId;
                findOne.Password = Utils.StrToMD5("12345"); ;
                db.Mst_UserInfo.Add(findOne);
                EmailSend(Email, "采购管理平台账号添加提醒", "采购管理平台自动提醒您：您的账号已经添加完毕，您的账号为:您的账号为:" + userid + "密码为12345.请及时修改密码");
            }
            else
            {
                TryUpdateModel<Mst_UserInfo>(findOne);
                EmailSend(Email, "采购平台账号信息变更提醒", "邮箱已经变更完毕，请及时确认");
            }
            SelectListTool.UserInfoList = null;
            db.SaveChanges();

            return Json("");
        }
        public void EmailSend(string toEmail, string toTitle, string toContent)
        {
            try
            {
                MasterService mstService = new MasterService();
                UserInfoDto userinfo = mstService.UserInfoSearchByUserId(UserInfo.UserId).FirstOrDefault();


                //发送邮件
                string[] mailTo = toEmail.Split(',');
                string[] mailCC = new string[] { userinfo.Email };

                string subject = toTitle;
                string content = toContent == null ? "" : toContent.Replace("\n", "<br>");

                List<string> attachPaths = new List<string>();


                //个人签名 图片形式
                string sign = "";
                if (!string.IsNullOrEmpty(UserInfo.EmailFooter))
                {
                    string emailSignFile = Server.MapPath("~/EmailSign/" + UserInfo.EmailFooter);
                    sign = emailSignFile;
                }
                //准备发送邮件对象
                ISendMail sendMail = new UseNetMail();
                sendMail.CreateHost(new ConfigHost()
                {
                    EnableSsl = false,
                    Username = userinfo.Email,
                    Password = userinfo.EmailPassword,
                });
                sendMail.CreateMultiMail(new ConfigMail()
                {
                    From = userinfo.Email,
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

        public ActionResult Accredit(string userId, string setUserId, string type)
        {
            MasterService masterService = new MasterService();
            var lst = masterService.SetInfoByUserId(userId, setUserId, type);
            ViewBag.Type = type;
            return PartialView("_PartialAccredit", lst);
        }
        public ActionResult AccreditReload(string userId, string setUserId, string type)
        {
            MasterService masterService = new MasterService();
            var lst = masterService.SetInfoByUserId(userId, setUserId, type);
            ViewBag.Type = type;
            return PartialView("_PartialAccreditResult", lst);
        }
        public ActionResult UserProjectSave(string userId,string setUserId,string type,string chkSetIds, string allSetIds)
        {
            MasterService masterService = new MasterService();
            List<string> chkSetIdList = JsonConvert.DeserializeObject<List<string>>(chkSetIds);
            List<string> allSetIdList = JsonConvert.DeserializeObject<List<string>>(allSetIds);
            masterService.SetInfoSave_New(allSetIdList,chkSetIdList,setUserId,userId,type); 

            return Json("");
        }
    }
}