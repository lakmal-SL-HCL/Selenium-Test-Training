using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using xl = Microsoft.Office.Interop.Excel;

namespace ABCShop
{
    class ExcelAPI
    {
        xl.Application xlApp = null;
        xl.Workbooks workbooks = null;
        xl.Workbook workbook = null;
        Hashtable sheets;
        public String xlfilePath;

        public ExcelAPI(string xlpath)
        {
            this.xlfilePath = xlpath;
        }

        public void OpenExcel()
        {
            xlApp = new xl.Application();
            workbooks = xlApp.Workbooks;
            workbook = workbooks.Open(xlfilePath);
            sheets = new Hashtable();
            int count = 1;
            foreach (xl.Worksheet sheet in workbook.Sheets)
            {
                sheets[count] = sheet.Name;
                count++;
            }
        }

        public int GetRowCount(string sheetName)
        {
            OpenExcel();
            int rowCount = 0;
            int sheetValue = 0;

            if (sheets.ContainsValue(sheetName))
            {
                foreach (DictionaryEntry sheet in sheets)
                {
                    if (sheet.Value.Equals(sheetName))
                    {
                        sheetValue = (int)sheet.Key;
                    }
                }
                xl.Worksheet worksheet = workbook.Worksheets[sheetValue] as xl.Worksheet;
                xl.Range range = worksheet.UsedRange;
                rowCount = range.Rows.Count;
            }
            CloseExcel();
            return rowCount;
        }

        public int GetColumnCount(string sheetName)
        {
            OpenExcel();
            int colCount = 0;
            int sheetValue = 0;
            if (sheets.ContainsValue(sheetName))
            {
                foreach (DictionaryEntry sheet in sheets)
                {
                    if (sheet.Value.Equals(sheetName))
                    {
                        sheetValue = (int)sheet.Key;
                    }
                }
                xl.Worksheet worksheet = workbook.Worksheets[sheetValue] as xl.Worksheet;
                xl.Range range = worksheet.UsedRange;
                colCount = range.Columns.Count;
            }
            CloseExcel();
            return colCount;
        }

        public string GetCellData(string sheetName, int row, int col)
        {
            OpenExcel();
            string value = string.Empty;
            int sheetValue = 0;
            if (sheets.ContainsValue(sheetName))
            {
                foreach (DictionaryEntry sheet in sheets)
                {
                    if (sheet.Value.Equals(sheetName))
                    {
                        sheetValue = (int)sheet.Key;
                    }
                }
                xl.Worksheet worksheet = workbook.Worksheets[sheetValue] as xl.Worksheet;
                xl.Range range = worksheet.UsedRange;
                value = (range.Cells[row, col] as xl.Range).Value2;
            }
            CloseExcel();
            return value;
        }

        public bool SetCellData(string sheetName, int row, int col, string value)
        {
            OpenExcel();
            int sheetValue = 0;
            try
            {
                if (sheets.ContainsValue(sheetName))
                {
                    foreach (DictionaryEntry sheet in sheets)
                    {
                        if (sheet.Value.Equals(sheetName))
                        {
                            sheetValue = (int)sheet.Key;
                        }
                    }
                    xl.Worksheet worksheet = workbook.Worksheets[sheetValue] as xl.Worksheet;
                    xl.Range range = worksheet.UsedRange;
                    range.Cells[row, col] = value;
                    workbook.Save();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            CloseExcel();
            return true;
        }

        public void CloseExcel()
        {
            workbook.Close(false, xlfilePath, null);
            Marshal.FinalReleaseComObject(workbook);
            workbook = null;

            workbooks.Close();
            Marshal.FinalReleaseComObject(workbooks);
            workbooks = null;

            xlApp.Quit();
            Marshal.FinalReleaseComObject(xlApp);
            xlApp = null;
        }
    }
}
