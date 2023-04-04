using System.Windows;
using System.Windows.Media;

namespace CSharpCodeGenerator.Controller
{
    internal class FormSettings
    {
        public static SolidColorBrush chosenColor = new SolidColorBrush(Colors.LightGray);
        public static FontFamily fontFamily = new FontFamily("Century Gothic");
        public static FontStyle fontStyle = FontStyles.Normal;
        public static FontWeight fontWeight = FontWeights.Bold;
        public static string defaultCode = "using System; \nnamespace CSharpCodeGenerator\n{\n\tclass Program\n\t{\n\t\tstatic void Main()" +
            "\n\t\t{\n\t\t\tConsole.WriteLine(\"Программа скомпилирована!\");\n\t\t\tConsole.ReadKey();\n\t\t}\n\t}\n}";

        public static void ApplySettings<T>(T Form)
            where T : Window
        {
            Form.Background = chosenColor;
            Form.FontFamily = fontFamily;
            Form.FontStyle = fontStyle;
            Form.FontWeight = fontWeight;
        }
    }
}
