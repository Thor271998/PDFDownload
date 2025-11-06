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

namespace PDFDownload
{
    //Defining a class for writing a status rapport
    public class Writer
    {
        private static string rapportPath = @"Output/StatusRapport.txt";
        public async Task<bool> WriteStatus(bool success, string pdfName)
        {
            bool write = false;
            string rapport = "";
            try
            {
                if (success)
                {
                    rapport = pdfName + ": Successfully downloaded." + Environment.NewLine;

                    await File.AppendAllTextAsync(rapportPath, rapport);

                    write = true;
                }
                else
                {
                    rapport = pdfName + ": Failed to download." + Environment.NewLine;

                    await File.AppendAllTextAsync(rapportPath, rapport);

                    write = false;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                write = false;
            }

            return write;
        }
    }
}
