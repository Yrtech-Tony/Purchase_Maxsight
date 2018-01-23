using Microsoft.Office.Interop.Excel;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XHX.Common;

namespace Purchase.Web.Common
{
    public class JuesuantongjiebiaoExport
    {
        #region =============================决算统计表-业务采购======================
        public void ExportYewucaigoubiao1(string path, DateTime startDate, DateTime endDate, List<Juesuantongjibiao1_HeaderDto> HeaderDtoList,
            List<Juesuantongjibiao1_LeftDto> LeftDtoList, List<Juesuantongjibiao1_DataDto> DataDtoList)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));

            //动态列头
            var dStartColIndex = 4;
            foreach (Juesuantongjibiao1_HeaderDto dto in HeaderDtoList)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.ExecuteMode);
            }

            var rowIndex = 3;
            foreach (Juesuantongjibiao1_LeftDto dto in LeftDtoList)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.SupplierSumAmt);
                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Juesuantongjibiao1_DataDto dto in DataDtoList)
            {
                dStartColIndex = 4;
                var dataRowIndex = 3;
                int dcolIndex = HeaderDtoList.FindIndex(x => x.ExecuteMode == dto.ExecuteMode);
                int drowIndex = LeftDtoList.FindIndex(x => x.SupplierId == dto.SupplierId);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.ExecuteModeSumAmt);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportYewucaigoubiao2(string path, DateTime startDate, DateTime endDate, List<Juesuantongjibiao2Dto> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));

            //动态列头
            var rowIndex = 3;
            int index = 1;
            foreach (Juesuantongjibiao2Dto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, dto.ExecuteMode);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExecuteModeSumAmt);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.SettleCount);
                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, rowIndex);
                    //msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.Execute_Avg);
                    rowIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, rowIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, rowIndex + 1);

                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        #endregion

        #region =============================决算统计表-内部采购======================
        public void ExportNewbucaigoubiao1(string path, DateTime startDate, DateTime endDate, List<Juesuantongjibiao1_inter_HeaderDto> HeaderDtoList,
            List<Juesuantongjibiao1_inter_LeftDto> LeftDtoList, List<Juesuantongjibiao1_inter_DataDto> DataDtoList)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 2, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));

            //动态列头
            var dStartColIndex = 4;
            foreach (Juesuantongjibiao1_inter_HeaderDto dto in HeaderDtoList)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 3, dto.SupplyService);
            }

            var rowIndex = 4;
            foreach (Juesuantongjibiao1_inter_LeftDto dto in LeftDtoList)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.SupplierSumAmt);
                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Juesuantongjibiao1_inter_DataDto dto in DataDtoList)
            {
                dStartColIndex = 4;
                var dataRowIndex = 4;
                int dcolIndex = HeaderDtoList.FindIndex(x => x.SupplyService == dto.SupplyService);
                int drowIndex = LeftDtoList.FindIndex(x => x.SupplierId == dto.SupplierId);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.SupplyServiceSumAmt);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportNewbucaigoubiao2(string path, DateTime startDate, DateTime endDate, List<Juesuantongjibiao2_interDto> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 2, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));

            //动态列头
            var rowIndex = 4;
            int index = 1;
            foreach (Juesuantongjibiao2_interDto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, dto.SupplyService);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.SupplyServiceSumAmt);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.SettleCount);
                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, rowIndex);
                    rowIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, rowIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, rowIndex + 1);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        #endregion
        #region 决算统计表-追加
        public void ExportZhuijia1(string path, List<Juesuantongjibiao1_Shoudong_HeaderDto> HeaderDtoList,
           List<Juesuantongjibiao1_Shoudong_LeftDto> LeftDtoList, List<Juesuantongjibiao1_Shoudong_DataDto> DataDtoList)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //动态列头
            var dStartColIndex = 4;
            foreach (Juesuantongjibiao1_Shoudong_HeaderDto dto in HeaderDtoList)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 1, dto.FeeContent);
            }

            var rowIndex = 2;
            foreach (Juesuantongjibiao1_Shoudong_LeftDto dto in LeftDtoList)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.SupplierSumAmt);
                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Juesuantongjibiao1_Shoudong_DataDto dto in DataDtoList)
            {
                dStartColIndex = 4;
                var dataRowIndex = 2;
                int dcolIndex = HeaderDtoList.FindIndex(x => x.FeeContent == dto.FeeContent);
                int drowIndex = LeftDtoList.FindIndex(x => x.SupplierId == dto.SupplierId);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.FeeContentSumAmt);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportZhuijia2(string path, List<Juesuantongjibiao2_ShoudongDto> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            var rowIndex = 2;
            int index = 1;
            foreach (Juesuantongjibiao2_ShoudongDto dto in lst)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, dto.FeeContent);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.FeeContentSumAmt);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.SettleCount);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.FeeContent_Avg);
                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, rowIndex);
                    //msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.Execute_Avg);
                    rowIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, rowIndex);
                }
                else
                {
                    msExcelUtil.DeleteRow(sheet, rowIndex + 1);

                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        #endregion

    }
}