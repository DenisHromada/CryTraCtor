using System.Globalization;
using CryTraCtor.Business.Models.KnownDomain;
using CsvHelper;

namespace CryTraCtor.Business.Services;

public class CsvService
{
    public IEnumerable<KnownDomainImportModel> ParseCsv(Stream stream)
    {
        using (var reader = new StreamReader(stream))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<KnownDomainImportModel>();
            
            foreach (var record in records)
            {
                yield return record;
            }
        }
    }
}