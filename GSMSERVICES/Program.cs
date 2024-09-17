using GSMSERVICES.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSMSERVICES
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            string language_version = "";
            LanguageVersionServices lang_service = new LanguageVersionServices();
            language_version = lang_service.updateLanguageVersion();
            lang_service.changeLanguageVersion(language_version);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoggerManager.InitializeLogger();
            Application.Run(new LoginForm());
        }
    }
}

