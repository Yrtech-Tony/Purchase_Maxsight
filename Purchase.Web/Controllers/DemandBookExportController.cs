using Purchase.Common;
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
    public class DemandBookExportController : BaseController
    {
        string suffix = ".xlsx";
        string tempPath = "~/DemandBookTemplate/";
        string basePath = "~/Export/DemandBook/";
        // GET: DemandBookExport
        public ActionResult ExportList(string list, string modelType,string zipPath)
        {
            string[] listArry = new string[] { };
            string result = "";
            if (list.Length > 0)
            {
                listArry = list.Split(';');
            }

            string[] wuxing = null;
            string[] youxing = null;
            List<string[]> argsList = new List<string[]>();
            foreach (string item in listArry)
            {
                string[] values = item.Split(',');
                if (modelType != "业务")
                {
                    if (item.Contains("Wuxingshangpincaigou"))
                    {
                        if (wuxing == null)
                        {
                            wuxing = values;
                            argsList.Add(wuxing);
                        }
                        else
                        {
                            wuxing[0] += "," + values[0];
                        }
                        continue;
                    }
                    else if (item.Contains("Youxingshangpincaigou"))
                    {
                        if (youxing == null)
                        {
                            youxing = values;
                            argsList.Add(youxing);
                        }
                        else
                        {
                            youxing[0] += "," + values[0];
                        }
                        continue;
                    }
                }
                argsList.Add(values);
            }

            foreach (string[] args in argsList)
            {
                string id = args[0];
                string type = args[1];
                string province = args[2];
                string city = args[3];
                string projectShortName = args[4];

                result += Export(id, type, province, city, projectShortName, zipPath) + ";";
            }


            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return Json(result);
        }
        public string Export(string id, string type, string province, string city, string projectShortName, string zipPath)
        {
            string name = province + "_" + city + "_" + projectShortName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_需求书" + suffix;
            string absPathTemp = "";
            if (string.IsNullOrEmpty(zipPath))
            {
                absPathTemp = Server.MapPath(basePath);
            }
            else
            {
                absPathTemp = Server.MapPath(zipPath);
                //absPathTemp = zipPath;
            }
            string fullName = absPathTemp + name;
            MethodInfo method = null;
            try
            {
                if (!Directory.Exists(absPathTemp))
                {
                    Directory.CreateDirectory(absPathTemp);
                }
                string templateFile = Server.MapPath(tempPath + type + ".xlsx");
                System.IO.File.Copy(templateFile, fullName);
                DemandBookExport export = new DemandBookExport();
                method = export.GetType().GetMethod("Export" + type);
                if (method != null)
                {
                    method.Invoke(export, new object[] { id, fullName });
                }
            }
            catch (Exception ex)
            {
                Utils.log(ex.ToString());
            }

            if (method == null)
            {
                throw new Exception("没有找到该类型的需求书导出方法，请检查需求书类型！");
            }

            return name + "," + fullName;
        }
        public ActionResult RequirementExport(string list, string modelType)
        {

            string zipPath = "~/Export/DemandBook/" + DateTime.Now.ToString("yyyyMMddHHmmssfff")+"/";
            ExportList(list, modelType, zipPath);
            string zipFile = Path.Combine(Server.MapPath("~/Export/DemandBook/"), "需求书" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip");
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
        public bool CompressFile(string zipPath,string zipFile)
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