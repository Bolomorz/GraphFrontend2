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
        [DataMember]
        public List<TextElement> elements { get; set; }

        public Directed(GraphType _type, DirectedGraph _graph, int _vertexcount, List<TextElement> _elements)
        {
            type = _type;
            graph = _graph;
            vertexcount = _vertexcount;
            elements = _elements;
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
        [DataMember]
        public List<TextElement> elements { get; set; }

        public UnDirected(GraphType _type, Graph _graph, int _vertexcount, List<TextElement> _elements)
        {
            type = _type;
            graph = _graph;
            vertexcount = _vertexcount;
            elements = _elements;
        }
    }

    [DataContract(Name = "TextElement", IsReference =true)]
    public class TextElement
    {
        [DataMember]
        public Position position { get; set; }
        [DataMember]
        public string text { get; set; }


        public TextElement()
        {
            position = new Position();
            text = string.Empty;
        }

        public TextElement(Position _position, string _text)
        {
            position = _position;
            text = _text;
        }

        public static bool operator ==(TextElement x, TextElement y)
        {
            if(x.position == y.position) return true; else return false;
        }

        public static bool operator !=(TextElement x, TextElement y)
        {
            if(x.position != y.position) return true; else return false;
        }
    }
}
