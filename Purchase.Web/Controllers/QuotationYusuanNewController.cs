using Newtonsoft.Json;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class QuotationYusuanNewController : BaseController
    {
        private PurchaseEntities db = new PurchaseEntities();
        QuotationService quotationService = new QuotationService();
        //
        // GET: /QuotationYusuanNew/
        public ActionResult Index(string ProjectId)
        {
            List<QuotationGroupDto> list = quotationService.QuotationGroupSearch(ProjectId, UserInfo.UserId);
            if (list != null)
            {
                list = list.Where(x => x.ApplyStatusCode == "完成").ToList();
            }
            return PartialView(list);
        }

        public ActionResult QuotationTypeSearch(string ProjectId, string QuotationGroupId)
        {
            List<QuotationTypeDto> lst = quotationService.QuotationTypeSearchByProjectIdAndGroupId(ProjectId, QuotationGroupId);
            return Json(lst);
        }

        public ActionResult LoadQuotationYusuan(string quotationType, string QuotationYusuanGroupId, string QuotationGroupId)
        {
            List<object> retData = new List<object>();
            switch (quotationType)
            {
                case "Biancheng":
                    retData.AddRange(quotationService.Yusuan_BianchengSearch(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                case "Zhixing":
                    retData.AddRange(quotationService.Yusuan_ZhixingSearch(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                case "Yanjiu":
                    retData.AddRange(quotationService.Yusuan_YanjiuSearch(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                case "Fuhe":
                    retData.AddRange(quotationService.Yusuan_FuheSearch(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                case "Qita1":
                    retData.AddRange(quotationService.Yusuan_Qita1Search(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                case "Qita2":
                    retData.AddRange(quotationService.Yusuan_Qita2Search(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                case "Chezhan":
                    retData.AddRange(quotationService.Yusuan_ChezhanSearch(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                case "Zhichi":
                    retData.AddRange(quotationService.Yusuan_ZhichiSearch(QuotationYusuanGroupId, QuotationGroupId, UserInfo.UserId));
                    break;
                default:
                    break;

            }

            return Json(retData);
        }

        public ActionResult UpdateYusuanAndContingency(string jsonArr, string ProjectId, string ContingencyFee)
        {
            SaveContingency(ProjectId, ContingencyFee);
            List<QuotationYusan> lst = JsonConvert.DeserializeObject<List<QuotationYusan>>(jsonArr);

            foreach (QuotationYusan yusuan in lst)
            {
                if (yusuan.QuotationType == "Youxingshangpincaigou")
                {
                    yusuan.QuotationType = "Qita1";
                }
                if (yusuan.QuotationType == "Wuxingshangpincaigou")
                {
                    yusuan.QuotationType = "Qita2";
                }

                quotationService.QuotationYusuanSave(yusuan.Id, yusuan.SeqNo, yusuan.QuotationYusanGroupId, yusuan.yusuandanjia, yusuan.yusuanshuliang, UserInfo.UserId);
            }

            try
            {
                // 预算确认单填写完毕之后，提醒应付流转单
                MasterService master = new MasterService();
                master.RemindCancelSave("流转单填写提醒", ProjectId, this.UserInfo.UserId, DateTime.Now.ToString());
            }
            catch (Exception)
            {
            }
            return Json("");
        }

        public void SaveContingency(string ProjectId, string ContingencyFee)
        {
            decimal? feeNullable = string.IsNullOrEmpty(ContingencyFee) ? new Nullable<decimal>() : new Decimal(Double.Parse(ContingencyFee));

            Contingency findOne = db.Contingency.Find(ProjectId);
            if (findOne != null)
            {
                findOne.ContingencyFee = feeNullable;
            }
            else
            {
                findOne = new Contingency()
                {
                    ProjectId = ProjectId,
                    ContingencyFee = feeNullable,
                    InDateTime = DateTime.Now,
                    InUserId = UserInfo.UserId
                };
                db.Contingency.Add(findOne);
            }
            db.SaveChanges();
        }


    }
}