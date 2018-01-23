using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Purchase.Web.Controllers
{
    [CustomHandleError]
    public class AccountController : BaseController
    {
        AccountService accountService = new AccountService();
        // GET: Account
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(string userName, string password, string returnUrl)
        {
            Mst_UserInfo account = accountService.Login(userName, password);
            //登陆成功 用户信息放到session，Cookie中
            FormsAuthentication.SetAuthCookie(account.UserName, false);

            Session["LoginUser"] = account;
            returnUrl = "/Home/Index";// string.IsNullOrEmpty(returnUrl) ? "/Home/Index" : returnUrl;

            return Json(new
            {
                redirectURL = returnUrl,
            });
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session["LoginUser"] = null;
            return this.Redirect("~/");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string sNewPassword)
        {
            using (PurchaseEntities db = new PurchaseEntities())
            {
                Mst_UserInfo userInfo = db.Mst_UserInfo.Find(UserInfo.UserId);
                userInfo.Password = Utils.StrToMD5(sNewPassword);
                db.SaveChanges();
            }
            Session["LoginUser"] = null;
            FormsAuthentication.SignOut();

            return Json(new { nAction = 2, sRedirectURL = "/Account/Login" });
        }
    }
}