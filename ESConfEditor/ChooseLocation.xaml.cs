using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace ESConfEditor
{
    /// <summary>
    /// Interaction logic for ChooseLocation.xaml
    /// </summary>
    public partial class ChooseLocation : Window
    {
        private App app;
        private bool DontShutDown = false;

        public ChooseLocation(App _app)
        {
            app = _app;
            InitializeComponent();
            textBoxFileLocation.Text = (Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH") + "/.emulationstation/es_systems.cfg").Replace('\\', '/');
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CFG files (*.cfg)|*.cfg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                textBoxFileLocation.Text = openFileDialog.FileName;
            }
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            DontShutDown = true;
            ESConf.FileLocation = textBoxFileLocation.Text;

            if(!File.Exists(ESConf.FileLocation))
            {
                MessageBox.Show("File does not exist");
                return;
            }

            string path = (Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH")) + "/ESConfEditor.txt";
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(textBoxFileLocation.Text);
            }

            this.Close();
            app.LoadUp();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!DontShutDown)
                Application.Current.Shutdown();
        }
    }
}