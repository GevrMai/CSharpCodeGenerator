using System;
using System.Data;
using System.Windows.Controls;

namespace CSharpCodeGenerator.Models
{
    internal static class MakeDataGrid                             // создание таблицы для заданного количество классов
    {
        public static char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                            'O', 'P', 'Q', 'R','S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static void MakeDataGridTable(DataGrid dataGrid, Matrix Matrix)
        {
            DataTable dt = new DataTable();

            DataColumn column = new DataColumn();
            DataRow row;
            DataView view;
            column.DataType = Type.GetType("System.Char");
            column.ColumnName = "-";
            dt.Columns.Add(column);

            column.DataType = Type.GetType("System.Char");
            for (int i = 0; i < Matrix.numberOfClasses; i++)
            {
                column = new DataColumn();
                column.ColumnName = MakeDataGrid.letters[i].ToString();
                dt.Columns.Add(column);
            }

            for (int i = 0; i < Matrix.numberOfClasses; i++)
            {
                row = dt.NewRow();
                row["-"] = MakeDataGrid.letters[i];
                for (int j = 1; j < Matrix.numberOfClasses + 1; j++)
                {
                    try
                    {
                        row[j] = Matrix.matrix[i, j - 1];
                    }
                    catch
                    {
                        row[j] = '0';
                    }

                }

                dt.Rows.Add(row);
            }

            view = new DataView(dt);
            dataGrid.ItemsSource = view;
        }
    }
}
