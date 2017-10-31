using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ESConfEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            bool OpenDialog = false;

            string path = (Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH")) + "/ESConfEditor.txt";
            if(!File.Exists(path))
            {
                OpenDialog = true;
            }
            else
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    if ((s = sr.ReadLine()) != null)
                    {
                        if(File.Exists(s))
                        {
                            ESConf.FileLocation = s;
                            LoadUp();
                        }
                        else
                        {
                            OpenDialog = true;
                        }
                    }
                    else
                    {
                        OpenDialog = true;
                    }
                }
            }

            if(OpenDialog)
            {
                MainWindow = new ChooseLocation(this);
                MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                MainWindow.Show();
            }
        }

        public void LoadUp()
        {
            ESConf esConf = new ESConf();
            esConf.Initiate();

            MainWindow = new MainWindow(esConf);
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainWindow.Show();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;


        }
    }
}
