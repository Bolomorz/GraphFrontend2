using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GraphFrontend2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void AppStartup(object sender, StartupEventArgs e)
        {
            MainWindow mw;
            if(e.Args.Length > 0)
            {
                mw = new MainWindow(e.Args[0]);
            }
            else
            {
                mw = new MainWindow();
            }
            mw.Show();
        }
    }
}
