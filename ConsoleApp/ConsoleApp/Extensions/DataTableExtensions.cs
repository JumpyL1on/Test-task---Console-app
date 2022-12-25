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

        public static void AddColumnAndCellFromAnotherTable(
            this DataTable table,
            DataColumn column,
            int columnIndex,
            object? value)
        {
            table.Columns
                .Add(column.ColumnName, column.DataType)
                .SetOrdinal(columnIndex);

            table.Rows[0].SetField(columnIndex, value);
        }
    }
}