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
    public class CanvasGraph
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

        public CanvasGraph(Graph _graph, Canvas _canvas)
        {
            graph = _graph;
            canvas = _canvas;
            activevertex = null;
            activeedge = null;
            vertexcount = 0;
            foreach(var element in graph.vertices) vertexcount++;
            canvas.Children.Clear();

        }

        public void ReDraw()
        {
            DrawGraph();
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
                }
                activevertex = null;
            }
            else if(activeedge is not null)
            {
                var edg = EdgeOfPosition(mouse);
                if(edg is not null && edg == activeedge)
                {
                    //Implement DoubleClick
                }
                else
                {
                    activeedge.position = new Position(mouse.X, mouse.Y);
                }
                activeedge = null;
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
            DrawGraph();
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
            FormattedText formatted = new FormattedText(line, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(Settings1.Default.Font), Settings1.Default.Fontsize, Brushes.Black, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);
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
            canvas.Children.Clear();
            foreach(var v in graph.vertices)
            {
                if(activevertex is not null && activevertex == v)
                {
                    DrawCircle(v.position, Settings1.Default.Vertexradius, Brushes.Blue);
                    DrawText(v.position, v.name, Brushes.Blue);
                }
                else
                {
                    DrawCircle(v.position, Settings1.Default.Vertexradius, Brushes.Black);
                    DrawText(v.position, v.name, Brushes.Black);
                }
            }
            foreach(var e in graph.edges)
            {
                if(activeedge is not null && activeedge == e)
                {
                    DrawLine(e.position, e.v1.position, e.v2.position, Brushes.Blue);
                    DrawText(e.position, e.name + ": " + e.weight, Brushes.Blue);
                }
                else
                {
                    DrawLine(e.position, e.v1.position, e.v2.position, Brushes.Black);
                    DrawText(e.position, e.name + ": " + e.weight, Brushes.Black);
                }
            }
        }

        private void DrawCircle(Position pos, int r, Brush brush)
        {
            Ellipse circle = new Ellipse()
            {
                Width = r,
                Height = r,
                Stroke = brush,
                StrokeThickness = 1
            };
            Canvas.SetLeft(circle, pos.x - r/2);
            Canvas.SetTop(circle, pos.y - r/2);
            canvas.Children.Add(circle);
        }

        private void DrawText(Position pos, string text, Brush brush)
        {
            FormattedText formatted = new FormattedText(text, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(Settings1.Default.Font), Settings1.Default.Fontsize, brush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);
            var width = formatted.Width;
            var height = formatted.Height;
            var bgeom = formatted.BuildGeometry(new System.Windows.Point(pos.x - width / 2, pos.y - height / 2));
            var path = new Path();
            path.Data = bgeom;
            path.Stroke = brush;
            path.StrokeThickness = 1;
            canvas.Children.Add(path);
        }

        private void DrawLine(Position pos, Position ver1, Position ver2, Brush brush)
        {
            Line line1 = new Line()
            {
                X1 = ver1.x,
                Y1 = ver1.y,
                X2 = pos.x,
                Y2 = pos.y,
                Stroke = brush,
                StrokeThickness = 1
            };
            canvas.Children.Add(line1);
            Line line2 = new Line()
            {
                X1 = pos.x,
                Y1 = pos.y,
                X2 = ver2.x,
                Y2 = ver2.y,
                Stroke = brush,
                StrokeThickness = 1
            };
            canvas.Children.Add(line2);
        }
    }
}
