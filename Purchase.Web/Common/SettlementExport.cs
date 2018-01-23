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
    public class SettlementExport
    {
        private PurchaseEntities db = new PurchaseEntities();
        private SettlementService service = new SettlementService();

        public void Export(string path, List<SettlementDtlDto> lst, SettlementMstDto settlementMstDto)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValueAndMultLine(sheet, 2, 7, settlementMstDto.ProjectName,13);
            msExcelUtil.SetCellValueAndMultLine(sheet, 5, 7, settlementMstDto.ProjectShortName, 10);
           // msExcelUtil.SetCellValueAndMultLine(sheet, 2, 8, settlementMstDto.ExecuteCycle, 40);
            msExcelUtil.SetCellValueAndMultLine(sheet, 5, 8, settlementMstDto.SupplierName, 10);
            msExcelUtil.SetCellValueAndMultLine(sheet, 2, 9, settlementMstDto.zhixingchengshi, 40);

            int startIndex = 11;
            startIndex = InsertSettlements(msExcelUtil, sheet, lst.Where(x => x.SettlementType == "1").ToList(), startIndex);
            startIndex += 3;
            startIndex = InsertSettlements(msExcelUtil, sheet, lst.Where(x => x.SettlementType == "2").ToList(), startIndex);
            startIndex += 3;
            startIndex = InsertSettlements(msExcelUtil, sheet, lst.Where(x => x.SettlementType == "3").ToList(), startIndex);

            startIndex += 4;
            msExcelUtil.SetCellValue(sheet, 4, startIndex, settlementMstDto.FlowOrderSum);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportNeibu(string path, List<SettlementDtlDto> lst, SettlementMstDto settlementMstDto)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            msExcelUtil.SetCellValueAndMultLine(sheet, 2, 7, settlementMstDto.ProjectName, 11);
            msExcelUtil.SetCellValueAndMultLine(sheet, 5, 7, settlementMstDto.ProjectCode, 11);
            //msExcelUtil.SetCellValueAndMultLine(sheet, 2, 8, settlementMstDto.ExecuteCycle, 40);
            msExcelUtil.SetCellValueAndMultLine(sheet, 5, 8, settlementMstDto.SupplyService, 10);
            msExcelUtil.SetCellValueAndMultLine(sheet, 2, 9, settlementMstDto.SupplierName, 11);
            msExcelUtil.SetCellValueAndMultLine(sheet, 5, 9, settlementMstDto.ServiceRegion, 10);

            int startIndex = 11;
            startIndex = InsertSettlements(msExcelUtil, sheet, lst.Where(x => x.SettlementType == "1").ToList(), startIndex);
            startIndex += 3;
            startIndex = InsertSettlements(msExcelUtil, sheet, lst.Where(x => x.SettlementType == "2").ToList(), startIndex);
            startIndex += 3;
            startIndex = InsertSettlements(msExcelUtil, sheet, lst.Where(x => x.SettlementType == "3").ToList(), startIndex);

            startIndex += 4;
            msExcelUtil.SetCellValue(sheet, 4, startIndex, settlementMstDto.FlowOrderSum);

            workbook.Save();
             msExcelUtil.dispose();
        }

        private int InsertSettlements(MSExcelUtil msExcelUtil, Worksheet sheet, List<SettlementDtlDto> lst, int startIndex)
        {
            int index = 0;
            foreach (SettlementDtlDto dto in lst)
            {
                if (!string.IsNullOrEmpty(dto.SettleAmt))
                {
                    if (index < lst.Count - 1)
                    {
                        msExcelUtil.CopyRow(sheet, startIndex);                      
                        msExcelUtil.AddRow(sheet, startIndex+1);
                    }

                    msExcelUtil.SetCellValueAndMultLine(sheet, 1, startIndex, dto.FeeContent,11);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, dto.danjia);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.SettleCount);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.SettleAmt);
                    if (dto.Remark != null && dto.FeeContent.Length < dto.Remark.Length)
                    {
                        msExcelUtil.SetCellValueAndMultLine(sheet, 5, startIndex, dto.Remark, 10);
                    }
                    else
                    {
                        msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.Remark);
                    }
                    
                    if (index < lst.Count - 1)
                    {
                        startIndex++;
                    }
                }

                index++;
            }
            msExcelUtil.DeleteRow(sheet, startIndex + 1);
            return startIndex;
        }
    }
}