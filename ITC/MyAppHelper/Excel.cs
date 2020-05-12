using System;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Web;

namespace ITC.MyAppHelper
{
    public class Excel : System.Web.UI.Page
    {
        public void GenerateExcel(DataTable dataToExcel, string sheetName, string fileName, string reportHeader, string reportFilter, string[] positionDateTime)
        {
            string currentDirectorypath = Environment.CurrentDirectory;
            string finalFileNameWithPath = string.Empty;
            fileName = string.Format("{0}_{1}", fileName, DateTime.Now.ToString("dd-MM-yyyy"));
            finalFileNameWithPath = string.Format("{0}\\{1}.xlsx", Server.MapPath("."), fileName);

            //Delete existing file with same file name.
            if (File.Exists(finalFileNameWithPath))
                File.Delete(finalFileNameWithPath);
            var newFile = new FileInfo(finalFileNameWithPath);
            using (var package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                //TableStyle tbs = new TableStyle();
                //tbs.BorderWidth = 1;
                //tbs.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                //OfficeOpenXml.Table.TableStyles offTbs = new OfficeOpenXml.Table.TableStyles();
                //worksheet.Cells["A1:B1:C1"].Merge = true;
                worksheet.Cells["A1"].LoadFromDataTable(dataToExcel, true);
                worksheet.Cells["A1"].LoadFromDataTable(dataToExcel, true, OfficeOpenXml.Table.TableStyles.Light1);

                int row = dataToExcel.Rows.Count + 1;
                if (dataToExcel.Rows.Count > 0)
                {
                    if (positionDateTime[0].ToString() != "")
                    {
                        for (int i = 2; i <= row; i++)
                        {
                            // FORMAT DATETIME
                            worksheet.Cells[positionDateTime[0] + i].Style.Numberformat.Format = "dd/mm/yyyy h:mm AM/PM";
                            worksheet.Cells[positionDateTime[1] + i].Style.Numberformat.Format = "dd/mm/yyyy h:mm AM/PM";
                        }
                    }
                }

                //worksheet.Row(4).Style.Font.Bold = true;
                package.Workbook.Properties.Title = @"PSCC";
                package.Workbook.Properties.Author = "PSCCApp";
                package.Workbook.Properties.Subject = @"Export From PSCC App";
                //package.Save();

                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
                package.SaveAs(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.End();
            }
        }
    }
}