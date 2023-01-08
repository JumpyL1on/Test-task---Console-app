using System.Data;

namespace ConsoleApp.Extensions
{
    internal static class DataTableExtensions
    {
        public static void AddRowIfThereIsNo(this DataTable table)
        {
            if (table.Rows.Count == 0)
            {
                table.Rows.Add();
            }
        }

        public static void AddColumnFromAnotherTable(this DataTable table, DataColumn column)
        {
            table.Columns
                .Add(column.ColumnName, column.DataType)
                .SetOrdinal(column.Ordinal);
        }

        public static void AddCellFromAnotherTable(this DataTable table, int rowIndex, int columnIndex, object? value)
        {
            table.Rows[rowIndex].SetField(columnIndex, value);
        }
    }
}