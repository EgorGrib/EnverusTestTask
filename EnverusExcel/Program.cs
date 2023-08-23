using Aspose.Cells;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

var basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json")
    .Build();

var websiteUrl = config["targetWebsite"];
var targetFile = config["targetFile"];
var outPath = config["outPath"];

using var client = new HttpClient();
try
{
    if (websiteUrl is null) throw new ArgumentNullException("targetWebsite");
    if (targetFile is null) throw new ArgumentNullException("targetFile");
    if (outPath is null) throw new ArgumentNullException("outPath");

    client.DefaultRequestHeaders.Add("User-Agent", 
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
    client.DefaultRequestHeaders.Add("Accept", 
        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng");
    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
    client.DefaultRequestHeaders.Add("Accept-Encoding", "deflate,br");
    
    var content = await client.GetStringAsync(websiteUrl);
    var doc = new HtmlDocument();
    doc.LoadHtml(content);

    HtmlNode linkNode = doc.DocumentNode.SelectSingleNode($"//a[contains(text(), '{targetFile}')]");

    string savePath;
    if (linkNode != null)
    {
        var href = linkNode.GetAttributeValue("href", "");

        var uri = new Uri(websiteUrl);
        var rootDomain = uri.Scheme + "://" + uri.Host;

        var link = rootDomain + href;
        savePath = outPath + @"/" + $"{targetFile}.xlsx";
        
        HttpResponseMessage response = await client.GetAsync(link);

        if (response.IsSuccessStatusCode)
        {
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(savePath);
            await contentStream.CopyToAsync(fileStream);
        }
        else
        {
            throw new Exception("Failed to download the file. Status code: " + response.StatusCode);
        }
    }
    else
    {
        throw new Exception("Target file not found on website");
    }
    
    var workbook = new Workbook(savePath);

    var csvPath = outPath + @"/" + $"{targetFile} [Converted].csv";
    
    workbook.Save(csvPath, SaveFormat.CSV);
    
    
    var filteredLines = FilterCSVByYears(csvPath, new List<int> { 2023, 2022 });

    File.WriteAllLines(csvPath, filteredLines);
    
    if (File.Exists(savePath))
    {
        File.Delete(savePath);
    }
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
}

static List<string> FilterCSVByYears(string filePath, List<int> yearsToKeep)
{
    List<string> filteredLines = new List<string>();
    using StreamReader reader = new StreamReader(filePath);
    bool isInsideDesiredYears = false;
    string line;

    while ((line = reader.ReadLine()) != null)
    {
        if (line.StartsWith(",20"))
        {
            int year = int.Parse(line.Substring(1, 4));
            isInsideDesiredYears = yearsToKeep.Contains(year);
        }

        if (isInsideDesiredYears)
        {
            filteredLines.Add(line);
        }
    }

    return filteredLines;
} 