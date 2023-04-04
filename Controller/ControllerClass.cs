using CSharpCodeGenerator.Model;
using CSharpCodeGenerator.Models;
using Haley.Services;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CSharpCodeGenerator.Controller
{
    public class ControllerClass
    {
        public Matrix matrix;
        public Matrix Matrix_pr { get { return matrix; } set { matrix = value; } }
        public ControllerClass(Matrix matrix)
        {
            Matrix_pr = matrix;
        }
        public void CheckClassesNumber(string numberOfClasses, EnterMatrix Form, bool fromFile)    // в форме EnterMatrix через slider вводится кол-во классов
        {
            if (Check.CheckClassesNumber(Matrix_pr, numberOfClasses, fromFile))
            {
                MakeDataGrid.MakeDataGridTable(Form.MatrixDataGrid, Matrix_pr);
            }
            Form.GraphPictureBox.Source = null;
        }
        public void ChangeValue(double value, EnterMatrix Form) // изменение числа кол-во классов у эл-та slider в EnterMatrix
        {
            Form.classesNumberLabel.Text = Convert.ToInt32(value).ToString();
        }

        public void ViewExistingCode(EnterMatrix Form)           // переход к CodeViewer от MainWindow(с открытием окна выбора файла)
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = false,
                Filter = "C# Code|*.cs",
                DefaultExt = "*.cs"
            };
            Nullable<bool> dialogOk = openFile.ShowDialog();
            if (dialogOk == true)
            {
                StreamReader sr = new StreamReader(openFile.FileName);
                string code = sr.ReadToEnd();
                sr.Dispose();
                Form.CodeViewer.Text = code;
            }
        }

        public void Translation(string code)    // компиляция кода
        {
            FileWorker.Translation(code);
        }

        public void SaveCSharpCode(string code, EnterMatrix Form)     // в CodeViewer сохранение C# кода
        {
            bool saved = FileWorker.SaveCSharpCode(code);
            if (saved && Form.TranslateCB.IsChecked == true) { Translation(Form.CodeViewer.Text); }
        }
        public void ChangeBackgroundColor()         // в MainWindow открытие ColorDialog и изменение цвета background всех форм
        {
            var colorPicker = new ColorPickerDialog();
            colorPicker.ShowDialog();
            FormSettings.chosenColor = colorPicker.SelectedBrush;
        }
        public void ApplyFormSettings<T>(T Form)      // конструктор формы вызывается эту функцию. здесь задаются background, fontfamily и тп
            where T : Window
        {
            FormSettings.ApplySettings(Form);
        }
        public void FontSettingDialog(EnterMatrix Form)      // вызов в MainWindow FontDialog/сохранение настроек
        {
            FontDialog fd = new FontDialog
            {
                MaxSize = 8
            };
            DialogResult dr = fd.ShowDialog();
            if (dr != DialogResult.Cancel)
            {
                FormSettings.fontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                FormSettings.fontWeight = fd.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                FormSettings.fontStyle = fd.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                FormSettings.ApplySettings(Form);
            }
        }
        public bool GetFilesWithMatrices(EnterMatrix Form)
        {
            bool directoryWithMatricesExists = FileWorker.LoadListOfFiles(Form);
            return directoryWithMatricesExists;
        }

        public void DragDrop(EnterMatrix Form, System.Windows.DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
                {
                    string[] files = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
                    if (files != null && files.Length > 0)
                    {
                        LoadMatrixFromFile(Form, files[0]);
                        Form.lblCursorPosition.Text = "Матрица загружена!";
                    }
                }
            }
            catch
            {
                MessageBox.Show(
                "Произошла ошибка с загружаемой матрицей!",
                "Информация",
                (MessageBoxButtons)MessageBoxButton.OK,
                (MessageBoxIcon)MessageBoxImage.Error);
            }
        }

        public void LoadMatrixFromFile(EnterMatrix Form, string fileFrom = null)
        {
            string nameFile;
            if(fileFrom == null)                                        // дефолтное значение. Выбирается из ListView
            {
                nameFile = @"D:\prog\CSharpCodeGenerator\matrices\" + Form.listWithMatrices.SelectedItem.ToString();
            }
            else if(fileFrom == "test")                                 // тестовый пример
            {
                nameFile = @"D:\prog\CSharpCodeGenerator\matrices\SavedMatrix.txt";
            }
            else                                                        // Drag & Drop
            {
                nameFile = fileFrom;
            }
            try
            {
                
                string[] str = File.ReadAllLines(nameFile);
                Matrix_pr.numberOfClasses = str.Length;
                Matrix_pr.matrix = new int[Matrix_pr.numberOfClasses, Matrix_pr.numberOfClasses];
                for (int i = 0; i < str.Length; i++)
                {
                    string[] numbers = str[i].Split();
                    for (int j = 0; j < str.Length; j++)
                    {
                        Matrix_pr.matrix[i, j] = Convert.ToInt32(numbers[j]);
                    }
                }
                CheckClassesNumber(Matrix_pr.numberOfClasses.ToString(), Form, true);
            }
            catch
            {
                MessageBox.Show(
                "Произошла ошибка с загружаемой матрицей!\nПроверьте правильность ввода.\nМатрица должна быть квадратной" +
                " и содержаться только числа 0, 1\nПример вводимой матрицы:\n" +
                "0 1 1\n1 0 0\n1 0 0",
                "Информация",
                (MessageBoxButtons)MessageBoxButton.OK,
                (MessageBoxIcon)MessageBoxImage.Error);
            }

        }
        public void SaveMatrix()
        {
            FileWorker.SaveMatrix(Matrix_pr);
        }

        public void EnterValues(EnterMatrix Form)
        {
            Matrix_pr.CheckValue(Form, this);
            DrawingGraph.DrawGraph(Form, Matrix_pr.graph);
        }

        public void GetMatrixFromClipBoard(EnterMatrix Form)
        {
            if (System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.Text))
            {
                string clipboardText = System.Windows.Clipboard.GetText(System.Windows.TextDataFormat.Text);
                clipboardText = clipboardText.Replace("\n", "");
                clipboardText = clipboardText.Replace("\r", "");
                clipboardText = clipboardText.Replace(" ", "");
                Matrix_pr.numberOfClasses = Convert.ToInt32(Math.Sqrt(clipboardText.Length));
                CheckClassesNumber(Convert.ToInt32(Math.Sqrt(clipboardText.Length)).ToString(), Form, false);
                int k = 0;
                for (int i = 0; i < Math.Sqrt(clipboardText.Length); i++)
                {
                    for (int j = 0; j < Math.Sqrt(clipboardText.Length); j++)
                    {
                        Matrix_pr.matrix[i, j] = Convert.ToInt32((clipboardText[k++]).ToString());
                    }
                }
                MakeDataGrid.MakeDataGridTable(Form.MatrixDataGrid, Matrix_pr);
            }
        }
    }
}
