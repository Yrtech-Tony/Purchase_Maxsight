using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Attributes;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    [RequirementSaveFilter]
    public class DemandBookController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        private RequirementService service = new RequirementService();
        //
        // GET: /DemandBook/
        public ActionResult Index()
        {
            return View();
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult EmailSend(string id)
        {
            RequiremetMstDto dto = service.RequirementMstSearchById(id).FirstOrDefault();
            return View(dto);
        }

        public ActionResult RequirementSaveEmail(SupplierRequirementEmail email, string attachs, string otherAttachs)
        {
            MasterService mstService = new MasterService();
            UserInfoDto userinfo = mstService.UserInfoSearchByUserId(UserInfo.UserId).FirstOrDefault();

            int maxSeqNo = db.SupplierRequirementEmail.
                Where(x => x.RequirementId == email.RequirementId).OrderByDescending(x => x.SeqNo)
                .Select(x => x.SeqNo).FirstOrDefault();
            if (maxSeqNo > 0)
            {
                email.SeqNo = maxSeqNo + 1;
            }
            else
            {
                email.SeqNo = 1;
            }
            email.InUserId = (Session["LoginUser"] as Mst_UserInfo).UserId;
            email.InDateTime = DateTime.Now;
            db.SupplierRequirementEmail.Add(email);
            db.SaveChanges();

            //发送邮件
            string[] mailTo = email.Recipients.Split(',');
            string[] mailCC = new string[] { userinfo.Email };
            if (!string.IsNullOrEmpty(email.CCPerson))
            {
                mailCC = mailCC.Union(email.CCPerson.Split(',')).ToArray();
            }
            string subject = email.Title;
            string content = email.EmailContent == null ? "" : email.EmailContent.Replace("\n", "<br>");

            List<string> attachPaths = new List<string>();
            if (attachs != null)
            {
                foreach (string attach in attachs.Split(';'))
                {
                    attachPaths.Add(Server.MapPath("~/Export/DemandBook/" + attach));
                }
                if (otherAttachs != null)
                {
                    foreach (string attach in otherAttachs.Split(';'))
                    {
                        attachPaths.Add(Server.MapPath(EmailAttachs + attach));
                    }
                }
            }


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

            return Json("");
        }

        string EmailAttachs = "~/EmailAttachs/";
        public ActionResult SaveOtherAttachs()
        {
            List<Object> retFiles = new List<Object>();
            string path = Server.MapPath(EmailAttachs);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if (Request.Files != null)
            {
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase fileBase = Request.Files.Get(key);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fileBase.FileName;
                    string fileFullName = path + fileName;
                    fileBase.SaveAs(fileFullName);
                    retFiles.Add(new { fileName = fileName, fileFullName = fileFullName });
                }
            }

            return Json(retFiles);
        }

        public ActionResult CopyDemandBook(string requirementId, string toSeqNO, string groupId)
        {
            // service.RequirementCopy(requirementId, toSeqNO, groupId, UserInfo.UserId);
            service.RequirementCopyInterGroup(requirementId, toSeqNO, groupId, UserInfo.UserId);
            return Json("");
        }
        #region 需求书详细
        public ActionResult FindBase(string ProjectId, string SeqNo, string GroupId)
        {
            List<RequirementDtl0Dto> lst = service.RequiermentDtl0Search(ProjectId, SeqNo, GroupId);

            return Json(lst);
        }

        private int SaveMst(Requirement_RequirementMst mstDto)
        {
            mstDto.InDateTime = DateTime.Now;
            mstDto.InUserId = UserInfo.UserId;
            return service.RequirementMstSave(mstDto);
        }
        public ActionResult SaveBase(int RequirementId, Requirement_RequirementMst mstDto)
        {
            Requirement_Anfang_RequirementDtl0 dtl0 = new Requirement_Anfang_RequirementDtl0();
            if (RequirementId > 0)
            {
                dtl0 = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == RequirementId).FirstOrDefault();
                if (dtl0 == null)
                {
                    dtl0 = new Requirement_Anfang_RequirementDtl0();
                    TryUpdateModel<Requirement_Anfang_RequirementDtl0>(dtl0);
                    db.Requirement_Anfang_RequirementDtl0.Add(dtl0);
                }
                else
                {
                    TryUpdateModel<Requirement_Anfang_RequirementDtl0>(dtl0);
                }
            }
            else
            {
                RequirementId = SaveMst(mstDto);

                TryUpdateModel<Requirement_Anfang_RequirementDtl0>(dtl0);
                dtl0.RequirementId = RequirementId;
                dtl0.InDateTime = DateTime.Now;
                dtl0.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl0.Add(dtl0);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadDtlLast(int RequirementId)
        {
            List<Requirement_RequirementDtlLast> lst = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveDtlLast(int RequirementId, int SeqNo)
        {
            Requirement_RequirementDtlLast dtlLast = new Requirement_RequirementDtlLast();

            if (RequirementId > 0 && SeqNo > 0)
            {
                dtlLast = db.Requirement_RequirementDtlLast.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_RequirementDtlLast>(dtlLast);
            }
            else
            {
                TryUpdateModel<Requirement_RequirementDtlLast>(dtlLast);
                dtlLast.SeqNO = 1;
                Requirement_RequirementDtlLast maxOne = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    dtlLast.SeqNO = maxOne.SeqNO + 1;
                }
                dtlLast.RequirementId = RequirementId;
                dtlLast.InDateTime = DateTime.Now;
                dtlLast.InUserId = UserInfo.UserId;
                db.Requirement_RequirementDtlLast.Add(dtlLast);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #region 暗访
        public ActionResult LoadAnfangDtl1(int RequirementId)
        {
            List<Requirement_Anfang_RequirementDtl1> lst = db.Requirement_Anfang_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveAnfangDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Anfang_RequirementDtl1 anfangDtl1 = new Requirement_Anfang_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Anfang_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Anfang_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Anfang_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Anfang_RequirementDtl1 maxOne = db.Requirement_Anfang_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadAnfangDtl2(int RequirementId)
        {
            List<Requirement_Anfang_RequirementDtl2> lst = db.Requirement_Anfang_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveAnfangDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Anfang_RequirementDtl2 anfangDtl2 = new Requirement_Anfang_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Anfang_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Anfang_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Anfang_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Anfang_RequirementDtl2 maxOne = db.Requirement_Anfang_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadAnfangDtl3(int RequirementId)
        {
            List<Requirement_Anfang_RequirementDtl3> lst = db.Requirement_Anfang_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveAnfangDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Anfang_RequirementDtl3 anfangDtl3 = new Requirement_Anfang_RequirementDtl3();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl3 = db.Requirement_Anfang_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Anfang_RequirementDtl3>(anfangDtl3);
            }
            else
            {
                TryUpdateModel<Requirement_Anfang_RequirementDtl3>(anfangDtl3);
                anfangDtl3.SeqNO = 1;
                Requirement_Anfang_RequirementDtl3 maxOne = db.Requirement_Anfang_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl3.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl3.InDateTime = DateTime.Now;
                anfangDtl3.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl3.Add(anfangDtl3);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadAnfangDtl4(int RequirementId)
        {
            List<Requirement_Anfang_RequirementDtl4> lst = db.Requirement_Anfang_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveAnfangDtl4List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Anfang_RequirementDtl4> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Anfang_RequirementDtl4>>(jsonArr);
            foreach (Requirement_Anfang_RequirementDtl4 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Anfang_RequirementDtl4 exists = db.Requirement_Anfang_RequirementDtl4.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Anfang_RequirementDtl4.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Anfang_RequirementDtl4.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Anfang_RequirementDtl4 findone = db.Requirement_Anfang_RequirementDtl4.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.ExecuteType = dtl.ExecuteType;
                        findone.CustomerType = dtl.CustomerType;
                        findone.BrandType = dtl.BrandType;
                        findone.IntoShopType = dtl.IntoShopType;
                        findone.CarChk = dtl.CarChk;
                        findone.IntoShopTypeRate = dtl.IntoShopTypeRate;
                        findone.CarType = dtl.CarType;
                        findone.CarPriceRange = dtl.CarPriceRange;
                        findone.CarLevel = dtl.CarLevel;
                        findone.Avoid = dtl.Avoid;
                        findone.IntoShopAgain = dtl.IntoShopAgain;
                        findone.IntoShopAgainRate = dtl.IntoShopAgainRate;
                        findone.SampleCount = dtl.SampleCount;
                        findone.Compensation = dtl.Compensation;
                        findone.ListSource = dtl.ListSource;
                        findone.RemarkDtl4 = dtl.RemarkDtl4;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveAnfangDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Anfang_RequirementDtl4 anfangDtl4 = new Requirement_Anfang_RequirementDtl4();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl4 = db.Requirement_Anfang_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Anfang_RequirementDtl4>(anfangDtl4);
            }
            else
            {
                TryUpdateModel<Requirement_Anfang_RequirementDtl4>(anfangDtl4);
                anfangDtl4.SeqNO = 1;
                Requirement_Anfang_RequirementDtl4 maxOne = db.Requirement_Anfang_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl4.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl4.InDateTime = DateTime.Now;
                anfangDtl4.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl4.Add(anfangDtl4);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadAnfangDtl5(int RequirementId)
        {
            List<Requirement_Anfang_RequirementDtl5> lst = db.Requirement_Anfang_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveAnfangDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Anfang_RequirementDtl5 anfangDtl5 = new Requirement_Anfang_RequirementDtl5();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl5 = db.Requirement_Anfang_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Anfang_RequirementDtl5>(anfangDtl5);
            }
            else
            {
                TryUpdateModel<Requirement_Anfang_RequirementDtl5>(anfangDtl5);
                anfangDtl5.SeqNO = 1;
                Requirement_Anfang_RequirementDtl5 maxOne = db.Requirement_Anfang_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl5.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl5.InDateTime = DateTime.Now;
                anfangDtl5.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl5.Add(anfangDtl5);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadAnfangDtl6(int RequirementId)
        {
            List<Requirement_Anfang_RequirementDtl6> lst = db.Requirement_Anfang_RequirementDtl6.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveAnfangDtl6(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Anfang_RequirementDtl6 anfangDtl6 = new Requirement_Anfang_RequirementDtl6();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl6 = db.Requirement_Anfang_RequirementDtl6.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Anfang_RequirementDtl6>(anfangDtl6);
            }
            else
            {
                TryUpdateModel<Requirement_Anfang_RequirementDtl6>(anfangDtl6);
                anfangDtl6.SeqNO = 1;
                Requirement_Anfang_RequirementDtl6 maxOne = db.Requirement_Anfang_RequirementDtl6.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl6.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl6.InDateTime = DateTime.Now;
                anfangDtl6.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl6.Add(anfangDtl6);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadAnfangDtl7(int RequirementId)
        {
            List<Requirement_Anfang_RequirementDtl7> lst = db.Requirement_Anfang_RequirementDtl7.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveAnfangDtl7(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Anfang_RequirementDtl7 anfangDtl7 = new Requirement_Anfang_RequirementDtl7();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl7 = db.Requirement_Anfang_RequirementDtl7.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Anfang_RequirementDtl7>(anfangDtl7);
            }
            else
            {
                TryUpdateModel<Requirement_Anfang_RequirementDtl7>(anfangDtl7);
                anfangDtl7.SeqNO = 1;
                Requirement_Anfang_RequirementDtl7 maxOne = db.Requirement_Anfang_RequirementDtl7.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl7.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl7.InDateTime = DateTime.Now;
                anfangDtl7.InUserId = UserInfo.UserId;
                db.Requirement_Anfang_RequirementDtl7.Add(anfangDtl7);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 明检
        public ActionResult LoadMingjianDtl1(int RequirementId)
        {
            List<Requirement_Mingjian_RequirementDtl1> lst = db.Requirement_Mingjian_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMingjianDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Mingjian_RequirementDtl1 anfangDtl1 = new Requirement_Mingjian_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Mingjian_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mingjian_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Mingjian_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Mingjian_RequirementDtl1 maxOne = db.Requirement_Mingjian_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Mingjian_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadMingjianDtl2(int RequirementId)
        {
            List<Requirement_Mingjian_RequirementDtl2> lst = db.Requirement_Mingjian_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveMingjianDtl2List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Mingjian_RequirementDtl2> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Mingjian_RequirementDtl2>>(jsonArr);
            foreach (Requirement_Mingjian_RequirementDtl2 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Mingjian_RequirementDtl2 exists = db.Requirement_Mingjian_RequirementDtl2.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Mingjian_RequirementDtl2.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Mingjian_RequirementDtl2.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Mingjian_RequirementDtl2 findone = db.Requirement_Mingjian_RequirementDtl2.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.fangwenshichang = dtl.fangwenshichang;
                        findone.zhixingnandu = dtl.zhixingnandu;
                        findone.jindianfangshi = dtl.jindianfangshi;
                        findone.jindianfangshibili = dtl.jindianfangshibili;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.baochoujine = dtl.baochoujine;
                        findone.jindianguibiyaoqiu = dtl.jindianguibiyaoqiu;
                        findone.qitashuoming2 = dtl.qitashuoming2;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveMingjianDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Mingjian_RequirementDtl2 anfangDtl2 = new Requirement_Mingjian_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Mingjian_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mingjian_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Mingjian_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Mingjian_RequirementDtl2 maxOne = db.Requirement_Mingjian_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Mingjian_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadMingjianDtl3(int RequirementId)
        {
            List<Requirement_Mingjian_RequirementDtl3> lst = db.Requirement_Mingjian_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMingjianDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Mingjian_RequirementDtl3 anfangDtl3 = new Requirement_Mingjian_RequirementDtl3();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl3 = db.Requirement_Mingjian_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mingjian_RequirementDtl3>(anfangDtl3);
            }
            else
            {
                TryUpdateModel<Requirement_Mingjian_RequirementDtl3>(anfangDtl3);
                anfangDtl3.SeqNO = 1;
                Requirement_Mingjian_RequirementDtl3 maxOne = db.Requirement_Mingjian_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl3.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl3.InDateTime = DateTime.Now;
                anfangDtl3.InUserId = UserInfo.UserId;
                db.Requirement_Mingjian_RequirementDtl3.Add(anfangDtl3);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadMingjianDtl4(int RequirementId)
        {
            List<Requirement_Mingjian_RequirementDtl4> lst = db.Requirement_Mingjian_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMingjianDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Mingjian_RequirementDtl4 anfangDtl4 = new Requirement_Mingjian_RequirementDtl4();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl4 = db.Requirement_Mingjian_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mingjian_RequirementDtl4>(anfangDtl4);
            }
            else
            {
                TryUpdateModel<Requirement_Mingjian_RequirementDtl4>(anfangDtl4);
                anfangDtl4.SeqNO = 1;
                Requirement_Mingjian_RequirementDtl4 maxOne = db.Requirement_Mingjian_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl4.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl4.InDateTime = DateTime.Now;
                anfangDtl4.InUserId = UserInfo.UserId;
                db.Requirement_Mingjian_RequirementDtl4.Add(anfangDtl4);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadMingjianDtl5(int RequirementId)
        {
            List<Requirement_Mingjian_RequirementDtl5> lst = db.Requirement_Mingjian_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMingjianDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Mingjian_RequirementDtl5 anfangDtl5 = new Requirement_Mingjian_RequirementDtl5();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl5 = db.Requirement_Mingjian_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mingjian_RequirementDtl5>(anfangDtl5);
            }
            else
            {
                TryUpdateModel<Requirement_Mingjian_RequirementDtl5>(anfangDtl5);
                anfangDtl5.SeqNO = 1;
                Requirement_Mingjian_RequirementDtl5 maxOne = db.Requirement_Mingjian_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl5.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl5.InDateTime = DateTime.Now;
                anfangDtl5.InUserId = UserInfo.UserId;
                db.Requirement_Mingjian_RequirementDtl5.Add(anfangDtl5);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 座谈会
        public ActionResult LoadZuotanhuiDtl1(int RequirementId)
        {
            List<Requirement_Zuotanhui_RequirementDtl1> lst = db.Requirement_Zuotanhui_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveZuotanhuiDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Zuotanhui_RequirementDtl1 anfangDtl1 = new Requirement_Zuotanhui_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Zuotanhui_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Zuotanhui_RequirementDtl1 maxOne = db.Requirement_Zuotanhui_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Zuotanhui_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadZuotanhuiDtl2(int RequirementId)
        {
            List<Requirement_Zuotanhui_RequirementDtl2> lst = db.Requirement_Zuotanhui_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveZuotanhuiDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Zuotanhui_RequirementDtl2 anfangDtl2 = new Requirement_Zuotanhui_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Zuotanhui_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Zuotanhui_RequirementDtl2 maxOne = db.Requirement_Zuotanhui_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Zuotanhui_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadZuotanhuiDtl3(int RequirementId)
        {
            List<Requirement_Zuotanhui_RequirementDtl3> lst = db.Requirement_Zuotanhui_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveZuotanhuiDtl3List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Zuotanhui_RequirementDtl3> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Zuotanhui_RequirementDtl3>>(jsonArr);
            foreach (Requirement_Zuotanhui_RequirementDtl3 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Zuotanhui_RequirementDtl3 exists = db.Requirement_Zuotanhui_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Zuotanhui_RequirementDtl3.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Zuotanhui_RequirementDtl3.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Zuotanhui_RequirementDtl3 findone = db.Requirement_Zuotanhui_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.yonghufenlei = dtl.yonghufenlei;
                        findone.xianyouhuoqianxian = dtl.xianyouhuoqianxian;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.goucheyugoushijianduan = dtl.goucheyugoushijianduan;
                        findone.jutigoucheshijian = dtl.jutigoucheshijian;
                        findone.shouhouweixiubaoyangshijian = dtl.shouhouweixiubaoyangshijian;
                        findone.cheliangleibie = dtl.cheliangleibie;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.chejiafanwei = dtl.chejiafanwei;
                        findone.canhuirenshu = dtl.canhuirenshu;
                        findone.danduyaoyue = dtl.danduyaoyue;
                        // findone.jingxiaosanfang = dtl.jingxiaosanfang;
                        findone.peiefenbu = dtl.peiefenbu;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.gongzuozhize = dtl.gongzuozhize;
                        findone.baochoufangshi = dtl.baochoufangshi;
                        findone.baochoujine = dtl.baochoujine;
                        findone.mingdanlaiyuan = dtl.mingdanlaiyuan;
                        findone.qitashuoming3 = dtl.qitashuoming3;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveZuotanhuiDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Zuotanhui_RequirementDtl3 ZuotanhuiDtl3 = new Requirement_Zuotanhui_RequirementDtl3();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ZuotanhuiDtl3 = db.Requirement_Zuotanhui_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl3>(ZuotanhuiDtl3);
            }
            else
            {
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl3>(ZuotanhuiDtl3);
                ZuotanhuiDtl3.SeqNO = 1;
                Requirement_Zuotanhui_RequirementDtl3 maxOne = db.Requirement_Zuotanhui_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ZuotanhuiDtl3.SeqNO = maxOne.SeqNO + 1;
                }

                ZuotanhuiDtl3.InDateTime = DateTime.Now;
                ZuotanhuiDtl3.InUserId = UserInfo.UserId;
                db.Requirement_Zuotanhui_RequirementDtl3.Add(ZuotanhuiDtl3);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadZuotanhuiDtl4(int RequirementId)
        {
            List<Requirement_Zuotanhui_RequirementDtl4> lst = db.Requirement_Zuotanhui_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveZuotanhuiDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Zuotanhui_RequirementDtl4 ZuotanhuiDtl4 = new Requirement_Zuotanhui_RequirementDtl4();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ZuotanhuiDtl4 = db.Requirement_Zuotanhui_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl4>(ZuotanhuiDtl4);
            }
            else
            {
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl4>(ZuotanhuiDtl4);
                ZuotanhuiDtl4.SeqNO = 1;
                Requirement_Zuotanhui_RequirementDtl4 maxOne = db.Requirement_Zuotanhui_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ZuotanhuiDtl4.SeqNO = maxOne.SeqNO + 1;
                }

                ZuotanhuiDtl4.InDateTime = DateTime.Now;
                ZuotanhuiDtl4.InUserId = UserInfo.UserId;
                db.Requirement_Zuotanhui_RequirementDtl4.Add(ZuotanhuiDtl4);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadZuotanhuiDtl5(int RequirementId)
        {
            List<Requirement_Zuotanhui_RequirementDtl5> lst = db.Requirement_Zuotanhui_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveZuotanhuiDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Zuotanhui_RequirementDtl5 ZuotanhuiDtl5 = new Requirement_Zuotanhui_RequirementDtl5();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ZuotanhuiDtl5 = db.Requirement_Zuotanhui_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl5>(ZuotanhuiDtl5);
            }
            else
            {
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl5>(ZuotanhuiDtl5);
                ZuotanhuiDtl5.SeqNO = 1;
                Requirement_Zuotanhui_RequirementDtl5 maxOne = db.Requirement_Zuotanhui_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ZuotanhuiDtl5.SeqNO = maxOne.SeqNO + 1;
                }

                ZuotanhuiDtl5.InDateTime = DateTime.Now;
                ZuotanhuiDtl5.InUserId = UserInfo.UserId;
                db.Requirement_Zuotanhui_RequirementDtl5.Add(ZuotanhuiDtl5);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadZuotanhuiDtl6(int RequirementId)
        {
            List<Requirement_Zuotanhui_RequirementDtl6> lst = db.Requirement_Zuotanhui_RequirementDtl6.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveZuotanhuiDtl6(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Zuotanhui_RequirementDtl6 ZuotanhuiDtl6 = new Requirement_Zuotanhui_RequirementDtl6();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ZuotanhuiDtl6 = db.Requirement_Zuotanhui_RequirementDtl6.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl6>(ZuotanhuiDtl6);
            }
            else
            {
                TryUpdateModel<Requirement_Zuotanhui_RequirementDtl6>(ZuotanhuiDtl6);
                ZuotanhuiDtl6.SeqNO = 1;
                Requirement_Zuotanhui_RequirementDtl6 maxOne = db.Requirement_Zuotanhui_RequirementDtl6.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ZuotanhuiDtl6.SeqNO = maxOne.SeqNO + 1;
                }

                ZuotanhuiDtl6.InDateTime = DateTime.Now;
                ZuotanhuiDtl6.InUserId = UserInfo.UserId;
                db.Requirement_Zuotanhui_RequirementDtl6.Add(ZuotanhuiDtl6);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 运输租车
        public ActionResult LoadYunshuzucheDtl1(int RequirementId)
        {
            List<Requirement_Yunshuzuche_RequirementDtl1> lst = db.Requirement_Yunshuzuche_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveYunshuzucheDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Yunshuzuche_RequirementDtl1 anfangDtl1 = new Requirement_Yunshuzuche_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Yunshuzuche_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Yunshuzuche_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Yunshuzuche_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Yunshuzuche_RequirementDtl1 maxOne = db.Requirement_Yunshuzuche_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Yunshuzuche_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadYunshuzucheDtl2(int RequirementId)
        {
            List<Requirement_Yunshuzuche_RequirementDtl2> lst = db.Requirement_Yunshuzuche_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveYunshuzucheDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Yunshuzuche_RequirementDtl2 anfangDtl2 = new Requirement_Yunshuzuche_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Yunshuzuche_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Yunshuzuche_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Yunshuzuche_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Yunshuzuche_RequirementDtl2 maxOne = db.Requirement_Yunshuzuche_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Yunshuzuche_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 研究及数据分析
        public ActionResult LoadYanjiujishujufenxiDtl1(int RequirementId)
        {
            List<Requirement_Yanjiujishujufenxi_RequirementDtl1> lst = db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveYanjiujishujufenxiDtl1List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Yanjiujishujufenxi_RequirementDtl1> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Yanjiujishujufenxi_RequirementDtl1>>(jsonArr);
            foreach (Requirement_Yanjiujishujufenxi_RequirementDtl1 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Yanjiujishujufenxi_RequirementDtl1 exists = db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Yanjiujishujufenxi_RequirementDtl1 findone = db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.gongzuofenlei = dtl.gongzuofenlei;
                        findone.gongzuoneirong = dtl.gongzuoneirong;
                        findone.renyuangangwei = dtl.renyuangangwei;
                        findone.leixingpinpai = dtl.leixingpinpai;
                        findone.guigeyaoqiu = dtl.guigeyaoqiu;
                        findone.zhiliangbiaozhun = dtl.zhiliangbiaozhun;
                        findone.shuliang = dtl.shuliang;
                        findone.shijiananpai = dtl.shijiananpai;
                        findone.qitashuoming1 = dtl.qitashuoming1;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveYanjiujishujufenxiDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Yanjiujishujufenxi_RequirementDtl1 anfangDtl1 = new Requirement_Yanjiujishujufenxi_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Yanjiujishujufenxi_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Yanjiujishujufenxi_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Yanjiujishujufenxi_RequirementDtl1 maxOne = db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadYanjiujishujufenxiDtl2(int RequirementId)
        {
            List<Requirement_Yanjiujishujufenxi_RequirementDtl2> lst = db.Requirement_Yanjiujishujufenxi_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveYanjiujishujufenxiDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Yanjiujishujufenxi_RequirementDtl2 anfangDtl2 = new Requirement_Yanjiujishujufenxi_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Yanjiujishujufenxi_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Yanjiujishujufenxi_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Yanjiujishujufenxi_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Yanjiujishujufenxi_RequirementDtl2 maxOne = db.Requirement_Yanjiujishujufenxi_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Yanjiujishujufenxi_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 网络调研
        public ActionResult LoadWangluodiaoyanDtl1(int RequirementId)
        {
            List<Requirement_Wangluodiaoyan_RequirementDtl1> lst = db.Requirement_Wangluodiaoyan_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveWangluodiaoyanDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Wangluodiaoyan_RequirementDtl1 anfangDtl1 = new Requirement_Wangluodiaoyan_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Wangluodiaoyan_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Wangluodiaoyan_RequirementDtl1 maxOne = db.Requirement_Wangluodiaoyan_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Wangluodiaoyan_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadWangluodiaoyanDtl2(int RequirementId)
        {
            List<Requirement_Wangluodiaoyan_RequirementDtl2> lst = db.Requirement_Wangluodiaoyan_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveWangluodiaoyanDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Wangluodiaoyan_RequirementDtl2 anfangDtl2 = new Requirement_Wangluodiaoyan_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Wangluodiaoyan_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Wangluodiaoyan_RequirementDtl2 maxOne = db.Requirement_Wangluodiaoyan_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Wangluodiaoyan_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadWangluodiaoyanDtl3(int RequirementId)
        {
            List<Requirement_Wangluodiaoyan_RequirementDtl3> lst = db.Requirement_Wangluodiaoyan_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveWangluodiaoyanDtl3List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Wangluodiaoyan_RequirementDtl3> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Wangluodiaoyan_RequirementDtl3>>(jsonArr);
            foreach (Requirement_Wangluodiaoyan_RequirementDtl3 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Wangluodiaoyan_RequirementDtl3 exists = db.Requirement_Wangluodiaoyan_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Wangluodiaoyan_RequirementDtl3.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Wangluodiaoyan_RequirementDtl3.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Wangluodiaoyan_RequirementDtl3 findone = db.Requirement_Wangluodiaoyan_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.fangwenshichang = dtl.fangwenshichang;
                        findone.yonghufenlei = dtl.yonghufenlei;
                        findone.xianyouhuoqianzai = dtl.xianyouhuoqianzai;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.goucheyugoushijianduan = dtl.goucheyugoushijianduan;
                        findone.jutigoucheshijian = dtl.jutigoucheshijian;
                        findone.shouhouweixiubaoyangshijian = dtl.shouhouweixiubaoyangshijian;
                        findone.cheliangleibie = dtl.cheliangleibie;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.chejiafanwen = dtl.chejiafanwen;
                        findone.peiefenbu = dtl.peiefenbu;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.gongzuozhize = dtl.gongzuozhize;
                        findone.baochoufangshi = dtl.baochoufangshi;
                        findone.baochoujine = dtl.baochoujine;
                        findone.mingdanlaiyuan = dtl.mingdanlaiyuan;
                        findone.qitashuoming3 = dtl.qitashuoming3;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveWangluodiaoyanDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Wangluodiaoyan_RequirementDtl3 WangluodiaoyanDtl3 = new Requirement_Wangluodiaoyan_RequirementDtl3();
            if (RequirementId > 0 && SeqNo > 0)
            {
                WangluodiaoyanDtl3 = db.Requirement_Wangluodiaoyan_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl3>(WangluodiaoyanDtl3);
            }
            else
            {
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl3>(WangluodiaoyanDtl3);
                WangluodiaoyanDtl3.SeqNO = 1;
                Requirement_Wangluodiaoyan_RequirementDtl3 maxOne = db.Requirement_Wangluodiaoyan_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    WangluodiaoyanDtl3.SeqNO = maxOne.SeqNO + 1;
                }

                WangluodiaoyanDtl3.InDateTime = DateTime.Now;
                WangluodiaoyanDtl3.InUserId = UserInfo.UserId;
                db.Requirement_Wangluodiaoyan_RequirementDtl3.Add(WangluodiaoyanDtl3);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadWangluodiaoyanDtl4(int RequirementId)
        {
            List<Requirement_Wangluodiaoyan_RequirementDtl4> lst = db.Requirement_Wangluodiaoyan_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveWangluodiaoyanDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Wangluodiaoyan_RequirementDtl4 WangluodiaoyanDtl4 = new Requirement_Wangluodiaoyan_RequirementDtl4();
            if (RequirementId > 0 && SeqNo > 0)
            {
                WangluodiaoyanDtl4 = db.Requirement_Wangluodiaoyan_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl4>(WangluodiaoyanDtl4);
            }
            else
            {
                TryUpdateModel<Requirement_Wangluodiaoyan_RequirementDtl4>(WangluodiaoyanDtl4);
                WangluodiaoyanDtl4.SeqNO = 1;
                Requirement_Wangluodiaoyan_RequirementDtl4 maxOne = db.Requirement_Wangluodiaoyan_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    WangluodiaoyanDtl4.SeqNO = maxOne.SeqNO + 1;
                }

                WangluodiaoyanDtl4.InDateTime = DateTime.Now;
                WangluodiaoyanDtl4.InUserId = UserInfo.UserId;
                db.Requirement_Wangluodiaoyan_RequirementDtl4.Add(WangluodiaoyanDtl4);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        #endregion
        #region 供应商差旅
        public ActionResult LoadGongyingshangchailvDtl1(int RequirementId)
        {
            List<Requirement_Gongyingshangchailv_RequirementDtl1> lst = db.Requirement_Gongyingshangchailv_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveGongyingshangchailvDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Gongyingshangchailv_RequirementDtl1 anfangDtl1 = new Requirement_Gongyingshangchailv_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Gongyingshangchailv_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Gongyingshangchailv_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Gongyingshangchailv_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Gongyingshangchailv_RequirementDtl1 maxOne = db.Requirement_Gongyingshangchailv_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Gongyingshangchailv_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 盘库及价格调查
        public ActionResult LoadPankujijiagediaochaDtl1(int RequirementId)
        {
            List<Requirement_Pankujijiagediaocha_RequirementDtl1> lst = db.Requirement_Pankujijiagediaocha_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SavePankujijiagediaochaDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Pankujijiagediaocha_RequirementDtl1 anfangDtl1 = new Requirement_Pankujijiagediaocha_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Pankujijiagediaocha_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Pankujijiagediaocha_RequirementDtl1 maxOne = db.Requirement_Pankujijiagediaocha_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Pankujijiagediaocha_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadPankujijiagediaochaDtl2(int RequirementId)
        {
            List<Requirement_Pankujijiagediaocha_RequirementDtl2> lst = db.Requirement_Pankujijiagediaocha_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SavePankujijiagediaochaDtl2List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Pankujijiagediaocha_RequirementDtl2> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Pankujijiagediaocha_RequirementDtl2>>(jsonArr);
            foreach (Requirement_Pankujijiagediaocha_RequirementDtl2 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Pankujijiagediaocha_RequirementDtl2 exists = db.Requirement_Pankujijiagediaocha_RequirementDtl2.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Pankujijiagediaocha_RequirementDtl2.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Pankujijiagediaocha_RequirementDtl2.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Pankujijiagediaocha_RequirementDtl2 findone = db.Requirement_Pankujijiagediaocha_RequirementDtl2.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.pingpaifenlei = dtl.pingpaifenlei;
                        findone.jindianfangshi = dtl.jindianfangshi;
                        findone.kaichejindian = dtl.kaichejindian;
                        findone.jindianfangshibili = dtl.jindianfangshibili;
                        findone.cheliangleixing = dtl.cheliangleixing;
                        findone.chejiafanwei = dtl.chejiafanwei;
                        findone.chexingjibie = dtl.chexingjibie;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.baochoujine = dtl.baochoujine;
                        findone.qitashuoming2 = dtl.qitashuoming2;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SavePankujijiagediaochaDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Pankujijiagediaocha_RequirementDtl2 anfangDtl1 = new Requirement_Pankujijiagediaocha_RequirementDtl2();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Pankujijiagediaocha_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl2>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl2>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Pankujijiagediaocha_RequirementDtl2 maxOne = db.Requirement_Pankujijiagediaocha_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Pankujijiagediaocha_RequirementDtl2.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadPankujijiagediaochaDtl3(int RequirementId)
        {
            List<Requirement_Pankujijiagediaocha_RequirementDtl3> lst = db.Requirement_Pankujijiagediaocha_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SavePankujijiagediaochaDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Pankujijiagediaocha_RequirementDtl3 anfangDtl1 = new Requirement_Pankujijiagediaocha_RequirementDtl3();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Pankujijiagediaocha_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl3>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl3>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Pankujijiagediaocha_RequirementDtl3 maxOne = db.Requirement_Pankujijiagediaocha_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Pankujijiagediaocha_RequirementDtl3.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadPankujijiagediaochaDtl4(int RequirementId)
        {
            List<Requirement_Pankujijiagediaocha_RequirementDtl4> lst = db.Requirement_Pankujijiagediaocha_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SavePankujijiagediaochaDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Pankujijiagediaocha_RequirementDtl4 anfangDtl1 = new Requirement_Pankujijiagediaocha_RequirementDtl4();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Pankujijiagediaocha_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl4>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl4>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Pankujijiagediaocha_RequirementDtl4 maxOne = db.Requirement_Pankujijiagediaocha_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Pankujijiagediaocha_RequirementDtl4.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadPankujijiagediaochaDtl5(int RequirementId)
        {
            List<Requirement_Pankujijiagediaocha_RequirementDtl5> lst = db.Requirement_Pankujijiagediaocha_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SavePankujijiagediaochaDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Pankujijiagediaocha_RequirementDtl5 anfangDtl1 = new Requirement_Pankujijiagediaocha_RequirementDtl5();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Pankujijiagediaocha_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl5>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Pankujijiagediaocha_RequirementDtl5>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Pankujijiagediaocha_RequirementDtl5 maxOne = db.Requirement_Pankujijiagediaocha_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Pankujijiagediaocha_RequirementDtl5.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        #endregion
        #region  面访及电访
        public ActionResult LoadMianfangjidianfangDtl1(int RequirementId)
        {
            List<Requirement_Mianfangjidianfang_RequirementDtl1> lst = db.Requirement_Mianfangjidianfang_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMianfangjidianfangDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Mianfangjidianfang_RequirementDtl1 anfangDtl1 = new Requirement_Mianfangjidianfang_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Mianfangjidianfang_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Mianfangjidianfang_RequirementDtl1 maxOne = db.Requirement_Mianfangjidianfang_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Mianfangjidianfang_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadMianfangjidianfangDtl2(int RequirementId)
        {
            List<Requirement_Mianfangjidianfang_RequirementDtl2> lst = db.Requirement_Mianfangjidianfang_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMianfangjidianfangDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Mianfangjidianfang_RequirementDtl2 anfangDtl1 = new Requirement_Mianfangjidianfang_RequirementDtl2();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Mianfangjidianfang_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl2>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl2>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Mianfangjidianfang_RequirementDtl2 maxOne = db.Requirement_Mianfangjidianfang_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Mianfangjidianfang_RequirementDtl2.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadMianfangjidianfangDtl3(int RequirementId)
        {
            List<Requirement_Mianfangjidianfang_RequirementDtl3> lst = db.Requirement_Mianfangjidianfang_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveMianfangjidianfangDtl3List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Mianfangjidianfang_RequirementDtl3> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Mianfangjidianfang_RequirementDtl3>>(jsonArr);
            foreach (Requirement_Mianfangjidianfang_RequirementDtl3 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Mianfangjidianfang_RequirementDtl3 exists = db.Requirement_Mianfangjidianfang_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Mianfangjidianfang_RequirementDtl3.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Mianfangjidianfang_RequirementDtl3.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Mianfangjidianfang_RequirementDtl3 findone = db.Requirement_Mianfangjidianfang_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.fangwenshichang = dtl.fangwenshichang;
                        findone.yonghufenlei = dtl.yonghufenlei;
                        findone.xianyouhuoqianzai = dtl.xianyouhuoqianzai;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.goucheyugoushijianduan = dtl.goucheyugoushijianduan;
                        findone.jutigoucheshijian = dtl.jutigoucheshijian;
                        findone.shouhouweixiubaoyangshijian = dtl.shouhouweixiubaoyangshijian;
                        findone.cheliangleibie = dtl.cheliangleibie;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.chejiafanwei = dtl.chejiafanwei;
                        findone.peiefenbu = dtl.peiefenbu;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.gongzuozhize = dtl.gongzuozhize;
                        findone.baochoufangshi = dtl.baochoufangshi;
                        findone.baochoujine = dtl.baochoujine;
                        findone.chenggonglv = dtl.chenggonglv;
                        findone.mingdanlaiyuan = dtl.mingdanlaiyuan;
                        findone.qitashuoming3 = dtl.qitashuoming3;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveMianfangjidianfangDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Mianfangjidianfang_RequirementDtl3 anfangDtl1 = new Requirement_Mianfangjidianfang_RequirementDtl3();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Mianfangjidianfang_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl3>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl3>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Mianfangjidianfang_RequirementDtl3 maxOne = db.Requirement_Mianfangjidianfang_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Mianfangjidianfang_RequirementDtl3.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadMianfangjidianfangDtl4(int RequirementId)
        {
            List<Requirement_Mianfangjidianfang_RequirementDtl4> lst = db.Requirement_Mianfangjidianfang_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMianfangjidianfangDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Mianfangjidianfang_RequirementDtl4 anfangDtl1 = new Requirement_Mianfangjidianfang_RequirementDtl4();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Mianfangjidianfang_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl4>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl4>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Mianfangjidianfang_RequirementDtl4 maxOne = db.Requirement_Mianfangjidianfang_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Mianfangjidianfang_RequirementDtl4.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadMianfangjidianfangDtl5(int RequirementId)
        {
            List<Requirement_Mianfangjidianfang_RequirementDtl5> lst = db.Requirement_Mianfangjidianfang_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveMianfangjidianfangDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Mianfangjidianfang_RequirementDtl5 anfangDtl1 = new Requirement_Mianfangjidianfang_RequirementDtl5();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Mianfangjidianfang_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl5>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Mianfangjidianfang_RequirementDtl5>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Mianfangjidianfang_RequirementDtl5 maxOne = db.Requirement_Mianfangjidianfang_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Mianfangjidianfang_RequirementDtl5.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        #endregion
        #region  管控
        public ActionResult LoadGuankongDtl1(int RequirementId)
        {
            List<Requirement_Guangkong_RequirementDtl1> lst = db.Requirement_Guangkong_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveGuankongDtl1List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Guangkong_RequirementDtl1> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Guangkong_RequirementDtl1>>(jsonArr);
            foreach (Requirement_Guangkong_RequirementDtl1 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Guangkong_RequirementDtl1 exists = db.Requirement_Guangkong_RequirementDtl1.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Guangkong_RequirementDtl1.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Guangkong_RequirementDtl1.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Guangkong_RequirementDtl1 findone = db.Requirement_Guangkong_RequirementDtl1.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.yaoyuechexingjiebie = dtl.yaoyuechexingjiebie;
                        findone.yaoyuechezhushenhe = dtl.yaoyuechezhushenhe;
                        findone.yaoyuechezhuzhenbie = dtl.yaoyuechezhuzhenbie;
                        findone.jindukongzhi = dtl.jindukongzhi;
                        findone.chuchaijiankong = dtl.chuchaijiankong;
                        findone.xiangmupeixun = dtl.xiangmupeixun;
                        findone.luyinjianting = dtl.luyinjianting;
                        findone.ziliaohecha = dtl.ziliaohecha;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.qitashuoming1 = dtl.qitashuoming1;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveGuankongDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Guangkong_RequirementDtl1 anfangDtl1 = new Requirement_Guangkong_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Guangkong_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Guangkong_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Guangkong_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Guangkong_RequirementDtl1 maxOne = db.Requirement_Guangkong_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Guangkong_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadGuankongDtl2(int RequirementId)
        {
            List<Requirement_Guangkong_RequirementDtl2> lst = db.Requirement_Guangkong_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveGuankongDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Guangkong_RequirementDtl2 anfangDtl1 = new Requirement_Guangkong_RequirementDtl2();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Guangkong_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Guangkong_RequirementDtl2>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Guangkong_RequirementDtl2>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Guangkong_RequirementDtl2 maxOne = db.Requirement_Guangkong_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Guangkong_RequirementDtl2.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadGuankongDtl3(int RequirementId)
        {
            List<Requirement_Guangkong_RequirementDtl3> lst = db.Requirement_Guangkong_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveGuankongDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Guangkong_RequirementDtl3 anfangDtl1 = new Requirement_Guangkong_RequirementDtl3();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Guangkong_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Guangkong_RequirementDtl3>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Guangkong_RequirementDtl3>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Guangkong_RequirementDtl3 maxOne = db.Requirement_Guangkong_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Guangkong_RequirementDtl3.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        #endregion
        #region 深访及日记留置
        public ActionResult LoadShenfangjirijiliuzhiDtl1(int RequirementId)
        {
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl1> lst = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveShenfangjirijiliuzhiDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Shenfangjirijiliuzhi_RequirementDtl1 anfangDtl1 = new Requirement_Shenfangjirijiliuzhi_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Shenfangjirijiliuzhi_RequirementDtl1 maxOne = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Shenfangjirijiliuzhi_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadShenfangjirijiliuzhiDtl2(int RequirementId)
        {
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl2> lst = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveShenfangjirijiliuzhiDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Shenfangjirijiliuzhi_RequirementDtl2 anfangDtl2 = new Requirement_Shenfangjirijiliuzhi_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Shenfangjirijiliuzhi_RequirementDtl2 maxOne = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Shenfangjirijiliuzhi_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadShenfangjirijiliuzhiDtl3(int RequirementId)
        {
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl3> lst = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveShenfangjirijiliuzhiDtl3List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl3> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Shenfangjirijiliuzhi_RequirementDtl3>>(jsonArr);
            foreach (Requirement_Shenfangjirijiliuzhi_RequirementDtl3 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Shenfangjirijiliuzhi_RequirementDtl3 exists = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Shenfangjirijiliuzhi_RequirementDtl3 findone = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.fangwenshichang = dtl.fangwenshichang;
                        findone.yonghufenlei = dtl.yonghufenlei;
                        findone.xianyouhuoqianzai = dtl.xianyouhuoqianzai;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.mingdanlaiyuan = dtl.mingdanlaiyuan;
                        findone.jingyingshijianyaoqiu = dtl.jingyingshijianyaoqiu;
                        findone.gongzuoshijianyaoqiu = dtl.gongzuoshijianyaoqiu;
                        findone.goucheyugoushijianduan = dtl.goucheyugoushijianduan;
                        findone.jutigoucheshijian = dtl.jutigoucheshijian;
                        findone.shouhouweixiubaoyang = dtl.shouhouweixiubaoyang;
                        findone.cheliangleixing = dtl.cheliangleixing;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.chejiafanwei = dtl.chejiafanwei;
                        findone.canhuirenshu = dtl.canhuirenshu;
                        findone.danduyaoyue = dtl.danduyaoyue;
                        findone.jingxiaosanfang = dtl.jingxiaosanfang;
                        findone.peiefenbu = dtl.peiefenbu;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.gongzouzhize = dtl.gongzouzhize;
                        findone.baochoufangshi = dtl.baochoufangshi;
                        findone.baochoujine = dtl.baochoujine;
                        findone.qitashuoming3 = dtl.qitashuoming3;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveShenfangjirijiliuzhiDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Shenfangjirijiliuzhi_RequirementDtl3 ShenfangjirijiliuzhiDtl3 = new Requirement_Shenfangjirijiliuzhi_RequirementDtl3();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ShenfangjirijiliuzhiDtl3 = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl3>(ShenfangjirijiliuzhiDtl3);
            }
            else
            {
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl3>(ShenfangjirijiliuzhiDtl3);
                ShenfangjirijiliuzhiDtl3.SeqNO = 1;
                Requirement_Shenfangjirijiliuzhi_RequirementDtl3 maxOne = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ShenfangjirijiliuzhiDtl3.SeqNO = maxOne.SeqNO + 1;
                }

                ShenfangjirijiliuzhiDtl3.InDateTime = DateTime.Now;
                ShenfangjirijiliuzhiDtl3.InUserId = UserInfo.UserId;
                db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Add(ShenfangjirijiliuzhiDtl3);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadShenfangjirijiliuzhiDtl4(int RequirementId)
        {
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl4> lst = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveShenfangjirijiliuzhiDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Shenfangjirijiliuzhi_RequirementDtl4 ShenfangjirijiliuzhiDtl4 = new Requirement_Shenfangjirijiliuzhi_RequirementDtl4();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ShenfangjirijiliuzhiDtl4 = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl4>(ShenfangjirijiliuzhiDtl4);
            }
            else
            {
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl4>(ShenfangjirijiliuzhiDtl4);
                ShenfangjirijiliuzhiDtl4.SeqNO = 1;
                Requirement_Shenfangjirijiliuzhi_RequirementDtl4 maxOne = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ShenfangjirijiliuzhiDtl4.SeqNO = maxOne.SeqNO + 1;
                }

                ShenfangjirijiliuzhiDtl4.InDateTime = DateTime.Now;
                ShenfangjirijiliuzhiDtl4.InUserId = UserInfo.UserId;
                db.Requirement_Shenfangjirijiliuzhi_RequirementDtl4.Add(ShenfangjirijiliuzhiDtl4);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadShenfangjirijiliuzhiDtl5(int RequirementId)
        {
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl5> lst = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveShenfangjirijiliuzhiDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Shenfangjirijiliuzhi_RequirementDtl5 ShenfangjirijiliuzhiDtl5 = new Requirement_Shenfangjirijiliuzhi_RequirementDtl5();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ShenfangjirijiliuzhiDtl5 = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl5>(ShenfangjirijiliuzhiDtl5);
            }
            else
            {
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl5>(ShenfangjirijiliuzhiDtl5);
                ShenfangjirijiliuzhiDtl5.SeqNO = 1;
                Requirement_Shenfangjirijiliuzhi_RequirementDtl5 maxOne = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ShenfangjirijiliuzhiDtl5.SeqNO = maxOne.SeqNO + 1;
                }

                ShenfangjirijiliuzhiDtl5.InDateTime = DateTime.Now;
                ShenfangjirijiliuzhiDtl5.InUserId = UserInfo.UserId;
                db.Requirement_Shenfangjirijiliuzhi_RequirementDtl5.Add(ShenfangjirijiliuzhiDtl5);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadShenfangjirijiliuzhiDtl6(int RequirementId)
        {
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl6> lst = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl6.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveShenfangjirijiliuzhiDtl6(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Shenfangjirijiliuzhi_RequirementDtl6 ShenfangjirijiliuzhiDtl6 = new Requirement_Shenfangjirijiliuzhi_RequirementDtl6();
            if (RequirementId > 0 && SeqNo > 0)
            {
                ShenfangjirijiliuzhiDtl6 = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl6.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl6>(ShenfangjirijiliuzhiDtl6);
            }
            else
            {
                TryUpdateModel<Requirement_Shenfangjirijiliuzhi_RequirementDtl6>(ShenfangjirijiliuzhiDtl6);
                ShenfangjirijiliuzhiDtl6.SeqNO = 1;
                Requirement_Shenfangjirijiliuzhi_RequirementDtl6 maxOne = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl6.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    ShenfangjirijiliuzhiDtl6.SeqNO = maxOne.SeqNO + 1;
                }

                ShenfangjirijiliuzhiDtl6.InDateTime = DateTime.Now;
                ShenfangjirijiliuzhiDtl6.InUserId = UserInfo.UserId;
                db.Requirement_Shenfangjirijiliuzhi_RequirementDtl6.Add(ShenfangjirijiliuzhiDtl6);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 场地布展
        public ActionResult LoadChangdibuzhanDtl1(int RequirementId)
        {
            List<Requirement_Changdibuzhan_RequirementDtl1> lst = db.Requirement_Changdibuzhan_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveChangdibuzhanDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Changdibuzhan_RequirementDtl1 anfangDtl1 = new Requirement_Changdibuzhan_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Changdibuzhan_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Changdibuzhan_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Changdibuzhan_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Changdibuzhan_RequirementDtl1 maxOne = db.Requirement_Changdibuzhan_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Changdibuzhan_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadChangdibuzhanDtl2(int RequirementId)
        {
            List<Requirement_Changdibuzhan_RequirementDtl2> lst = db.Requirement_Changdibuzhan_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveChangdibuzhanDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Changdibuzhan_RequirementDtl2 anfangDtl2 = new Requirement_Changdibuzhan_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Changdibuzhan_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Changdibuzhan_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Changdibuzhan_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Changdibuzhan_RequirementDtl2 maxOne = db.Requirement_Changdibuzhan_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Changdibuzhan_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 编程及编辑
        public ActionResult LoadBianchengjibianjiDtl1(int RequirementId)
        {
            List<Requirement_Bianchengjibianji_RequirementDtl1> lst = db.Requirement_Bianchengjibianji_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveBianchengjibianjiDtl1List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Bianchengjibianji_RequirementDtl1> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Bianchengjibianji_RequirementDtl1>>(jsonArr);
            foreach (Requirement_Bianchengjibianji_RequirementDtl1 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Bianchengjibianji_RequirementDtl1 exists = db.Requirement_Bianchengjibianji_RequirementDtl1.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Bianchengjibianji_RequirementDtl1.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Bianchengjibianji_RequirementDtl1.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Bianchengjibianji_RequirementDtl1 findone = db.Requirement_Bianchengjibianji_RequirementDtl1.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.gongzuofenlei = dtl.gongzuofenlei;
                        findone.gongzuoneirong = dtl.gongzuoneirong;
                        findone.renyuangangwei = dtl.renyuangangwei;
                        findone.gongzuodidian = dtl.gongzuodidian;
                        findone.leixingpinpai = dtl.leixingpinpai;
                        findone.guigeyaoqiu = dtl.guigeyaoqiu;
                        findone.zhiliangbiaozhun = dtl.zhiliangbiaozhun;
                        findone.shuliang = dtl.shuliang;
                        findone.shijiananpai = dtl.shijiananpai;
                        findone.qitashuoming1 = dtl.qitashuoming1;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveBianchengjibianjiDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Bianchengjibianji_RequirementDtl1 anfangDtl1 = new Requirement_Bianchengjibianji_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Bianchengjibianji_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianchengjibianji_RequirementDtl1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Bianchengjibianji_RequirementDtl1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Bianchengjibianji_RequirementDtl1 maxOne = db.Requirement_Bianchengjibianji_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Bianchengjibianji_RequirementDtl1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadBianchengjibianjiDtl2(int RequirementId)
        {
            List<Requirement_Bianchengjibianji_RequirementDtl2> lst = db.Requirement_Bianchengjibianji_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianchengjibianjiDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianchengjibianji_RequirementDtl2 anfangDtl2 = new Requirement_Bianchengjibianji_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl2 = db.Requirement_Bianchengjibianji_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianchengjibianji_RequirementDtl2>(anfangDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Bianchengjibianji_RequirementDtl2>(anfangDtl2);
                anfangDtl2.SeqNO = 1;
                Requirement_Bianchengjibianji_RequirementDtl2 maxOne = db.Requirement_Bianchengjibianji_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                anfangDtl2.InDateTime = DateTime.Now;
                anfangDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Bianchengjibianji_RequirementDtl2.Add(anfangDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 编码及复核
        public ActionResult LoadBianmajifuheDtl1(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl1> lst = db.Requirement_Bianmajifuhe_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Bianmajifuhe_RequirementDtl1 BianmajifuheDtl1 = new Requirement_Bianmajifuhe_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl1 = db.Requirement_Bianmajifuhe_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl1>(BianmajifuheDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl1>(BianmajifuheDtl1);
                BianmajifuheDtl1.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl1 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                BianmajifuheDtl1.RequirementId = RequirementId;
                BianmajifuheDtl1.InDateTime = DateTime.Now;
                BianmajifuheDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl1.Add(BianmajifuheDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadBianmajifuheDtl2(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl2> lst = db.Requirement_Bianmajifuhe_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveBianmajifuheDtl2List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Bianmajifuhe_RequirementDtl2> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Bianmajifuhe_RequirementDtl2>>(jsonArr);
            foreach (Requirement_Bianmajifuhe_RequirementDtl2 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl2 exists = db.Requirement_Bianmajifuhe_RequirementDtl2.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Bianmajifuhe_RequirementDtl2.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Bianmajifuhe_RequirementDtl2.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Bianmajifuhe_RequirementDtl2 findone = db.Requirement_Bianmajifuhe_RequirementDtl2.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.fuheyaoqiu = dtl.fuheyaoqiu;
                        findone.dianhuazixun = dtl.dianhuazixun;
                        findone.dianhuhuifang = dtl.dianhuhuifang;
                        findone.yanbenbianji = dtl.yanbenbianji;
                        findone.fuzhuyongpin = dtl.fuzhuyongpin;
                        findone.bianmaleixing = dtl.bianmaleixing;
                        findone.wenjuanbiancheng = dtl.wenjuanbiancheng;
                        findone.timushuliang = dtl.timushuliang;
                        findone.zishushuliang = dtl.zishushuliang;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.abjuan = dtl.abjuan;
                        findone.abjuanyangbenlianghuobili = dtl.abjuanyangbenlianghuobili;
                        findone.wenjuanzuodaluyinfuhe = dtl.wenjuanzuodaluyinfuhe;
                        findone.wenjuanzuodaluyinfuheyangbenlianghuobili = dtl.wenjuanzuodaluyinfuheyangbenlianghuobili;
                        findone.qitashuoming2 = dtl.qitashuoming2;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveBianmajifuheDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl2 BianmajifuheDtl2 = new Requirement_Bianmajifuhe_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl2 = db.Requirement_Bianmajifuhe_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl2>(BianmajifuheDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl2>(BianmajifuheDtl2);
                BianmajifuheDtl2.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl2 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl2.InDateTime = DateTime.Now;
                BianmajifuheDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl2.Add(BianmajifuheDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadBianmajifuheDtl3(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl3> lst = db.Requirement_Bianmajifuhe_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl3 BianmajifuheDtl3 = new Requirement_Bianmajifuhe_RequirementDtl3();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl3 = db.Requirement_Bianmajifuhe_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl3>(BianmajifuheDtl3);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl3>(BianmajifuheDtl3);
                BianmajifuheDtl3.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl3 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl3.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl3.InDateTime = DateTime.Now;
                BianmajifuheDtl3.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl3.Add(BianmajifuheDtl3);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadBianmajifuheDtl4(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl4> lst = db.Requirement_Bianmajifuhe_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl4 BianmajifuheDtl4 = new Requirement_Bianmajifuhe_RequirementDtl4();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl4 = db.Requirement_Bianmajifuhe_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl4>(BianmajifuheDtl4);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl4>(BianmajifuheDtl4);
                BianmajifuheDtl4.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl4 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl4.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl4.InDateTime = DateTime.Now;
                BianmajifuheDtl4.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl4.Add(BianmajifuheDtl4);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadBianmajifuheDtl5(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl5> lst = db.Requirement_Bianmajifuhe_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl5 BianmajifuheDtl5 = new Requirement_Bianmajifuhe_RequirementDtl5();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl5 = db.Requirement_Bianmajifuhe_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl5>(BianmajifuheDtl5);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl5>(BianmajifuheDtl5);
                BianmajifuheDtl5.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl5 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl5.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl5.InDateTime = DateTime.Now;
                BianmajifuheDtl5.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl5.Add(BianmajifuheDtl5);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadBianmajifuheDtl6(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl6> lst = db.Requirement_Bianmajifuhe_RequirementDtl6.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl6(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl6 BianmajifuheDtl6 = new Requirement_Bianmajifuhe_RequirementDtl6();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl6 = db.Requirement_Bianmajifuhe_RequirementDtl6.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl6>(BianmajifuheDtl6);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl6>(BianmajifuheDtl6);
                BianmajifuheDtl6.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl6 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl6.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl6.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl6.InDateTime = DateTime.Now;
                BianmajifuheDtl6.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl6.Add(BianmajifuheDtl6);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadBianmajifuheDtl7(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl7> lst = db.Requirement_Bianmajifuhe_RequirementDtl7.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl7(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl7 BianmajifuheDtl7 = new Requirement_Bianmajifuhe_RequirementDtl7();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl7 = db.Requirement_Bianmajifuhe_RequirementDtl7.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl7>(BianmajifuheDtl7);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl7>(BianmajifuheDtl7);
                BianmajifuheDtl7.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl7 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl7.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl7.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl7.InDateTime = DateTime.Now;
                BianmajifuheDtl7.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl7.Add(BianmajifuheDtl7);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadBianmajifuheDtl71(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl71> lst = db.Requirement_Bianmajifuhe_RequirementDtl71.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl71(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl71 BianmajifuheDtl71 = new Requirement_Bianmajifuhe_RequirementDtl71();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl71 = db.Requirement_Bianmajifuhe_RequirementDtl71.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl71>(BianmajifuheDtl71);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl71>(BianmajifuheDtl71);
                BianmajifuheDtl71.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl71 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl71.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl71.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl71.InDateTime = DateTime.Now;
                BianmajifuheDtl71.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl71.Add(BianmajifuheDtl71);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadBianmajifuheDtl8(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl8> lst = db.Requirement_Bianmajifuhe_RequirementDtl8.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl8(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl8 BianmajifuheDtl8 = new Requirement_Bianmajifuhe_RequirementDtl8();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl8 = db.Requirement_Bianmajifuhe_RequirementDtl8.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl8>(BianmajifuheDtl8);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl8>(BianmajifuheDtl8);
                BianmajifuheDtl8.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl8 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl8.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl8.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl8.InDateTime = DateTime.Now;
                BianmajifuheDtl8.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl8.Add(BianmajifuheDtl8);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadBianmajifuheDtl9(int RequirementId)
        {
            List<Requirement_Bianmajifuhe_RequirementDtl9> lst = db.Requirement_Bianmajifuhe_RequirementDtl9.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveBianmajifuheDtl9(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Bianmajifuhe_RequirementDtl9 BianmajifuheDtl9 = new Requirement_Bianmajifuhe_RequirementDtl9();
            if (RequirementId > 0 && SeqNo > 0)
            {
                BianmajifuheDtl9 = db.Requirement_Bianmajifuhe_RequirementDtl9.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl9>(BianmajifuheDtl9);
            }
            else
            {
                TryUpdateModel<Requirement_Bianmajifuhe_RequirementDtl9>(BianmajifuheDtl9);
                BianmajifuheDtl9.SeqNO = 1;
                Requirement_Bianmajifuhe_RequirementDtl9 maxOne = db.Requirement_Bianmajifuhe_RequirementDtl9.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    BianmajifuheDtl9.SeqNO = maxOne.SeqNO + 1;
                }

                BianmajifuheDtl9.InDateTime = DateTime.Now;
                BianmajifuheDtl9.InUserId = UserInfo.UserId;
                db.Requirement_Bianmajifuhe_RequirementDtl9.Add(BianmajifuheDtl9);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 车辆展评会
        public ActionResult LoadCheliangzhanpinghuiDtl1(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl1> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl1.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl1(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Cheliangzhanpinghui_RequirementDtl1 CheliangzhanpinghuiDtl1 = new Requirement_Cheliangzhanpinghui_RequirementDtl1();
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl1 = db.Requirement_Cheliangzhanpinghui_RequirementDtl1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl1>(CheliangzhanpinghuiDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl1>(CheliangzhanpinghuiDtl1);
                CheliangzhanpinghuiDtl1.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl1 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                CheliangzhanpinghuiDtl1.RequirementId = RequirementId;
                CheliangzhanpinghuiDtl1.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl1.Add(CheliangzhanpinghuiDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadCheliangzhanpinghuiDtl2(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl2> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl2.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl2(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl2 CheliangzhanpinghuiDtl2 = new Requirement_Cheliangzhanpinghui_RequirementDtl2();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl2 = db.Requirement_Cheliangzhanpinghui_RequirementDtl2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl2>(CheliangzhanpinghuiDtl2);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl2>(CheliangzhanpinghuiDtl2);
                CheliangzhanpinghuiDtl2.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl2 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl2.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl2.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl2.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl2.Add(CheliangzhanpinghuiDtl2);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadCheliangzhanpinghuiDtl3(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl3> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveCheliangzhanpinghuiDtl3List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Cheliangzhanpinghui_RequirementDtl3> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Cheliangzhanpinghui_RequirementDtl3>>(jsonArr);
            foreach (Requirement_Cheliangzhanpinghui_RequirementDtl3 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl3 exists = db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Cheliangzhanpinghui_RequirementDtl3 findone = db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.yonghufenlei = dtl.yonghufenlei;
                        findone.xianyouhuoqianzai = dtl.xianyouhuoqianzai;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.mingdanlaiyuan = dtl.mingdanlaiyuan;
                        findone.goucheyugoushijianduan = dtl.goucheyugoushijianduan;
                        findone.jutigoucheshijian = dtl.jutigoucheshijian;
                        findone.shouhouweixiugabaoyang = dtl.shouhouweixiugabaoyang;
                        findone.cheliangleibie = dtl.cheliangleibie;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.chejiafanwei = dtl.chejiafanwei;
                        findone.peiefenbu = dtl.peiefenbu;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.gongzuozhize = dtl.gongzuozhize;
                        findone.zhixingdidian = dtl.zhixingdidian;
                        findone.baochoufangshi = dtl.baochoufangshi;
                        findone.baochoujine = dtl.baochoujine;
                        findone.qitashuoming3 = dtl.qitashuoming3;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveCheliangzhanpinghuiDtl3(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl3 CheliangzhanpinghuiDtl3 = new Requirement_Cheliangzhanpinghui_RequirementDtl3();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl3 = db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl3>(CheliangzhanpinghuiDtl3);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl3>(CheliangzhanpinghuiDtl3);
                CheliangzhanpinghuiDtl3.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl3 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl3.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl3.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl3.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Add(CheliangzhanpinghuiDtl3);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl31(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl31> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl31.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl31(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl31 CheliangzhanpinghuiDtl31 = new Requirement_Cheliangzhanpinghui_RequirementDtl31();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl31 = db.Requirement_Cheliangzhanpinghui_RequirementDtl31.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl31>(CheliangzhanpinghuiDtl31);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl31>(CheliangzhanpinghuiDtl31);
                CheliangzhanpinghuiDtl31.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl31 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl31.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl31.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl31.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl31.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl31.Add(CheliangzhanpinghuiDtl31);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl4(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl4> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl4.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl4(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl4 CheliangzhanpinghuiDtl4 = new Requirement_Cheliangzhanpinghui_RequirementDtl4();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl4 = db.Requirement_Cheliangzhanpinghui_RequirementDtl4.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl4>(CheliangzhanpinghuiDtl4);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl4>(CheliangzhanpinghuiDtl4);
                CheliangzhanpinghuiDtl4.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl4 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl4.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl4.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl4.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl4.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl4.Add(CheliangzhanpinghuiDtl4);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadCheliangzhanpinghuiDtl5(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl5> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl5.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl5(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl5 CheliangzhanpinghuiDtl5 = new Requirement_Cheliangzhanpinghui_RequirementDtl5();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl5 = db.Requirement_Cheliangzhanpinghui_RequirementDtl5.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl5>(CheliangzhanpinghuiDtl5);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl5>(CheliangzhanpinghuiDtl5);
                CheliangzhanpinghuiDtl5.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl5 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl5.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl5.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl5.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl5.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl5.Add(CheliangzhanpinghuiDtl5);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadCheliangzhanpinghuiDtl6(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl6> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveCheliangzhanpinghuiDtl6List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Cheliangzhanpinghui_RequirementDtl6> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Cheliangzhanpinghui_RequirementDtl6>>(jsonArr);
            foreach (Requirement_Cheliangzhanpinghui_RequirementDtl6 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl6 exists = db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Cheliangzhanpinghui_RequirementDtl6 findone = db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.yonghufenlei = dtl.yonghufenlei;
                        findone.xianyouhuoqianzai = dtl.xianyouhuoqianzai;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.mingdanlaiyuan = dtl.mingdanlaiyuan;
                        findone.goucheyugoushijianduan = dtl.goucheyugoushijianduan;
                        findone.jutigoucheshijian = dtl.jutigoucheshijian;
                        findone.shouhouweixiubaoyangshijian = dtl.shouhouweixiubaoyangshijian;
                        findone.cheliangleibie = dtl.cheliangleibie;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.chejiafanwei = dtl.chejiafanwei;
                        findone.canhuirenshu = dtl.canhuirenshu;
                        findone.danduyaoyue = dtl.danduyaoyue;
                        findone.peiefenbu = dtl.peiefenbu;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.gongzuozhize = dtl.gongzuozhize;
                        findone.baochoufangshi = dtl.baochoufangshi;
                        findone.baochoujine = dtl.baochoujine;
                        findone.qitashuomong6 = dtl.qitashuomong6;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveCheliangzhanpinghuiDtl6(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl6 CheliangzhanpinghuiDtl6 = new Requirement_Cheliangzhanpinghui_RequirementDtl6();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl6 = db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl6>(CheliangzhanpinghuiDtl6);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl6>(CheliangzhanpinghuiDtl6);
                CheliangzhanpinghuiDtl6.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl6 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl6.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl6.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl6.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Add(CheliangzhanpinghuiDtl6);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }


        public ActionResult LoadCheliangzhanpinghuiDtl7(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl7> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl7.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl7(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl7 CheliangzhanpinghuiDtl7 = new Requirement_Cheliangzhanpinghui_RequirementDtl7();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl7 = db.Requirement_Cheliangzhanpinghui_RequirementDtl7.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl7>(CheliangzhanpinghuiDtl7);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl7>(CheliangzhanpinghuiDtl7);
                CheliangzhanpinghuiDtl7.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl7 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl7.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl7.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl7.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl7.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl7.Add(CheliangzhanpinghuiDtl7);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl71(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl71> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl71.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl71(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl71 CheliangzhanpinghuiDtl71 = new Requirement_Cheliangzhanpinghui_RequirementDtl71();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl71 = db.Requirement_Cheliangzhanpinghui_RequirementDtl71.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl71>(CheliangzhanpinghuiDtl71);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl71>(CheliangzhanpinghuiDtl71);
                CheliangzhanpinghuiDtl71.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl71 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl71.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl71.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl71.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl71.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl71.Add(CheliangzhanpinghuiDtl71);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl8(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl8> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl8.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl8(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl8 CheliangzhanpinghuiDtl8 = new Requirement_Cheliangzhanpinghui_RequirementDtl8();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl8 = db.Requirement_Cheliangzhanpinghui_RequirementDtl8.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl8>(CheliangzhanpinghuiDtl8);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl8>(CheliangzhanpinghuiDtl8);
                CheliangzhanpinghuiDtl8.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl8 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl8.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl8.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl8.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl8.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl8.Add(CheliangzhanpinghuiDtl8);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl9(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl9> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl9.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl9(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl9 CheliangzhanpinghuiDtl9 = new Requirement_Cheliangzhanpinghui_RequirementDtl9();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl9 = db.Requirement_Cheliangzhanpinghui_RequirementDtl9.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl9>(CheliangzhanpinghuiDtl9);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl9>(CheliangzhanpinghuiDtl9);
                CheliangzhanpinghuiDtl9.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl9 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl9.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl9.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl9.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl9.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl9.Add(CheliangzhanpinghuiDtl9);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl10(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl10> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl10.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl10(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl10 CheliangzhanpinghuiDtl10 = new Requirement_Cheliangzhanpinghui_RequirementDtl10();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl10 = db.Requirement_Cheliangzhanpinghui_RequirementDtl10.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl10>(CheliangzhanpinghuiDtl10);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl10>(CheliangzhanpinghuiDtl10);
                CheliangzhanpinghuiDtl10.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl10 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl10.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl10.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl10.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl10.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl10.Add(CheliangzhanpinghuiDtl10);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl11(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl11> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }
        public ActionResult SaveCheliangzhanpinghuiDtl11List(string jsonArr, int requirementId, int projectId, int citySeqNO, int requirementGroupId, string requirementType)
        {
            if (requirementId == 0)
            {
                Requirement_RequirementMst mstDto = new Requirement_RequirementMst();
                mstDto.ProjectId = projectId;
                mstDto.CitySeqNo = citySeqNO;
                mstDto.RequirementGroupId = requirementGroupId;
                mstDto.RequirementType = requirementType;
                mstDto.InUserId = UserInfo.UserId;
                requirementId = SaveMst(mstDto);
            }
            List<Requirement_Cheliangzhanpinghui_RequirementDtl11> anfangDtl1 = JsonConvert.DeserializeObject<List<Requirement_Cheliangzhanpinghui_RequirementDtl11>>(jsonArr);
            foreach (Requirement_Cheliangzhanpinghui_RequirementDtl11 dtl in anfangDtl1)
            {
                int seqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl11 exists = db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Find(requirementId, dtl.SeqNO);
                if (exists == null)
                {
                    var maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Where(x => x.RequirementId == requirementId)
                           .OrderByDescending(x => x.SeqNO).Select(x => x.SeqNO).FirstOrDefault();
                    if (maxOne > 0)
                    {
                        seqNO = maxOne + 1;
                    }
                    dtl.SeqNO = seqNO;
                    dtl.RequirementId = requirementId;
                    dtl.InUserId = UserInfo.UserId;
                    dtl.InDateTime = DateTime.Now;
                    db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Add(dtl);
                    db.SaveChanges();
                }
                else
                {
                    Requirement_Cheliangzhanpinghui_RequirementDtl11 findone = db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Find(requirementId, dtl.SeqNO);
                    if (findone != null)
                    {
                        findone.zhixingfenlei = dtl.zhixingfenlei;
                        findone.yonghufenlei = dtl.yonghufenlei;
                        findone.xianyouhuoqianzai = dtl.xianyouhuoqianzai;
                        findone.kehufenlei = dtl.kehufenlei;
                        findone.mingdanlaiyuan = dtl.mingdanlaiyuan;
                        findone.jingyingshijianyaoqiu = dtl.jingyingshijianyaoqiu;
                        findone.gongzuoshijianyaoqiu = dtl.gongzuoshijianyaoqiu;
                        findone.goucheyugoushijianduan = dtl.goucheyugoushijianduan;
                        findone.jutigoucheshijian = dtl.jutigoucheshijian;
                        findone.shouhouweixiubaoyang = dtl.shouhouweixiubaoyang;
                        findone.cheliangleibie = dtl.cheliangleibie;
                        findone.pinpaifenlei = dtl.pinpaifenlei;
                        findone.chejiafanwei = dtl.chejiafanwei;
                        findone.canhuirenshu = dtl.canhuirenshu;
                        findone.danduyaoyue = dtl.danduyaoyue;
                        findone.jingxiaosanfang = dtl.jingxiaosanfang;

                        findone.peiefenbu = dtl.peiefenbu;
                        findone.peieshuoming = dtl.peieshuoming;
                        findone.yangbenliang = dtl.yangbenliang;
                        findone.gongzuozhize = dtl.gongzuozhize;
                        findone.baochoufangshi = dtl.baochoufangshi;
                        findone.baochoujine = dtl.baochoujine;
                        findone.qitashuoming11 = dtl.qitashuoming11;
                        //findone.InUserId = UserInfo.UserId;
                        //findone.InDateTime = DateTime.Now;
                    }
                    db.SaveChanges();
                }
            }

            return Json(requirementId);
        }
        public ActionResult SaveCheliangzhanpinghuiDtl11(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl11 CheliangzhanpinghuiDtl11 = new Requirement_Cheliangzhanpinghui_RequirementDtl11();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl11 = db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl11>(CheliangzhanpinghuiDtl11);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl11>(CheliangzhanpinghuiDtl11);
                CheliangzhanpinghuiDtl11.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl11 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl11.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl11.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl11.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Add(CheliangzhanpinghuiDtl11);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl12(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl12> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl12.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl12(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl12 CheliangzhanpinghuiDtl12 = new Requirement_Cheliangzhanpinghui_RequirementDtl12();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl12 = db.Requirement_Cheliangzhanpinghui_RequirementDtl12.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl12>(CheliangzhanpinghuiDtl12);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl12>(CheliangzhanpinghuiDtl12);
                CheliangzhanpinghuiDtl12.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl12 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl12.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl12.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl12.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl12.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl12.Add(CheliangzhanpinghuiDtl12);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        public ActionResult LoadCheliangzhanpinghuiDtl13(int RequirementId)
        {
            List<Requirement_Cheliangzhanpinghui_RequirementDtl13> lst = db.Requirement_Cheliangzhanpinghui_RequirementDtl13.Where(x => x.RequirementId == RequirementId).ToList();
            return Json(lst);
        }

        public ActionResult SaveCheliangzhanpinghuiDtl13(int RequirementId, int SeqNo, Requirement_RequirementMst mstDto)
        {
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }

            Requirement_Cheliangzhanpinghui_RequirementDtl13 CheliangzhanpinghuiDtl13 = new Requirement_Cheliangzhanpinghui_RequirementDtl13();
            if (RequirementId > 0 && SeqNo > 0)
            {
                CheliangzhanpinghuiDtl13 = db.Requirement_Cheliangzhanpinghui_RequirementDtl13.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl13>(CheliangzhanpinghuiDtl13);
            }
            else
            {
                TryUpdateModel<Requirement_Cheliangzhanpinghui_RequirementDtl13>(CheliangzhanpinghuiDtl13);
                CheliangzhanpinghuiDtl13.SeqNO = 1;
                Requirement_Cheliangzhanpinghui_RequirementDtl13 maxOne = db.Requirement_Cheliangzhanpinghui_RequirementDtl13.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    CheliangzhanpinghuiDtl13.SeqNO = maxOne.SeqNO + 1;
                }

                CheliangzhanpinghuiDtl13.InDateTime = DateTime.Now;
                CheliangzhanpinghuiDtl13.InUserId = UserInfo.UserId;
                db.Requirement_Cheliangzhanpinghui_RequirementDtl13.Add(CheliangzhanpinghuiDtl13);
            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #region 其他1和其他2
        public ActionResult LoadYouxingshangpincaigou(string projectId, string citySeqNO, string groupId, int RequirementId)
        {
            List<RequirementInternalTangibleDto> lst = service.RequiermentIntelTangibleSearch1(projectId, citySeqNO, groupId, RequirementId.ToString());
            return Json(lst);
        }
        public ActionResult SaveYouxingshangpincaigouDtl1(int RequirementId, int? SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Internal_Tangible_Qita1 anfangDtl1 = new Requirement_Internal_Tangible_Qita1();
            if (SeqNo == null)
            {
                SeqNo = 0;
            }
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Internal_Tangible_Qita1.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Internal_Tangible_Qita1>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Internal_Tangible_Qita1>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Internal_Tangible_Qita1 maxOne = db.Requirement_Internal_Tangible_Qita1.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Internal_Tangible_Qita1.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }

        public ActionResult LoadWuxingshangpincaigou(string projectId, string citySeqNO, string groupId, int RequirementId)
        {
            List<RequirementInternalIntangibleDto> lst = service.RequiermentIntelIntangibleSearch1(projectId, citySeqNO, groupId, RequirementId.ToString());
            return Json(lst);
        }

        public ActionResult SaveWuxingshangpincaigouDtl1(int RequirementId, int? SeqNo, Requirement_RequirementMst mstDto)
        {
            Requirement_Internal_Intangible_Qita2 anfangDtl1 = new Requirement_Internal_Intangible_Qita2();
            if (SeqNo == null)
            {
                SeqNo = 0;
            }
            if (RequirementId == 0)
            {
                RequirementId = SaveMst(mstDto);
            }
            if (RequirementId > 0 && SeqNo > 0)
            {
                anfangDtl1 = db.Requirement_Internal_Intangible_Qita2.Find(RequirementId, SeqNo);
                TryUpdateModel<Requirement_Internal_Intangible_Qita2>(anfangDtl1);
            }
            else
            {
                TryUpdateModel<Requirement_Internal_Intangible_Qita2>(anfangDtl1);
                anfangDtl1.SeqNO = 1;
                Requirement_Internal_Intangible_Qita2 maxOne = db.Requirement_Internal_Intangible_Qita2.Where(x => x.RequirementId == RequirementId).OrderByDescending(x => x.SeqNO).FirstOrDefault();
                if (maxOne != null)
                {
                    anfangDtl1.SeqNO = maxOne.SeqNO + 1;
                }
                anfangDtl1.RequirementId = RequirementId;
                anfangDtl1.InDateTime = DateTime.Now;
                anfangDtl1.InUserId = UserInfo.UserId;
                db.Requirement_Internal_Intangible_Qita2.Add(anfangDtl1);

            }
            db.SaveChanges();

            return Json(RequirementId);
        }
        #endregion
        #endregion

        #region 需求书查询
        public ActionResult RequirementMstSearch(string modelType, string projectCode, string projectName, string projectShortName, string province, string city, string history, string RequirementGroupId, int pageNum, int? pageSize)
        {
            List<RequiremetMstDto> list = service.RequirementMstSearch(modelType, projectCode, projectName, projectShortName, province, city, "0", (Session["LoginUser"] as Mst_UserInfo).UserId, RequirementGroupId);

            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }
        public ActionResult RequirementSendEmailSearch(string requirementId)
        {
            List<RequirementEmailSendDto> list = service.RequirementEmailSendSearch(requirementId);
            return PartialView("_PartialDemandBookSendDetail", list);

        }
        //string loginUserchk ：是否需要要按照登陆人来查询项目
        public ActionResult ProjectSelect(string modelType, string projectCode, string projectName, string projectShortName, string serviceTrade, string loginUserChk)
        {
            MasterService masterService = new MasterService();
            if (modelType != null && modelType.Length == 1)
            {
                modelType = modelTypes[Int32.Parse(modelType)];
            }
            List<ProjectDto> lst = new List<ProjectDto>();
            if (string.IsNullOrEmpty(loginUserChk))
            {
                lst = masterService.ProjectSearchMaster(modelType, projectCode, projectName, projectShortName, serviceTrade, "");
            }
            else
            {
                lst = masterService.ProjectSearchMaster(modelType, projectCode, projectName, projectShortName, serviceTrade, UserInfo.UserId);
            }

            return PartialView("_PartialDemandBookProjectSelect", lst);
        }
        //string loginUserchk ：是否需要要按照登陆人来查询项目
        public ActionResult ProjectSelectSearch(string modelType, string projectCode, string projectName, string projectShortName, string serviceTrade, string loginUserChk)
        {
            //if (modelType == null)
                modelType = "";
            MasterService masterService = new MasterService();
            List<ProjectDto> lst = new List<ProjectDto>();
            if (string.IsNullOrEmpty(loginUserChk))
            {
                lst = masterService.ProjectSearchMaster(modelType, projectCode, projectName, projectShortName, serviceTrade, "");
            }
            else
            {
                lst = masterService.ProjectSearchMaster(modelType, projectCode, projectName, projectShortName, serviceTrade, UserInfo.UserId);
            }

            return Json(new { List = lst });
        }

        public ActionResult GroupSelect(int projectId)
        {
            var lst = db.Requirement_Group.Where(x => x.ProjectId == projectId);
            return PartialView("_PartialDemandBookGroupSelect", lst);
        }
        #endregion
        #region 根据项目查询需求书
        public ActionResult RequirementSearchByProjectId(string projectId, string RequirementGroupId, string province, string city, string groupId)
        {
            List<RequiremetMstDto> list = service.RequirementMstByProjectIdSearch(projectId, province, city, groupId);
            return PartialView("_PartialDemandBookSelect", list);
        }
        public ActionResult RequirementSearchByProjectIdSearch(string projectId, string RequirementGroupId, string province, string city, string groupId)
        {
            List<RequiremetMstDto> list = service.RequirementMstByProjectIdSearch(projectId, province, city, groupId);
            return Json(new { List = list });
        }
        #endregion
        public ActionResult ApplyCommit(string recheckUserId, Apply apply, string applyIdexists, string seqNO, string projectId, string groupId, string attachArray)
        {
            ApplyService applayService = new ApplyService();
            apply.InDateTime = DateTime.Now;
            apply.ApplyUserId = UserInfo.UserId;
            apply.ApplyTypeId = ApplyType.需求书.ToString();
            apply.ProjectId = Convert.ToInt32(groupId);// 需求书申请时ProjectId 保存为groupId
            int applyId = 0;
            if (Convert.ToInt32(applyIdexists) == 0)
            {
                applyId = applayService.ApplyCommit(apply, recheckUserId);
            }
            else
            {
                applyId = Convert.ToInt32(applyIdexists);
                ApplyRecheckStatus status = new ApplyRecheckStatus();
                status.ApplyId = Convert.ToInt32(applyIdexists);
                status.RecheckUserId = ((Mst_UserInfo)Session["LoginUser"]).UserId;
                status.RecheckStatusCode = "申请";
                status.RecheckReason = apply.ApplyReason == null ? "" : apply.ApplyReason;
                status.SeqNO = Convert.ToInt32(seqNO);
                status.InDateTime = DateTime.Now;
                applayService.ApplyRecheckStatusUpdate(status, recheckUserId);

            }
            applayService.ApplyDtlDelete(new ApplyDtl { ApplyId = applyId });
            List<RequiremetMstDto> list = service.RequirementMstSearchByGroupId(projectId, groupId);
            foreach (RequiremetMstDto item in list)
            {
                ApplyDtl applyDtl = new ApplyDtl()
                {
                    ApplyId = applyId,
                    ApplyContentId = Convert.ToInt32(item.RequirementId),
                    ApplyTypeId = ApplyType.需求书.ToString(),
                    InDateTime = DateTime.Now,
                    InUserId = UserInfo.UserId
                };
                applayService.ApplyDtlSave(applyDtl);
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
                    //mailTo = "".Split(',');
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
        public ActionResult RecheckUserSelect()
        {
            ApplyService applayService = new ApplyService();
            List<RecheckUserSelectDto> lst = applayService.RecheckUserSelect(ApplyType.需求书.ToString(), UserInfo.UserId);

            return PartialView("_PartialRecheckUserSelect", lst);
        }
        #region 需求书提交审核验证
        public ActionResult CommitCheckSearch(string projectId)
        {
            CommitCheckDto commitCheck = service.CommitCheckSearch(projectId, UserInfo.UserId).FirstOrDefault();
            return Json(commitCheck, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 需求书配额样本量查询
        public ActionResult RequirementSampleCountSearch(string requirementType, string requirementId, string seqNO)
        {
            int count = service.RequirementSampleCount(requirementType, requirementId, seqNO);
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 需求书审核状态查询
        public ActionResult RequirementGroupSearchById(string projectId, string groupId)
        {
            RequirementGroupDto group = service.RequirementGroupSearchById(projectId, groupId, UserInfo.UserId).FirstOrDefault();
            return Json(group, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult DeleteDtl(string RequirementId, string RequirementType, string Dtl, string SeqNOs)
        {
            if ("Guankong" == RequirementType)
            {
                RequirementType = "Guangkong";
            }
            string tableName = "Requirement_" + RequirementType + "_RequirementDtl" + Dtl;
            if ("Wuxingshangpincaigou" == RequirementType)
            {
                tableName = "Requirement_Internal_Intangible_Qita2";
            }

            if (Dtl == "999")
            {
                tableName = "Requirement_RequirementDtlLast";
            }
            string sql = string.Format("delete from {0} where RequirementId={1} and SeqNo in ({2})", tableName, RequirementId, SeqNOs);
            db.Database.ExecuteSqlCommand(sql);

            return Json("");
        }

        public ActionResult RequirementDeleteById(string RequirementIds)
        {
            if (!string.IsNullOrEmpty(RequirementIds))
            {
                string[] reqIds = RequirementIds.Split(',');
                foreach (string reqId in reqIds)
                {
                    if (!string.IsNullOrEmpty(reqId))
                    {
                        service.RequirementDeleteById(reqId);
                    }
                }
            }

            return Json("");
        }

    }
}