using Purchase.Common;
using Purchase.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service
{
    public class AccountService
    {
        PurchaseEntities db = new PurchaseEntities();
        public Mst_UserInfo Login(string userName, string password)
        {
           Mst_UserInfo account = db.Mst_UserInfo.Where(x => x.UserId == userName).FirstOrDefault();
           
            if (account == null)
            {
                throw new Exception("用户名不存在！");
            }
            if (account.UseChk == false)
            {
                throw new Exception("该账号已被禁用！");
            }
            if (account.Password != Utils.StrToMD5(password))
            {
                throw new Exception("密码不正确！");
            }

            return account;
        }
    }
}
