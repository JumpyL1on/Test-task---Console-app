using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.Services
{
    internal class DataSetService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonConverter<DataSet> _dataSetConverter;

        public DataSetService(HttpClient httpClient, JsonConverter<DataSet> dataSetConverter)
        {
            _httpClient = httpClient;
            _dataSetConverter = dataSetConverter;
        }

        public async Task<(DataSet doc1, DataSet doc2)> FormFromIncomplete(DataSet doc1, DataSet doc2)
        {
            for (var i = 0; i < doc1.Tables.Count; i++)
            {
                var table1 = doc1.Tables[i];

                table1.AddRowIfThereIsNo();

                var table2 = doc2.Tables[i];

                table2.AddRowIfThereIsNo();

                var columns = table1.Columns.Count >= table2.Columns.Count
                    ? table1.Columns.ToArray<DataColumn>()
                    : table2.Columns.ToArray<DataColumn>();

                for (var j = 0; j < columns.Length; j++)
                {
                    if (j == table1.Columns.Count)
                    {
                        table1.AddColumnAndCellToTableFromAnotherTable(columns[j], j, table2.Rows[0].ItemArray[j]);
                    }
                    else if (j == table2.Columns.Count)
                    {
                        table2.AddColumnAndCellToTableFromAnotherTable(columns[j], j, table1.Rows[0].ItemArray[j]);
                    }
                    else
                    {
                        var column1 = table1.Columns[j].ColumnName;
                        var column2 = table2.Columns[j].ColumnName;

                        if (column1 != column2)
                        {
                            if (column1 == columns[j].ColumnName)
                            {
                                table2.AddColumnAndCellToTableFromAnotherTable(columns[j], j, table1.Rows[0].ItemArray[j]);
                            }
                            else
                            {
                                table1.AddColumnAndCellToTableFromAnotherTable(columns[j], j, table2.Rows[0].ItemArray[j]);
                            }
                        }
                    }
                }
            }

            return (doc1, doc2);
        }

        public async Task<DataSet?> GetFromUrlAsync(string fileName)
        {
            var json = await _httpClient.GetStringAsync(fileName);

            return JsonConvert.DeserializeObject<DataSet>(json, _dataSetConverter);
        }
    }
}