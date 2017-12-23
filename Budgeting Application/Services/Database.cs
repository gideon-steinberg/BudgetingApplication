using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Budgeting_Application.Services
{
    public class Database
    {
        private const string ExpectedFileName = "expected.csv";
    
        public IEnumerable<ExpectedDTO> ReadDatabase()
        {
            var streamreader = new StreamReader(ExpectedFileName);
            var reader = new CsvReader(streamreader);
            return reader.GetRecords<ExpectedDTO>();
        }

        public void WriteDatabase(IEnumerable<ExpectedDTO> rows)
        {
            var streamWriter = new StreamWriter(ExpectedFileName);
            var writer = new CsvWriter(streamWriter);
            writer.WriteRecords(rows);
        }
    }
}
