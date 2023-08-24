namespace EnverusExcel.Tests;

public class WebDataDownloaderTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        //arrange
        var downloader = new WebDataDownloader();
        const string url = "https://bakerhughesrigcount.gcs-web.com/intl-rig-count";
        const string targetFile = "File";
        const string savePath = @"..\..\..\TestData\downloaded.xlsx";

        //act
        //assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await downloader.Download(url, targetFile, savePath));
        Assert.That(ex?.Message, Is.EqualTo("Target file not found on website"));
    }
}