using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class JuesuantongjiebiaoController : BaseController
    {
        JuesuantongjiebiaoService service = new JuesuantongjiebiaoService();
        MasterService masterService = new MasterService();
        // GET: Juesuantongjiebiao
        public ActionResult yewucaigoubiao1()
        {
            return View();
        }

        public ActionResult yewucaigoubiao2()
        {
            return View();
        }
        public ActionResult neibucaigoubiao1()
        {
            return View();
        }

        public ActionResult neibucaigoubiao2()
        {
            return View();
        }
        public ActionResult qitacaigoubiao1()
        {
            return View();
        }

        public ActionResult qitacaigoubiao2()
        {
            return View();
        }

        public ActionResult zhuijiaJuesuan1()
        {
            return View();
        }
        public ActionResult zhuijiaJuesuan2()
        {
            return View();
        }
        public ActionResult LoadYewucaigoubiao1(string modelType, string serviceTrade, string QuotationType, DateTime startDate, DateTime endDate, int pageNum = 1, int pageSize = 10)
        {
            List<Juesuantongjibiao1_HeaderDto> HeaderDtoList = service.Juesuantongjiebiao1_Head_Search(serviceTrade, QuotationType, startDate, endDate);
            List<Juesuantongjibiao1_LeftDto> LeftDtoList = service.Juesuantongjiebiao1_yewu_Left_R(serviceTrade, QuotationType, startDate, endDate);
            List<Juesuantongjibiao1_DataDto> DataDtoList = service.Juesuantongjiebiao1_Data_Search(serviceTrade, QuotationType, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadYewucaigoubiao2(string modelType, string serviceTrade, string QuotationType, DateTime startDate, DateTime endDate, int pageNum = 1, int pageSize = 10)
        {
            List<Juesuantongjibiao2Dto> lst = service.Juesuantongjiebiao2_yewu_Search(serviceTrade, QuotationType, startDate, endDate);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadNeibugoubiao1(string modelType, string serviceTrade, string purchaseType, DateTime startDate, DateTime endDate, int pageNum = 1, int pageSize = 10)
        {
            List<Juesuantongjibiao1_inter_HeaderDto> HeaderDtoList = service.Juesuantongjiebiao1_inter_Head_Search(modelType, serviceTrade, purchaseType, startDate, endDate);
            List<Juesuantongjibiao1_inter_LeftDto> LeftDtoList = service.Juesuantongjiebiao1_inter_Left_R(modelType, serviceTrade, purchaseType, startDate, endDate);
            List<Juesuantongjibiao1_inter_DataDto> DataDtoList = service.Juesuantongjiebiao1_inter_Data_Search(modelType, serviceTrade, purchaseType, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadNeibugoubiao2(string modelType, string serviceTrade, string purchaseType, DateTime startDate, DateTime endDate, int pageNum = 1, int pageSize = 10)
        {
            List<Juesuantongjibiao2_interDto> lst = service.Juesuantongjiebiao2_inter_Search(modelType, serviceTrade, purchaseType, startDate, endDate);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadZhuijiaJuesuan1(string serviceTrade, DateTime startDate, DateTime endDate, int pageNum = 1, int pageSize = 10)
        {
            List<Juesuantongjibiao1_Shoudong_HeaderDto> HeaderDtoList = service.Juesuantongjiebiao1_Shoudong_Head_Search(serviceTrade, startDate, endDate);
            List<Juesuantongjibiao1_Shoudong_LeftDto> LeftDtoList = service.Juesuantongjiebiao1_Shoudong_Left_R(serviceTrade, startDate, endDate);
            List<Juesuantongjibiao1_Shoudong_DataDto> DataDtoList = service.Juesuantongjiebiao1_Shoudong_Data_Search(serviceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadZhuijiaJuesuan2(string serviceTrade, DateTime startDate, DateTime endDate, int pageNum = 1, int pageSize = 10)
        {
            List<Juesuantongjibiao2_ShoudongDto> lst = service.Juesuantongjiebiao2_Shoudong_Search(serviceTrade, startDate, endDate);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        string tempPath = "~/CommonTemplate/";
        string basePath = "~/Export/Juesuantongjiebiao/";
        public ActionResult ExportYewucaigoubiao1(string modelType, string serviceTrade, string QuotationType, DateTime startDate, DateTime endDate)
        {
            List<Juesuantongjibiao1_HeaderDto> HeaderDtoList = service.Juesuantongjiebiao1_Head_Search(serviceTrade, QuotationType, startDate, endDate);
            List<Juesuantongjibiao1_LeftDto> LeftDtoList = service.Juesuantongjiebiao1_yewu_Left_R(serviceTrade, QuotationType, startDate, endDate);
            List<Juesuantongjibiao1_DataDto> DataDtoList = service.Juesuantongjiebiao1_Data_Search(serviceTrade, QuotationType, startDate, endDate);
            ProjectDto projectDto = masterService.ProjectStartDateSearch("", "", modelType, serviceTrade).FirstOrDefault();
            if (projectDto != null)
            {
                if (startDate.ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    startDate = projectDto.StartDate_Min;
                }
                if (endDate.ToString("yyyy-MM-dd") == "2999-01-01")
                {
                    endDate = projectDto.StartDate_Max;
                }
            }
            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "决算统计报表1-业务采购_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "决算统计报表1-业务采购.xlsx");
            System.IO.File.Copy(templateFile, path);
            JuesuantongjiebiaoExport export = new JuesuantongjiebiaoExport();
            export.ExportYewucaigoubiao1(path, startDate, endDate, HeaderDtoList, LeftDtoList, DataDtoList);

            return Json(new { ExportPath = path });
        }

        public ActionResult ExportYewucaigoubiao2(string modelType, string serviceTrade, string QuotationType, DateTime startDate, DateTime endDate)
        {
            List<Juesuantongjibiao2Dto> lst = service.Juesuantongjiebiao2_yewu_Search(serviceTrade, QuotationType, startDate, endDate);

            ProjectDto projectDto = masterService.ProjectStartDateSearch("", "", modelType, serviceTrade).FirstOrDefault();
            if (projectDto != null)
            {
                if (startDate.ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    startDate = projectDto.StartDate_Min;
                }
                if (endDate.ToString("yyyy-MM-dd") == "2999-01-01")
                {
                    endDate = projectDto.StartDate_Max;
                }
            }

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "决算统计报表2-业务采购_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "决算统计报表2-业务采购.xlsx");
            System.IO.File.Copy(templateFile, path);
            JuesuantongjiebiaoExport export = new JuesuantongjiebiaoExport();
            export.ExportYewucaigoubiao2(path, startDate, endDate, lst);

            return Json(new { ExportPath = path });
        }

        public ActionResult ExportNeibugoubiao1(string modelType, string serviceTrade, string purchaseType, DateTime startDate, DateTime endDate)
        {
            List<Juesuantongjibiao1_inter_HeaderDto> HeaderDtoList = service.Juesuantongjiebiao1_inter_Head_Search(modelType, serviceTrade, purchaseType, startDate, endDate);
            List<Juesuantongjibiao1_inter_LeftDto> LeftDtoList = service.Juesuantongjiebiao1_inter_Left_R(modelType, serviceTrade, purchaseType, startDate, endDate);
            List<Juesuantongjibiao1_inter_DataDto> DataDtoList = service.Juesuantongjiebiao1_inter_Data_Search(modelType, serviceTrade, purchaseType, startDate, endDate);
            ProjectDto projectDto = masterService.ProjectStartDateSearch("", "", modelType, serviceTrade).FirstOrDefault();
            if (projectDto != null)
            {
                if (startDate.ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    startDate = projectDto.StartDate_Min;
                }
                if (endDate.ToString("yyyy-MM-dd") == "2999-01-01")
                {
                    endDate = projectDto.StartDate_Max;
                }
            }
            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }

            string createFileName = "决算统计报表1-内部采购_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string templateFile = Server.MapPath(tempPath + "决算统计报表1-内部采购.xlsx");
            if (modelType == "其他")
            {
                createFileName = "决算统计报表1-其他_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
                templateFile = Server.MapPath(tempPath + "决算统计报表1-其他.xlsx");
            }
            if (LeftDtoList.Count == 0)
            {
                throw new Exception("没有可以导出的数据");
            }
            string path = absPath + createFileName;
            System.IO.File.Copy(templateFile, path);
            JuesuantongjiebiaoExport export = new JuesuantongjiebiaoExport();
            export.ExportNewbucaigoubiao1(path, startDate, endDate, HeaderDtoList, LeftDtoList, DataDtoList);

            return Json(new { ExportPath = path });
        }

        public ActionResult ExportNeibugoubiao2(string modelType, string serviceTrade, string purchaseType, DateTime startDate, DateTime endDate)
        {
            List<Juesuantongjibiao2_interDto> lst = service.Juesuantongjiebiao2_inter_Search(modelType, serviceTrade, purchaseType, startDate, endDate);
            ProjectDto projectDto = masterService.ProjectStartDateSearch("", "", modelType, serviceTrade).FirstOrDefault();
            if (projectDto != null)
            {
                if (startDate.ToString("yyyy-MM-dd") == "1900-01-01")
                {
                    startDate = projectDto.StartDate_Min;
                }
                if (endDate.ToString("yyyy-MM-dd") == "2999-01-01")
                {
                    endDate = projectDto.StartDate_Max;
                }
            }
            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }

            string createFileName = "决算统计报表2-内部采购_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string templateFile = Server.MapPath(tempPath + "决算统计报表2-内部采购.xlsx");
            if (modelType == "其他")
            {
                createFileName = "决算统计报表2-其他_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
                templateFile = Server.MapPath(tempPath + "决算统计报表2-其他.xlsx");
            }
            if (lst.Count == 0)
            {
                throw new Exception("没有可以导出的数据");
            }
            string path = absPath + createFileName;
            System.IO.File.Copy(templateFile, path);
            JuesuantongjiebiaoExport export = new JuesuantongjiebiaoExport();
            export.ExportNewbucaigoubiao2(path, startDate, endDate, lst);

            return Json(new { ExportPath = path });
        }
        public ActionResult ExportZhuijiaJuesuan1(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            List<Juesuantongjibiao1_Shoudong_HeaderDto> HeaderDtoList = service.Juesuantongjiebiao1_Shoudong_Head_Search(serviceTrade, startDate, endDate);
            List<Juesuantongjibiao1_Shoudong_LeftDto> LeftDtoList = service.Juesuantongjiebiao1_Shoudong_Left_R(serviceTrade, startDate, endDate);
            List<Juesuantongjibiao1_Shoudong_DataDto> DataDtoList = service.Juesuantongjiebiao1_Shoudong_Data_Search(serviceTrade, startDate, endDate);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }

            string createFileName = "决算统计报表1-追加_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string templateFile = Server.MapPath(tempPath + "决算统计报表1-追加.xlsx");
            if (LeftDtoList.Count == 0)
            {
                throw new Exception("没有可以导出的数据");
            }
            string path = absPath + createFileName;
            System.IO.File.Copy(templateFile, path);
            JuesuantongjiebiaoExport export = new JuesuantongjiebiaoExport();
            export.ExportZhuijia1(path, HeaderDtoList, LeftDtoList, DataDtoList);

            return Json(new { ExportPath = path });
        }
        public ActionResult ExportZhuijiaJuesuan2(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            List<Juesuantongjibiao2_ShoudongDto> lst = service.Juesuantongjiebiao2_Shoudong_Search(serviceTrade, startDate, endDate);

            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string createFileName = "决算统计报表2-追加_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string templateFile = Server.MapPath(tempPath + "决算统计报表2-追加.xlsx");
            if (lst.Count == 0)
            {
                throw new Exception("没有可以导出的数据");
            }

            string path = absPath + createFileName;
            System.IO.File.Copy(templateFile, path);
            JuesuantongjiebiaoExport export = new JuesuantongjiebiaoExport();
            export.ExportZhuijia2(path, lst);

            return Json(new { ExportPath = path });
        }

    }
}