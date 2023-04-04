using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace CSharpCodeGenerator.Model
{
    public class DrawingGraph
    {
        Bitmap bitmap;
        Pen roundPen;
        Pen darkGoldPen;
        Graphics gr;
        Graphics g;
        public float X;
        public int R = 10; //радиус окружности точки

        public DrawingGraph(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            gr = Graphics.FromImage(bitmap);
            gr.Clear(Color.White);
            g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            roundPen = new Pen(ColorTranslator.FromHtml("#FF1040C1"));
            roundPen.Width = 3;
            darkGoldPen = new Pen(Color.Pink, 3); // цвет прямой и дуг
            X = 0;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        //нарисовать точку
        public void drawVertex(PointF p)
        {
            gr.FillEllipse(Brushes.LightBlue, p.X - R, p.Y - R, 2 * R, 2 * R); //коорд радиуса точек
            gr.DrawEllipse(roundPen, p.X - R, p.Y - R, 2 * R, 2 * R);
        }
        public void drawString(PointF p, int i)
        {

            //Имена вершин.
            String[] drawString = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N",
                "O", "P", "Q", "R","S", "T", "U", "V", "W", "X", "Y", "Z" };

            //Шрифт надписи.
            Font drawFont = new Font("Century Gothic", 8);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            //Позиция буквы.
            float x = p.X + 6;
            float y = p.Y - 6;

            //Ориентация надписи.
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            //Отрисовка буквы.
            gr.DrawString(drawString[i], drawFont, drawBrush, x, y, drawFormat);
        }
        //нарисовать прямую между точками
        public void drawEdge(Graph.Node V1, Graph.Node V2, Graph.Edge E, Pen lineColor = null)
        {
            //определяем точки, откуда будет строиться прямая
            float x1 = V1.p.X, y1 = V1.p.Y, x2 = V2.p.X, y2 = V2.p.Y;
            if (lineColor == null)
                lineColor = darkGoldPen;
            //рисуем прямую
            gr.DrawLine(lineColor, x1, y1, x2, y2);
        }

        //нарисовать все
        public void drawALLGraph(List<Graph.Node> V, List<Graph.Edge> E, Pen lineColor = null)
        {
            gr.Clear(Color.White);
            //рисуем прямую и дуги
            for (int i = 0; i < E.Count; i++)
            {
                drawEdge(E[i].NF, E[i].NT, E[i], lineColor);
            }
            //рисуем точки
            for (int i = 0; i < V.Count; i++)
            {
                drawVertex(V[i].p);
                drawString(V[i].p, i);
            }
        }
        public static void DrawGraph(EnterMatrix Form, Graph graph)
        {
            graph.G_graph.drawALLGraph(graph.V, graph.E);
            Form.GraphPictureBox.Source = BitmapToImageSource(graph.G_graph.GetBitmap());
            try
            {
                graph.G_graph.GetBitmap().Save(@"D:\prog\CSharpCodeGenerator\images\graph.png", ImageFormat.Png);
            }
            catch
            {
                MessageBox.Show("Возникла ошибка с сохранением файла в папку images");
            }
        }

        static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
