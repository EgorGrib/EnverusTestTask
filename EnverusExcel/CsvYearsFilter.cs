namespace EnverusExcel;

public class CsvYearsFilter : ICsvFilter
{
    private readonly List<int> _yearsToKeep;
    
    public CsvYearsFilter(List<int> yearsToKeep)
    {
        _yearsToKeep = yearsToKeep;
    }
    
    public void Filter(string path)
    {
        var filteredLines = new List<string>();
        using (var reader = new StreamReader(path))
        {
            var isInsideDesiredYears = false;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(",20") || line.StartsWith(",19"))
                {
                    var year = int.Parse(line.Substring(1, 4));
                    isInsideDesiredYears = _yearsToKeep.Contains(year);
                }

                if (isInsideDesiredYears)
                {
                    filteredLines.Add(line);
                }
            }
        }
        File.WriteAllLines(path, filteredLines);
    }
}