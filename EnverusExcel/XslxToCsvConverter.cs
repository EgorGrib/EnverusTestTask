using Aspose.Cells;

namespace EnverusExcel;

public static class XslxToCsvConverter
{
    public static void Convert(string excelPath, string csvPath)
    {
        var workbook = new Workbook(excelPath);
        workbook.Save(csvPath, SaveFormat.CSV);
    }
}