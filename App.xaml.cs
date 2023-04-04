using System;
using System.Windows;

namespace CSharpCodeGenerator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            
            base.OnStartup(e);
            EnterMatrix matrix = new EnterMatrix(new Controller.ControllerClass(new Models.Matrix()));
            matrix.Show();
        }

        
    }
}
