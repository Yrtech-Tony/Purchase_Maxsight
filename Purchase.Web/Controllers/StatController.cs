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
    public class StatController : BaseController
    {
        PurchaseEntities db = new PurchaseEntities();
        //
        // GET: /Stat/
        public ActionResult ProjectStat()
        {
            return View();
        }
        public ActionResult ProjectDetailStat()
        {
            return View();
        }
        public ActionResult SupplierStat()
        {
            return View();
        }
        public ActionResult CityStat()
        {
            return View();
        }

        public ActionResult LoadProjectStat()
        {
            return Json("");
        }

        public ActionResult LoadSupplierStat()
        {
            return Json("");
        }

        public ActionResult LoadProjectDetailStat()
        {
            return Json("");
        }

        public ActionResult LoadCityStat()
        {
            return Json("");
        }

        public ActionResult ProjectGeneralStat()
        {
            return View();
        }

        public ActionResult GeneralStat(string condition, string statResult, string statMode, string countType, string sumType)
        {
            Dictionary<string, string> conditionDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(condition);
            List<string> statResultList = JsonConvert.DeserializeObject<List<string>>(statResult);
            List<string> subTypes = JsonConvert.DeserializeObject<List<string>>(sumType);
            if (!string.IsNullOrEmpty(countType))
                countType = JsonConvert.DeserializeObject<string>(countType);
            List<string> subtype = new List<string>();
            List<string> subtypebase = new List<string>();
            if (subTypes != null)
            {
                foreach (string type in subTypes)
                {
                    if (type == "chengshiyangbenliang" || type == "peieshuliang")
                    {
                        subtypebase.Add(type);
                    }
                    else
                    {
                        subtype.Add(type);
                    }
                }
            }

            List<CustomerColumnDto> lst = new List<CustomerColumnDto>();
            MasterService service = new MasterService();
            if (statMode == "DistinctCount")
            {
                lst = service.CustomerSearchDistinctCount(conditionDic, statResultList, statMode, countType);
            }
            else
            {
                lst = service.CustomerSearch(conditionDic, statResultList, statMode, subtype, subtypebase);
            }

            return Json(lst);
        }

        public ActionResult GetCustomerColumns()
        {
            var customerColumnList = db.CustomerColumn.Where(x => x.ColumnName != "").Select(x => new
            {
                ColumnName = x.ColumnName,
                ColumnName_CN = x.ColumnName_CN,
                QuotationType = x.QuotationType,
                ShowType = x.ShowType
            }).OrderBy(x => x.QuotationType).ToList();
            return Json(customerColumnList);
        }

        public ActionResult ProjectSelect(string projectCode, string projectName, string projectShortName)
        {
            string modelType = "";
            MasterService masterService = new MasterService();
            List<ProjectDto> lst = masterService.ProjectSearchMaster(modelType, projectCode, projectName, projectShortName, "", "");

            return PartialView("_PartialProjectSelect", lst);
        }

        public ActionResult SupplierMngSelect(string SupplierCode, string SupplierName, string SupplierShortName, string province, string city)
        {
            MasterService masterService = new MasterService();
            List<SupplierDto> lst = masterService.SupplierPopupSearch(SupplierCode, SupplierName, SupplierShortName, province, city);

            return PartialView("_PartialSupplierMngSelect", lst);
        }
    }
}