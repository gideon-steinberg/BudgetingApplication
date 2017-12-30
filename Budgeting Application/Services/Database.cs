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
            using (var streamreader = new StreamReader(ExpectedFileName))
                using (var reader = new CsvReader(streamreader))
                    return reader.GetRecords<ExpectedDTO>().ToList();
        }

        public void WriteExpectedValuesToDatabase(List<ExpectedDTO> rows)
        {
            using (var streamWriter = new StreamWriter(ExpectedFileName))
                using(var writer = new CsvWriter(streamWriter))
                    writer.WriteRecords(rows);
        }
    }
}
