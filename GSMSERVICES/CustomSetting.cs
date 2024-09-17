using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES
{
    public class DisplaySettingSection:ConfigurationSection
    {
        [ConfigurationProperty("theme",IsRequired =true)]
        public string Theme
        {
            get
            {
                return (string)this["theme"];
            }
            set
            {
                this["theme"] = value;
            }
        }
        [ConfigurationProperty("color",IsRequired =true)]
        public string Color
        {
            get
            {
                return (string)this["color"];
            }
            set 
            {
                this["color"] = value;
            }
        }
        [ConfigurationProperty("baudrate",IsRequired =true)]
        public string Baudrate
        {
            get
            {
                return (string)this["baudrate"];
            }
            set
            {
                this["baudrate"] = value;
            }
        }
    }
    public class SaveTokenSection:ConfigurationSection
    {
        [ConfigurationProperty("access_token",IsRequired =true)]
        public string AccessToken
        {
            get
            {
                return (string)this["access_token"];
            }
            set
            {
                this["access_token"] = value;
            }
        }
        [ConfigurationProperty("switch_toggle",IsRequired =true)]
        public bool SwitchToggleState
        {  
            get
            {
                return (bool)this["switch_toggle"];
            }
            set
            {
                this["switch_toggle"] = value;
            }
        }
    }
    public class LanguageSection:ConfigurationSection
    {
        [ConfigurationProperty("language",IsRequired =true)]
        public string Language
        {
            get
            {
                return (string)this["language"];
            }
            set
            {
                this["language"] = value;
            }
        }
    }        
}
