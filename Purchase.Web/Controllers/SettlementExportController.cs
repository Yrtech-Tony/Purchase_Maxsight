using Purchase.Common;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using Purchase.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Controllers
{
    public class SettlementExportController : BaseController
    {

        string suffix = ".xlsx";
        string tempPath = "~/SettlementTemplate/";
        string basePath = "~/Export/Settlement/";
        private SettlementService service = new SettlementService();
        private PurchaseEntities db = new PurchaseEntities();
        public SettlementExportController()
        {
        }

        // GET: QuotationExport
        public ActionResult ExportList(string ProjectId, string SupplierId, string ProjectShortName, string ProjectCode,
            string SupplierName, string ProjectName, string ModelType, string quotationGroupId, string settlementId, string zipPath)
        {
            SettlementMstDto mstDto = service.SettleDtlMainSearch(ProjectId, SupplierId, settlementId,quotationGroupId, UserInfo.UserId).FirstOrDefault();
            SettlementExport export = new SettlementExport();
            mstDto.ProjectName = ProjectName;
            mstDto.ProjectCode = ProjectCode;
            mstDto.ProjectShortName = ProjectShortName;
            mstDto.SupplierName = SupplierName;
            string path = GetExcelPath(ProjectShortName, SupplierName, ModelType, zipPath);

            List<SettlementDtlDto> lst = service.SettlementDtlSearch_After(settlementId, UserInfo.UserId);
            lst.AddRange(service.SettlementDtlSearch_After_shoudong(settlementId, UserInfo.UserId));
            if (ModelType == "业务")
            {
                export.Export(path, lst, mstDto);
            }
            else
            {
                export.ExportNeibu(path, lst, mstDto);
            }

            return Json(path + "," + Path.GetFileName(path));
        }

        private string GetExcelPath(string ProjectShortName, string SupplierName, string ModelType,string zipPath)
        {
            string name = ProjectShortName + "_" + SupplierName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_结算单" + suffix;
            string absPath = "";
            if (string.IsNullOrEmpty(zipPath))
            {
                absPath = Server.MapPath(basePath);
            }
            else {
                absPath = Server.MapPath(zipPath);
            }
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string fullName = absPath + name;

            string templateFile = "";
            if (ModelType == "业务")
            {
                templateFile = Server.MapPath(tempPath + "SettlementTemplate" + suffix);
            }
            else
            {
                templateFile = Server.MapPath(tempPath + "SettlementTemplate_Neibu" + suffix);
            }

            System.IO.File.Copy(templateFile, fullName);

            return fullName;
        }


        public ActionResult SettlementExport(string list)
        {
            string zipPath = "~/Export/Settlement/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "/";
            string zipFile = Path.Combine(Server.MapPath("~/Export/Settlement/"), "结算单" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip");

            string[] listArry = new string[] { };
            string result = "";
            if (list.Length > 0)
            {
                listArry = list.Split(';');
            }
            List<string[]> argsList = new List<string[]>();

            foreach (string item in listArry)
            {
                string[] values = item.Split(',');
                argsList.Add(values);
            }

            foreach (string[] args in argsList)
            {
                string projectId = args[0];
                string supplierId = args[1];
                string projectShortName = args[2];
                string projectCode = args[3];
                string supplierName = args[4];
                string projectName = args[5];
                string modelType = args[6];
                string quotationGroupId = args[7];
                string settlementId = args[8];

                result += ExportList(projectId, supplierId, projectShortName, projectCode, supplierName, projectName, modelType,quotationGroupId,settlementId,zipPath) + ";";
            }
           
            if (System.IO.File.Exists(zipFile))
            {
                System.IO.File.Delete(zipFile);
            }
            if (!CompressFile(zipPath, zipFile))
            {
                throw new Exception("打包文件失败！");
            }
            return Json(new { ExportPath = zipFile });
        }
        public bool CompressFile(string zipPath, string zipFile)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath(zipPath));
                FileInfo[] fileInfo = directoryInfo.GetFiles();
                DateTime dt = DateTime.Now;
                List<FileInfo> logsInOneDay = new List<FileInfo>();
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    if (fileInfo[i].Name.Substring(fileInfo[i].Name.Length - 3) != "zip")
                    {
                        logsInOneDay.Add(fileInfo[i]);
                    }
                }
                if (logsInOneDay.Count > 0)
                {
                    try
                    {
                        Compress(logsInOneDay, zipFile, 9, 100);
                        //foreach (FileInfo fileInfo in logsInOneDay)
                        //{
                        //    try
                        //    {
                        //        fileInfo.Delete();
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        //错误信息记录处理
                        //    }
                        //}
                    }
                    catch (Exception e)
                    {
                        return false;
                        //错误信息记录处理
                    }
                }
            }
            catch (Exception e)
            {
                return false;
                //错误信息记录处理
            }
            return true;
        }

    }
}