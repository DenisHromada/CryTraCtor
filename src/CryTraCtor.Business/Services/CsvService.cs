using System.Globalization;
using CryTraCtor.Business.Models.KnownDomain;
using CsvHelper;
using Microsoft.AspNetCore.Http;

namespace CryTraCtor.Business.Services;

public class CsvService
{
    public IEnumerable<KnownDomainImportModel> ParseCsv(IFormFile file)
    {
        using (var reader = new StreamReader(file.OpenReadStream()))
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