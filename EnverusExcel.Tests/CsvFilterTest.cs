using System.Security.Cryptography;
using System.Text;

namespace EnverusExcel.Tests;

public class CsvFilterTest
{
    private const string SourceCsvFilePath = @"..\..\..\TestData\All data.csv";
    
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void TestLastTwoYears()
    {
        //arrange
        const string outFilePath = @"..\..\..\TestData\last two years [test].csv";
        const string correctFilePath = @"..\..\..\TestData\last two years.csv";
        File.Copy(SourceCsvFilePath, outFilePath);
        
        var yearsToKeep = new List<int> {2023, 2022};
        var filter = new CsvYearsFilter(yearsToKeep);
        
        //act
        filter.Filter(outFilePath);
        
        //assert
        var originalHash = GetFileHash(correctFilePath);
        var copiedHash = GetFileHash(outFilePath);

        Assert.That(originalHash, Is.EqualTo(copiedHash));
        
        if (File.Exists(outFilePath))
        {
            File.Delete(outFilePath);
        }
    }
    
    [Test]
    public void TestLastTenYears()
    {
        //arrange
        const string outFilePath = @"..\..\..\TestData\last ten years [test].csv";
        const string correctFilePath = @"..\..\..\TestData\last ten years.csv";
        File.Copy(SourceCsvFilePath, outFilePath);
        
        var yearsToKeep = new List<int> {2023, 2022, 2021, 2020, 2019, 2018, 2017, 2016, 2015, 2014};
        var filter = new CsvYearsFilter(yearsToKeep);
        
        //act
        filter.Filter(outFilePath);
        
        //assert
        var originalHash = GetFileHash(correctFilePath);
        var copiedHash = GetFileHash(outFilePath);

        Assert.That(originalHash, Is.EqualTo(copiedHash));
        
        if (File.Exists(outFilePath))
        {
            File.Delete(outFilePath);
        }
    }
    
    [Test]
    public void Test1999year()
    {
        //arrange
        const string outFilePath = @"..\..\..\TestData\1999 year [test].csv";
        const string correctFilePath = @"..\..\..\TestData\1999 year.csv";
        File.Copy(SourceCsvFilePath, outFilePath);
        
        var yearsToKeep = new List<int> { 1999 };
        var filter = new CsvYearsFilter(yearsToKeep);
        
        //act
        filter.Filter(outFilePath);
        
        //assert
        var originalHash = GetFileHash(correctFilePath);
        var copiedHash = GetFileHash(outFilePath);

        Assert.That(originalHash, Is.EqualTo(copiedHash));
        
        if (File.Exists(outFilePath))
        {
            File.Delete(outFilePath);
        }
    }
    
    [Test]
    public void TestZeroYears()
    {
        //arrange
        const string outFilePath = @"..\..\..\TestData\zero years [test].csv";
        const string correctFilePath = @"..\..\..\TestData\zero years.csv";
        File.Copy(SourceCsvFilePath, outFilePath);
        
        var yearsToKeep = new List<int>();
        var filter = new CsvYearsFilter(yearsToKeep);
        
        //act
        filter.Filter(outFilePath);
        
        //assert
        var originalHash = GetFileHash(correctFilePath);
        var copiedHash = GetFileHash(outFilePath);

        Assert.That(originalHash, Is.EqualTo(copiedHash));
        
        if (File.Exists(outFilePath))
        {
            File.Delete(outFilePath);
        }
    }

    private string GetFileHash(string filename)
    {
        var hash = new SHA1Managed();
        var clearBytes = File.ReadAllBytes(filename);
        var hashedBytes = hash.ComputeHash(clearBytes);
        return ConvertBytesToHex(hashedBytes);
    }

    private string ConvertBytesToHex(byte[] bytes)
    {
        var sb = new StringBuilder();

        foreach (var t in bytes)
        {
            sb.Append(t.ToString("x"));
        }
        return sb.ToString();
    }
}