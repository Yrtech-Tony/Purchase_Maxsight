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
    public class QuotationExportController : BaseController
    {

        string suffix = ".xlsx";
        string tempPath = "~/QuotationTemplate/";
        string basePath = "~/Export/DemandBook/";
        private RequirementService service = new RequirementService();
        private QuotationService quotationService = new QuotationService();
        private PurchaseEntities db = new PurchaseEntities();
        private Dictionary<string, List<QuotationDto>> typeQuotationDic = new Dictionary<string, List<QuotationDto>>();
        private Dictionary<string, string> typePathDic = new Dictionary<string, string>();
        private Dictionary<string, string> respQuotationTypeDic = new Dictionary<string, string>();
        public QuotationExportController()
        {
            respQuotationTypeDic.Add("Anfang", "Zhixing");
            respQuotationTypeDic.Add("Mingjian", "Zhixing");
            respQuotationTypeDic.Add("Pankujijiagediaocha", "Zhixing");
            respQuotationTypeDic.Add("Mianfangjidianfang", "Zhixing");
            respQuotationTypeDic.Add("Zuotanhui", "Zhixing");
            respQuotationTypeDic.Add("Shenfangjirijiliuzhi", "Zhixing");
            respQuotationTypeDic.Add("Wangluodiaoyan", "Zhixing");
            respQuotationTypeDic.Add("Guankong", "Zhixing");

            respQuotationTypeDic.Add("Bianmajifuhe", "Fuhe");
            respQuotationTypeDic.Add("Bianchengjibianji", "Biancheng");
            respQuotationTypeDic.Add("Yanjiujishujufenxi", "Yanjiu");
            respQuotationTypeDic.Add("Changdibuzhan", "Zhichi");
            respQuotationTypeDic.Add("Yunshuzuche", "Zhichi");
            respQuotationTypeDic.Add("Gongyingshangchailv", "Zhichi");
            respQuotationTypeDic.Add("Youxingshangpincaigou", "Qita1");
            respQuotationTypeDic.Add("Wuxingshangpincaigou", "Qita2");
            respQuotationTypeDic.Add("Cheliangzhanpinghui", "Chezhan");
        }

        // GET: QuotationExport
        public ActionResult ExportList(string list, ProjectDto projectDto, string supplierName, string modelType)
        {
            string[] listArry = new string[] { };
            string result = "";
            if (list.Length > 0)
            {
                listArry = list.Split(';');
            }

            foreach (string item in listArry)
            {
                string[] subItems = item.Split(',');
                RequiremetMstDto mstDto = new RequiremetMstDto()
                {
                    ProjectId = "",
                    SeqNO = "",
                    RequirementId = subItems[0],
                    RequirementType = subItems[1],
                    Province = subItems[2],
                    City = subItems[3],
                    ProjectShortName = subItems[4],
                    Responsibilites = subItems[5],
                    ModelType = modelType,
                };
                string quotationType = "";
                respQuotationTypeDic.TryGetValue(mstDto.RequirementType, out quotationType);
                if (typeQuotationDic.ContainsKey(quotationType))
                {
                    List<QuotationDto> value = null;
                    typeQuotationDic.TryGetValue(quotationType, out value);
                    value.AddRange(QuotationGenSearch(mstDto, quotationType));
                }
                else
                {
                    List<QuotationDto> value = new List<QuotationDto>();
                    value.AddRange(QuotationGenSearch(mstDto, quotationType));
                    typeQuotationDic.Add(quotationType, value);
                }
                if (!typePathDic.ContainsKey(quotationType))
                {
                    typePathDic.Add(quotationType, GetExcelPath(mstDto, modelType, quotationType));
                }
            }

            foreach (KeyValuePair<string, List<QuotationDto>> keyValue in typeQuotationDic)
            {
                MethodInfo method = null;
                try
                {
                    string path = "";
                    typePathDic.TryGetValue(keyValue.Key, out path);

                    QuotationExport export = new QuotationExport();

                    method = export.GetType().GetMethod("Export" + keyValue.Key);
                    if (method != null)
                    {
                        method.Invoke(export, new object[] { path, keyValue.Value, projectDto, supplierName });
                    }

                    string fileName = Path.GetFileName(path);
                    result += fileName + "," + path + ";";
                }
                catch (Exception ex)
                {
                    Utils.log(ex.ToString());
                }

                if (method == null)
                {
                    throw new Exception("没有找到该类型的确认单导出方法，请检查确认单类型！");
                }
            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return Json(result);
        }
        
        
        private string GetExcelPath(RequiremetMstDto mstDto, string modelType, string quotationType)
        {
            string name = mstDto.Province + "_" + mstDto.City + "_" + mstDto.ProjectShortName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_确认单" + suffix;
            string absPath = Server.MapPath(basePath);
            if (!Directory.Exists(absPath))
            {
                Directory.CreateDirectory(absPath);
            }
            string fullName = absPath + name;

            if (modelType == "内部采购")
            {//内部采购  无形商品(其他2)=>wuxingshangpin 有形商品(其他1)=>youxingshangpin
                if (mstDto.RequirementType == "Youxingshangpincaigou")
                {
                    quotationType = "Youxingshangpincaigou";
                }
                if (mstDto.RequirementType == "Wuxingshangpincaigou")
                {
                    quotationType = "Wuxingshangpincaigou";
                }
            }

            string templateFile = Server.MapPath(tempPath + quotationType + suffix);
            System.IO.File.Copy(templateFile, fullName);

            return fullName;
        }

        private List<QuotationDto> QuotationGenSearch(RequiremetMstDto mstDto, string quotationType)
        {
            if (quotationType == "Zhixing")
            {
                return quotationService.QuotationGenSearch_Zhixing(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
            }
            else if (quotationType == "Fuhe")
            {
                return quotationService.QuotationGenSearch_Fuhe(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
            }
            else if (quotationType == "Biancheng")
            {
                return quotationService.QuotationGenSearch_Biancheng(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
            }
            else if (quotationType == "Yanjiu")
            {
                return quotationService.QuotationGenSearch_Yanjiu(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
            }
            else if (quotationType == "Zhichi")
            {
                var lst1 = quotationService.QuotationGenSearch_Zhichi01(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
                var lst2 = quotationService.QuotationGenSearch_Zhichi02(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();

                lst1.AddRange(lst2);
                return lst1;
            }
            else if (quotationType == "Qita1")
            {
                return quotationService.QuotationGenSearch_Qita1(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
            }
            else if (quotationType == "Qita2")
            {
                return quotationService.QuotationGenSearch_Qita2(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
            }
            else if (quotationType == "Chezhan")
            {
                var lst1 = quotationService.QuotationGenSearch_Chenzhan01(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
                var lst2 = quotationService.QuotationGenSearch_Chenzhan02(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
                var lst3 = quotationService.QuotationGenSearch_Chenzhan02(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();

                lst1.AddRange(lst2);
                lst1.AddRange(lst3);
                return lst1;
            }
            else
            {
                if (mstDto.RequirementType == "Youxingshangpincaigou")
                {
                    return quotationService.QuotationGenSearch_Qita1(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
                }
                else if (mstDto.RequirementType == "Wuxingshangpincaigou")
                {
                    return quotationService.QuotationGenSearch_Qita2(mstDto.ProjectId, mstDto.SeqNO, mstDto.RequirementType, mstDto.RequirementId).ToList<QuotationDto>();
                }
            }
            return new List<QuotationDto>();
        }


        private string GetQuotationType(string reqType, string responsibilites)
        {
            //if (reqType == "Anfang"
            //    || reqType == "Mingjian"
            //    || reqType == "Pankujijiagediaocha"
            //    || reqType == "Mianfangjidianfang"
            //    || reqType == "Zuotaihui"
            //    || reqType == "Shenfangjirijiliuzhi"
            //    || reqType == "Wangluodiaoyan"
            //    || reqType == "Guangkong")


            //{
            //    return "执行";
            //}
            //else if (reqType == "Bianmajifuhe")
            //{
            //    return "复核";
            //}
            //else if (reqType == "Bianchengjibianji")
            //{
            //    return "编程";
            //}
            //else if (reqType == "Yanjiujishujufenxi")
            //{
            //    return "研究";
            //}
            //else if (reqType == "Changdibuzhan" || reqType == "Yunshuzuche" || reqType == "Gongyingshangchailv")
            //{
            //    return "支持";
            //}
            //else if (reqType == "Youxingshangpincaigou" && responsibilites == "业务-其他1")
            //{
            //    return "业务-其他1";
            //}
            //else if (reqType == "Youxingshangpincaigou" && responsibilites != "业务-其他1")
            //{
            //    return "有形商品采购";
            //}
            return "";
        }
    }
}