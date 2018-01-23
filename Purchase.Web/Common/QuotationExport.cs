using Microsoft.Office.Interop.Excel;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XHX.Common;

namespace Purchase.Web.Common
{
    public class QuotationExport
    {
        private PurchaseEntities db = new PurchaseEntities();
        private QuotationService service = new QuotationService();

        public void ExportBiancheng(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);

            int startIndex = 3;
            int index = 1;
            foreach (Quotation_BianChengDto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.zhixingfangshi);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.gongzuofenlei);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.gongzuoneirong);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.guigeyaoqiu);
                msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.zhiliangbiaozhun);
                msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.beizhu);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }
            }

            workbook.Save();
             msExcelUtil.dispose();
        }
        public void ExportZhixing(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);

            int startIndex = 0;
            startIndex += 3;
            int startRow =startIndex;
            int index = 1;
            foreach (Quotation_ZhiXingDto dto in lst)
            {                
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.IntoShopType);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.ExcuteType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.UserType);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.ExistingOrPotential);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.CustomerType);
                msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.Count);
                msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.Remark);
                               
                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);                    
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }
            }

            workbook.Save();
             msExcelUtil.dispose();
        }       
        public void ExportFuhe(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);

            int startIndex = 0;
            startIndex += 3;
            int index = 1;
            foreach (Quotation_FuHeDto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.fuheyaoqiu);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.dianhuazixun);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.dianhuahuifang);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.yanbenbianji);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.fuzhuyongpin);
                msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.bianmaleixing);
                msExcelUtil.SetCellValue(sheet, 11, startIndex, dto.wenjuanbiancheng);
                msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, 16, startIndex, dto.beizhu);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }                
            }

            workbook.Save();
             msExcelUtil.dispose();
        }
        public void ExportYanjiu(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);
            
            int startIndex = 0;
            startIndex += 3;
            int index = 1;
            foreach (Quotation_YanJiuDto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.zhixingfangshi);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.gongzuofenlei);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.gongzuoneirong);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.guigeyaoqiu);
                msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.zhiliangbiaozhun);
                msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.beizhu);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }
            }

            workbook.Save();
             msExcelUtil.dispose();
        }
        public void ExportQita1(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);

            int startIndex = 0;
            startIndex += 3;
            int index = 1;
            foreach (Quotation_QiTa1Dto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.caigoufenlei);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.caigoufangshi);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.tigongfuwu);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.pinming);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.guige);
                msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.xinghao);
                msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.beizhu);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }
            }

            workbook.Save();
             msExcelUtil.dispose();
        }
        public void ExportQita2(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);

            int startIndex = 0;
            startIndex += 3;
            int index = 1;
            foreach (Quotation_QiTa2Dto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.caigoufenlei);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.caigoufangshi);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.tigongfuwu);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.feiyonggoucheng);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.pinming);
                msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.guige);
                msExcelUtil.SetCellValue(sheet, 11, startIndex, dto.xinghao);
                msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.beizhu);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }
            }

            workbook.Save();
             msExcelUtil.dispose();
        }
        public void ExportChezhan(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);

            int startIndex = 4;
            startIndex = InsertChezhans(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Chenzhan01").ToList(), startIndex, supplierName);
            startIndex += 4;
            startIndex = InsertChezhans(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Chenzhan02").ToList(), startIndex, supplierName);
            startIndex += 4;
            startIndex = InsertChezhans(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Chenzhan03").ToList(), startIndex, supplierName);
            
            workbook.Save();
             msExcelUtil.dispose();
        }
        private int InsertChezhans(MSExcelUtil msExcelUtil, Worksheet sheet, List<QuotationDto> lst, int startIndex, string supplierName)
        {
            int index = 1;
            foreach (Quotation_CheZhanDto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.ExcuteType);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.Responsibilites);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.UserType);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.ExistingOrPotential);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.CustomerType);
                msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.Count);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }
            }

            return startIndex;
        }

        public void ExportZhichi(string path, List<QuotationDto> lst, ProjectDto projectDto, string supplierName)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValue(sheet, 2, 1, projectDto.ProjectShortName);
            int startIndex = 5;
            startIndex = InsertZhiChis(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Changdibuzhan_Zhichi01").ToList(), startIndex, supplierName);
            startIndex += 4;
            startIndex = InsertZhiChis(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Changdibuzhan_Zhichi02").ToList(), startIndex, supplierName);
            startIndex += 5;
            startIndex = InsertZhiChis(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Yunshuzuche_Zhichi01").ToList(), startIndex, supplierName);
            startIndex += 4;
            startIndex = InsertZhiChis(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Yunshuzuche_Zhichi02").ToList(), startIndex, supplierName);
            startIndex += 5;
            startIndex = InsertZhiChis(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Gongyingshangchailv_Zhichi01").ToList(), startIndex, supplierName);
            startIndex += 4;
            startIndex = InsertZhiChis(msExcelUtil, sheet, lst.Where(x => x.DtoType == "Gongyingshangchailv_Zhichi02").ToList(), startIndex, supplierName);
            
            workbook.Save();
             msExcelUtil.dispose();
        }

        private int InsertZhiChis(MSExcelUtil msExcelUtil,Worksheet sheet, List<QuotationDto> lst, int startIndex, string supplierName)
        {
            int index = 1;
            foreach (ZhiChi01Dto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, supplierName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.fenlei);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.shuliang);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, startIndex + 1);
                }
            }

            return startIndex;
        }
    }
}