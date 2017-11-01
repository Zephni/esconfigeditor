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
using System.Windows.Shapes;
using System.Xml;

namespace ESConfEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ESConf esConf;
        public int currentIndex;
        public bool changesMade = false;

        public MainWindow(ESConf esConf)
        {
            this.esConf = esConf;
            currentIndex = 0;
            InitializeComponent();
            FillSystemList();
            changesMade = false;
        }

        public void FillSystemList()
        {
            systemList.Items.Clear();

            foreach(SystemObject systemObject in esConf.SystemObjects)
                systemList.Items.Add(systemObject.fullname);

            SelectSystemObject(0);
        }

        public void SelectSystemObject(int Number)
        {
            if (Number < 0)
                Number = 0;

            if(systemList.Items.Count == 0)
            {
                buttonMoveUp.IsEnabled = false;
                buttonMoveDown.IsEnabled = false;
                buttonCopy.IsEnabled = false;
                buttonRemove.IsEnabled = false;
                textBoxFullName.IsEnabled = false;
                textBoxName.IsEnabled = false;
                textBoxPath.IsEnabled = false;
                textBoxExt.IsEnabled = false;
                textBoxCommand.IsEnabled = false;
                textBoxPlatform.IsEnabled = false;
                textBoxTheme.IsEnabled = false;

                currentIndex = 0;
                return;
            }
            else
            {
                buttonMoveUp.IsEnabled = true;
                buttonMoveDown.IsEnabled = true;
                buttonCopy.IsEnabled = true;
                buttonRemove.IsEnabled = true;
                textBoxFullName.IsEnabled = true;
                textBoxName.IsEnabled = true;
                textBoxPath.IsEnabled = true;
                textBoxExt.IsEnabled = true;
                textBoxCommand.IsEnabled = true;
                textBoxPlatform.IsEnabled = true;
                textBoxTheme.IsEnabled = true;
            }

            systemList.SelectedIndex = Number;
            currentIndex = Number;
            UpdateForm();
        }

        public void UpdateForm()
        {
            if (this.currentIndex == -1)
                return;

            SystemObject currentSystemObject = esConf.SystemObjects[this.currentIndex];

            textBoxFullName.Text = currentSystemObject.fullname;
            textBoxName.Text = currentSystemObject.name;
            textBoxPath.Text = currentSystemObject.path;
            textBoxExt.Text = currentSystemObject.extension;
            textBoxCommand.Text = currentSystemObject.command;
            textBoxPlatform.Text = currentSystemObject.platform;
            textBoxTheme.Text = currentSystemObject.theme;
        }

        public void SaveChanges()
        {
            int tempCurrent = this.currentIndex;

            XmlDocument newDocument = new XmlDocument();

            XmlElement systemList = newDocument.CreateElement("systemList");

            foreach (SystemObject systemObject in this.esConf.SystemObjects)
            {
                XmlElement systemElement = newDocument.CreateElement("system");

                XmlElement el = newDocument.CreateElement("fullname");
                XmlText text = newDocument.CreateTextNode(systemObject.fullname);
                el.AppendChild(text); systemElement.AppendChild(el);

                el = newDocument.CreateElement("name");
                text = newDocument.CreateTextNode(systemObject.name);
                el.AppendChild(text); systemElement.AppendChild(el);

                el = newDocument.CreateElement("path");
                text = newDocument.CreateTextNode(systemObject.path);
                el.AppendChild(text); systemElement.AppendChild(el);

                el = newDocument.CreateElement("extension");
                text = newDocument.CreateTextNode(systemObject.extension);
                el.AppendChild(text); systemElement.AppendChild(el);

                el = newDocument.CreateElement("command");
                text = newDocument.CreateTextNode(systemObject.command);
                el.AppendChild(text); systemElement.AppendChild(el);

                el = newDocument.CreateElement("platform");
                text = newDocument.CreateTextNode(systemObject.platform);
                el.AppendChild(text); systemElement.AppendChild(el);

                el = newDocument.CreateElement("theme");
                text = newDocument.CreateTextNode(systemObject.theme);
                el.AppendChild(text); systemElement.AppendChild(el);

                systemList.AppendChild(systemElement);
            }

            newDocument.AppendChild(systemList);

            newDocument.Save(ESConf.FileLocation);

            this.esConf.Initiate();
            FillSystemList();
            SelectSystemObject(tempCurrent);
        }

        private void systemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectSystemObject(systemList.SelectedIndex);
            systemList.Focus();
        }

        private void textBoxFullName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (esConf.SystemObjects.Count == 0) return;
            esConf.SystemObjects[this.currentIndex].fullname = textBoxFullName.Text;
            changesMade = true;
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (esConf.SystemObjects.Count == 0) return;
            esConf.SystemObjects[this.currentIndex].name = textBoxName.Text;
            changesMade = true;
        }

        private void textBoxPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (esConf.SystemObjects.Count == 0) return;
            esConf.SystemObjects[this.currentIndex].path = textBoxPath.Text;
            changesMade = true;
        }

        private void textBoxExt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (esConf.SystemObjects.Count == 0) return;
            esConf.SystemObjects[this.currentIndex].extension = textBoxExt.Text;
            changesMade = true;
        }

        private void textBoxCommand_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (esConf.SystemObjects.Count == 0) return;
            esConf.SystemObjects[this.currentIndex].command = textBoxCommand.Text;
            changesMade = true;
        }

        private void textBoxPlatform_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (esConf.SystemObjects.Count == 0) return;
            esConf.SystemObjects[this.currentIndex].platform = textBoxPlatform.Text;
            changesMade = true;
        }

        private void textBoxTheme_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (esConf.SystemObjects.Count == 0) return;
            esConf.SystemObjects[this.currentIndex].theme = textBoxTheme.Text;
            changesMade = true;
        }

        private void buttonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (systemList.SelectedIndex > 0)
            {
                systemList.Items.Insert(this.currentIndex - 1, systemList.SelectedItem);
                esConf.SystemObjects.Insert(this.currentIndex - 1, esConf.SystemObjects[this.currentIndex]);
                systemList.Items.RemoveAt(this.currentIndex+1);
                esConf.SystemObjects.RemoveAt(this.currentIndex+1);
                SelectSystemObject(this.currentIndex - 1);
                changesMade = true;
            }
        }

        private void buttonMoveDown_Click(object sender, RoutedEventArgs e)
        {
            if ((systemList.SelectedIndex != -1) && (systemList.SelectedIndex < systemList.Items.Count - 1))
            {
                int Temp = this.currentIndex;
                systemList.Items.Insert(Temp + 2, systemList.SelectedItem);
                esConf.SystemObjects.Insert(Temp + 2, esConf.SystemObjects[this.currentIndex]);
                systemList.Items.RemoveAt(Temp);
                esConf.SystemObjects.RemoveAt(Temp);
                SelectSystemObject(Temp+1);
                changesMade = true;
            }
        }

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            if (systemList.Items.Count == 0)
                return;

            SystemObject systemObject = new SystemObject();
            systemObject.fullname = esConf.SystemObjects[this.currentIndex].fullname + " - New";
            systemObject.name = esConf.SystemObjects[this.currentIndex].name;
            systemObject.path = esConf.SystemObjects[this.currentIndex].path;
            systemObject.extension = esConf.SystemObjects[this.currentIndex].extension;
            systemObject.command = esConf.SystemObjects[this.currentIndex].command;
            systemObject.platform = esConf.SystemObjects[this.currentIndex].platform;
            systemObject.theme = esConf.SystemObjects[this.currentIndex].theme;

            systemList.Items.Insert(this.currentIndex+1, systemObject.fullname);
            esConf.SystemObjects.Insert(this.currentIndex+1, systemObject);
            SelectSystemObject(this.currentIndex+1);

            textBoxFullName.Focus();
            textBoxFullName.CaretIndex = textBoxFullName.Text.Length;

            changesMade = true;
        }

        private void buttonAddNew_Click(object sender, RoutedEventArgs e)
        {
            SystemObject newObject = new SystemObject();
            newObject.fullname = "New item";
            systemList.Items.Insert(this.currentIndex + 1, newObject.fullname);
            esConf.SystemObjects.Insert(this.currentIndex+1, newObject);
            SelectSystemObject(this.currentIndex+1);
            textBoxFullName.Focus();
            textBoxFullName.CaretIndex = textBoxFullName.Text.Length;
            changesMade = true;
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            int Temp = this.currentIndex;
            esConf.SystemObjects.RemoveAt(this.currentIndex);
            systemList.Items.RemoveAt(this.currentIndex);
            SelectSystemObject(Temp -1);
            changesMade = true;

            if(systemList.Items.Count == 0)
            {
                textBoxFullName.Text = "";
                textBoxName.Text = "";
                textBoxPath.Text = "";
                textBoxExt.Text = "";
                textBoxCommand.Text = "";
                textBoxPlatform.Text = "";
                textBoxTheme.Text = "";
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            changesMade = false;
            MessageBoxResult result = MessageBox.Show("File updated", "Saved", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void buttonOpenXML_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + ESConf.FileLocation;
            process.StartInfo = startInfo;
            process.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!changesMade)
                Application.Current.Shutdown();
            else
            {
                MessageBoxResult result = MessageBox.Show("Would you like to save your changes before exiting?", "Are you sure?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if(result == MessageBoxResult.Yes)
                {
                    SaveChanges();
                    Application.Current.Shutdown();
                }
                else if (result == MessageBoxResult.No)
                {
                    Application.Current.Shutdown();
                }
                else if(result == MessageBoxResult.Cancel || result == MessageBoxResult.None)
                {
                    e.Cancel = true;
                }
            }
        }

        private void textBoxFullName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.Home || e.Key == Key.End)
                return;

            int pos = textBoxFullName.CaretIndex;

            int Temp = this.currentIndex;

            systemList.Items.Insert(this.currentIndex+1, textBoxFullName.Text);
            systemList.Items.RemoveAt(this.currentIndex);
            SelectSystemObject(Temp);
            
            textBoxFullName.Focus();
            textBoxFullName.CaretIndex = pos;
        }
    }
}
