using OfficeOpenXml;

namespace EnverusExcel;

public static class XlsxToCsvConverter
{
    public static void Convert(string excelPath, string csvPath)
    {
        using var package = new ExcelPackage(new FileInfo(excelPath));
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var worksheet = package.Workbook.Worksheets[0];
        
        using (var writer = new StreamWriter(csvPath))
        {
            for (int row = 1; row <= worksheet.Dimension.Rows; row++)
            {
                string line = "";
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    if (col > 1)
                        line += ",";
                    
                    var cellValue = worksheet.Cells[row, col].Value;
                    line += cellValue != null ? cellValue.ToString() : "";
                }
                
                if (line.EndsWith(","))
                {
                    line = line.Substring(0, line.Length - 1);
                }
                
                writer.WriteLine(line);
            }
        }
    }
}