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
    public enum GraphType { WeightedGraph, WeightedDirectedGraph, WeightlessGraph, WeightlessDirectedGraph }
    internal class CanvasGraph<T>
    {
        protected Graph graph { get; set; }
        protected Canvas canvas { get; set; }
        protected Vertex? activevertex { get; set; }
        protected Edge? activeedge { get; set; }
        protected GraphType type { get; set; }

        public CanvasGraph(GraphType _gtype, Canvas _canvas)
        {
            switch (_gtype)
            {
                case GraphType.WeightedGraph:
                    graph = new WeightedGraph();
                    break;
                case GraphType.WeightedDirectedGraph:
                    graph = new WeightedDirectedGraph();
                    break;
                case GraphType.WeightlessGraph:
                    graph = new WeightlessGraph();
                    break;
                case GraphType.WeightlessDirectedGraph:
                    graph = new WeightlessDirectedGraph();
                    break;
                default:
                    graph = new WeightlessGraph();
                    break;
            }
            canvas = _canvas;
            activevertex = null;
            activeedge = null;
            type = _gtype;
            canvas.Children.Clear();
        }

        public void OnClick(Point mouse)
        {

        }

        private bool IsPointInCircleOfVertex(Point p, Vertex u)
        {
            var lhs = Math.Pow(u.position.x - p.X, 2) + Math.Pow(u.position.y - p.Y, 2);
            var rhs = Math.Pow(Settings1.Default.Vertexradius, 2);
            if (lhs <= rhs) return true; else return false;
        }

        private bool IsPointInRectangleOfEdge<T>(Point p, T e) where T: WeightedEdge, WeightlessEdge
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

        private bool IsPointInRectangleOfEdge<T>(Point p, T e) where T:WeightlessEdge
        {
            string line = e.name;
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

        private bool IsPointInRectangleOfWeighting(Point p, WeightedEdge e)
        {
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

        private WeightedEdge EdgeOfPosition(Point p)
        {
            foreach(var e in graph.edges)
            {
                if(IsPointInRectangleOfEdge(p, e))
            }
        }
    }
}
