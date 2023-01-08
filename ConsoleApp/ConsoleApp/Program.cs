using ConsoleApp.JsonConverters;
using ConsoleApp.Services;
using Newtonsoft.Json;
using System.Data;

var httpClient = new HttpClient()
{
    BaseAddress = new Uri("https://keysystems.ru/files/misc/tasks/")
};

var dataSetService = new DataSetService(httpClient, CreateDataSetConverter());

var doc1 = await dataSetService.GetFromUrlAsync("doc1.json");

var doc2 = await dataSetService.GetFromUrlAsync("doc2.json");

(var formedDoc1, var formedDoc2) = DataSetService.FormFromIncomplete(doc1, doc2);

HTMLService.SaveAsHTMLFile(formedDoc1, $"{nameof(doc1)}.html");

HTMLService.SaveAsHTMLFile(formedDoc2, $"{nameof(doc2)}.html");

Console.Write("Press any key to close this window . . .");

Console.ReadKey();

static JsonConverter<DataSet> CreateDataSetConverter()
{
    var dataColumnCollectionConverter = new DataColumnCollectionConverter();

    var dataRowConverter = new DataRowConverter();

    var dataRowCollectionConverter = new DataRowCollectionConverter(dataRowConverter);

    var dataTableConverter = new DataTableConverter(dataColumnCollectionConverter, dataRowCollectionConverter);

    var dataTableCollectionConverter = new DataTableCollectionConverter(dataTableConverter);

    var dataSetConverter = new DataSetConverter(dataTableCollectionConverter);

    return dataSetConverter;
}