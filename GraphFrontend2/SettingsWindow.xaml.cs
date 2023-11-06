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
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MainWindow mw { get; set; }
        public SettingsWindow(MainWindow _mw)
        {
            InitializeComponent();
            mw = _mw;
            CreateControls();
        }

        private void CreateControls()
        {
            canvas.Children.Clear();
            int height = (int)Settings1.Default.Fontsize + 10;
            CreateRTB(100, 10, 10, true, new RichTextBox(), "Vertexradius", Brushes.White);
            CreateRTB(300, 110, 10, false, new RichTextBox(), Settings1.Default.Vertexradius.ToString(), Brushes.Gainsboro);
            CreateRTB(100, 10, 1 * (height + 10) + 10, true, new RichTextBox(), "Font", Brushes.White);
            CreateRTB(300, 110, 1 * (height + 10) + 10, false, new RichTextBox(), Settings1.Default.Font, Brushes.Gainsboro);
            CreateRTB(100, 10, 2 * (height + 10) + 10, true, new RichTextBox(), "Fontsize", Brushes.White);
            CreateRTB(300, 110, 2 * (height + 10) + 10, false, new RichTextBox(), Settings1.Default.Fontsize.ToString(), Brushes.Gainsboro);
            CreateRTB(100, 10, 3 * (height + 10) + 10, true, new RichTextBox(), "Graphtype", Brushes.White);
            CreateRTB(300, 110, 3 * (height + 10) + 10, false, new RichTextBox(), Settings1.Default.Graphtype, Brushes.Gainsboro);
            CreateButton(10, 4 * (height + 10) + 10, new Button());
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

        private void CreateButton(int left, int top, Button bt)
        {
            bt.BorderThickness = new Thickness(1);
            bt.Width = 100;
            bt.Height = (int)Settings1.Default.Fontsize + 10;
            bt.FontFamily = new FontFamily(Settings1.Default.Font);
            bt.FontSize = Settings1.Default.Fontsize;
            bt.Margin = new Thickness(left, top, 0, 0);
            bt.Content = "Submit Changes";
            bt.Click += BtClick;
            canvas.Children.Add(bt);
        }

        private void BtClick(object sender, RoutedEventArgs e)
        {
            RichTextBox tb = (RichTextBox)canvas.Children[1];
            TextRange tr = new TextRange(tb.Document.ContentStart, tb.Document.ContentEnd);
            if(int.TryParse(tr.Text, out int radius)) { Settings1.Default.Vertexradius = radius; }
            tb = (RichTextBox)canvas.Children[3];
            tr = new TextRange(tb.Document.ContentStart, tb.Document.ContentEnd);
            Settings1.Default.Font = tr.Text;
            tb = (RichTextBox)canvas.Children[5];
            tr = new TextRange(tb.Document.ContentStart, tb.Document.ContentEnd);
            if(double.TryParse(tr.Text, out double fontsize)) { Settings1.Default.Fontsize = fontsize; }
            tb = (RichTextBox)canvas.Children[7];
            tr = new TextRange(tb.Document.ContentStart, tb.Document.ContentEnd);
            if(Enum.TryParse(typeof(GraphType), tr.Text, out object? gtype))
            {
                if(gtype is not null)
                {
                    GraphType gt = (GraphType)gtype;
                    Settings1.Default.Graphtype = gt.ToString();
                }
            }
            Settings1.Default.Save();
            mw.ReDraw();
            CreateControls();
        }
    }
}
