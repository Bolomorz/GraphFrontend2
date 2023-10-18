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
using System.Runtime.ConstrainedExecution;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;

namespace GraphFrontend2
{
    [DataContract(Name = "Type")]
    public enum GraphType {
        [EnumMember] Graph,
        [EnumMember] DirectedGraph, 
        [EnumMember] WeightedGraph, 
        [EnumMember] WeightedDirectedGraph }

    public class CanvasGraph
    {
        protected AGraph graph { get; set; }
        protected Canvas canvas;
        protected Vertex? activevertex { get; set; }
        protected Edge? activeedge { get; set; }
        public GraphType type { get; set; }
        protected int vertexcount { get; set; }

        protected CanvasGraph(GraphType _gtype, Canvas _canvas, AGraph _graph, int _vertexcount)
        {
            graph = _graph;
            canvas = _canvas;
            type = _gtype;
            activeedge = null;
            activevertex = null;
            vertexcount = _vertexcount;
        }

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

        public void Serialize(string filepath)
        {
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                switch(System.IO.Path.GetExtension(filepath))
                {
                    case ".cagrdg":
                        DataContractSerializer serd = new DataContractSerializer(typeof(Directed));
                        serd.WriteObject(stream, new Directed(type, (DirectedGraph)graph, vertexcount));
                        break;
                    case ".cagrug":
                        DataContractSerializer seru = new DataContractSerializer(typeof(UnDirected));
                        seru.WriteObject(stream, new UnDirected(type, (Graph)graph, vertexcount));
                        break;
                }
            }
        }

        public CanvasGraph? Deserialize(string filepath)
        {
            CanvasGraph? cg = null;
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                switch (System.IO.Path.GetExtension(filepath))
                {
                    case ".cagrdg":
                        DataContractSerializer serd = new DataContractSerializer(typeof(Directed));
                        Directed? d = (Directed?)serd.ReadObject(reader, true);
                        if(d is not null)
                        {
                            cg = new CanvasGraph(d.type, canvas, d.graph, d.vertexcount);
                        }
                        break;
                    case ".cagrug":
                        DataContractSerializer seru = new DataContractSerializer(typeof(UnDirected));
                        UnDirected? u = (UnDirected?)seru.ReadObject(reader, true);
                        if (u is not null)
                        {
                            cg = new CanvasGraph(u.type, canvas, u.graph, u.vertexcount);
                        }
                        break;
                }
                reader.Close();
            }
            return cg;
        }

        public void Draw()
        {
            DrawGraph();
        }

        public void OnClick(Point mouse, MainWindow mw)
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
                    ConfigWindow cw = new ConfigWindow(ver, mw);
                    cw.Show();
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
                    ConfigWindow cw = new ConfigWindow(edg, mw);
                    cw.Show();
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

        public List<string> Command(string command)
        {
            command = command.ToLower();
            var strings = command.Split(' ');
            var lines = new List<string>();
            switch(strings[0])
            {
                case "help":
                    lines.Add("[Help]");
                    lines.Add("[Controls]");
                    lines.Add("\tenter 'controls' for controls");
                    lines.Add("[Algorithms]");
                    lines.Add("\tenter vertices (source, target) by name");
                    lines.Add("[Dijkstra]");
                    lines.Add("\tcalculate shortestpath from source vertex to every other vertex");
                    lines.Add("\tsyntax: dijkstra source");
                    lines.Add("\tsyntax: dijkstra source target");
                    lines.Add("[BellmanFord]");
                    lines.Add("\tcalculate distance from source vertex to every other vertex");
                    lines.Add("\tsyntax: bellmanford source");
                    lines.Add("\tsyntax: bellmanford source target");
                    break;
                case "controls":
                    lines.Add("[Controls]");
                    lines.Add("click on free space to create new vertex");
                    lines.Add("activate vertex by clicking on circle of vertex");
                    lines.Add("activate edge by clicking on label of edge");
                    lines.Add("activated vertex or edge will be highlighted blue");
                    lines.Add("[Activated vertex]");
                    lines.Add("\tclick on activated vertex to open config menu for vertex");
                    lines.Add("\tclick on other vertex to create edge between vertices");
                    lines.Add("\tclick on free space to relocate vertex");
                    lines.Add("\tpress 'DEL' to delete vertex");
                    lines.Add("[Activated edge]");
                    lines.Add("\tclick on activated edge to open config menu for edge");
                    lines.Add("\tclick on free space to relocate label of edge");
                    lines.Add("\tpress 'DEL' to delete edge");
                    break;
                case "dijkstra":
                    if(type != GraphType.WeightedDirectedGraph)
                    {
                        lines.Add("[Dijkstra]");
                        lines.Add("cannot use Dijkstra on graph other than WeightedDirectedGraph");
                        break;
                    }
                    if(strings.Length == 2)
                    {
                        var source = GetVertexInGraphByName(strings[1]);
                        if(source is null && strings[1] != "?")
                        {
                            lines.Add("[Dijkstra]");
                            lines.Add(String.Format("vertex {0} not found", strings[1]));
                        }
                        else if (source is null)
                        {
                            lines.Add("[Dijkstra]");
                            lines.Add("\tcalculate shortestpath from source vertex to every other vertex");
                            lines.Add("\tsyntax: dijkstra source");
                            lines.Add("\tsyntax: dijkstra source target");
                        }
                        else
                        {
                            Dijkstra djk = new Dijkstra((DirectedGraph)graph, source);
                            var solution = djk.GetElements();
                            lines.Add("[Dijkstra]");
                            lines.Add("startvertex: " + source);
                            foreach(var element in solution)
                            {
                                lines.Add("-----------------------------------------------");
                                lines.Add("vertex: " + element.Item1);
                                lines.Add("distance: " + element.Item2);
                                lines.Add("predecessor: " + element.Item3);
                                string path = "shortestpath: ";
                                foreach(var ele in element.Item4)
                                {
                                    path += "(" + ele + ") ";
                                }
                                lines.Add(path);
                            }
                        }
                    }
                    else if(strings.Length == 3)
                    {
                        var source = GetVertexInGraphByName(strings[1]);
                        var target = GetVertexInGraphByName(strings[2]);
                        if(source is null)
                        {
                            lines.Add("[Dijkstra]");
                            lines.Add(String.Format("vertex {0} not found", strings[1]));
                        }
                        else if(target is null)
                        {
                            lines.Add("[Dijkstra]");
                            lines.Add(String.Format("vertex {0} not found", strings[1]));
                        }
                        else
                        {
                            Dijkstra djk = new Dijkstra((DirectedGraph)graph, source);
                            var solution = djk.GetElement(target);
                            lines.Add("[Dijkstra]");
                            lines.Add("startvertex: " + source);
                            lines.Add("-----------------------------------------------");
                            if (solution is not null)
                            {
                                lines.Add("vertex: " + solution.Item1);
                                lines.Add("distance: " + solution.Item2);
                                lines.Add("predecessor: " + solution.Item3);
                                string path = "shortestpath: ";
                                foreach (var ele in solution.Item4)
                                {
                                    path += "(" + ele + ") ";
                                }
                                lines.Add(path);
                            }
                            else
                            {
                                lines.Add(String.Format("vertex {0} not found", target));
                            }
                        }
                    }
                    else
                    {
                        lines.Add("[Dijkstra]");
                        lines.Add("\tcalculate shortestpath from source vertex to every other vertex");
                        lines.Add("\tsyntax: dijkstra source");
                        lines.Add("\tsyntax: dijkstra source target");
                    }
                    break;
                case "bellmanford":
                    if (type != GraphType.WeightedDirectedGraph)
                    {
                        lines.Add("[BellmanFord]");
                        lines.Add("cannot use BellmanFord on graph other than WeightedDirectedGraph");
                        break;
                    }
                    if(strings.Length == 2)
                    {
                        var source = GetVertexInGraphByName(strings[1]);
                        if(source is null && strings[1] != "?")
                        {
                            lines.Add("[BellmanFord]");
                            lines.Add(String.Format("vertex {0} not found", strings[1]));
                        }
                        else if(source is null)
                        {
                            lines.Add("[BellmanFord]");
                            lines.Add("\tcalculate distance from source vertex to every other vertex");
                            lines.Add("\tsyntax: bellmanford source");
                            lines.Add("\tsyntax: bellmanford source target");
                        }
                        else
                        {
                            var bf = new BellmanFord((DirectedGraph)graph, source);
                            var solution = bf.GetElements();
                            lines.Add("[BellmanFord]");
                            lines.Add("startvertex: " + source);
                            foreach (var element in solution)
                            {
                                lines.Add("-----------------------------------------------");
                                lines.Add("vertex: " + element.Item1);
                                lines.Add("distance: " + element.Item2);
                                lines.Add("predecessor: " + element.Item3);
                            }
                        }
                    }
                    else if(strings.Length == 3)
                    {
                        var source = GetVertexInGraphByName(strings[1]);
                        var target = GetVertexInGraphByName(strings[2]);
                        if (strings[1] == "?" || strings[2] == "?")
                        {
                            lines.Add("[BellmanFord]");
                            lines.Add("\tcalculate distance from source vertex to every other vertex");
                            lines.Add("\tsyntax: bellmanford source");
                            lines.Add("\tsyntax: bellmanford source target");
                        }
                        else if(source is null)
                        {
                            lines.Add("[BellmanFord]");
                            lines.Add(String.Format("vertex {0} not found", strings[1]));
                        }
                        else if(target is null)
                        {
                            lines.Add("[BellmanFord]");
                            lines.Add(String.Format("vertex {0} not found", strings[2]));
                        }
                        else
                        {
                            var bf = new BellmanFord((DirectedGraph)graph, source);
                            var solution = bf.GetElement(target);
                            lines.Add("[BellmanFord]");
                            lines.Add("startvertex: " + source);
                            if (solution is not null)
                            {
                                lines.Add("-----------------------------------------------");
                                lines.Add("vertex: " + solution.Item1);
                                lines.Add("distance: " + solution.Item2);
                                lines.Add("predecessor: " + solution.Item3);
                            }
                            else
                            {
                                lines.Add(String.Format("vertex {0} not found", target));
                            }
                        }
                    }
                    else
                    {
                        lines.Add("[BellmanFord]");
                        lines.Add("\tcalculate distance from source vertex to every other vertex");
                        lines.Add("\tsyntax: bellmanford source");
                        lines.Add("\tsyntax: bellmanford source target");
                    }
                    break;
                default:
                    lines.Add("enter 'help' for help");
                    lines.Add("enter 'controls' for controls");
                    break;
            }
            return lines;
        }

        private Vertex? GetVertexInGraphByName(string name)
        {
            foreach(var ver in graph.vertices)
            {
                if(ver.name == name) return ver;
            }
            return null;
        }

        private bool IsPointInCircleOfVertex(Point p, Vertex u)
        {
            var lhs = Math.Pow(u.position.x - p.X, 2) + Math.Pow(u.position.y - p.Y, 2);
            var rhs = Math.Pow(Settings1.Default.Vertexradius/2, 2);
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
                Width = r/2,
                Height = r/2,
                Stroke = brush,
                StrokeThickness = 1
            };
            Canvas.SetLeft(circle, pos.x - r/4);
            Canvas.SetTop(circle, pos.y - r/4);
            canvas.Children.Add(circle);
        }

        private void DrawText(Position pos, string text, Brush brush)
        {
            FormattedText formatted = new FormattedText(text, CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(Settings1.Default.Font), Settings1.Default.Fontsize, brush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);
            var width = formatted.Width;
            var height = formatted.Height;
            var bgeom = formatted.BuildGeometry(new System.Windows.Point(pos.x - width / 2, pos.y - height / 2));
            var path = new System.Windows.Shapes.Path();
            path.Data = bgeom;
            path.Stroke = brush;
            path.StrokeThickness = 0.5;
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
            if (type == GraphType.DirectedGraph || type == GraphType.WeightedDirectedGraph)
            {
                DrawArrow(ver1, pos, brush);
                DrawArrow(pos, ver2, brush);
            }
        }

        private void DrawArrow(Position p1, Position p2, Brush brush)
        {
            GeometryGroup lg = new GeometryGroup();
            double theta = Math.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Math.PI;

            PathGeometry pg = new PathGeometry();
            PathFigure pf = new PathFigure();
            System.Windows.Point p = new System.Windows.Point(p1.x + ((p2.x - p1.x) / 1.35), p1.y + ((p2.y - p1.y) / 1.35));
            pf.StartPoint = p;

            System.Windows.Point lpoint = new System.Windows.Point(p.X + 3, p.Y + 15);
            System.Windows.Point rpoint = new System.Windows.Point(p.X - 3, p.Y + 15);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pf.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pf.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p;
            pf.Segments.Add(seg3);

            pg.Figures.Add(pf);
            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p.X;
            transform.CenterY = p.Y;
            pg.Transform = transform;
            lg.Children.Add(pg);

            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Data = lg;
            path.StrokeThickness = 1;
            path.Stroke = path.Fill = brush;

            canvas.Children.Add(path);
        }
    }
}
