using Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphFrontend2
{
    /// <summary>
    /// Interaktionslogik für ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private enum CWType { Vertex, Edge}

        private CWType type;
        private Vertex? vertex;
        private Edge? edge;
        public ConfigWindow(Vertex _vertex)
        {
            InitializeComponent();
            type = CWType.Vertex;
            vertex = _vertex;
            edge = null;
            ControlsForVertex();
        }

        public ConfigWindow(Edge _edge)
        {
            InitializeComponent();
            type = CWType.Edge;
            edge = _edge;
            vertex = null;
        }

        private void ControlsForVertex()
        {
            int height = (int)Settings1.Default.Fontsize + 10;
            RichTextBox lbid = new RichTextBox();
            CreateRTB(75, 10, 10, true, lbid, "ID:", Brushes.White);
            RichTextBox tbid = new RichTextBox();
            CreateRTB(300, 85, 10, true, tbid, vertex.id.ToString(), Brushes.White);
            RichTextBox lbname = new RichTextBox();
            CreateRTB(75, 10, 1 * (height + 10) + 10, true, lbname, "Name:", Brushes.White);
            RichTextBox tbname = new RichTextBox();
            CreateRTB(300, 85, 1 * (height + 10) + 10, false, tbname, vertex.name.ToString(), Brushes.Gainsboro);
        }

        private void CreateRTB(int width, int left, int top, bool ro, RichTextBox tb, string text, Brush brush)
        {
            tb.IsReadOnly = ro;
            tb.BorderThickness = new Thickness(1);
            tb.Width = width;
            tb.Height = Settings1.Default.Fontsize + 10;
            tb.FontFamily = new FontFamily(Settings1.Default.Font);
            tb.FontSize = Settings1.Default.Fontsize;
            tb.Margin = new Thickness(left, top, 0, 0);
            tb.AppendText(text);
            tb.Background = brush;
            canvas.Children.Add(tb);
        }
    }
}
