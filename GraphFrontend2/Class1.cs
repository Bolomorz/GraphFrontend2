using Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GraphFrontend2
{
    [DataContract(Name = "Directed", IsReference = true)]
    public class Directed
    {
        [DataMember]
        public GraphType type { get; set; }
        [DataMember]
        public DirectedGraph graph { get; set; }
        [DataMember]
        public int vertexcount { get; set; }

        public Directed(GraphType _type, DirectedGraph _graph, int _vertexcount)
        {
            type = _type;
            graph = _graph;
            vertexcount = _vertexcount;
        }
    }

    [DataContract(Name = "Undirected", IsReference = true)]
    public class UnDirected
    {
        [DataMember]
        public GraphType type { get; set; }
        [DataMember]
        public Graph graph { get; set; }
        [DataMember]
        public int vertexcount { get; set; }

        public UnDirected(GraphType _type, Graph _graph, int _vertexcount)
        {
            type = _type;
            graph = _graph;
            vertexcount = _vertexcount;
        }
    }
}
