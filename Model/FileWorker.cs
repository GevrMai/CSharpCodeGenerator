using CSharpCodeGenerator.Models;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace CSharpCodeGenerator.Model
{
    internal class FileWorker
    {
        public static bool LoadListOfFiles(EnterMatrix Form)
        {
            try
            {
                List<string> files = new List<string>();
                DirectoryInfo place = new DirectoryInfo(@"D:\prog\CSharpCodeGenerator\matrices");
                FileInfo[] Files = place.GetFiles("*.txt");
                foreach (var file in Files)
                {
                    files.Add(file.Name);
                    Console.WriteLine("File Name - {0}", file.Name);
                }
                Form.listWithMatrices.ItemsSource = files;
                return true;
            }
            catch (IOException)
            {
                if(EnterMatrix.directoryWithMatricesExists)
                {
                    System.Windows.MessageBox.Show("Не удалось загрузить файлы с матрицами", "Ошибка");
                }
                return false;
            }
            finally
            {
                ///
            }
            
        }

        public static void Translation(string code)
        {
            CSharpCodeProvider csc = new CSharpCodeProvider(new Dictionary<string, string>()
                                            { {"CompillerVersion", "v4.0" } });
            CompilerParameters parameters = new CompilerParameters(new[]
                                        { "mscorlib.dll", "System.Core.dll" }, "test.exe", true);
            parameters.GenerateExecutable = true;
            CompilerResults result = csc.CompileAssemblyFromSource(parameters, code);
            if (result.Errors.HasErrors)
            {
                System.Windows.MessageBox.Show("Не удалось скомпилировать код");
            }
            else
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\test.exe";
                Process.Start(path);
            }
        }
        public static bool SaveCSharpCode(string code)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, code);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void SaveMatrix(Matrix matrix)
        {
            try
            {
                using (TextWriter tw = new StreamWriter(@"D:\prog\CSharpCodeGenerator\matrices\SavedMatrix.txt"))
                {
                    for (int i = 0; i < matrix.numberOfClasses; i++)
                    {
                        for (int j = 0; j < matrix.numberOfClasses; j++)
                        {
                            tw.Write(matrix.matrix[i, j] + " ");
                        }
                        tw.WriteLine();
                    }
                }
                System.Windows.MessageBox.Show("Сохраненная матрица лежит в директории D:\\prog\\CSharpCodeGenerator\\matrices\\SavedMatrix.txt",
                "Информация");
            }
            catch
            {
                System.Windows.MessageBox.Show("Не удалось записать файл!", "Ошибка");
            }


        }
    }
}
