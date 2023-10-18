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
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                current = new CanvasGraph((GraphType)Enum.Parse(typeof(GraphType), Settings1.Default.Graphtype), canvas1);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                current = new CanvasGraph(GraphType.Graph, canvas1);
            }
        }

        public static System.Drawing.Point ConvertToPoint(System.Windows.Point pos)
        {
            return new System.Drawing.Point(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y));
        }

        public void LoadGraph(CanvasGraph cg)
        {
            current = cg;
            current.Draw();
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
        }

        public void ReDraw()
        {
            current.Draw();
        }

        private void CanvasButtonClick(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(canvas1);
            current.OnClick(MainWindow.ConvertToPoint(pos), this);
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
            ofd.Filter = "CanvasGraph Files(.cagrdg)|*.cagrdg|CanvasGraph Files(.cagrug)|*cagrug";
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
            try
            {
                current = new CanvasGraph((GraphType)Enum.Parse(typeof(GraphType), Settings1.Default.Graphtype), canvas1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                current = new CanvasGraph(GraphType.Graph, canvas1);
            }
        }
    }
}
