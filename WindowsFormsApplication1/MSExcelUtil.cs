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

        public MSExcelUtil()
        {
            _excelApp = new Application();
            _objWorkbooks = _excelApp.Workbooks;
            _excelApp.Visible = false;
            _excelApp.DisplayAlerts = false;
        }

        public Workbook OpenExcelByMSExcel(string filePath)
        {
            return _objWorkbooks.Open(filePath, Type.Missing, false, true, Type.Missing, Type.Missing,
                                                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
        public void SetCellHeight(Worksheet worksheet, string x, int y, int height)
        {
            int xInt = ColumnToIndex(x);
            (worksheet.Cells[y, xInt] as Range).RowHeight = height;
        }
        public void SetCellHeight(Worksheet worksheet, int x, int y, int height)
        {
            //int xInt = ColumnToIndex(x);
            (worksheet.Cells[y, x] as Range).RowHeight = height;
        }
        public string GetCellValue(Worksheet worksheet, string x, int y)
        {
            int xInt = ColumnToIndex(x);
            return (worksheet.Cells[y, xInt] as Range).Value2 == null ? "" : (worksheet.Cells[y, xInt] as Range).Value2.ToString();
        }
        public string GetCellValue(Worksheet worksheet, int x, int y)
        {
            return (worksheet.Cells[y, x] as Range).Value2 == null ? "" : (worksheet.Cells[y, x] as Range).Value2.ToString();
        }
        public void SetCellFunction(Worksheet worksheet, string x, int y)
        {
            
        }
        public void SetCellValue(Worksheet worksheet, string x, int y, object value)
        {
            int xInt = ColumnToIndex(x);
            //worksheet.Rows.Delete(1);
            (worksheet.Cells[y, xInt] as Range).Value2 = value;
        }
        public void SetCellValue(Worksheet worksheet, int x, int y, object value)
        {
            //int xInt = ColumnToIndex(x);
            //worksheet.Rows.Delete(1);
            (worksheet.Cells[y, x] as Range).Value2 = value;
        }

        public void SetCellValue(Worksheet worksheet, string xy, object value)
        {
            int x, y, index;
            index = xy.Length - 1;
            bool isNum = true;
            while (isNum)
            {
                isNum = Int32.TryParse(xy.Substring(index--), out y);
            }
            index += 2;
            x = ColumnToIndex(xy.Substring(0, index));
            Int32.TryParse(xy.Substring(index), out y);
            (worksheet.Cells[y, x] as Range).Value2 = value;
        }

        private int ColumnToIndex(string column)
        {
            if (!Regex.IsMatch(column.ToUpper(), @"[A-Z]+"))
            {
                throw new Exception("Invalid parameter");
            }
            int index = 0;
            char[] chars = column.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index;
        }
        private char IndexToColumn(int index)
        {
            char column = 'A';
            for (int i = 0; i < index; i++)
            {
                column= (char)((int)'A' + i);
            }
            return column;
        }
        public void InsertPicture(Worksheet m_objSheet, Range RangeName, string PicturePath, int rowIndex)
        {
            object m_objOpt = System.Reflection.Missing.Value;
            Range m_objRange = RangeName;
            //m_objRange.Select();
            float PicLeft, PicTop;
            float PictuteWidth = float.Parse((m_objRange.Width).ToString()), PictureHeight = float.Parse(m_objRange.Height.ToString());
            PicLeft = Convert.ToSingle(m_objRange.Left);
            //PicTop = Convert.ToSingle(m_objRange.Top) + ((float)rowIndex - 3f) * 0.4f;
            PicTop = Convert.ToSingle(m_objRange.Top);
            m_objSheet.Shapes.AddPicture(PicturePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, PictuteWidth, PictureHeight);
        }
        public void DeleteRow(Worksheet m_objSheet, int rowIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
        }
        public void DeleteRowRange(Worksheet m_objSheet, int startRowIndex,int count)
        {
            for (int i = 0; i <count; i++)
            {
                DeleteRow(m_objSheet, startRowIndex);
            }
        }


        public void AddRow(Worksheet m_objSheet, int rowIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).Insert(System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
           
            
        }
        public void SetWrapText(Worksheet m_objSheet, int rowIndexStart, int rowIndexEnd, bool wrapText)
        {
            for (int i = rowIndexStart; i < rowIndexEnd + 1; i++)
            {
                SetWrapTextOneRow(m_objSheet, i, wrapText);
            }
        }
        public void SetWrapTextOneRow(Worksheet m_objSheet, int rowIndex,bool wrapText)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).WrapText = wrapText;
        }
        public void CopyRow(Worksheet m_objSheet, int rowIndex)
        {
            ((Microsoft.Office.Interop.Excel.Range)m_objSheet.Rows[rowIndex, System.Reflection.Missing.Value]).Copy();
        }

        public void InsertSum(Worksheet m_objSheet, int x, int y,int startRow,int endRow)
        {
             string startColumn = IndexToColumn(x).ToString() + startRow.ToString();
             string endColumn = IndexToColumn(x).ToString() + endRow.ToString();
             string gongshi =  "=Sum({0}:{1})";//设置公式
             (m_objSheet.Cells[y, x] as Range).Formula = string.Format(gongshi, startColumn, endColumn);
        }
    }
}
