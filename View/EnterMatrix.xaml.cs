using CSharpCodeGenerator.Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace CSharpCodeGenerator
{
    public partial class EnterMatrix : Window
    {
        private DispatcherTimer timer;
        private int ticks;
        private ControllerClass controller;
        public static bool directoryWithMatricesExists = true;
        public EnterMatrix(ControllerClass controller)
        {
            SetProcessDPIAware();

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            ticks = 1;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            InitializeComponent();
            this.controller = controller;

            controller.CheckClassesNumber(2.ToString(), this, false);
            controller.GetFilesWithMatrices(this);
            controller.ApplyFormSettings(this);

            Bitmap bMap = new Bitmap(925, 400);
            Graphics gr = Graphics.FromImage(bMap);
            gr.Clear(Color.White);
            Graphics g = Graphics.FromImage(bMap);
            g.Clear(Color.White);
            Pen roundPen = new Pen(Color.Blue);
            roundPen.Width = 3;
            Pen darkGoldPen = new Pen(Color.Pink, 3); // цвет прямой и дуг
            gr.FillEllipse(Brushes.LightBlue, 15, 15, 10, 10); //коорд радиуса точек
            gr.DrawEllipse(roundPen, 15, 15, 10, 10);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ticks += 1;
            if (ticks == 600)
            {
                MessageBox.Show("Программа сейчас закроется", "Истечение времени", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
            ProgramCloseTimer.Value += 1;
            directoryWithMatricesExists = controller.GetFilesWithMatrices(this);
        }

        
        private void classeNumberSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.ChangeValue(classeNumberSlider.Value, this);
        }

        private void classeNumberSlider_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            if (e.Key == Key.Enter)
            {
                controller.CheckClassesNumber(classesNumberLabel.Text, this, false);
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            MessageBox.Show(
                "1. Порядок агрегации определяет прохождением по строкам\n" +
                "2. Число '1' значит, что между классами имеется отношение агрегации\n" +
                "3. Число '0' значит, что агрегации между классами нет\n" +
                "4. В ячейках допустимы только целочисленные значения от 0 до 1\n" +
                "5. Вы можете перетащить .txt файл, содержищий матрицу на таблицу и она будет изменена\n" +
                "6. Вы можете скопировать матрицу из текста и,нажав Ctrv + V, изменить таблицу с матрицей",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            Application.Current.Shutdown();
        }

        private void ChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.ChangeBackgroundColor();
            Background = FormSettings.chosenColor;
        }

        private void ChangeFont_Click(object sender, RoutedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.FontSettingDialog(this);
        }

        private void GetExistingCode_Click(object sender, RoutedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.ViewExistingCode(this);
        }

        private void saveCodeBTN_Click(object sender, RoutedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.SaveCSharpCode(CodeViewer.Text, this);
        }

        private void listWithMatrices_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.LoadMatrixFromFile(this);
        }

        private void CodeViewer_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            int row = CodeViewer.GetLineIndexFromCharacterIndex(CodeViewer.CaretIndex);
            int col = CodeViewer.CaretIndex - CodeViewer.GetCharacterIndexFromLineIndex(row);
            lblCursorPosition.Text = "Line " + (row + 1) + ", Char " + (col + 1);
        }

        private void MatrixDataGrid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.DragDrop(this, e);
        }

        private void MatrixDataGrid_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            e.Handled = true;
        }

        private void SaveMatrix_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveMatrix();
        }

        private void enterValuesBTN_Click(object sender, RoutedEventArgs e)
        {
            CodeViewer.Text = "";
            ticks = 0;
            ProgramCloseTimer.Value = 0;
            controller.EnterValues(this);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                controller.LoadMatrixFromFile(this, "test");
            }
            catch
            {
                MessageBox.Show("В директории D:\\prog\\CSharpCodeGenerator\\matrices не было файла SavedMatrix.txt",
                    "Ошибка",
                    (MessageBoxButton)MessageBoxImage.Error);
            }
            
        }
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ticks = 0;
            ProgramCloseTimer.Value = 0;

            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                try
                {
                    controller.GetMatrixFromClipBoard(this);
                }
                catch
                {
                    MessageBox.Show("Не удалось загрузить матрицу из буфера обмена. Привер вводимого значения:\n" +
                        "0 1 1\n1 0 0\n1 0 0",
                        "Ошибка",
                        (MessageBoxButton)MessageBoxIcon.Error);
                }
                
            }
            
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
