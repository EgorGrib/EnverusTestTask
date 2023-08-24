using HtmlAgilityPack;

namespace EnverusExcel;

public class WebDataDownloader
{
    public async Task Download(string websiteUrl, string targetFile, string savePath)
    {
        using var client = new HttpClient();
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

        if (linkNode != null)
        {
            var href = linkNode.GetAttributeValue("href", "");

            var uri = new Uri(websiteUrl);
            var rootDomain = uri.Scheme + "://" + uri.Host;

            var link = rootDomain + href;
        
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
    }
}