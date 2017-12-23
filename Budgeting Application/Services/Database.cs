using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Budgeting_Application.Services
{
    public class Database
    {
        private const string ExpectedFileName = "expected.csv";
    
        public List<ExpectedDTO> ReadDatabase()
        {
            var streamreader = new StreamReader(ExpectedFileName);
            var reader = new CsvReader(streamreader);
            return reader.GetRecords<ExpectedDTO>().ToList();
        }

        public void WriteDatabase(List<ExpectedDTO> rows)
        {
            var streamWriter = new StreamWriter(ExpectedFileName);
            var writer = new CsvWriter(streamWriter);
            writer.WriteRecords(rows);
        }
    }
}
