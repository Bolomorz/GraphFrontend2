using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using Graphs;

namespace GraphFrontend2
{
    enum GraphType { WeightedGraph, WeightedDirectedGraph, WeightlessGraph, WeightlessDirectedGraph }
    internal class CanvasGraph
    {
        protected Graph graph { get; set; }
        protected Canvas canvas { get; set; }
        protected Vertex? activevertex { get; set; }
        protected Edge? activeedge { get; set; }

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
                    break;
            }
            canvas = _canvas;
        }

        public void OnClick(Point mouse)
        {

        }

        private bool IsPointInCircleOfVertex(Point p, Vertex u)
        {

        }
    }
}
