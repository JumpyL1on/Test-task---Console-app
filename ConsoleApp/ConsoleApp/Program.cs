using ConsoleApp.JsonConverters;
using ConsoleApp.Services;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

var httpClient = new HttpClient()
{
    BaseAddress = new Uri("https://keysystems.ru/files/misc/tasks/")
};

var converter = new DataSetConverter(
    new DataTableCollectionConverter(
        new DataTableConverter(
            new DataColumnCollectionConverter(),
            new DataRowCollectionConverter(
                new DataRowConverter()))));

var service = new DataSetService(httpClient, converter);

var docs = await service.Foo();

for (var i = 0; i < 2; i++)
{
    var j = i + 1;

    service.SaveAsHTMLFile(docs[i], $"doc{j}.html");
}