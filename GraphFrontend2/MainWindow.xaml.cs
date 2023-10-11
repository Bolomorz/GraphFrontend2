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

        private void CanvasButtonClick(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(canvas1);
            current.OnClick(MainWindow.ConvertToPoint(pos));
        }

        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {

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

        }
    }
}
