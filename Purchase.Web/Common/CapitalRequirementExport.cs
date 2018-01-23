using Microsoft.Office.Interop.Excel;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XHX.Common;

namespace Purchase.Web.Common
{
    public class CapitalRequirementExport
    {
        #region
        public void ExportCapitalRequirement(string path, DateTime startDate, DateTime endDate, List<HeaderDto> HeaderDtoList,
            List<LeftDto> LeftDtoList, List<DataDto> DataDtoList)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));

            //动态列头
            var dStartColIndex = 6;
            foreach (HeaderDto dto in HeaderDtoList)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex+1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex, 2, dto.Display);
                dStartColIndex++;
            }

            int index = 1;
            var rowIndex = 3;
            foreach (LeftDto dto in LeftDtoList)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, dto.DepartmentCode);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExpendType);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.ExpendPattern);
                msExcelUtil.SetCellValue(sheet, 5, rowIndex, dto.PaySumAmt);
                var dataColumnIndex = 6;
                foreach (DataDto dataDto in DataDtoList)
                {
                    if (dataDto.ProjectId == dto.ProjectId)
                    {
                        msExcelUtil.SetCellValue(sheet, dataColumnIndex++, rowIndex, dataDto.SumAmt);
                    }
                }

                if (index < LeftDtoList.Count)
                {
                    msExcelUtil.CopyRow(sheet, rowIndex);
                    rowIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, rowIndex);
                }
            }
            msExcelUtil.DeleteRow(sheet, rowIndex + 1);

            workbook.Save();
            msExcelUtil.dispose();
        }
        #endregion

    }
}