using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Krypton.Toolkit;
using System.Configuration;
using Guna.UI2.WinForms.Enums;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.CompilerServices;
using GSMSERVICES.Services;
using MaterialSkin;
using System.Management.Instrumentation;

namespace GSMSERVICES
{
    public partial class LoginForm : MaterialSkin.Controls.MaterialForm
    {
        public string access_token = "";
        public bool is_save_token;
        public string language = "";
        public ModifyConfig modify_config= new ModifyConfig();
        public SocketIoHelpers ws;
        public string place_holder;
        private static LoginForm instance;
        private Timer openTimer;
        private Timer closeTimer;
        public static LoginForm ReturnLoginInstance()
        {
            return instance;
        }
        public LoginForm()
        {
            InitializeComponent();
            MaterialSkin.MaterialSkinManager skinManager = MaterialSkin.MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Indigo500, MaterialSkin.Primary.Indigo700, MaterialSkin.Primary.Indigo100, MaterialSkin.Accent.Pink200, MaterialSkin.TextShade.WHITE);
            skinManager.EnforceBackcolorOnAllComponents = false;
            skinManager.RemoveFormToManage(this);
            this.StartPosition = FormStartPosition.CenterScreen;
            openTimer = new Timer();
            openTimer.Interval = 5;
            openTimer.Tick += OpenTimerTick;
            this.Opacity = 0;
            closeTimer = new Timer();
            closeTimer.Interval = 5;
            closeTimer.Tick += CloseTimerTick;
            instance = this;
        }
        private void OpenTimerTick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.05;
            }
            else
            {
                this.openTimer.Stop();
            }
        }

        private void CloseTimerTick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.05;
            }
            else
            {
                this.closeTimer.Stop();
                Environment.Exit(Environment.ExitCode);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.openTimer.Start();
            updateFontSetting();
            updateAccessToken();
            language = new LanguageVersionServices().updateLanguageVersion();
            ws = new SocketIoHelpers(language);
            this.ws.handlingDisconnect();
            this.saveTokenSwitch.Checked = true;
            this.tokenTextBox.Text = this.access_token;
            if (!language.Equals("English"))
            {
                place_holder = "Nhập token của bạn ở đây";
            }
            else
            {
                place_holder = "Put your access token here";
            }
            if (string.IsNullOrEmpty(this.tokenTextBox.Text))
            {
                this.tokenTextBox.Text = place_holder;
                this.tokenTextBox.ForeColor = Color.Gray;
            }
            this.tokenTextBox.Enter += textboxInputEnter;
            this.tokenTextBox.Leave += textboxInputLeave;

        }

        private void TokenTextBox_Leave(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void textboxInputEnter(object sender,EventArgs e)
        {
            if(this.tokenTextBox.Text==place_holder)
            {
                this.tokenTextBox.Text = "";
                this.tokenTextBox.ForeColor = Color.DimGray;
            }
        }
        public void textboxInputLeave(object sender,EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.tokenTextBox.Text))
            {  
                this.tokenTextBox.Text = place_holder;
                this.tokenTextBox.ForeColor =Color.Gray;
            }
        }
        public void updateAccessToken()
        {
            SaveTokenSection loginConfig = (SaveTokenSection)ConfigurationManager.GetSection("loginSet");
            if (loginConfig != null) 
            {
                this.access_token = loginConfig.AccessToken;
                this.is_save_token = loginConfig.SwitchToggleState;
            }
        }
        
        
      
        private string getPCUIID()
        {
            string pc_uuid = "";
            try
            {
                ManagementObjectSearcher ob_searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
                ManagementObjectCollection collection = ob_searcher.Get();
                foreach(var ob in collection)
                {
                    pc_uuid = ob["UUID"].ToString();
                    break;
                }
            }
            catch(ManagementException er)
            {
                LoggerManager.LogError(er.Message);
            }
            return pc_uuid;
        }
        private void updateFontSetting()
        { try
            {
                string font_file_path = Path.Combine(Path.GetTempPath(), "ShadeBlue_2OozX.ttf");
                if (!File.Exists(font_file_path))
                {
                    File.WriteAllBytes(font_file_path, Properties.Resources.ShadeBlue_2OozX);
                }
                PrivateFontCollection pfc = new PrivateFontCollection();
                pfc.AddFontFile(font_file_path);
                loginLabel.Font = new Font(pfc.Families[0], 25);
            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message);
                
            }
        }
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            string saved_token = "";
            if(saveTokenSwitch.Checked)
            {
                saved_token=tokenTextBox.Text.Trim();
                modify_config.updateConfigSetting("access_token", saved_token, "loginSet");
            }
            else
            {
                saved_token = "";
                modify_config.updateConfigSetting("access_token", saved_token, "loginSet");
            }
            this.Login.Enabled = false;
            string pc_id = getPCUIID();
            string access_token = this.tokenTextBox.Text.Trim();
            Environment.SetEnvironmentVariable("PC_ID", pc_id);
            Environment.SetEnvironmentVariable("ACCESS_TOKEN", access_token);
            string url = $"ws://162.0.222.106:3030?token={access_token}&device_id={pc_id}&type=connect";
            string url_temp = "wss://socketsbay.com/wss/v2/1/demo/";
            this.ws.connectWithServer(url);
            await Task.Delay(2000);
            while (ws.canAccess == -1 && ws.login_status==1)
            {
                await Task.Delay(100);
            }
            if (ws.canAccess==1)
            {
                System.Diagnostics.Process.Start("AutoUpdateClient.exe");
                this.Close();
            }
            if (this.ws.login_status==1)
            {   
                ws.is_login = true;
                Form1 main_form = new Form1();
                this.Hide();
                main_form.Show();
            }
            else
            {   
                MessageBox.Show(ws.login_message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Login.Enabled = true;
            }
        }
       
        private void saveTokenSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if(this.saveTokenSwitch.Checked)
            {
                modify_config.updateConfigSetting("switch_toggle", "true", "loginSet");
            }
            else
            {
                modify_config.updateConfigSetting("switch_toggle", "false", "loginSet");
            }
        }

        private void tokenTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void loginToken_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void tokenTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Login.PerformClick();
            }
        }

        private void tokenTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.saveTokenSwitch.Checked)
            {
                modify_config.updateConfigSetting("switch_toggle", "true", "loginSet");
            }
            else
            {
                modify_config.updateConfigSetting("switch_toggle", "false", "loginSet");
            }
        }

        private void materialSwitch1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (this.saveTokenSwitch.Checked)
            {
                modify_config.updateConfigSetting("switch_toggle", "true", "loginSet");
            }
            else
            {
                modify_config.updateConfigSetting("switch_toggle", "false", "loginSet");
            }
        }

        private void loginLabel_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.closeTimer.Start();
        }
    }
}
