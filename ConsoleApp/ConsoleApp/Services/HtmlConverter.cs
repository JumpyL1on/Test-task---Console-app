using Aspose.Html;
using ConsoleApp.Extensions;
using System.Data;

namespace ConsoleApp.Services
{
    internal static class HtmlConverter
    {
        public static void WriteHTML(DataSet ds, string name)
        {
            using var document = new HTMLDocument();

            var path = Path.Combine(Directory.GetCurrentDirectory(), name);

            foreach (var e in ds.Tables.ToArray<DataTable>())
            {
                var div = (HTMLDivElement)document.CreateElement("div");

                var table = (HTMLTableElement)document.CreateElement("table");

                table.Border = "1";

                var caption = (HTMLTableCaptionElement)table.CreateCaption();

                caption.TextContent = e.TableName;

                var tHead = (HTMLTableSectionElement)table.CreateTHead();

                var header = (HTMLTableRowElement)tHead.InsertRow(0);

                for (var i = 0; i < e.Columns.Count; i++)
                {
                    var cell = (HTMLTableCellElement)header.InsertCell(i);

                    cell.TextContent = e.Columns[i].ColumnName;
                }

                for (var i = 0; i < e.Rows.Count; i++)
                {
                    var row = (HTMLTableRowElement)table.InsertRow(1);

                    for (var j = 0; j < e.Rows[i].ItemArray.Length; j++)
                    {
                        var cell = (HTMLTableCellElement)row.InsertCell(j);

                        cell.TextContent = e.Rows[i].ItemArray[j].ToString();
                    }
                }

                div.AppendChild(table);

                document.Body.AppendChild(div);

                document.Body.AppendChild(document.CreateElement("br"));
            }

            document.Save(path);
        }
    }
}