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

        private Vertex? ver;
        private Edge? edg;
        private MainWindow mw;

        public ConfigWindow(Vertex _vertex, MainWindow _mw)
        {
            InitializeComponent();
            ControlsForVertex(_vertex);
            ver = _vertex;
            edg = null;
            mw = _mw;
        }

        public ConfigWindow(Edge _edge, MainWindow _mw)
        {
            InitializeComponent();
            ControlsForEdge(_edge);
            ver = null;
            edg = _edge;
            mw = _mw;
        }

        private void ControlsForVertex(Vertex vertex)
        {
            canvas.Children.Clear();
            int height = (int)Settings1.Default.Fontsize + 10;
            RichTextBox lbid = new RichTextBox();
            CreateRTB(75, 10, 10, true, lbid, "ID:", Brushes.White);
            RichTextBox tbid = new RichTextBox();
            CreateRTB(300, 85, 10, true, tbid, vertex.id.ToString(), Brushes.White);
            RichTextBox lbname = new RichTextBox();
            CreateRTB(75, 10, 1 * (height + 10) + 10, true, lbname, "Name:", Brushes.White);
            RichTextBox tbname = new RichTextBox();
            CreateRTB(300, 85, 1 * (height + 10) + 10, false, tbname, vertex.name.ToString(), Brushes.Gainsboro);
            Button btver = new Button();
            CreateButton(10, 2 * (height + 10) + 10, btver, CWType.Vertex);
        }

        private void ControlsForEdge(Edge edge)
        {
            canvas.Children.Clear();
            int height = (int)Settings1.Default.Fontsize + 10;
            RichTextBox lbid = new RichTextBox(); RichTextBox tbid = new RichTextBox();
            CreateRTB(75, 10, 10, true, lbid, "ID:", Brushes.White);
            CreateRTB(300, 85, 10, true, tbid, edge.id.ToString(), Brushes.White);
            RichTextBox lbver1 = new RichTextBox(); RichTextBox tbver1 = new RichTextBox();
            CreateRTB(75, 10, 1 * (height + 10) + 10, true, lbver1, "Source:", Brushes.White);
            CreateRTB(300, 85, 1 * (height + 10) + 10, true, tbver1, edge.v1.name, Brushes.White);
            RichTextBox lbver2 = new RichTextBox(); RichTextBox tbver2 = new RichTextBox();
            CreateRTB(75, 10, 2 * (height + 10) + 10, true, lbver2, "Target:", Brushes.White);
            CreateRTB(300, 85, 2 * (height + 10) + 10, true, tbver2, edge.v2.name, Brushes.White);
            RichTextBox lbname = new RichTextBox(); RichTextBox tbname = new RichTextBox();
            CreateRTB(75, 10, 3 * (height + 10) + 10, true, lbname, "Name:", Brushes.White);
            CreateRTB(300, 85, 3 * (height + 10) + 10, false, tbname, edge.name, Brushes.Gainsboro);
            if (edge.weight is not null)
            {
                RichTextBox lbweight = new RichTextBox(); RichTextBox tbweight = new RichTextBox();
                CreateRTB(75, 10, 4 * (height + 10) + 10, true, lbweight, "Weight:", Brushes.White);
                CreateRTB(300, 85, 4 * (height + 10) + 10, false, tbweight, edge.weight.ToString(), Brushes.Gainsboro);
            }
            CreateButton(10, 5 * (height + 10) + 10, new Button(), CWType.Edge); 
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

        private void VertexButtonClick(object sender, RoutedEventArgs e)
        {
            RichTextBox tbname = (RichTextBox)canvas.Children[3];
            TextRange tr = new TextRange(tbname.Document.ContentStart, tbname.Document.ContentEnd);
            string text = tr.Text;
            ver.name = text;
            mw.ReDraw();
            this.Close();
        }

        private void EdgeButtonClick(object sender, RoutedEventArgs e) 
        {

        }

        private void CreateButton(int left, int top, Button bt, CWType type)
        {
            bt.BorderThickness = new Thickness(1);
            bt.Width = 100;
            bt.Height = (int)Settings1.Default.Fontsize + 10;
            bt.FontFamily = new FontFamily(Settings1.Default.Font);
            bt.FontSize = Settings1.Default.Fontsize;
            bt.Margin = new Thickness(left, top, 0, 0);
            bt.Content = "Submit Changes";
            switch(type)
            {
                case CWType.Vertex:
                    bt.Click += VertexButtonClick;
                    break;
                case CWType.Edge:
                    bt.Click += EdgeButtonClick;
                    break;
            }
            canvas.Children.Add(bt);
        }
    }
}
