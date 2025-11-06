using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using ExcelDataReader;
using System.Data;
using OfficeOpenXml;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace PDFDownload
{
    //Defining a class for reading Excel files
    public class Reader
    {
        //Defining the method that will read an Excel file from a path and return a DataTable containing the data
        public DataTable ReadFile(string filePath)
        {
            //Defining a DataTable to hold the data
            DataTable dataTable = new DataTable();

            //Try catch block to handle potential exceptions when reading the file
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                //Reading the excel file using FileStream and ExcelDataReader
                //The 'using' statement ensures that the FileStream is properly disposed of after use
                //The dataTable is used to store the data read from the Excel file 
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    dataTable = excelReader.AsDataSet().Tables[0];
                    excelReader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the Excel file: {ex.Message}");
            }
            
            return dataTable;
        }

        public DataTable ReadExcelFile(string filePath)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    List<string> columns = GetColumnNames(worksheet);

                    foreach (string column in columns)
                    {
                        dataTable.Columns.Add(column);
                    }

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) {
                        DataRow newRow = dataTable.NewRow();

                        for (int col = 1; col <= worksheet.Dimension.End.Row; col++) {
                            newRow[col - 1] = worksheet.Cells[row, col].Text;
                        }
                        dataTable.Rows.Add(newRow);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return dataTable;
        }

        private List<string> GetColumnNames(ExcelWorksheet sheet)
        {
            List<string> columnNames = new List<string>();
            for (int col = 1; col <= sheet.Dimension.End.Column; col++)
            {
                string columnName = sheet.Cells[1, col].Text.Trim();
                if (string.IsNullOrEmpty(columnName))
                    columnName = $"Column{col}";
                columnNames.Add(columnName);
            }
            return columnNames;
        }
    }
}
