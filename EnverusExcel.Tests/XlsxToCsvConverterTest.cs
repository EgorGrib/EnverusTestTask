using System.Security.Cryptography;
using System.Text;

namespace EnverusExcel.Tests;

public class XlsxToCsvConverterTest
{
    private const string SourceExcelFilePath = @"..\..\..\TestData\source.xlsx";
    
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void TestConversion()
    {
        //arrange
        const string outFilePath = @"..\..\..\TestData\converted [test].csv";
        const string correctFilePath = @"..\..\..\TestData\correct csv.csv";
        File.Copy(SourceExcelFilePath, outFilePath);

        //act
        XlsxToCsvConverter.Convert(SourceExcelFilePath, outFilePath);
        
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