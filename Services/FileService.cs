using CsvHelper;
using CsvHelper.Configuration;
using envio_correos_batch.Model;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace envio_correos_batch.Services
{
    public class FileService
    {

        private readonly static string CONFIG_FILE = @"configuration.ecb";

        public static void WriteConfigurationFile(DataModel dataModel)
        {
            string jsonContent = JsonConvert.SerializeObject(dataModel);
            File.WriteAllText(CONFIG_FILE, jsonContent);
        }

        public static DataModel? ReadConfigurationFile()
        {
            try
            {
                var jsonContent = File.ReadAllText(CONFIG_FILE);
                return JsonConvert.DeserializeObject<DataModel>(jsonContent);
            }
            catch (Exception)
            {
                return null;
            }

        }

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
