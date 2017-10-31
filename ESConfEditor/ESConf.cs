using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;
using System.Windows.Controls;

namespace ESConfEditor
{
    public class ESConf
    {
        public static string FileLocation;
        public List<SystemObject> SystemObjects;

        public void Initiate()
        {
            this.SystemObjects = new List<SystemObject>();

            XmlDocument xmlDoc = this.LoadXML(ESConf.FileLocation);
            this.SystemObjects = this.BuildSystemObjectsFromXML(xmlDoc, "/systemList/system");
        }

        public XmlDocument LoadXML(string location)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(location);
            }
            catch(Exception e)
            {
                MessageBox.Show("es_systems.cfg is not a valid XML document, please fix and try again.\n\n("+e.Message+")", "Invalid XML document", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
            }

            return xmlDoc;
        }

        public List<SystemObject> BuildSystemObjectsFromXML(XmlDocument xmlDoc, string parentNodes)
        {
            List<SystemObject> systemObjects = new List<SystemObject>();

            foreach(XmlNode xmlNode in xmlDoc.SelectNodes(parentNodes))
            {
                SystemObject systemObject = new SystemObject();
                systemObject.fullname = xmlNode.SelectSingleNode("fullname").InnerText;
                systemObject.name = xmlNode.SelectSingleNode("name").InnerText;
                systemObject.path = xmlNode.SelectSingleNode("path").InnerText;
                systemObject.extension = xmlNode.SelectSingleNode("extension").InnerText;
                systemObject.command = xmlNode.SelectSingleNode("command").InnerText;
                systemObject.platform = xmlNode.SelectSingleNode("platform").InnerText;
                systemObject.theme = xmlNode.SelectSingleNode("theme").InnerText;
                systemObjects.Add(systemObject);
            }

            return systemObjects;
        }
    }
}
