using Purchase.Common;
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
    public class JiagetongjiebiaoController : BaseController
    {
        JiagetongjiebiaoService service = new JiagetongjiebiaoService();
        // GET: Jiagetongjiebiao
        public ActionResult Index()
        {
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
        public ActionResult LoadZhuijia(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Shoudong_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Shoudong_Dto> LeftDtoList = service.Jiagetongjiebiao_Shoudong_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Shoudong_Dto> DataDtoList = service.Jiagetongjiebiao_Shoudong_Data_R(ServiceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });

        }
        public ActionResult LoadBiancheng(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Biancheng_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Biancheng_Dto> LeftDtoList = service.Jiagetongjiebiao_Biancheng_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Biancheng_Dto> DataDtoList = service.Jiagetongjiebiao_Biancheng_Data_R(ServiceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });

        }

        public ActionResult LoadZhixing(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Zhixing_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Zhixing_Dto> LeftDtoList = service.Jiagetongjiebiao_Zhixing_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Zhixing_Dto> DataDtoList = service.Jiagetongjiebiao_Zhixing_Data_R(ServiceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });

        }

        public ActionResult LoadFuhe(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Fuhe_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Fuhe_Dto> LeftDtoList = service.Jiagetongjiebiao_Fuhe_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Fuhe_Dto> DataDtoList = service.Jiagetongjiebiao_Fuhe_Data_R(ServiceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadYanjiu(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Yanjiu_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Yanjiu_Dto> LeftDtoList = service.Jiagetongjiebiao_Yanjiu_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Yanjiu_Dto> DataDtoList = service.Jiagetongjiebiao_Yanjiu_Data_R(ServiceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadQita1(string modelType, DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_Qita1_Dto> lst = service.Jiajiatongjibiao_Qita1_R(modelType, ServiceTrade, startDate, endDate);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadQita2(string modelType, DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_Qita2_Dto> lst = service.Jiajiatongjibiao_Qita2_R(modelType, ServiceTrade, startDate, endDate);
            int total = lst.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            lst = lst.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = lst, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadChezhan(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Chezhan_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Chezhan_Dto> LeftDtoList = service.Jiagetongjiebiao_Chezhan_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Chezhan_Dto> DataDtoList = service.Jiagetongjiebiao_Chezhan_Data_R(ServiceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });
        }

        public ActionResult LoadZhichi(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "", int pageNum = 1, int pageSize = 10)
        {
            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Zhichi_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Zhichi_Dto> LeftDtoList = service.Jiagetongjiebiao_Zhichi_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Zhichi_Dto> DataDtoList = service.Jiagetongjiebiao_Zhichi_Data_R(ServiceTrade, startDate, endDate);
            int total = LeftDtoList.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            LeftDtoList = LeftDtoList.Skip(start).Take(_countPerPage).ToList();

            return Json(new { DataDtoList = DataDtoList, HeaderDtoList = HeaderDtoList, LeftDtoList = LeftDtoList, PageCount = pageCount, CurPage = pageNum });
        }
        #region 导出excel
        string suffix = ".xlsx";
        string tempPath = "~/CommonTemplate/jiagetongjiebiao/";
        string basePath = "~/Export/Jiagetongjiebiao/";
        public ActionResult ExportJiagetongjibiao(string quotationType, DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "")
        {
            try
            {
                MasterService masterService = new MasterService();
                // 如果没有选择开始时间和结束时间，默认的时间周期为项目的最早开始时间到项目最晚的开始时间
                ProjectDto projectDto = masterService.ProjectStartDateSearch("", "", ModelType, ServiceTrade).FirstOrDefault();
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
                string createFileName = "价格统计表_" + quotationType + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
                string path = absPath + createFileName;
                string templateFile = Server.MapPath(tempPath + quotationType + suffix);
                System.IO.File.Copy(templateFile, path);
                JiagetongjiebiaoExport export = new JiagetongjiebiaoExport();
                if (quotationType == "Biancheng")
                {
                    List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Biancheng_Head_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Left_Biancheng_Dto> LeftDtoList = service.Jiagetongjiebiao_Biancheng_Left_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Data_Biancheng_Dto> DataDtoList = service.Jiagetongjiebiao_Biancheng_Data_R(ServiceTrade, startDate, endDate);
                    export.ExportBiancheng(path, HeaderDtoList, LeftDtoList, DataDtoList, startDate, endDate);
                }
                else if (quotationType == "Zhixing")
                {
                    List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Zhixing_Head_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Left_Zhixing_Dto> LeftDtoList = service.Jiagetongjiebiao_Zhixing_Left_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Data_Zhixing_Dto> DataDtoList = service.Jiagetongjiebiao_Zhixing_Data_R(ServiceTrade, startDate, endDate);
                    export.ExportZhixing(path, HeaderDtoList, LeftDtoList, DataDtoList, startDate, endDate);
                }
                else if (quotationType == "Fuhe")
                {
                    List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Fuhe_Head_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Left_Fuhe_Dto> LeftDtoList = service.Jiagetongjiebiao_Fuhe_Left_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Data_Fuhe_Dto> DataDtoList = service.Jiagetongjiebiao_Fuhe_Data_R(ServiceTrade, startDate, endDate);
                    export.ExportFuhe(path, HeaderDtoList, LeftDtoList, DataDtoList, startDate, endDate);
                }
                else if (quotationType == "Yanjiu")
                {
                    List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Yanjiu_Head_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Left_Yanjiu_Dto> LeftDtoList = service.Jiagetongjiebiao_Yanjiu_Left_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Data_Yanjiu_Dto> DataDtoList = service.Jiagetongjiebiao_Yanjiu_Data_R(ServiceTrade, startDate, endDate);
                    export.ExportYanjiu(path, HeaderDtoList, LeftDtoList, DataDtoList, startDate, endDate);
                }
                else if (quotationType == "Qita1")
                {
                    List<Jiagetongjibiao_Qita1_Dto> lst = service.Jiajiatongjibiao_Qita1_R(ModelType, ServiceTrade, startDate, endDate);
                    export.ExportQita1(path, lst, startDate, endDate);
                }
                else if (quotationType == "Qita2")
                {
                    List<Jiagetongjibiao_Qita2_Dto> lst = service.Jiajiatongjibiao_Qita2_R(ModelType, ServiceTrade, startDate, endDate);
                    export.ExportQita2(path, lst, startDate, endDate);
                }
                else if (quotationType == "Chezhan")
                {
                    List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Chezhan_Head_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Left_Chezhan_Dto> LeftDtoList = service.Jiagetongjiebiao_Chezhan_Left_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Data_Chezhan_Dto> DataDtoList = service.Jiagetongjiebiao_Chezhan_Data_R(ServiceTrade, startDate, endDate);
                    export.ExportChezhan(path, HeaderDtoList, LeftDtoList, DataDtoList, startDate, endDate);
                }
                else if (quotationType == "Zhichi")
                {
                    List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Zhichi_Head_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Left_Zhichi_Dto> LeftDtoList = service.Jiagetongjiebiao_Zhichi_Left_R(ServiceTrade, startDate, endDate);
                    List<Jiagetongjibiao_Data_Zhichi_Dto> DataDtoList = service.Jiagetongjiebiao_Zhichi_Data_R(ServiceTrade, startDate, endDate);
                    export.ExportZhichi(path, HeaderDtoList, LeftDtoList, DataDtoList, startDate, endDate);
                }

                return Json(new { ExportPath = path });
            }
            catch (Exception ex)
            {

                Utils.log(ex.Message.ToString());
                return Json(new { ExportPath = "" });

            }
        }
        public ActionResult ExportZhuijia(DateTime startDate, DateTime endDate, string ModelType = "", string ServiceTrade = "")
        {
            MasterService masterService = new MasterService();
            // 如果没有选择开始时间和结束时间，默认的时间周期为项目的最早开始时间到项目最晚的开始时间
            ProjectDto projectDto = masterService.ProjectStartDateSearch("", "", ModelType, ServiceTrade).FirstOrDefault();
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
            string createFileName = "价格统计表_" + "zhuijia" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
            string path = absPath + createFileName;
            string templateFile = Server.MapPath(tempPath + "zhuijia" + suffix);
            System.IO.File.Copy(templateFile, path);
            JiagetongjiebiaoExport export = new JiagetongjiebiaoExport();

            List<Jiagetongjibiao_HeaderDto> HeaderDtoList = service.Jiagetongjiebiao_Shoudong_Head_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Left_Shoudong_Dto> LeftDtoList = service.Jiagetongjiebiao_Shoudong_Left_R(ServiceTrade, startDate, endDate);
            List<Jiagetongjibiao_Data_Shoudong_Dto> DataDtoList = service.Jiagetongjiebiao_Shoudong_Data_R(ServiceTrade, startDate, endDate);
            export.ExportZhuijia(path, HeaderDtoList, LeftDtoList, DataDtoList, startDate, endDate);
            return Json(new { ExportPath = path });
        }
        #endregion
    }
}