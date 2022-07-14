using CsvHelper;
using CsvHelper.Configuration;
using envio_correos_batch.Model;
using System.Globalization;

namespace envio_correos_batch.Services
{
    public class FileService
    {

        public static bool IsValidCsvFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, GetConfiguration());
            return csv.Read();
        }
        public static IEnumerable<CsvData<SendMailRow>> ReadRowsFromFile(string filePath)
        {
            var i = 0;
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, GetConfiguration());
            return csv.GetRecords<SendMailRow>()
                .Select(x => new CsvData<SendMailRow>(x, ++i))
                .ToList();
        }

        private static CsvConfiguration GetConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Comment = '%',
                HasHeaderRecord = true
            };
        }

    }
}
