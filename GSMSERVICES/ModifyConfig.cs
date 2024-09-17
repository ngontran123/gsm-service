using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GSMSERVICES
{
    public class ModifyConfig
    {  
        public ModifyConfig() { }
        public void updateConfigSetting(string key, string value,string section)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                xml.SelectSingleNode($"//{section}").Attributes[key].Value = value;
                xml.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                ConfigurationManager.RefreshSection($"{section}");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
