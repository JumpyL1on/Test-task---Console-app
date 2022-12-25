using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.Services
{
    internal class DataSetService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonConverter _dataSetConverter;

        public DataSetService(HttpClient httpClient, JsonConverter dataSetConverter)
        {
            _httpClient = httpClient;
            _dataSetConverter = dataSetConverter;
        }

        public async Task<DataSet[]> Foo()
        {
            var doc1 = await GetFromUrlAsync("doc1.json");

            var doc2 = await GetFromUrlAsync("doc2.json");

            for (var i = 0; i < doc1.Tables.Count; i++)
            {
                var table1 = doc1.Tables[i];

                var table2 = doc2.Tables[i];

                var columns = table1.Columns.Count >= table2.Columns.Count
                    ? table1.Columns.ToArray<DataColumn>()
                    : table2.Columns.ToArray<DataColumn>();

                var row1 = new List<object>();

                var row2 = new List<object>();

                for (var j = 0; j < columns.Length; j++)
                {
                    if (j == table2.Columns.Count)
                    {
                        table2.Columns.Add(new DataColumn(columns[j].ColumnName, columns[j].DataType));

                        row1.Add(table1.Rows[0].ItemArray[j]);

                        row2.Add(table1.Rows[0].ItemArray[j]);
                    }
                    else if (j == table1.Columns.Count)
                    {
                        table1.Columns.Add(new DataColumn(columns[j].ColumnName, columns[j].DataType));

                        row1.Add(table2.Rows[0].ItemArray[j]);

                        row2.Add(table2.Rows[0].ItemArray[j]);
                    }
                    else
                    {
                        var column1 = table1.Columns[j].ColumnName;
                        var column2 = table2.Columns[j].ColumnName;

                        if (column1 != column2)
                        {
                            if (column1 == columns[j].ColumnName)
                            {
                                var column = table2.Columns.Add(columns[j].ColumnName, columns[j].DataType);

                                column.SetOrdinal(j);

                                table2.Columns.Add(column);

                                row1.Add(table1.Rows[0].ItemArray[j]);

                                row2.Add(table1.Rows[0].ItemArray[j]);
                            }
                            else
                            {
                                var column = table1.Columns.Add(columns[j].ColumnName, columns[j].DataType);

                                column.SetOrdinal(j);

                                row1.Add(table2.Rows[0].ItemArray[j]);

                                row2.Add(table2.Rows[0].ItemArray[j]);
                            }
                        }
                        else
                        {
                            row1.Add(table1.Rows[0].ItemArray[j]);
                            row2.Add(table2.Rows[0].ItemArray[j]);
                        }
                    }
                }

                table1.Rows[0].ItemArray = row1.ToArray();

                if (table2.Rows.Count == 0)
                {
                    table2.Rows.Add(row2.ToArray());
                }
                else
                {
                    table2.Rows[0].ItemArray = row2.ToArray();
                }
            }

            return new[] { doc1, doc2 };
        }

        public void SaveAsHTMLFile(DataSet ds, string fileName)
        {
            HtmlConverter.WriteHTML(ds, fileName);
        }

        private async Task<DataSet?> GetFromUrlAsync(string name)
        {
            var json = await _httpClient.GetStringAsync(name);

            return JsonConvert.DeserializeObject<DataSet>(json, _dataSetConverter);
        }
    }
}