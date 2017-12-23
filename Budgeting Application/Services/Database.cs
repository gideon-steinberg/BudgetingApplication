using Budgeting_Application.DataTypes;
using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Budgeting_Application.Services
{
    public class Database
    {
        private const string ExpectedFileName = "expected.csv";
    
        public List<ExpectedDTO> ReadExpectedValuesFromDatabase()
        {
            var streamreader = new StreamReader(ExpectedFileName);
            var reader = new CsvReader(streamreader);
            return reader.GetRecords<ExpectedDTO>().ToList();
        }

        public void WriteExpectedValuesToDatabase(List<ExpectedDTO> rows)
        {
            var streamWriter = new StreamWriter(ExpectedFileName);
            var writer = new CsvWriter(streamWriter);
            writer.WriteRecords(rows);
        }
    }
}
