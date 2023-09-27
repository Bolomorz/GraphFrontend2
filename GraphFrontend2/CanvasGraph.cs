using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing;
using Graphs;
using System.Globalization;

namespace GraphFrontend2
{
    public enum GraphType { Graph, DirectedGraph, WeightedGraph, WeightedDirectedGraph }
    internal class CanvasGraph
    {
        protected Graph graph { get; set; }
        protected Canvas canvas { get; set; }
        protected Vertex? activevertex { get; set; }
        protected Edge? activeedge { get; set; }
        protected GraphType type { get; set; }
        protected int vertexcount { get; set; }

        public CanvasGraph(GraphType _gtype, Canvas _canvas)
        {
            switch (_gtype)
            {
                case GraphType.Graph:
                    graph = new Graph();
                    break;
                case GraphType.WeightedGraph:
                    graph = new Graph();
                    break; 
                case GraphType.WeightedDirectedGraph:
                    graph = new DirectedGraph();
                    break;
                case GraphType.DirectedGraph:
                    graph = new DirectedGraph();
                    break;
                default:
                    graph = new Graph();
                    break;
            }
            canvas = _canvas;
            activevertex = null;
            activeedge = null;
            type = _gtype;
            vertexcount = 0;
            canvas.Children.Clear();
        }

        public void OnClick(Point mouse)
        {
            if(activevertex is not null)
            {
                var ver = VertexOfPosition(mouse);
                if(ver is not null && activevertex != ver)
                {
                    Position pos = new Position((ver.position.x + activevertex.position.x)/2, (ver.position.y + activevertex.position.y) / 2);
                    string name = activevertex.name + ver.name;
                    if(type == GraphType.Graph || type == GraphType.DirectedGraph)
                    {
                        Edge edg = new Edge(name, activevertex, ver, pos);
                        graph.AddEdge(edg);

                    }
                    else
                    {
                        Edge edg = new Edge(name, activevertex, ver, 0, pos);
                        graph.AddEdge(edg);
                    }
                }
                else if(ver is not null && activevertex == ver)
                {
                    //Implement DoubleClick
                }
                else
                {
                    activevertex.position = new Position(mouse.X, mouse.Y);
                    DrawGraph();
                    activevertex = null; 
                }
            }
            else if(activeedge is not null)
            {
                var edg = EdgeOfPosition(mouse);
                if(edg is not null && edg == activeedge)
                {
                    //Implement DoubleClick
                    if(IsPointInRectangleOfWeighting(mouse, edg))
                    {

                    }
                }
                else if(edg is not null)
                {
                    edg.position = new Position(mouse.X, mouse.Y);
                }
            }
            else
            {
                var ver = VertexOfPosition(mouse);
                var edg = EdgeOfPosition(mouse);
                if(ver is not null)
                {
                    activevertex = ver;
                }
                else if(edg is not null)
                {
                    activeedge = edg;
                }
                else
                {
                    Vertex vnew = new Vertex("ver" + vertexcount, new Position(mouse.X, mouse.Y));
                    graph.AddVertex(vnew);
                    vertexcount++;
                }
            }
        }

        private bool IsPointInCircleOfVertex(Point p, Vertex u)
        {
            var lhs = Math.Pow(u.position.x - p.X, 2) + Math.Pow(u.position.y - p.Y, 2);
            var rhs = Math.Pow(Settings1.Default.Vertexradius, 2);
            if (lhs <= rhs) return true; else return false;
        }

        private bool IsPointInRectangleOfEdge(Point p, Edge e)
        {
            string line = e.name + ": " + e.weight;
            FormattedText formatted = new FormattedText(line, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(Settings1.Default.Font), Settings1.Default.Fontsize, Brushes.Black);
            var width = formatted.Width;
            var height = formatted.Height;
            var x = e.position.x;
            var y = e.position.y;
            var left = x - width / 2;
            var top = y + height / 2;
            var right = x + width / 2;
            var bottom = y - height / 2;
            if (p.X >= left && p.X <= right && p.Y <= top && p.Y >= bottom) return true; else return false;
        }

        private bool IsPointInRectangleOfWeighting(Point p, Edge e)
        {
            if (e.weight is null) return false;
            string line = e.name + ": " + e.weight;
            string weight = e.weight.ToString();
            FormattedText formattedline = new FormattedText(line, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(Settings1.Default.Font), Settings1.Default.Fontsize, Brushes.Black);
            var widthline = formattedline.Width;
            var heightline = formattedline.Height;
            FormattedText formattedweight = new FormattedText(weight, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(Settings1.Default.Font), Settings1.Default.Fontsize, Brushes.Black);
            var widthweight = formattedweight.Width;
            var heightweight = formattedweight.Height;
            var x = e.position.x;
            var y = e.position.y;
            var right = x + widthline / 2;
            var left = right - widthweight;
            var top = y + heightline / 2;
            var bottom = y - heightline / 2;
            if (p.X >= left && p.X <= right && p.Y <= top && p.Y >= bottom) return true; else return false;
        }

        private Vertex? VertexOfPosition(Point p)
        {
            foreach(var v in graph.vertices)
            {
                if(IsPointInCircleOfVertex(p, v)) return v;
            }
            return null;
        }

        private Edge? EdgeOfPosition(Point p)
        {
            foreach(var e in graph.edges)
            {
                if(IsPointInRectangleOfEdge(p, e)) return e;
            }
            return null;
        }

        private void DrawGraph()
        {

        }
    }
}
