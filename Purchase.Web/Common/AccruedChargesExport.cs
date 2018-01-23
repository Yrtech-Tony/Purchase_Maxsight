using Microsoft.Office.Interop.Excel;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XHX.Common;

namespace Purchase.Web.Common
{
    public class AccruedChargesExport
    {
        public void ExportAccruedCharges(string path, List<AccruedChargesDto> list)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            var rowIndex = 2;
            foreach (AccruedChargesDto dto in list)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, rowIndex-1);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.CostType);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.ProjectCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.AccruedChargesAmt);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.PayMonth);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.FlowInvoceAmt);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SourceChk);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.DepartmentCode);
                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

    }
}