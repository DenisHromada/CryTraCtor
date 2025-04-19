using System.Globalization;
using CryTraCtor.Business.Models.KnownDomain;
using CsvHelper;

namespace CryTraCtor.Business.Services;

public class CsvService
{
    public async IAsyncEnumerable<KnownDomainImportModel> ParseCsvAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        await foreach (var record in csv.GetRecordsAsync<KnownDomainImportModel>())
        {
            yield return record;
        }
    }
}
