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
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        public OutputWindow(List<string> _lines, string command)
        {
            InitializeComponent();
            rtb.FontFamily = new FontFamily(Settings1.Default.Font);
            rtb.FontSize = Settings1.Default.Fontsize;
            rtb.AppendText("Command: " + command);
            foreach(string line in _lines)
            {
                rtb.AppendText(Environment.NewLine + ">>> " + line);
            }
        }
    }
}
