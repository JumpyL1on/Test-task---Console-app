using Aspose.Html;
using ConsoleApp.Extensions;
using System.Data;

namespace ConsoleApp.Services
{
    internal class HTMLService
    {
        public static void SaveAsHTMLFile(DataSet ds, string fileName)
        {
            using var document = new HTMLDocument();

            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            foreach (var dataTable in ds.Tables.OfType<DataTable>())
            {
                var div = (HTMLDivElement)document.CreateElement("div");

                var table = CreateTable(document, dataTable);

                div.AppendChild(table);

                document.Body.AppendChild(div);

                document.Body.AppendChild(document.CreateElement("br"));
            }

            document.Save(path);
        }

        private static HTMLTableElement CreateTable(HTMLDocument document, DataTable dataTable)
        {
            var table = (HTMLTableElement)document.CreateElement("table");

            table.Border = "1";

            AddCaptionToTable(table, dataTable.TableName);

            AddHeaderToTable(table, dataTable);

            AddRowsToTable(table, dataTable.Rows);

            return table;
        }

        private static void AddCaptionToTable(HTMLTableElement table, string tableName)
        {
            var caption = (HTMLTableCaptionElement)table.CreateCaption();

            caption.TextContent = tableName;
        }

        private static void AddHeaderToTable(HTMLTableElement table, DataTable dataTable)
        {
            var tHead = (HTMLTableSectionElement)table.CreateTHead();

            var header = (HTMLTableRowElement)tHead.InsertRow(0);

            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                var cell = (HTMLTableCellElement)header.InsertCell(i);

                cell.TextContent = dataTable.Columns[i].ColumnName;
            }
        }

        private static void AddRowsToTable(HTMLTableElement table, DataRowCollection rows)
        {
            for (var i = 0; i < rows.Count; i++)
            {
                var row = (HTMLTableRowElement)table.InsertRow(i + 1);

                for (var j = 0; j < rows[i].ItemArray.Length; j++)
                {
                    var cell = (HTMLTableCellElement)row.InsertCell(j);

                    cell.TextContent = rows[i].ItemArray[j].ToString();
                }
            }
        }
    }
}