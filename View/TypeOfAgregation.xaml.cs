using CSharpCodeGenerator.Controller;
using System.Collections.Generic;
using System.Windows;

namespace CSharpCodeGenerator.View
{
    public partial class TypeOfAgregation : Window
    {
        string key;
        ControllerClass controller;
        public TypeOfAgregation(KeyValuePair<string, string> link, ControllerClass controller)
        {
            InitializeComponent();
            this.controller = controller;
            LinkTextBlock.Text = "Выберите тип агрегации для связи:   " + link.Key;
            key = link.Key;
        }

        private void ByLinkButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Matrix_pr.links[key] = "Link_Aggregation";
            Close();
        }

        private void ByValuekButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Matrix_pr.links[key] = "Meant_Aggregation";
            Close();
        }

        private void NestkButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Matrix_pr.links[key] = "Attachment_Aggregation";
            Close();
        }
    }
}
