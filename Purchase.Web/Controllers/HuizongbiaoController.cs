using Newtonsoft.Json;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Service.DTO.汇总表;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class HuizongbiaoController : BaseController
    {
        HuizongbiaoService service = new HuizongbiaoService();
        // GET: Huizongbiao
        public ActionResult Index()
        {
            ViewBag.QuotationTypeAndText = QuotationTypeAndText;
            return View();
        }
        public ActionResult NeibuIndex()
        {
            return View();
        }
        public ActionResult QitaIndex()
        {
            return View();
        }
        public ActionResult ZhuijiaIndex()
        {
            return View();
        }
        public ActionResult LoadZhuijia(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum = 1, int pageSize = 20)
        {
            var lst = service.HuizongbiaoSearch_Shoudong(ModelType, ProjectName, ProjectShortName, SupplierName);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadBiancheng(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum = 1, int pageSize = 20)
        {
            var lst = service.HuizongbiaoSearch_Biancheng(ModelType,ProjectName,ProjectShortName,SupplierName);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadZhixing(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum = 1, int pageSize = 20)
        {
            var lst = service.HuizongbiaoSearch_Zhixing(ModelType,ProjectName,ProjectShortName,SupplierName);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadFuhe(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum=1, int pageSize=20)
        {
            var lst = service.HuizongbiaoSearch_Fuhe(ModelType,ProjectName,ProjectShortName,SupplierName);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadYanjiu(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum=1, int pageSize=20)
        {
            var lst = service.HuizongbiaoSearch_Yanjiu(ModelType,ProjectName,ProjectShortName,SupplierName);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadQita1(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "",string ServiceTrade = "", int pageNum=1, int pageSize=20)
        {
            
            var lst = service.HuizongbiaoSearch_Qita1(ModelType,ProjectName,ProjectShortName,SupplierName,ServiceTrade);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadQita2(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", string ServiceTrade = "",int pageNum=1, int pageSize=20)
        {
            var lst = service.HuizongbiaoSearch_Qita2(ModelType, ProjectName, ProjectShortName, SupplierName, ServiceTrade);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadChezhan(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum=1, int pageSize=20)
        {
            var lst = service.HuizongbiaoSearch_Chezhan(ModelType,ProjectName,ProjectShortName,SupplierName);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadZhichi(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum=1, int pageSize=20)
        {
            var lst = service.HuizongbiaoSearch_Zhichi(ModelType,ProjectName,ProjectShortName,SupplierName);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }
        public ActionResult Save(string jsonArr)
        {
            List<HuizongbiaoSaveDto> lst = JsonConvert.DeserializeObject<List<HuizongbiaoSaveDto>>(jsonArr);

            foreach (HuizongbiaoSaveDto saveDto in lst)
            {
                service.HuizongbiaoSave(saveDto.HuizongbiaoId, saveDto.ProjectId, saveDto.SupplierId, saveDto.SettlementId, saveDto.SettlementSeqNO, saveDto.Remark, UserInfo.UserId);
            }
            return Json("");
        }

        #region 导出excel
        string suffix = ".xlsx";
        string tempPath = "~/CommonTemplate/huizongbiao/";
        string basePath = "~/Export/Juesuantongjiebiao/";
        public ActionResult ExportHuizongbiao(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", string ServiceTrade = "")
        {
            string zip = "";
            string absPath = Server.MapPath(basePath);
            string createFloader = absPath + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\\";
            if (!Directory.Exists(createFloader))
            {
                Directory.CreateDirectory(createFloader);
            }
            foreach (string key in QuotationTypeAndText.Keys)
            {
                #region ===========导出单类型汇总表
                string quotationType = key;

                List<Object> lst = new List<Object>();
                if (quotationType == "Zhixing")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Zhixing(ModelType, ProjectName, ProjectShortName, SupplierName));
                }
                else if (quotationType == "Fuhe")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Fuhe(ModelType, ProjectName, ProjectShortName, SupplierName));
                }
                else if (quotationType == "Biancheng")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Biancheng(ModelType, ProjectName, ProjectShortName, SupplierName));
                }
                else if (quotationType == "Yanjiu")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Yanjiu(ModelType, ProjectName, ProjectShortName, SupplierName));
                }
                else if (quotationType == "Zhichi")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Zhichi(ModelType, ProjectName, ProjectShortName, SupplierName));
                }
                else if (quotationType == "Qita1")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Qita1(ModelType, ProjectName, ProjectShortName, SupplierName, ServiceTrade));
                }
                else if (quotationType == "Qita2")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Qita2(ModelType, ProjectName, ProjectShortName, SupplierName, ServiceTrade));
                }
                else if (quotationType == "Chezhan")
                {
                    lst.AddRange(service.HuizongbiaoSearch_Chezhan(ModelType, ProjectName, ProjectShortName, SupplierName));
                }

                if (ModelType != "业务")
                {
                    if (quotationType == "Qita1")
                    {
                        quotationType = "Youxingshangpincaigou";
                    }
                    else if (quotationType == "Qita2")
                    {
                        quotationType = "Wuxingshangpincaigou";
                    }
                }

                string createFileName = "汇总表_" + quotationType + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
                string path = createFloader + createFileName;
                string templateFile = Server.MapPath(tempPath + quotationType + suffix);
                System.IO.File.Copy(templateFile, path);

                if (lst.Count == 0)
                {
                    continue;
                }
                HuizongbiaoExport export = new HuizongbiaoExport();
                MethodInfo method = null;
                method = export.GetType().GetMethod("Export" + quotationType);
                if (method != null)
                {
                    method.Invoke(export, new object[] { path, lst });
                }
                #endregion
            }

            string zipName = absPath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_确认单" + ".zip";
            List<FileInfo> fileList = new List<FileInfo>();

            foreach (string file in Directory.GetFiles(createFloader))
            {
                fileList.Add(new FileInfo(file));
            }
            Compress(fileList, zipName, 9, 100);
            zip = zipName;

            return Json(new { ExportPath = zip });
        }

        public ActionResult ExportZhuijia(string ModelType = "", string ProjectName = "", string ProjectShortName = "", string SupplierName = "", int pageNum = 1, int pageSize = 20)
        {
            var lst = service.HuizongbiaoSearch_Shoudong(ModelType, ProjectName, ProjectShortName, SupplierName);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "汇总表_追加" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath+"zhuijia" + suffix);
            System.IO.File.Copy(templateFile, path);

            if (lst.Count == 0)
            {
                throw new Exception("没有可以导出的数据");
            }
            HuizongbiaoExport export = new HuizongbiaoExport();
            export.ExportZhuijia(path, lst);

            return Json(new { ExportPath = path });
        }
        #endregion
    }
}