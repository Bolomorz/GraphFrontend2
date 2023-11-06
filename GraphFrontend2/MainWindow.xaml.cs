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
using System.Windows.Navigation;
using Graphs;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Printing;
using System.Net.Mail;

namespace GraphFrontend2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected CanvasGraph current { get; set; }
        public MainWindow(string args)
        {
            InitializeComponent();
            NewGraph();
            if (System.IO.Path.GetExtension(args).ToLower() == ".cagrdg" || System.IO.Path.GetExtension(args).ToLower() == ".cagrug")
            {
                CanvasGraph? cg = current.Deserialize(args);
                if (cg is not null)
                {
                    current = cg;
                    current.Draw();
                    GTL.Content = "Graphtype: " + current.type.ToString();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            NewGraph();
        }

        public static System.Drawing.Point ConvertToPoint(System.Windows.Point pos)
        {
            return new System.Drawing.Point(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y));
        }

        public void NewGraph()
        {
            try
            {
                current = new CanvasGraph((GraphType)Enum.Parse(typeof(GraphType), Settings1.Default.Graphtype), canvas1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                current = new CanvasGraph(GraphType.Graph, canvas1);
            }
            GTL.Content = "Graphtype: " + current.type.ToString();
        }

        public void ReDraw()
        {
            current.Draw();
        }

        private void CanvasLeftClick(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(canvas1);
            current.OnClick(MainWindow.ConvertToPoint(pos), this);
            canvas1.Focus();
        }

        private void CanvasRightClick(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(canvas1);
            current.RightClick(MainWindow.ConvertToPoint(pos));
            canvas1.Focus();
        }

        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            string path = "";
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open File";
            ofd.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(dir, @"..\..\..\DataFiles"));
            ofd.Filter = "CanvasGraph Files(.cagrdg)|*.cagrdg|CanvasGraph Files(.cagrug)|*.cagrug";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;
            ofd.RestoreDirectory = false;

            if(ofd.ShowDialog() == true)
            {
                path = ofd.FileName;
            }
            if(File.Exists(path))
            {
                CanvasGraph? cg = current.Deserialize(path);
                if(cg is not null)
                {
                    current = cg;
                    current.Draw();
                    GTL.Content = "Graphtype: " + current.type.ToString();
                }
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            string path;
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;
            string id = System.IO.Path.GetFullPath(System.IO.Path.Combine(dir, @"..\..\..\DataFiles"));
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save File";
            sfd.InitialDirectory = id;
            switch (current.type)
            {
                case GraphType.Graph:
                    sfd.Filter = "CanvasGraph Files|*.cagrug";
                    sfd.DefaultExt = ".cagrug";
                    break;
                case GraphType.DirectedGraph:
                    sfd.Filter = "CanvasGraph Files|*.cagrdg";
                    sfd.DefaultExt = ".cagrdg";
                    break;
                case GraphType.WeightedGraph:
                    sfd.Filter = "CanvasGraph Files|*.cagrug";
                    sfd.DefaultExt = ".cagrug";
                    break;
                case GraphType.WeightedDirectedGraph:
                    sfd.Filter = "CanvasGraph Files|*.cagrdg";
                    sfd.DefaultExt = ".cagrdg";
                    break;

            }
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if(sfd.ShowDialog() == true)
            {
                path = sfd.FileName;
                current.Serialize(path);
            }
        }

        private void tbkeydown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var lines = current.Command(CommandTB.Text);
                OutputWindow op = new OutputWindow(lines, CommandTB.Text);
                op.Show();
            }
        }

        private void NewButtonClick(object sender, RoutedEventArgs e)
        {
            ListBox lb = new ListBox();
            canvas1.Children.Add(lb);
            lb.Items.Clear();
            lb.Items.Add(GraphType.Graph);
            lb.Items.Add(GraphType.DirectedGraph);
            lb.Items.Add(GraphType.WeightedGraph);
            lb.Items.Add(GraphType.WeightedDirectedGraph);
            lb.SelectionChanged += Lb_SelectionChanged;
            lb.Margin = new Thickness(1380, 10, 0, 0);
            lb.Width = 150;
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            Settings1.Default.Graphtype = lb.SelectedItem.ToString();
            Settings1.Default.Save();
            canvas1.Children.Remove(lb);
            NewGraph();
        }

        private void canvas1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Delete:
                    current.DeleteElement();
                    current.Draw();
                    break;
                case Key.Enter:
                    current.EnterOnTextElement();
                    current.Draw();
                    break;
                case Key.Back:
                    current.BackspaceOnTextElement();
                    current.Draw();
                    break;
                default:
                    string key = e.Key.ToString();
                    current.KeyOnTextElement(key);
                    current.Draw();
                    break;

            }
        }
    }
}
