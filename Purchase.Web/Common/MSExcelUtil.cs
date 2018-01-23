using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;

namespace XHX.Common
{
    public class MSExcelUtil
    {
        Application _excelApp = null;
        Workbooks _objWorkbooks = null;
        Workbook workbook = null;


        public MSExcelUtil()
        {
            _excelApp = new Application();
            _objWorkbooks = _excelApp.Workbooks;
            _excelApp.Visible = false;
            _excelApp.DisplayAlerts = false;
        }

        public Workbook OpenExcelByMSExcel(string filePath)
        {
            workbook = _objWorkbooks.Open(filePath, Type.Missing, false, true, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            return workbook;
        }
        public void SetCellHeight(Worksheet worksheet, int x, int y, int height)
        {
            //int xInt = ColumnToIndex(x);
            (worksheet.Cells[y, x] as Range).RowHeight = height;
        }
        public void SetColumnWidth(Worksheet worksheet, int columnIndex,int width)
        {
            ((Range)worksheet.Columns[columnIndex, System.Reflection.Missing.Value]).ColumnWidth = width;
        }
        public string GetCellValue(Worksheet worksheet, int x, int y)
        {
            return (worksheet.Cells[y, x] as Range).Value2 == null ? "" : (worksheet.Cells[y, x] as Range).Value2.ToString();
        }
        public void SetCellValue(Worksheet worksheet, int x, int y, object value)
        {
            (worksheet.Cells[y, x] as Range).Value2 = value;
        }
        public void SetCellValueAndMultLine(Worksheet worksheet, int x, int y, object value, int linePage)
        {
            (worksheet.Cells[y, x] as Range).Value2 = value;
            if (value == null) return;
            var lineCount = value.ToString().Length % linePage == 0 ? value.ToString().Length / linePage : value.ToString().Length / linePage + 1;
            SetCellHeight(worksheet, x, y, lineCount * 14);
        }

        public void DeleteRow(Worksheet m_objSheet, int rowIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
        }
        public void DeleteRowRange(Worksheet m_objSheet, int startRowIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                DeleteRow(m_objSheet, startRowIndex);
            }
        }

        public void AddRow(Worksheet m_objSheet, int rowIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).Insert(System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
        }
        public void AddColumn(Worksheet m_objSheet, int columnIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Columns[columnIndex, System.Reflection.Missing.Value]).Insert(System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight);
        }
        public void SetWrapText(Worksheet m_objSheet, int rowIndexStart, int rowIndexEnd, bool wrapText)
        {
            for (int i = rowIndexStart; i < rowIndexEnd + 1; i++)
            {
                SetWrapTextOneRow(m_objSheet, i, wrapText);
            }
        }
        public void SetWrapTextOneRow(Worksheet m_objSheet, int rowIndex, bool wrapText)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).WrapText = wrapText;
        }
        public void CopyRow(Worksheet m_objSheet, int rowIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).Copy();
        }
        public void CopyColumn(Worksheet m_objSheet, int columnIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Columns[columnIndex, System.Reflection.Missing.Value]).Copy();
        }

        public void CopyAndInsertColumns(Worksheet m_objSheet, string sCell, string eCell, string iCell)
        {
            Range cols = m_objSheet.get_Range(sCell, eCell);
            cols.Copy();
            m_objSheet.get_Range(iCell).Insert(System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
        }

        public void CopyAndInsertColumns(Worksheet m_objSheet, int sCol, int sRow, int eCol, int eRow, int iCol, int iRow)
        {
            string sCell = MSExcelUtil.ToName(sCol) + sRow;
            string eCell = MSExcelUtil.ToName(eCol) + eRow;
            string iCell = MSExcelUtil.ToName(iCol) + iRow;
            CopyAndInsertColumns(m_objSheet, sCell, eCell, iCell);
        }

        #region - 由数字转换为Excel中的列字母 -

        public static int ToIndex(string columnName)
        {
            if (!Regex.IsMatch(columnName.ToUpper(), @"[A-Z]+")) { throw new Exception("invalid parameter"); }

            int index = 0;
            char[] chars = columnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index - 1;
        }


        public static string ToName(int index)
        {
            if (index < 0) { throw new Exception("invalid parameter"); }

            List<string> chars = new List<string>();
            do
            {
                index--;
                chars.Insert(0, ((char)(index % 26 + (int)'A')).ToString());
                index = (int)((index - index % 26) / 26);
            } while (index > 0);

            return String.Join(string.Empty, chars.ToArray());
        }
        #endregion

        public void dispose()
        {
            if (workbook != null)
            {
                workbook.Close();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbook);
            }
            if (_excelApp != null)
            {
                _excelApp.Quit();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }
    }
}
