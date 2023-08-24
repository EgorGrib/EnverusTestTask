using EnverusExcel;
using Microsoft.Extensions.Configuration;

var basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json")
    .Build();

var websiteUrl = config["targetWebsite"];
var targetFile = config["targetFile"];
var outPath = config["outPath"];
var yearsToSave = config["lastYearsOfData"];

try
{
    if (websiteUrl is null) throw new ArgumentNullException("targetWebsite");
    if (targetFile is null) throw new ArgumentNullException("targetFile");
    if (outPath is null) throw new ArgumentNullException("outPath");
    if (yearsToSave is null) throw new ArgumentNullException("lastYearsOfData");
    
    var excelSavePath = outPath + @"/" + $"{targetFile}.xlsx";
    
    var downloader = new WebDataDownloader();
    await downloader.Download(websiteUrl, targetFile, excelSavePath);

    var csvPath = outPath + @"/" + $"{targetFile} [Converted].csv";
    
    XlsxToCsvConverter.Convert(excelSavePath, csvPath);
    
    var yearsToKeep = new List<int>();
    var currentYear = DateTime.Now.Year;
    var yearCount = Convert.ToInt32(yearsToSave);
    for (int i = 0; i < yearCount; i++)
    {
        yearsToKeep.Add(currentYear - i);
    }

    var csvFilter = new CsvYearsFilter(yearsToKeep);
    csvFilter.Filter(csvPath);
    
    if (File.Exists(excelSavePath))
    {
        File.Delete(excelSavePath);
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}