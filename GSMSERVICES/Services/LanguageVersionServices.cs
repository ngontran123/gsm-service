using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
namespace GSMSERVICES.Services
{
    public class LanguageVersionServices
    {
        public void changeLanguageVersion(string language)
        {
            try
            {
                switch (language)
                {
                    case "English":
                        Thread.CurrentThread.CurrentCulture=new System.Globalization.CultureInfo("en");
                        break;
                    case "Tiếng việt":
                       
                        Thread.CurrentThread.CurrentCulture= new System.Globalization.CultureInfo("vi-VN");
                        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");
                        break;
                    default:
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                        break;
                }
            }
            catch (Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
        }


        public string updateLanguageVersion()
        {
            string language = "";
            LanguageSection languageConfig = (LanguageSection)ConfigurationManager.GetSection("languageSet");
            if (languageConfig != null)
            {
                language = languageConfig.Language;
            }
            return language;
        }
    }
}
