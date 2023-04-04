using CSharpCodeGenerator.Controller;
using CSharpCodeGenerator.Model;
using CSharpCodeGenerator.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static CSharpCodeGenerator.Model.Graph;

namespace CSharpCodeGenerator.Models
{
    public class Matrix
    {
        public int numberOfClasses = 2;
        public int[,] matrix;

        public Graph graph = new Graph();

        public Dictionary<string, string> links = new Dictionary<string, string>();

        public void CheckValue(EnterMatrix Form, ControllerClass controller)
        {
            links.Clear();
            graph.V.Clear();
            graph.E.Clear();

            for (int i = 0; i < numberOfClasses; i++)
            {
                FilleTheMatrix(i, Form);
            }
            for (int i = 0; i < numberOfClasses - 1; i++)
            {
                DataGridRow row = (DataGridRow)Form.MatrixDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                if (row == null)                                                                   // чтобы избавиться от ошибки, когда мат-ца больше 13на13
                {
                    Form.MatrixDataGrid.ScrollIntoView(Form.MatrixDataGrid.Items[i]);
                    Updates.WaitFor(TimeSpan.Zero, DispatcherPriority.SystemIdle);

                    row = (DataGridRow)Form.MatrixDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                }
                for (int j = 1; j < numberOfClasses + 1; j++)
                {
                    try
                    {
                        TextBlock cellContent = Form.MatrixDataGrid.Columns[j].GetCellContent(row) as TextBlock;
                        if (Convert.ToInt32(cellContent.Text) > 1 || Convert.ToInt32(cellContent.Text) < 0)
                        {
                            matrix[i, j - 1] = Convert.ToInt32(cellContent.Text);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Неправильное значение элемента матрицы!");
                        break;

                    }
                }
            }
            for(int i = 0; i < numberOfClasses; i++)
            {
                int x = (int)Math.Truncate(200 * Math.Cos(i * 2 * Math.PI / 26)) + 500;
                int y = (int)Math.Truncate(200 * Math.Sin(i * 2 * Math.PI / 26)) + 220;
                graph.V.Add(new Node(new System.Drawing.Point(x, y)));
            }
            try
            {
                for (int i = 0; i < links.Count; i++)
                {
                    TypeOfAgregation typeOfAgregation = new TypeOfAgregation(links.ElementAt(i), controller);
                    typeOfAgregation.ShowDialog();
                }
                MakeGraphFromMatrix();
                CodeMaker.MakeCode(links, Form);
            }
            catch { }
        }
        public void MakeGraphFromMatrix()
        {
            for (int i = 0; i < numberOfClasses; i++)
            {
                for (int j = 0; j < numberOfClasses; j++)
                {
                    if ((matrix[i, j] == 1 || matrix[i, j] == 0) && matrix[i, i] == 0)
                    {
                        if (matrix[i, j] == 1 && !graph.E.Contains(new Edge(graph.V[i], graph.V[j])))
                        {
                            graph.E.Add(new Edge(graph.V[i], graph.V[j]));
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
            }
        }

        public void FilleTheMatrix(int idx, EnterMatrix Form)
        {
            DataGridRow row = (DataGridRow)Form.MatrixDataGrid.ItemContainerGenerator.ContainerFromIndex(idx);
            if (row == null)
            {
                Form.MatrixDataGrid.ScrollIntoView(Form.MatrixDataGrid.Items[idx]);
                Updates.WaitFor(TimeSpan.Zero, DispatcherPriority.SystemIdle);

                row = (DataGridRow)Form.MatrixDataGrid.ItemContainerGenerator.ContainerFromIndex(idx);
            }
            for (int index = 1; index < numberOfClasses + 1; index++)
            {
                TextBlock cellContent = Form.MatrixDataGrid.Columns[index].GetCellContent(row) as TextBlock;
                int firstClass = idx + 1;
                int secondClass = index;
                string linkName = "From_" + firstClass + "_to_" + secondClass;
                if (Convert.ToInt32(cellContent.Text) == 1)
                {
                    links.Add(linkName, "");
                    matrix[idx, index - 1] = Convert.ToInt32(cellContent.Text);
                }
            }
        }
    }
}
