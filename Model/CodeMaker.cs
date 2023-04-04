using CSharpCodeGenerator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace CSharpCodeGenerator.Models
{
    public class CodeMaker
    {
        public static string code;                 // строка сгенерированного кода
        public static void MakeCode(Dictionary<string,string> links, EnterMatrix Form)
        {
            foreach (var Con in links)
            {
                string KeyCon = Con.Key;
                string ValueCon = Con.Value;
                string[] SplitKeyCon = KeyCon.Split(new char[] { '_' });
                int ConClass_1 = Int32.Parse(SplitKeyCon[1]);
                int ConClass_2 = Int32.Parse(SplitKeyCon[3]);
                if (ValueCon == "Link_Aggregation")
                {
                    LinkAggeregation(ConClass_1, ConClass_2, Form);
                }
                else if (ValueCon == "Meant_Aggregation")
                {
                    ValueAggregation(ConClass_1, ConClass_2, Form);
                }
                else if (ValueCon == "Attachment_Aggregation")
                {
                    NestAggregation(ConClass_1, ConClass_2, Form);
                }
            }
            Form.CodeViewer.Text = AddBegginingAndEnd(code);
            WriteCode(code);
            FileWorker.Translation(Form.CodeViewer.Text);
        }

        private static string AddBegginingAndEnd(string codeToEdit)
        {
            string code = "";
            code += "using System; \n";
            code += "namespace CSharpCodeGenerator\n{\nclass Program\n{";
            code += codeToEdit;
            code += "\nstatic void Main()\n{Console.WriteLine(\"Программа скомпилирована!\");\nConsole.ReadKey();\n}\n}\n}";
            return code;
        }

        public static void NestAggregation(int ConClass_1, int ConClass_2, EnterMatrix Form)
        {
            code = Form.CodeViewer.Text;
            string textCon1 = string.Format("Class{0}", ConClass_1);
            int IndexOfClass1 = code.IndexOf(textCon1);
            if (IndexOfClass1 == -1)
            {
                string Class1STR = string.Format("class Class{0}\n{{\n\tpublic Class{0}()\n\t{{\n\n\t}}\n\t~Class{0}(){{}}\n\n\t}}\n", ConClass_1);
                code += Class1STR;
            }
            string ChangeSTR = string.Format("~Class{0}(){{}}\n\n", ConClass_1);
            string ChangeString = string.Format("~Class{0}(){{}}\n\n\n\tpublic сlass Class{1}\n\t{{\n\t\tpublic Class{1}()\n\t\t{{\n\n\t\t}}\n\t\t" +
                "~Class{1}(){{}}\n\n\t}}\n\tpublic Class{1} class{1}A {{ get {{ return objClass{1}; }}" +
                "}}\n\tprivate Class{1} objClass{1} = new Class{1}();\n", ConClass_1, ConClass_2);
            code = code.Replace(ChangeSTR, ChangeString);
            Form.CodeViewer.Text = code;
        }

        public static void ValueAggregation(int ConClass_1, int ConClass_2, EnterMatrix Form)
        {
            code = Form.CodeViewer.Text;

            string textCon1 = string.Format("Class{0}", ConClass_1);
            int IndexOfClass1 = code.IndexOf(textCon1);
            if (IndexOfClass1 == -1)
            {
                string Class1STR = string.Format("class Class{0}\n{{\n\tpublic Class{0}()\n\t{{\n\n\t}}\n\t~Class{0}(){{}}\n\n}}\n", ConClass_1);
                code += Class1STR;
            }
            string textCon2 = string.Format("Class{0}", ConClass_2);
            int IndexOfClass2 = code.IndexOf(textCon2);
            if (IndexOfClass2 == -1)
            {
                string Class2STR = string.Format("class Class{0}\n{{\n\tpublic Class{0}()\n\t{{\n\n\t}}\n\t~Class{0}(){{}}\n\n}}\n", ConClass_2);
                code += Class2STR;
            }
            string ChangeSTR = string.Format("~Class{0}(){{}}\n\n", ConClass_1);
            string ChangeString = string.Format("~Class{0}(){{}}\n\n\n\tpublic Class{1} class{1}A {{ get {{ Console.Write(\"get objClass{1} ->\");" +
                "return objClass{1}; }} }}\n\tprivate Class{1} objClass{1} = new Class{1}();\n", ConClass_1, ConClass_2);
            code = code.Replace(ChangeSTR, ChangeString);
            Form.CodeViewer.Text = code;
        }

        public static void LinkAggeregation(int ConClass_1, int ConClass_2, EnterMatrix Form)
        {
            code = Form.CodeViewer.Text;
            string textCon1 = string.Format("Class{0}", ConClass_1);
            int IndexOfClass1 = code.IndexOf(textCon1);
            if (IndexOfClass1 == -1)
            {
                string Class1STR = string.Format("class Class{0}\n{{\n\tpublic Class{0}()\n\t{{\n\n\t}}\n\t~Class{0}(){{}}\n\n}}\n", ConClass_1);
                code += Class1STR;
            }
            string textCon2 = string.Format("Class{0}", ConClass_2);
            int IndexOfClass2 = code.IndexOf(textCon2);
            if (IndexOfClass2 == -1)
            {
                string Class2STR = string.Format("class Class{0}\n{{\n\tpublic Class{0}()\n\t{{\n\n\t}}\n\t~Class{0}(){{}}\n\n}}\n", ConClass_2);
                code += Class2STR;
            }
            string textProv = string.Format("public Class{0}()", ConClass_1);
            int IndexOfProv = code.IndexOf(textProv);
            string ChangeString = string.Format("public Class{0}( Class{1} objClass{1}", ConClass_1, ConClass_2);
            if (IndexOfProv == -1)
            {
                ChangeString += ",";
            }
            string ChangeSTR = string.Format("public Class{0}(", ConClass_1);
            code = code.Replace(ChangeSTR, ChangeString);
            string Change2STR = string.Format("\n\n\t}}\n\t~Class{0}(){{}}\n\n}}\n", ConClass_1);
            string ChangeString2 = string.Format("\n\t\tthis.objClass{1} = objClass{1};\n\n\t}}\n\t~Class{0}(){{}}\n\n}}\n", ConClass_1, ConClass_2);
            code = code.Replace(Change2STR, ChangeString2);
            Form.CodeViewer.Text = code;
            string Change3STR = string.Format("~Class{0}(){{}}\n\n", ConClass_1);
            string ChangeString3 = string.Format("~Class{0}(){{}}\n\n\tpublic Class{1} class{1}_A\n\t{{\n\t\tset{{" +
                "objClass{1} = value;}}\n\t\tget{{ return objClass{1}; }}\n\t}}\n\tprivate Class{1} objClass{1};\n", ConClass_1, ConClass_2);
            code = code.Replace(Change3STR, ChangeString3);
            Form.CodeViewer.Text = code;
        }

        private static void WriteCode(string code)
        {
            string path = @"D:\prog\CSharpCodeGenerator\GeneratedCodes\classes.cs";

            try
            {
                File.WriteAllText(path, code);
                MessageBox.Show(
                "Сгенерированный код лежит в папке \" D:\\prog\\CSharpCodeGenerator\\GeneratedCodes\\classes.cs \"",
                "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Возникла ошибка при создании C# файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
