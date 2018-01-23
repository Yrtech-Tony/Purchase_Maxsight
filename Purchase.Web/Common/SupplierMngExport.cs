using Microsoft.Office.Interop.Excel;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XHX.Common;

namespace Purchase.Web.Common
{
    public class SupplierMngExport
    {
        public void ExportSupplier(string path, List<SupplierDto> list)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            var rowIndex = 3;
            foreach (SupplierDto dto in list)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.Province);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.City);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.ServiceTrade);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierType);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.PurchaseType);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.CooperationState);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierType1);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.NotServiceType);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.PayCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.RecommendDepartment);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.SupplierShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.ProvideService);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BranchCompanyAddress);

                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.CorporateName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.CorporateTel);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.CorporateFixTel);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.CorporateEmail);

                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessMainName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessMainTel);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessMainFixTel);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessMainEmail);

                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessSecondName);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessSecondTel);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessSecondFixTel);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.BussinessSecondEmail);

                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.CompanyFaxCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.Address);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.PostCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, rowIndex, dto.Remark);

                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportBankInfo(string path, List<SupplierDto> list)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            var startIndex = 2;
            foreach (SupplierDto dto in list)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, startIndex - 1);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ModifyDateTime);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.CooperationState);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.AccountBankFullName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.AccountName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.AccountBankNo);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.VATRate);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ServiceTrade);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.RecommendDepartment);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.CooperationState);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                startIndex++;
                msExcelUtil.AddRow(sheet, startIndex);
            }

            workbook.Save();
             msExcelUtil.dispose();

        }
    }
}