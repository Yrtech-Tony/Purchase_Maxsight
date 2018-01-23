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
    public class QuotationHorExport
    {
        private PurchaseEntities db = new PurchaseEntities();
        private QuotationService service = new QuotationService();

        public void ExportQuotationHor(string path, string projectName, string projectShortName, string projectCode,
            List<QuotationExport_Head_Dto> HeaderDtoList,List<QuotationExport_Left_Dto> LeftDtoList, List<QuotationExport_Data_Dto> DataDtoList)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            //项目名称、简称、编号
            msExcelUtil.SetCellValue(sheet, 2, 1, projectName);
            msExcelUtil.SetCellValue(sheet, 4, 1, projectShortName);
            msExcelUtil.SetCellValue(sheet, 6, 1, projectCode);

            //动态列头
            int dStartColIndex = 5;
            int beizhuCount = 0;
            for(int i=0;i<HeaderDtoList.Count;i++)
            {
                QuotationExport_Head_Dto dto = HeaderDtoList[i]; 
                msExcelUtil.SetCellValue(sheet, dStartColIndex, 2, dto.QuotationMode);
                dStartColIndex = dStartColIndex + 3;
                if (i < (HeaderDtoList.Count - 1))
                {
                    msExcelUtil.CopyAndInsertColumns(sheet,dStartColIndex-3,2,dStartColIndex-1,8,dStartColIndex,2);
                    if (beizhuCount == 0)
                    {
                        msExcelUtil.SetColumnWidth(sheet, dStartColIndex + 1, 10);
                    }                   
                    //复制备注
                    beizhuCount++;
                    msExcelUtil.CopyColumn(sheet, dStartColIndex + 3 + beizhuCount);
                    msExcelUtil.AddColumn(sheet, dStartColIndex + 4 + beizhuCount);
                    msExcelUtil.SetCellValue(sheet, dStartColIndex + 4 + beizhuCount, 2, "备注" + (i + 2));                     
                }
            }
            beizhuCount = 0;
            for (int i = 0; i < HeaderDtoList.Count; i++)
            {
                //复制备注
                beizhuCount++;
                msExcelUtil.SetColumnWidth(sheet, dStartColIndex + beizhuCount, 20);
            }
           

            var rowIndex = 4;           
            foreach (QuotationExport_Left_Dto dto in LeftDtoList)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, rowIndex - 3);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.zhixingchengshi);
                //msExcelUtil.CopyRow(sheet, rowIndex);
                rowIndex++;               
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);
            msExcelUtil.DeleteRow(sheet, rowIndex);

            int beizhuStart = dStartColIndex + 1;
            decimal[] zongjiArr = new decimal[LeftDtoList.Count];
            foreach (QuotationExport_Data_Dto dto in DataDtoList)
            {
                dStartColIndex = 5;
                var dataRowIndex = 4;
                int dcolIndex = HeaderDtoList.FindIndex(x => x.QuotationMode == dto.QuotationMode);
                int drowIndex = LeftDtoList.FindIndex(x => (x.SupplierName == dto.SupplierName) && (x.zhixingshengfen == dto.zhixingshengfen) && (x.zhixingchengshi == dto.zhixingchengshi));
                dStartColIndex += dcolIndex*3;
                dataRowIndex += drowIndex;
                decimal heji = 0;
                if (dto.danjia.HasValue && dto.shuliang.HasValue)
                {
                    heji = dto.danjia.Value * dto.shuliang.Value;
                }
                zongjiArr[drowIndex] += heji;
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, dataRowIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, dataRowIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, dataRowIndex, heji);
                msExcelUtil.SetCellValue(sheet, beizhuStart + dcolIndex, dataRowIndex, dto.beizhu);

                msExcelUtil.SetCellValue(sheet, beizhuStart - 1, dataRowIndex, zongjiArr[drowIndex]);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportQuotationHorQita(string path, string projectName, string projectShortName, string projectCode,
               List<QuotationExport_Head_Dto> HeaderDtoList, List<QuotationExport_Left_Dto> LeftDtoList, List<QuotationExport_Data_Dto> DataDtoList)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            //项目名称、简称、编号
            msExcelUtil.SetCellValue(sheet, 2, 1, projectName);
            msExcelUtil.SetCellValue(sheet, 4, 1, projectShortName);
            msExcelUtil.SetCellValue(sheet, 6, 1, projectCode);

            //动态列头
            int dStartColIndex = 3;
            int beizhuCount = 0;
            for (int i = 0; i < HeaderDtoList.Count; i++)
            {
                QuotationExport_Head_Dto dto = HeaderDtoList[i];
                msExcelUtil.SetCellValue(sheet, dStartColIndex, 2, dto.QuotationMode);
                dStartColIndex = dStartColIndex + 3;
                if (i < (HeaderDtoList.Count - 1))
                {
                    msExcelUtil.CopyAndInsertColumns(sheet, dStartColIndex - 3, 2, dStartColIndex - 1, 8, dStartColIndex, 2);
                    if (beizhuCount == 0)
                    {
                        msExcelUtil.SetColumnWidth(sheet, dStartColIndex + 1, 10);
                    }
                    //复制备注
                    beizhuCount++;
                    msExcelUtil.CopyColumn(sheet, dStartColIndex + 3 + beizhuCount);
                    msExcelUtil.AddColumn(sheet, dStartColIndex + 4 + beizhuCount);
                    msExcelUtil.SetCellValue(sheet, dStartColIndex + 4 + beizhuCount, 2, "备注" + (i + 2));
                }
            }
            beizhuCount = 0;
            for (int i = 0; i < HeaderDtoList.Count; i++)
            {
                //复制备注
                beizhuCount++;
                msExcelUtil.SetColumnWidth(sheet, dStartColIndex + beizhuCount, 20);
            }

            var rowIndex = 4;
            foreach (QuotationExport_Left_Dto dto in LeftDtoList)
            {
                int colIndex = 1;
                string chengshi = dto.zhixingshengfen;
                if (!string.IsNullOrEmpty(dto.zhixingchengshi)){
                    chengshi+="-"+dto.zhixingchengshi;
                }
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, chengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierName);
                
                //msExcelUtil.CopyRow(sheet, rowIndex);
                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);
            msExcelUtil.DeleteRow(sheet, rowIndex);

            int beizhuStart = dStartColIndex + 1;
            decimal[] zongjiArr = new decimal[LeftDtoList.Count];
            foreach (QuotationExport_Data_Dto dto in DataDtoList)
            {
                dStartColIndex = 3;
                var dataRowIndex = 4;
                int dcolIndex = HeaderDtoList.FindIndex(x => x.QuotationMode == dto.QuotationMode);
                int drowIndex = LeftDtoList.FindIndex(x => (x.SupplierName == dto.SupplierName) && (x.zhixingshengfen == dto.zhixingshengfen) && (x.zhixingchengshi == dto.zhixingchengshi));
                dStartColIndex += dcolIndex * 3;
                dataRowIndex += drowIndex;
                decimal heji = 0;
                if (dto.danjia.HasValue && dto.shuliang.HasValue)
                {
                    heji = dto.danjia.Value * dto.shuliang.Value;
                }
                zongjiArr[drowIndex] += heji;
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, dataRowIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, dataRowIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, dataRowIndex, heji);
                msExcelUtil.SetCellValue(sheet, beizhuStart + dcolIndex, dataRowIndex, dto.beizhu);

                msExcelUtil.SetCellValue(sheet, beizhuStart - 1, dataRowIndex, zongjiArr[drowIndex]);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
    
    
    }
}