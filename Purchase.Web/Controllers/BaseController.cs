using ICSharpCode.SharpZipLib.Zip;
using Infragistics.Documents.Excel;
using Newtonsoft.Json;
using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Attributes;
using Purchase.Web.Common;
using Purchase.Web.Models;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    [CustomHandleError]
    [AuthenAdmin]
    public class BaseController : Controller
    {
        public List<string> modelTypes = new List<string>() { "业务", "内部采购", "其他", "其他" };
        private static Dictionary<string, string> _QuotationTypeAndText;
        public static Dictionary<string, string> QuotationTypeAndText {
            get { 
                if(_QuotationTypeAndText == null){
                    _QuotationTypeAndText = new Dictionary<string,string>();
                    _QuotationTypeAndText.Add("Biancheng","编程");
                    _QuotationTypeAndText.Add("Zhixing","执行");
                    _QuotationTypeAndText.Add("Fuhe","复核");
                    _QuotationTypeAndText.Add("Yanjiu","研究");
                    _QuotationTypeAndText.Add("Zhichi","支持");
                    _QuotationTypeAndText.Add("Chezhan","车展");
                    _QuotationTypeAndText.Add("Qita1","其他1");
                    _QuotationTypeAndText.Add("Qita2","其他2");
                }
                return _QuotationTypeAndText;
            }
        }
        public string DateFormatString = "yyyy-MM-dd";
        protected string baseUploadPath = "/Upload/";
        protected int _countPerPage = 10;
        private MasterService service = new MasterService();
        public BaseController()
        {
        }

        private static List<CityCode> cityCodes;
        public List<CityCode> CityCodes
        {
            get
            {
                if (cityCodes == null)
                {
                    string filePath = Server.MapPath("/data/citycode.json");
                    //构建Json.net的读取流  
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        JsonReader reader = new JsonTextReader(sr);
                        //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                        cityCodes = serializer.Deserialize<List<CityCode>>(reader);
                    }
                }
                return cityCodes;
            }

        }

        public Mst_UserInfo UserInfo
        {
            get
            {
                if ((Mst_UserInfo)Session["LoginUser"] == null)
                {
                    return new Mst_UserInfo()
                    {
                        InUserId = "admin"
                    };
                }
                return (Mst_UserInfo)Session["LoginUser"];
            }
        }

        protected int CalcPages(int total, int? pageSize)
        {
            _countPerPage = pageSize.HasValue ? pageSize.Value : _countPerPage;
            int pages = total % _countPerPage == 0 ? total / _countPerPage : (total / _countPerPage + 1);
            return pages;
        }
        protected int CalcStartIndex(int pageNum)
        {
            return pageNum > 0 ? (pageNum - 1) * _countPerPage : 0;
        }

        public void DeleteFile(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            string controllerName = RouteData.Values["controller"].ToString();
            string filePath = Path.Combine(baseUploadPath, controllerName);
            filePath = Server.MapPath(filePath);
            if (!Directory.Exists(filePath))
            {
                return;
            }
            foreach (string file in Directory.GetFiles(filePath))
            {
                if (file.StartsWith(id + "_"))
                {
                    System.IO.File.Delete(file);
                }
            }
        }
        public string SaveFile(HttpPostedFileBase file, string id)
        {
            if (file == null) return "";
            string controllerName = RouteData.Values["controller"].ToString();
            string filePath = Path.Combine(baseUploadPath, controllerName);
            filePath = Server.MapPath(filePath);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fileName = file.FileName;
            if (!string.IsNullOrWhiteSpace(id))
            {
                fileName = id + "_" + file.FileName;
            }
            string fileFullName = Path.Combine(filePath, fileName);
            file.SaveAs(fileFullName);

            return fileName;
        }

        public string GetFullFileName(string fileName)
        {
            string controllerName = RouteData.Values["controller"].ToString();
            string filePath = Path.Combine(baseUploadPath, controllerName);
            filePath = Server.MapPath(filePath);
            fileName = Path.Combine(filePath, fileName);

            return fileName;
        }

        public string GetRelativeFileName(string fileName)
        {
            string controllerName = RouteData.Values["controller"].ToString();
            string filePath = Path.Combine(baseUploadPath, controllerName);
            fileName = Path.Combine(filePath, fileName);
            return fileName;
        }

        public ActionResult DownloadFile(string fileName)
        {
            string contentType = "application/x-msdownload";
            string downloadFileName = Path.GetFileName(fileName);
            return this.File(fileName, contentType, downloadFileName);
        }

        public ActionResult GetImageSrc(string fileName)
        {
            fileName = GetRelativeFileName(fileName);
            return File(fileName, "image/jpeg");
        }

        protected void ExportExcel<T>(string excelName, List<ColumModel> excelColumList, List<T> dataList)
        {
            Workbook book = new Workbook(WorkbookFormat.Excel97To2003);
            Worksheet sheet = book.Worksheets.Add(excelName);

            #region 创建列头

            for (int k = 0; k < excelColumList.Count; k++)
            {
                ColumModel colModel = excelColumList[k];
                sheet.Rows[0].Cells[k].Value = colModel.Name;
                sheet.Rows[1].Cells[k].Value = colModel.Label;

                if (colModel.Hidden == true)
                {
                    sheet.Columns[k].Hidden = true;
                }

                string align = colModel.Align;
                if (!string.IsNullOrEmpty(align))
                {
                    if (align.ToLower() == "left")
                    {
                        sheet.Columns[k].CellFormat.Alignment = HorizontalCellAlignment.Left;
                    }
                    else if (align.ToLower() == "right")
                    {
                        sheet.Columns[k].CellFormat.Alignment = HorizontalCellAlignment.Right;
                    }
                    else
                    {
                        sheet.Columns[k].CellFormat.Alignment = HorizontalCellAlignment.Center;
                    }
                }

                sheet.Columns[k].Width = colModel.Width * 35;
            }

            sheet.Rows[0].Hidden = true;

            #endregion

            #region 创建数据

            if (dataList != null && dataList.Count > 0)
            {
                T genericObject = default(T);
                for (int r = 0; r < dataList.Count; r++)
                {
                    for (int c = 0; c < excelColumList.Count; c++)
                    {
                        ColumModel colModel = excelColumList[c];

                        genericObject = dataList[r];
                        Type type = genericObject.GetType();
                        PropertyInfo propertyInfo = type.GetProperty(colModel.Name); //获取指定名称的属性
                        if (propertyInfo != null)
                        {
                            object value = propertyInfo.GetValue(genericObject, null); //获取属性值
                            if (value is bool)
                            {
                                sheet.Rows[r + 2].Cells[c].Value = (bool)value ? "√" : "×";
                            }
                            else
                            {
                                sheet.Rows[r + 2].Cells[c].Value = value;
                            }
                        }
                    }
                }
            }

            #endregion

            string fileName = excelName + @".xls";
            //保存excel文件
            string dirPath = this.Server.MapPath("~/Temp/");
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
            book.Save(dirPath + fileName);

            DownloadExcel(fileName, dirPath + fileName, true);
        }
        protected void DownloadExcel(string excelName, string filePath, bool isDeleteAfterDownload = false)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open);
            if (stream == null) return;
            if (string.IsNullOrEmpty(excelName))
            {
                excelName = "GridtoExcel" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            byte[] bytes = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            Response.Clear();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.AddHeader("content-type", "application/x-msdownload");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(excelName, Encoding.GetEncoding("UTF-8")));
            Response.BinaryWrite(bytes);
            Response.End();
            if (isDeleteAfterDownload)
            {
                System.IO.File.Delete(filePath);
            }
        }


        protected bool SevenZipCompress(string folderToZip, string zipedFile, int level)
        {
            if (!Directory.Exists(folderToZip))
            {
                return false;
            }

            try
            {
                //string newZipFolder = zipedFile.Replace(Path.GetExtension(zipedFile), @"\");
                //if (!Directory.Exists(newZipFolder))
                //{
                //    Directory.CreateDirectory(newZipFolder);
                //}

                SevenZipCompressor.SetLibraryPath(Server.MapPath(@"~/bin/7z64.dll"));
                SevenZipCompressor sevenZipCom = new SevenZipCompressor();
                sevenZipCom.CompressDirectory(folderToZip, zipedFile);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SevenZipCompress Error !" + ex.ToString());
                return false;
            }
        }
        public static void Compress(List<FileInfo> fileNames, string GzipFileName, int CompressionLevel, int SleepTimer)
        {
            ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(GzipFileName));
            try
            {
                s.SetLevel(CompressionLevel);   //0 - store only to 9 - means best compression
                foreach (FileInfo file in fileNames)
                {
                    FileStream fs = null;
                    try
                    {
                        fs = file.Open(FileMode.Open, FileAccess.ReadWrite);
                    }
                    catch
                    {
                        continue;
                    }
                    // 将文件分批读入缓冲区
                    byte[] data = new byte[2048];
                    int size = 2048;
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file.Name));
                    entry.DateTime = (file.CreationTime > file.LastWriteTime ? file.LastWriteTime : file.CreationTime);
                    s.PutNextEntry(entry);
                    while (true)
                    {
                        size = fs.Read(data, 0, size);
                        if (size <= 0) break;
                        s.Write(data, 0, size);
                    }
                    fs.Close();
                    Thread.Sleep(SleepTimer);
                }
            }
            finally
            {
                s.Finish();
                s.Close();
            }
        }
        [HttpPost]
        public ActionResult LoadPartial(string partialView)
        {
            return PartialView(partialView);
        }

        public ActionResult GetProvinces()
        {
            string filePath = Server.MapPath("/data/province.json");
            //构建Json.net的读取流  
            using (StreamReader sr = new StreamReader(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(sr);
                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                List<Province> provinces = serializer.Deserialize<List<Province>>(reader);

                return Json(provinces, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetCitys()
        {
            string filePath = Server.MapPath("/data/city.json");
            //构建Json.net的读取流  
            using (StreamReader sr = new StreamReader(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(sr);
                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                List<City> citys = serializer.Deserialize<List<City>>(reader);

                return Json(citys, JsonRequestBehavior.AllowGet);
            }
        }
        protected override JsonResult Json(object data, string contentType,
        Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            JsonNetResult result = new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
            result.Settings.DateFormatString = DateFormatString;
            return result;
        }
        #region 首页提醒
        public ActionResult RemindSearch(int? pageSize, int pageNum)
        {
            if (pageSize.HasValue)
            {
                _countPerPage = pageSize.Value;
            }
            List<RemindDto> list = service.RemindSearch(UserInfo.UserId);
            int total = list.Count();
            int pageCount = CalcPages(total, pageSize);
            int start = CalcStartIndex(pageNum);
            list = list.Skip(start).Take(_countPerPage).ToList();

            return Json(new { List = list, PageCount = pageCount, CurPage = pageNum });
        }
        public ActionResult RemindCancel(string remindType, string remindId, string alterUserId)
        {
            service.RemindCancel(remindType, remindId, alterUserId);
            return Json("");
        }
        #endregion

        public ActionResult LoadChildHiddenCode(string type)
        {
            using (PurchaseEntities db = new PurchaseEntities())
            {
                List<string> lst = db.HiddenCode.Where(x => x.UseChk.HasValue && x.UseChk.Value && x.Type == type).OrderBy(x => x.InDateTime).Select(x => x.Name).ToList();
                lst.Insert(0, "");
                return Json(lst);
            }
        }
        public List<RequiremetMstDto> GetRequirementMstDtoByQuotationType(string quotationType, List<RequiremetMstDto> list)
        {
            List<RequiremetMstDto> returenList = new List<RequiremetMstDto>();
            if (string.IsNullOrEmpty(quotationType))
            { returenList = list; }

            foreach (RequiremetMstDto mst in list)
            {
                if (quotationType == "Biancheng" && mst.RequirementType == "Bianchengjibianji")
                {
                    returenList.Add(mst);
                }
                else if (quotationType == "Fuhe" && mst.RequirementType == "Bianmajifuhe")
                {
                    returenList.Add(mst);
                }
                else if (quotationType == "Chezhan" && mst.RequirementType == "Cheliangzhanpinghui")
                {
                    returenList.Add(mst);
                }
                else if (quotationType == "Qita1" && mst.RequirementType == "Youxingshangpincaigou")
                {
                    returenList.Add(mst);
                }
                else if (quotationType == "Qita2" && mst.RequirementType == "Wuxingshangpincaigou")
                {
                    returenList.Add(mst);
                }
                else if (quotationType == "Yanjiu" && mst.RequirementType == "Yanjiujishujufenxi")
                {
                    returenList.Add(mst);
                }
                else if (quotationType == "Zhichi" &&
                    (mst.RequirementType == "Gongyingshangchailv"
                    || mst.RequirementType == "Yunshuzuche"
                    || mst.RequirementType == "Changdibuzhan"))
                {
                    returenList.Add(mst);
                }
                else if (quotationType == "Zhixing" &&
                    (mst.RequirementType == "Mianfangjidianfang"
                    || mst.RequirementType == "Zuotanhui"
                    || mst.RequirementType == "Shenfangjirijiliuzhi"
                    || mst.RequirementType == "Mingjian"
                    || mst.RequirementType == "Pankujijiagediaocha"
                    || mst.RequirementType == "Guankong"
                    || mst.RequirementType == "Anfang"
                    || mst.RequirementType == "Wangluodiaoyan"))
                {
                    returenList.Add(mst);
                }
            }
            return returenList;

        }
    }
}