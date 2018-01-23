using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Purchase.Common;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using Purchase.Service.DTO;
namespace Purchase.DAL.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory); ;

            using (var db = new PurchaseEntities())
            {
                db.Projects.Add(new Projects
                {
                    Id=5,
                    InUserId = "1",
                    DepartmentCode = "DP",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    ExcuteType = "adsf",
                    InDateTime = DateTime.Now,
                });
                db.SaveChanges();
            }
        }
        [TestMethod]
        public void TestQueryProjects()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory); ;

            using (var db = new PurchaseEntities())
            {
                foreach (Projects project in db.Projects.ToList())
                {
                    Console.WriteLine(project.ProjectCode);
                }
            }
        }
        [TestMethod]
        public void TestSaveSupplierMng()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            using (var db = new PurchaseEntities())
            {
                SupplierMng supplier = db.SupplierMng.Find(0);
               // supplier.SupplierName = "供应商名称2";
                supplier.SupplierMngAttachmentFile.Clear();
                for (int i = 0; i < 2; i++)
                {
                    SupplierMngAttachmentFile attach = new SupplierMngAttachmentFile();
                    attach.FileName = "fileName1";
                    attach.SeqNO = i+1;
                    attach.SupplierId = supplier.Id;
                    attach.SupplierMng = supplier;
                    attach.UploadChk = true;
                    attach.InDateTime = DateTime.Now;
                    attach.InUserId = "admin";

                    supplier.SupplierMngAttachmentFile.Add(attach);
                }
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void TestProject()
        {
            PurchaseEntities db = new PurchaseEntities();
            Type t = typeof(RemindDto);
            List<RemindDto> list =  db.Database.SqlQuery(t, "EXEC up_Remind_R").Cast<RemindDto>().ToList();
        }
        [TestMethod]
        public void PrintMD5()
        {
            Console.WriteLine(Utils.StrToMD5("12345"));
        }
    }
}
