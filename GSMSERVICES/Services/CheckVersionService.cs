using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSMSERVICES.Services
{
    public class CheckVersionService
    {
        public string language_version = "";
     
        public bool checkVersion(int current_agent_version)
        { bool res = false;
            int min_version = int.Parse(Environment.GetEnvironmentVariable("MIN_VERSION"));
            int current_version = int.Parse(Environment.GetEnvironmentVariable("CURRENT_VERSION"));
            LanguageVersionServices lang_service = new LanguageVersionServices();
            language_version = lang_service.updateLanguageVersion();
            DialogResult dialog;
            if (current_agent_version < min_version)
            {
                if (!language_version.Equals("English"))
                {
                    dialog = MessageBox.Show("Đã có phiên bản mới,vui lòng nhấn Ok để cập nhật phần mềm.", "Cập nhật phiên bản mới", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dialog = MessageBox.Show("There is new version for this application.Please choose OK to update this new version.", "Update new version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                    if (dialog == DialogResult.OK)
                {
                    res = true;
                    string file_name = "tool.zip";
                    string address = "https://portal-gsm.mobimart.xyz/static/files/GSM_SERVICES.zip";
                    using (WebClient client=new WebClient())
                    {
                        try
                        {
                            client.DownloadFile(address, file_name);
                        }
                        catch(Exception ex) 
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
         else if(current_agent_version>=min_version && current_agent_version<current_version)
            {
                if (!language_version.Equals("English"))
                {
                    dialog = MessageBox.Show("Đã có phiên bản mới,vui lòng nhấn Ok để cập nhật phần mềm.", "Cập nhật phiên bản mới", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else
                {
                    dialog = MessageBox.Show("There is new version for this application.Please choose OK to update this new version.", "Update new version", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                if (dialog == DialogResult.OK) 
                {
                    res = true;
                    string file_name = "tool.zip";
                    string address = "https://portal-gsm.mobimart.xyz/static/files/GSM_SERVICES.zip";
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(address, file_name);
                    }
                }
            }
            return res;
        }
    }
}
