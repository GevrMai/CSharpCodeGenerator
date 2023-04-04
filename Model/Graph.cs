using System.Collections.Generic;
using System.Drawing;

namespace CSharpCodeGenerator.Model
{
    public class Graph
    {
        private DrawingGraph G;
        public DrawingGraph G_graph { get { return G; } set { G = value; } }
        public List<Node> V;
        public List<Edge> E;
        public Node selectNode = null;//выбранная точка, для соединения прямой
        public Node changeNode = null;//определение опций для мыши

        public Graph()
        {
            G = new DrawingGraph(1035, 450);
            V = new List<Node>();
            E = new List<Edge>();
        }
        public class Node                   // точка ( класс )
        {
            public PointF p;               // позиция точки по X, Y
            public Node(PointF p)
            {
                this.p = p;
            }
        }

        public class Edge                   // ребро, соединяющее две точки
        {
            public Node NF;                 // Node Frome откуда
            public Node NT;                 //Node To куда

            public Edge(Node v1, Node v2)
            {
                NF = new Node(v1.p);
                NT = new Node(v2.p);
            }
        }
    }
}
