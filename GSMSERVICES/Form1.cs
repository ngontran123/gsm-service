using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using GSMSERVICES.GSM;
using GSMSERVICES.Item;
using GSMSERVICES.Services;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GSMSERVICES
{
    public partial class Form1 : MaterialForm
    {
        private MaterialSkin.MaterialSkinManager materialSkinManager;
        private readonly MaterialSkin.MaterialSkinManager themeSkinManager;
        private MaterialContextMenuStrip contextMenuStrip;
        private string theme="";
        private string color="";
        private int new_port_num = -1;
        public static Form1 instance;
        public List<GSMObject> gsmList = new List<GSMObject>();
        public List<string> List_Receive = new List<string>();
        public string baudrate = "";
        private int num_of_sim = 0;
        private int is_first = 1;
        public string current_language = "";
        public List<string> phoneList = new List<string>();
        public List<string> temp_phone_list = new List<string>();
        public ModifyConfig modify_config = new ModifyConfig();
        public LoginForm login_instance = LoginForm.ReturnLoginInstance();
        public List<string> Imei_List = new List<string>();
        public ChangeImeiService phone_imei_ob=new ChangeImeiService();
        private Timer openTimer;
        private Timer closeTimer;
        public static Form1 GetInstance()
        {
            return instance;
        }
        public Form1()
        {
            instance = this;
            InitializeComponent();
           
            initializeContextMenuStrip();
            themeSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager =MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Indigo500, MaterialSkin.Primary.Indigo700, MaterialSkin.Primary.Indigo100, MaterialSkin.Accent.Pink200, MaterialSkin.TextShade.WHITE);
            CheckForIllegalCrossThreadCalls = false;
            openTimer = new Timer();
            openTimer.Interval = 5;
            openTimer.Tick += OpenTimerTick;
            this.Opacity = 0;
            closeTimer = new Timer();
            closeTimer.Interval = 5;
            closeTimer.Tick += CloseTimerTick;
            UpdateGUI.dataRow += new UIViewRow(UpdateListViewRow);

        }

        private void OpenTimerTick(object sender,EventArgs e)
        {
            if(this.Opacity<1)
            {
                this.Opacity += 0.05;
            }
            else
            {
                this.openTimer.Stop();
            }
        }

        private void CloseTimerTick(object sender,EventArgs e)
        {
            if(this.Opacity>0)
            {
                this.Opacity -= 0.05;
            }
            else
            {
                this.closeTimer.Stop();
                Environment.Exit(Environment.ExitCode);
            }
        }
        private void initializeContextMenuStrip()
        {
            this.contextMenuStrip = new MaterialContextMenuStrip();

            if (login_instance.language == "English")
            {
                contextMenuStrip.Items.Add("Copy Port").Click += copyPort;
                contextMenuStrip.Items.Add("Copy Phone Number").Click += copyPhone;
                contextMenuStrip.Items.Add("Copy Imei").Click += copyImeiDevice;
                contextMenuStrip.Items.Add("Reload Port").Click += reloadPort;
            }
            else
            {
                contextMenuStrip.Items.Add("Sao chép cổng").Click += copyPort;
                contextMenuStrip.Items.Add("Sao chép số điện thoại").Click += copyPhone;
                contextMenuStrip.Items.Add("Sao chép mã Imei").Click += copyImeiDevice;
                contextMenuStrip.Items.Add("Tải lại cổng").Click += reloadPort;
            }
            this.dataGSM.ContextMenuStrip = contextMenuStrip;
        }
        private void reloadPort(object sender, EventArgs e)
        {
            try
            {
                List<ListViewItem> selected_row=this.dataGSM.SelectedItems.Cast<ListViewItem>().ToList();
                Parallel.ForEach(selected_row, row =>
                {
                    int index = 0;
                    if (login_instance.language == "English")
                    {
                        index = getColumnIndex("Port(COM)");
                    }
                    else
                    {
                        index = getColumnIndex("Cổng");
                    }
                    string port_name = row.SubItems[index].Text;
                    GSMObject gsm = gsmList.SingleOrDefault(x => x.Port == port_name);
                    if (gsm != null)
                    {
                        gsm.reset(port_name, row);
                    }
                });
            }
            catch(Exception ex)
            {
                LoggerManager.LogError(ex.Message);
            }
        }
        private void copyPort(object sender,EventArgs e)
        {  if (this.dataGSM.Items.Count > 0)
            {
                ListViewItem selected_item = this.dataGSM.SelectedItems[0];
                Clipboard.Clear();
                int index = 0;
                if (login_instance.language == "English")
                {
                   index = getColumnIndex("Port(COM)");
                }
                else
                {
                    index = getColumnIndex("Cổng");
                }
                Clipboard.SetText(selected_item.SubItems[index].Text);
                if (login_instance.language == "English")
                {
                    MessageBox.Show("Copy port successfully", "Copy port", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Copy cổng thành công", "Copy cổng", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        else
            { if (login_instance.language == "English")
                {
                    MessageBox.Show("There is no port found", "Copy port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cổng", "Copy cổng", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void copyPhone(object sender,EventArgs e)
        {
            if (this.dataGSM.Items.Count > 0)
            {
                ListViewItem selected_item = this.dataGSM.SelectedItems[0];
                Clipboard.Clear();
                int index = 0;
                if (login_instance.language == "English")
                {
                    index = getColumnIndex("Phone");
                }
                else
                {
                    index = getColumnIndex("Số điện thoại");
                }
                Clipboard.SetText(selected_item.SubItems[index].Text);
                if (login_instance.language == "English")
                {
                    MessageBox.Show("Copy phone successfully", "Copy phone", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Copy số điện thoại thành công", "Copy số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {  if (login_instance.language == "English")
                {
                    MessageBox.Show("There is no port found", "Copy phone", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cổng", "Copy số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void copyImeiDevice(object sender,EventArgs e)
        {
            if (this.dataGSM.Items.Count > 0)
            {
                ListViewItem selected_item = this.dataGSM.SelectedItems[0];
                Clipboard.Clear();
                int index = 0;
                if (login_instance.language == "English")
                {
                   index = getColumnIndex("Imei Device");
                }
                else
                {
                    index = getColumnIndex("Imei");
                }
                Clipboard.SetText(selected_item.SubItems[index].Text);
                if (login_instance.language == "English")
                {
                    MessageBox.Show("Copy imei successfully", "Copy imei", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Copy imei thành công", "Copy imei", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {  if (login_instance.language == "English")
                {
                    MessageBox.Show("There is no port found", "Copy imei", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cổng", "Copy imei", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        private void materialListBox1_SelectedIndexChanged(object sender, MaterialSkin.MaterialListBoxItem selectedItem)
        {
          
        }
        public void UpdateListViewRow(ListViewItem item,string name,string value)
        {
            try
            {
                if(item==null)
                {
                    return;
                }
                this.dataGSM.BeginInvoke(new MethodInvoker(() =>
                {
                    try
                    {
                        int index = getColumnIndex(name);
                        item.SubItems[index].Text = value;                    
                    }
                    catch (Exception er)
                    {
                        Console.WriteLine(er.Message);
                    }
                }));
            }
            catch(Exception e)
            {
                LoggerManager.LogError(e.Message);
            }
        }
        public ListViewItem newRow()
        {
            int rowId = -1;
            try
            {
                ListViewItem item = new ListViewItem();


                    for (int i = 0; i < dataGSM.Columns.Count; i++)
                    {
                        item.SubItems.Add("");
                    }
                    this.dataGSM.Items.Add(item);
            }
            catch (Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            rowId = dataGSM.Items.Count - 1;
            this.dataGSM.BeginInvoke(new MethodInvoker(() =>
            {
                dataGSM.Items[rowId].SubItems[0].Text = (rowId + 1).ToString();
            }));
            return dataGSM.Items[rowId];
        }
        public void changeColumnSize(ListViewItem item,MaterialSkin.Controls.MaterialListView data)
        {
            try
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    data.Columns[i].Width = -1;
                }
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
        }
        private async Task<int> countAvailableSim()
        {
            int num_of_port = 0;
            foreach (ListViewItem item in dataGSM.Items)
            {
                string phone = item.SubItems[getColumnIndex("Phone")].Text;
                if(!string.IsNullOrEmpty(phone))
                {
                    num_of_port++;
                    await Task.Delay(100);
                }
            }
            return num_of_port;
        }
        public ListViewItem newRowMessage()
        {
            int rowId = -1;
            try
            {
                ListViewItem item = new ListViewItem();
                for(int i=0;i<messageListView.Columns.Count;i++)
                {
                    item.SubItems.Add("");
                }
                this.messageListView.Items.Add(item);
                rowId = messageListView.Items.Count - 1;
                this.messageListView.BeginInvoke(new MethodInvoker(() =>
                {
                    messageListView.Items[rowId].SubItems[0].Text = (rowId + 1).ToString();
                }));
            }
            catch(Exception er)
            {
               LoggerManager.LogError(er.Message);
            }
            rowId = messageListView.Items.Count - 1;
            this.messageListView.BeginInvoke(new MethodInvoker(() =>
            {
                messageListView.Items[rowId].SubItems[0].Text = (rowId + 1).ToString();
            }));
            return messageListView.Items[rowId];
        }
        public int getColumnIndex(string columnName)
        {
            int column_index = -1;
            try
            {
                for(int i=0;i<dataGSM.Columns.Count;i++)
                {
                    if (dataGSM.Columns[i].Text.Equals(columnName))
                    {
                        column_index = i;
                        break;
                    }
                    }
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            return column_index;
        }
        public void updateDisplaySetting()
        {
            DisplaySettingSection displayConfig = (DisplaySettingSection)ConfigurationManager.GetSection("displaySet");
            if (displayConfig != null)
            {
                theme = displayConfig.Theme;
                color = displayConfig.Color;
            }
        }
        public void updateBaudRateSetting()
        {
            DisplaySettingSection displayConfig = (DisplaySettingSection)ConfigurationManager.GetSection("displaySet");
            if (displayConfig != null)
            {
                baudrate = displayConfig.Baudrate;
            }
        }
        private void AutoResizeListViewColumns(MaterialListView listView)
        {
            foreach (ColumnHeader column in listView.Columns)
            {
                column.Width = -2;
            }
        }
        public void addNewMessageRow(string res)
        {

            this.messageListView.BeginInvoke(
                new MethodInvoker(() =>
                {
                    try
                    {

                        List<string> data = new List<string>();
                        if (res.Contains("~"))
                        {
                            data = res.Split('~').ToList();
                        }
                        else
                        {
                            data = res.Split('@').ToList();
                        }

                        try
                        {
                            string com = data[4];
                            if (!com.Contains("COM"))
                            {
                                return;
                            }
                            int first_index = data[3].IndexOf("0");
                            if (first_index == 0)
                            {
                                data[3] = data[3].Remove(first_index, 1).Insert(first_index, "84");
                            }
                            ListViewItem item = newRowMessage();
                            item.SubItems[1].Text = data[4];
                            item.SubItems[3].Text = data[3];
                            item.SubItems[4].Text = data[2];
                            item.SubItems[5].Text = data[1];
                            item.SubItems[2].Text = data[0];
                            if (data.Count > 5)
                            {
                            item.SubItems[6].Text = data[5];
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    catch (Exception er)
                    {
                        //MessageBox.Show(er.Message + "here");
                    }
                }));
        }

        public void updateDisplaySetting(string key, string value)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                xml.SelectSingleNode("//displaySet").Attributes[key].Value = value;
                xml.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                ConfigurationManager.RefreshSection("displaySet");
            }
            catch (Exception ex)
            {
                LoggerManager.LogError(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }
        private void themeToogle_CheckedChanged(object sender, EventArgs e)
        {
            if (this.themeToogle.Checked)
            {
                themeSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
                modify_config.updateConfigSetting("theme", "DARK","displaySet");
            }
            else
            {
                themeSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
                modify_config.updateConfigSetting("theme", "LIGHT","displaySet");
            }
        }

        private void materialRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            themeSkinManager.ColorScheme = new MaterialSkin.ColorScheme(Primary.Green700, Primary.Green900, Primary.Green500, Accent.Green400, TextShade.WHITE);
            this.BackColor = Color.Green;
            modify_config.updateConfigSetting("color", "green","displaySet");
        }

        private void materialRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            themeSkinManager.ColorScheme = new ColorScheme(Primary.Blue700, Primary.Blue900, Primary.Blue500, Accent.Blue400, TextShade.WHITE);
            this.BackColor = Color.Blue;
            modify_config.updateConfigSetting("color", "blue","displaySet");

        }
        public void extractFile()
        {
            string file_name = Application.StartupPath + "\\tool.zip";
            if (!File.Exists(file_name))
            {
                MessageBox.Show("Không tìm thấy tập tin.");
                return;
            }
            this.extractZipFileContent(file_name, Application.StartupPath);
            System.Diagnostics.Process.Start("GSMSERVICES.exe");
        }
        public void extractZipFileContent(string file_path, string output_file_path)
        {
            ZipFile zipFile = null;
            try
            {
                zipFile = new ZipFile(File.OpenRead(file_path));
                List<string> selected_file = new List<string>();
                selected_file.Add("AutoUpdateClient.exe");
                selected_file.Add("ICSharpCode.SharpZipLib.dll");
                foreach (ZipEntry zip in zipFile)
                {
                    if (zip.IsFile)
                    {
                        string file_name = zip.Name;
                        if (!selected_file.Contains(file_name))
                        {
                            byte[] buffer = new byte[4096];
                            Stream inputStream = zipFile.GetInputStream(zip);
                            string[] files_name = file_name.Split('/');
                            string path = Path.Combine(output_file_path, file_name);
                            string directoryName = Path.GetDirectoryName(path.Replace(@"\GSM_SERVICE", ""));
                            if (directoryName == Application.StartupPath)
                            {
                                if (directoryName.Length > 0)
                                {
                                    Directory.CreateDirectory(directoryName);
                                }
                                if (selected_file.Contains(files_name[1]))
                                {
                                    using (FileStream file_des = File.Create(Path.Combine(directoryName, files_name[1])))
                                    {
                                        StreamUtils.Copy(inputStream, file_des, buffer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception er)
            {
                //MessageBox.Show(er.Message);
            }
            finally
            {
                if (zipFile != null)
                {
                    zipFile.IsStreamOwner = true;
                    zipFile.Close();
                }
            }

        }
        private string pushListSimCardInfo()
        { string res = "";
            try
            {
                res =is_first==1 ? $"{{\"type\":\"add_port\",\"first\":\"1\",\"data\":[": $"{{\"type\":\"add_port\",\"first\":\"\",\"data\":[";
                is_first = 0;
                foreach (ListViewItem item in dataGSM.Items)
                {  
                    string telco = login_instance.language == "English" ? item.SubItems[getColumnIndex("Network")].Text : item.SubItems[getColumnIndex("Nhà mạng")].Text;
                    if (!string.IsNullOrEmpty(telco))
                    {  
                        int telco_signal = convertTelco(telco);
                        string phone = login_instance.language == "English" ? item.SubItems[getColumnIndex("Phone")].Text : item.SubItems[getColumnIndex("Số điện thoại")].Text;
                        string imei = login_instance.language == "English" ? item.SubItems[getColumnIndex("Imei Device")].Text.Replace("\r\n",string.Empty): item.SubItems[getColumnIndex("Imei")].Text.Replace("\r\n", string.Empty);
                        string com = login_instance.language == "English" ? item.SubItems[getColumnIndex("Port(COM)")].Text : item.SubItems[getColumnIndex("Cổng")].Text;
                        string tkc = login_instance.language == "English" ? item.SubItems[getColumnIndex("Main Balance")].Text : item.SubItems[getColumnIndex("Số dư")].Text;
                        string expire = login_instance.language == "English" ? item.SubItems[getColumnIndex("Expire Date")].Text : item.SubItems[getColumnIndex("HSD")].Text;
                        string serial = login_instance.language == "English" ? item.SubItems[getColumnIndex("Serial_Sim")].Text : item.SubItems[getColumnIndex("Serial Sim")].Text;
                        SimDetail sim_detail = new SimDetail(phone, telco_signal, imei, com, tkc, expire, serial);
                        res += simDataPattern(sim_detail) + ",";
                    }
                }
                res += $"]}}";
                var res_ob = JsonConvert.DeserializeObject(res);
                res = JsonConvert.SerializeObject(res_ob, Newtonsoft.Json.Formatting.Indented);
                return res;
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            return res;
        }
        private string simDataPattern(SimDetail data)
        {
            string res = "";
            if(data!=null)
            {
                res = $"{{\"phone\":\"{data.Phone}\",\"telco\":\"{data.Telco}\",\"imei\":\"{data.Imei}\",\"com\":\"{data.Com}\",\"tkc\":\"{data.Tkc}\",\"hsd\":\"{data.Hsd}\",\"serial\":\"{data.Serial}\"}}";
            }
            return res;
        }
        private void materialRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            themeSkinManager.ColorScheme=new ColorScheme(Primary.Amber700,Primary.Amber900,Primary.Amber500,Accent.Amber400, TextShade.WHITE);
            this.BackColor = Color.OrangeRed;
            modify_config.updateConfigSetting("color", "amber", "displaySet");    
        }
        public void handleDisplaySetting(string theme,string color)
        {
            switch(theme)
            {
                case "LIGHT": themeSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT; this.themeToogle.Checked=false; break;
                case "DARK": themeSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK; this.themeToogle.Checked=true ; break;
            }
            switch(color)
            {
                case "blue": themeSkinManager.ColorScheme = new ColorScheme(Primary.Blue700, Primary.Blue900, Primary.Blue500, Accent.Blue400, TextShade.WHITE);
                    blueBtn.Checked = true;
                    break;
                case "green": themeSkinManager.ColorScheme = new MaterialSkin.ColorScheme(Primary.Green700, Primary.Green900, Primary.Green500, Accent.Green400, TextShade.WHITE);
                    greenBtn.Checked = true;
                    break;
                case "amber": themeSkinManager.ColorScheme = new ColorScheme(Primary.Amber700, Primary.Amber900, Primary.Amber500, Accent.Amber400, TextShade.WHITE);
                    amberBtn.Checked = true;
                    break;
            }

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            openTimer.Start();
           /* if(checkUpdateFileExist())
            {
                await Task.Delay(60000);
                this.extractFile();
                File.Delete(Path.Combine(Application.StartupPath,"tool.zip"));
            }*/
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width)/2,(Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2); 
            updateDisplaySetting();
            handleDisplaySetting(theme, color);
            updateBaudRateSetting();
            if (Properties.Settings.Default.reportTransactions == null)
            {
                Properties.Settings.Default.reportTransactions = new System.Collections.Specialized.StringCollection();
            }
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            if (login_instance.language == "English")
            {
                this.version_txt.Text = $"Version:{version}";
            }
            else
            {
                this.version_txt.Text = $"Phiên bản:{version}";
            }
            checkExternalLogin();
            handlingEvent();
            int index = this.baudrateComboBox.Items.IndexOf(baudrate);
            this.baudrateComboBox.SelectedIndex = index;
            int index_language = languagesComboBox.Items.IndexOf(login_instance.language);
            languagesComboBox.SelectedIndex = index_language;
            current_language = this.languagesComboBox.SelectedItem.ToString();
            Imei_List = Imei_List.Concat(phone_imei_ob.getAllPhoneTalco("Iphone")).Concat(phone_imei_ob.getAllPhoneTalco("Xiaomi")).Concat(phone_imei_ob.getAllPhoneTalco("Samsung")).ToList();
            run_port:
            setupPortHandling();
            if(this.login_instance.ws.resend_list_phone)
            {
                string list_sim = pushListSimCardInfo();
                if (!string.IsNullOrEmpty(list_sim))
                {
                    var json_ob = JsonConvert.DeserializeObject(list_sim);
                    var res = JsonConvert.SerializeObject(json_ob, Newtonsoft.Json.Formatting.Indented);
                    this.login_instance.ws.sendMessageToServer(list_sim);
                }
                this.login_instance.ws.resend_list_phone = false;
            }
       
                if(Properties.Settings.Default.reportTransactions.Count>0)
                {
                    foreach(string report in Properties.Settings.Default.reportTransactions)
                    {
                    var report_json = JsonConvert.DeserializeObject<TransId>(report);
                    string trans_id = report_json.Tran_Id;
                    if (!string.IsNullOrEmpty(trans_id))
                        {
                        if(Properties.Settings.Default.repushTransactions != null)
                        {
                            bool is_contained = Properties.Settings.Default.repushTransactions.Contains(trans_id);
                            if(is_contained)
                            {
                                Properties.Settings.Default.reportTransactions.Remove(report);
                                Properties.Settings.Default.repushTransactions.Remove(trans_id);
                                Properties.Settings.Default.Save();
                            }
                            else
                            {  if (!string.IsNullOrEmpty(report))
                                {
                                    this.login_instance.ws.sendMessageToServer(report);
                                }
                            }
                        }
                        }                      
                    }
                
            }
            await Task.Delay(10000);
            goto run_port;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            closeTimer.Start();
        /* Environment.Exit(Environment.ExitCode);*/
        }
        private void materialListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkExternalLogin()
        {
            Task.Run(async() =>
            {
                while (true)
                {
                    if (this.login_instance.ws.is_external_login)
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            DialogResult mess = MessageBox.Show(this.login_instance.ws.login_message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (mess == DialogResult.OK)
                            {
                                Environment.Exit(Environment.ExitCode);
                            }
                        }
                       ));
                        break;
                    }
                    await Task.Delay(1000);
                }
            });
        }
        private bool checkUpdateFileExist()
        {
            string filename = Application.StartupPath + "\\tool.zip";
            if(File.Exists(filename))
            {
                return true;
            }
            return false;
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {
        }
        private async void handlingEvent()
        {
        handle_event:
           if (login_instance.ws.Report_Port_List.Count > 0)
            {
                var report_sim_list = login_instance.ws.Report_Port_List;
                foreach (var report in report_sim_list)
                {   
                    var report_phone = JsonConvert.DeserializeObject<CheckPort>(report);
                    login_instance.ws.Report_Port_List = login_instance.ws.Report_Port_List.Remove(report);
                    if (report_phone != null)
                    {
                        string phone = report_phone.Data.Phone;
                        string trans_id = report_phone.Data.Tran_Id;                       
                        if (!string.IsNullOrEmpty(phone))
                        {
                            GSMObject gsm = gsmList.SingleOrDefault(p => p.Phone == phone);
                            if (gsm != null)
                            {
                                string imei = gsm.Imei.Replace("\r\n", "");
                                string com = gsm.Port;
                                int telco = convertTelco(gsm.NetWork);
                                string tkc = gsm.Balance;
                                string hsd = gsm.Expire_Date;
                                string serial = gsm.Serial_Sim;
                                string res = $"{{\"type\":\"check_port\",\"data\":[";
                                SimDetail sim_detail = new SimDetail(phone, telco, imei, com, tkc, hsd, serial);
                                res += simDataPatternWithType(sim_detail, "check_port");
                                res += $"]}}";
                                var res_ob = JsonConvert.DeserializeObject(res);
                                res = JsonConvert.SerializeObject(res_ob, Newtonsoft.Json.Formatting.Indented);
                                if (!string.IsNullOrEmpty(res))
                                {
                                    this.login_instance.ws.sendMessageToServer(res);
                                }
                                
                            }
                            gsm = null;
                        }
                        report_phone = null;
                    }
                    await Task.Delay(1000);
                }
            }
            await Task.Delay(100);
           if(login_instance.ws.Change_Imei_List.Count>0)
            {
                var change_imei_list=login_instance.ws.Change_Imei_List;

                foreach (string imei_data in change_imei_list)
                {
                    login_instance.ws.Change_Imei_List = login_instance.ws.Change_Imei_List.Remove(imei_data);
                    Task.Run(async() => 
                    {
                        var imei_info = JsonConvert.DeserializeObject<ChangeImei>(imei_data);

                        if (imei_info != null)
                    {
                        string phone = imei_info.Data.Phone;
                        string trans_id = imei_info.Data.Tran_Id;

                        if (!string.IsNullOrEmpty(phone))
                        {
                            GSMObject gsm = gsmList.SingleOrDefault(p => p.Phone == phone);
                            if (gsm != null)
                            {
                                string imei = imei_info.Data.Imei;
                                string handle_response = $"{{\"type\":\"callback\",\"data\":{{\"phone\":\"{phone}\",\"service\":\"change_imei\"}}}}";
                                var handle_res_ob = JsonConvert.DeserializeObject(handle_response);
                                handle_response = JsonConvert.SerializeObject(handle_res_ob, Newtonsoft.Json.Formatting.Indented);
                                login_instance.ws.sendMessageToServer(handle_response);

                                int val = await gsm.changeImei(imei);
                                if (val == 1)
                                {
                                    while(string.IsNullOrEmpty(gsm.Imei))
                                    {
                                        await Task.Delay(100);
                                    }
                                    gsm.Note = "Thực hiện đổi Imei thành công.";
                                    if (login_instance.language == "English")
                                    {
                                        gsm.Note = "Change Imei successfully.";
                                    }
                                    string response = $"{{\"type\":\"change_imei\",\"success\":true,\"data\":{{\"phone\":\"{phone}\",\"imei\":\"{gsm.Imei.Replace("\r\n", "")}\",\"tran_id\":\"{trans_id}\"}}}}";
                                    Properties.Settings.Default.reportTransactions.Add(response);
                                    Properties.Settings.Default.Save();
                                    var res_ob = JsonConvert.DeserializeObject(response);
                                    response = JsonConvert.SerializeObject(res_ob, Newtonsoft.Json.Formatting.Indented);
                                    this.login_instance.ws.sendMessageToServer(response);

                                }
                                else
                                {
                                    gsm.Note = "Thực hiện đổi Imei thất bại.";
                                    if (login_instance.language == "English")
                                    {
                                        gsm.Note = "Fail to change Imei.";
                                    }
                                    string response = $"{{\"type\":\"change_imei\",\"success\":false,\"data\":{{\"phone\":\"{phone}\",\"imei\":\"{gsm.Imei.Replace("\r\n", "")}\",\"tran_id\":\"{trans_id}\"}}}}";
                                    Properties.Settings.Default.reportTransactions.Add(response);
                                    Properties.Settings.Default.Save();
                                    var res_ob = JsonConvert.DeserializeObject(response);
                                    response = JsonConvert.SerializeObject(res_ob, Newtonsoft.Json.Formatting.Indented);
                                    this.login_instance.ws.sendMessageToServer(response);
                                }
                            }
                            gsm = null;
                        }
                    }
                    imei_info = null;
                    return;
                    //await Task.Delay(500);
                });
                    await Task.Delay(500);
                }
            }
            if(login_instance.ws.Send_Sms_List.Count>0)
            {
                var sms_list=login_instance.ws.Send_Sms_List;
                foreach (string sms in sms_list)
                { 
                    login_instance.ws.Send_Sms_List = login_instance.ws.Send_Sms_List.Remove(sms);
                    Task.Run(async () =>
                    {
                        var sms_detail = JsonConvert.DeserializeObject<SendSms>(sms);
                        if (sms_detail != null)
                    {
                        string phone = sms_detail.Data.Phone;
                        string phone_sent = sms_detail.Data.Phone_Sent;
                        string content = sms_detail.Data.Content;
                        string trans_id = sms_detail.Data.Tran_Id;
                        string handle_response = $"{{\"type\":\"callback\",\"data\":{{\"phone\":\"{phone}\",\"phone_sent\":\"{phone_sent}\",\"service\":\"send_sms\"}}}}";
                        var handle_res_ob = JsonConvert.DeserializeObject(handle_response);
                        handle_response = JsonConvert.SerializeObject(handle_res_ob, Newtonsoft.Json.Formatting.Indented);
                        login_instance.ws.sendMessageToServer(handle_response);
                        if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(phone_sent) && !string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(trans_id))
                        {
                            GSMObject gsm = gsmList.SingleOrDefault(p => p.Phone == phone);
                            if (gsm != null)
                            {
                                /*  long tkc = long.Parse(balance_standard(gsm.Balance));
                                  if (tkc < 300)
                                  {
                                      string response = $"{{\"type\":\"send_sms\",\"success\":false,\"message\":\"TKC không đủ tiền để thực hiện gửi tin nhắn.\",\"data\":{{\"phone\":\"{phone}\",\"phone_sent\":\"{phone_sent}\",\"content\":\"{content}\",\"tran_id\":\"{trans_id}\"}}}}";
                                      var response_ob = JsonConvert.DeserializeObject(response);
                                      response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                      this.login_instance.ws.sendMessageToServer(response);
                                      gsm = null;
                                      continue;
                                  }*/
                                bool send_res = await gsm.SendMessage(content, phone_sent);
                                if (send_res)
                                {
                                    gsm.Note = $"Đã thực hiện gửi tin nhắn thành công.";
                                    if (login_instance.language == "English")
                                    {
                                        gsm.Note = "Send message successfully.";
                                    }

                                    string response = $"{{\"type\":\"send_sms\",\"success\":true,\"message\":\"Đã gửi tin nhắn thành công.\",\"data\":{{\"phone\":\"{phone}\",\"phone_sent\":\"{phone_sent}\",\"content\":\"{content}\",\"tran_id\":\"{trans_id}\"}}}}";
                                    Properties.Settings.Default.reportTransactions.Add(response);
                                    Properties.Settings.Default.Save();
                                    var response_ob = JsonConvert.DeserializeObject(response);
                                    response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                    this.login_instance.ws.sendMessageToServer(response);
                                }
                                else
                                {
                                    gsm.Note = "Gửi tin nhắn thất bại.";
                                    if (login_instance.language == "English")
                                    {
                                        gsm.Note = "Fail to send message.";
                                    }
                                    string response = $"{{\"type\":\"send_sms\",\"success\":false,\"message\":\"Gửi tin nhắn thất bại.\",\"data\":{{\"phone\":\"{phone}\",\"phone_sent\":\"{phone_sent}\",\"content\":\"{content}\",\"tran_id\":\"{trans_id}\"}}}}";
                                    Properties.Settings.Default.reportTransactions.Add(response);
                                    Properties.Settings.Default.Save();
                                    var response_ob = JsonConvert.DeserializeObject(response);
                                    response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                    this.login_instance.ws.sendMessageToServer(response);
                                }
                                gsm = null;
                            }
                        }
                    }
                    sms_detail = null;
                    return;
                    //await Task.Delay(500);
                });
                    await Task.Delay(500);
                }
            }
            if(login_instance.ws.Ussd_Code_List.Count>0)
            {
                var ussd_code_list = login_instance.ws.Ussd_Code_List;
                foreach (var ussd_code in ussd_code_list)
                { 
                    login_instance.ws.Ussd_Code_List = login_instance.ws.Ussd_Code_List.Remove(ussd_code);
                 Task.Run(async()=>
                 {
                     var ussd_ob = JsonConvert.DeserializeObject<UssdCode>(ussd_code);

                     if (ussd_ob != null)
                    {
                        string phone = ussd_ob.Data.Phone;
                        if (!string.IsNullOrEmpty(phone))
                        {
                            GSMObject gsm = gsmList.SingleOrDefault(p => p.Phone == phone);
                            if (gsm != null)
                            {
                                string list_ussd = ussd_ob.Data.Ussd_Content;
                                string trans_id = ussd_ob.Data.Tran_Id;
                                string unlock_topup = ussd_ob.Data.Unlock_Topup;
                                string extra = ussd_ob.Data.Extra;
                                string handle_response = $"{{\"type\":\"callback\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{list_ussd}\",\"service\":\"ussd_code\"}}}}";
                                var handle_res_ob = JsonConvert.DeserializeObject(handle_response);
                                handle_response = JsonConvert.SerializeObject(handle_res_ob, Newtonsoft.Json.Formatting.Indented);
                                login_instance.ws.sendMessageToServer(handle_response);
                                if (list_ussd.Contains(">"))
                                {
                                    List<string> list_ussd_detail = list_ussd.Split('>').ToList();

                                    foreach (string ussd_detail in list_ussd_detail)
                                    {
                                        string ussd_response = await gsm.handlingUssdCode(ussd_detail);

                                        if (!string.IsNullOrEmpty(ussd_response))
                                        {
                                            string response = $"{{\"type\":\"ussd_code\",\"success\":true,\"message\":\"Truy vấn ussd thành công.\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{ussd_detail}\",\"ussd_response\":{ussd_response},\"tran_id\":\"{trans_id}\"}}}}";
                                            Properties.Settings.Default.reportTransactions.Add(response);
                                            Properties.Settings.Default.Save();
                                            var response_ob = JsonConvert.DeserializeObject(response);
                                            response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                            this.login_instance.ws.sendMessageToServer(response);
                                        }
                                        else
                                        {
                                            string response = $"{{\"type\":\"ussd_code\",\"success\":false,\"message\":\"Truy vấn ussd thất bại.\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{ussd_detail}\",\"ussd_response\":{ussd_response},\"tran_id\":\"{trans_id}\"}}}}";
                                            Properties.Settings.Default.reportTransactions.Add(response);
                                            Properties.Settings.Default.Save();
                                            var response_ob = JsonConvert.DeserializeObject(response);
                                            response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                            this.login_instance.ws.sendMessageToServer(response);
                                        }
                                        await Task.Delay(1000);
                                    }
                                }
                                else
                                {
                                    if (unlock_topup == "1")
                                    {
                                        if (string.IsNullOrEmpty(extra))
                                        {
                                            gsm.Note = "Thực hiện truy vấn USSD với code serial thất bại.";
                                            if (login_instance.language == "English")
                                            {
                                                gsm.Note = "Fail to execute USSD command with serial code.";
                                            }
                                            string response = $"{{\"type\":\"ussd_code\",\"success\":false,\"message\":\"Truy vấn ussd thất bại.\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{list_ussd}\",\"ussd_response\":\"Không được để trống mã serial\",\"tran_id\":\"{trans_id}\"}}}}";
                                            Properties.Settings.Default.reportTransactions.Add(response);
                                            Properties.Settings.Default.Save();
                                            var response_ob = JsonConvert.DeserializeObject(response);
                                            response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                            this.login_instance.ws.sendMessageToServer(response);
                                            LoggerManager.LogTrace("response");
                                            return;
                                        }
                                        string unlock_ussd = await gsm.ussdWithDtmf(list_ussd, extra);
                                        if (!string.IsNullOrEmpty(unlock_ussd))
                                        {
                                            gsm.Note = "Thực hiện truy vấn USSD với code serial thành công";
                                            if (login_instance.language == "English")
                                            {
                                                gsm.Note = "Execute USSD command successfully with serial code";
                                            }
                                            string response = $"{{\"type\":\"ussd_code\",\"success\":true,\"message\":\"Truy vấn ussd thành công.\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{list_ussd}\",\"ussd_response\":{unlock_ussd},\"tran_id\":\"{trans_id}\"}}}}";
                                            Properties.Settings.Default.reportTransactions.Add(response);
                                            Properties.Settings.Default.Save();
                                            var response_ob = JsonConvert.DeserializeObject(response);

                                            response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);

                                            this.login_instance.ws.sendMessageToServer(response);
                                            LoggerManager.LogTrace(response);
                                        }
                                        else
                                        {
                                            gsm.Note = "Thực hiện truy vấn USSD với code serial thất bại.";
                                            if (login_instance.language == "English")
                                            {
                                                gsm.Note = "Fail to execute USSD command with serial code.";
                                            }
                                            string response = $"{{\"type\":\"ussd_code\",\"success\":false,\"message\":\"Truy vấn ussd thất bại.\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{list_ussd}\",\"ussd_response\":{unlock_ussd},\"tran_id\":\"{trans_id}\"}}}}";
                                            Properties.Settings.Default.reportTransactions.Add(response);
                                            Properties.Settings.Default.Save();
                                            var response_ob = JsonConvert.DeserializeObject(response);
                                            response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                            this.login_instance.ws.sendMessageToServer(response);

                                            LoggerManager.LogTrace(response);
                                        }
                                    }
                                    else
                                    {
                                        string ussd_response = await gsm.handlingUssdCode(list_ussd);
                                        if (!string.IsNullOrEmpty(ussd_response))
                                        {
                                            gsm.Note = "Thực hiện truy vấn USSD thành công";
                                            if (login_instance.language == "English")
                                            {
                                                gsm.Note = "Execute USSD command successfully";
                                            }
                                            string response = $"{{\"type\":\"ussd_code\",\"success\":true,\"message\":\"Truy vấn ussd thành công.\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{list_ussd}\",\"ussd_response\":{ussd_response},\"tran_id\":\"{trans_id}\"}}}}";
                                            Properties.Settings.Default.reportTransactions.Add(response);
                                            Properties.Settings.Default.Save();
                                            var response_ob = JsonConvert.DeserializeObject(response);

                                            response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);

                                            this.login_instance.ws.sendMessageToServer(response);
                                        }
                                        else
                                        {
                                            gsm.Note = "Thực hiện truy vấn USSD thất bại.";
                                            if (login_instance.language == "English")
                                            {
                                                gsm.Note = "Fail to execute USSD command.";
                                            }
                                            string response = $"{{\"type\":\"ussd_code\",\"success\":false,\"message\":\"Truy vấn ussd thất bại.\",\"data\":{{\"phone\":\"{phone}\",\"ussd_code\":\"{list_ussd}\",\"ussd_response\":{ussd_response},\"tran_id\":\"{trans_id}\"}}}}";
                                            Properties.Settings.Default.reportTransactions.Add(response);
                                            Properties.Settings.Default.Save();
                                            var response_ob = JsonConvert.DeserializeObject(response);
                                            response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                            this.login_instance.ws.sendMessageToServer(response);
                                        }
                                    }
                                }
                            }
                            gsm = null;
                        }
                    }
                    ussd_ob = null;
                     return;
                    //await Task.Delay(500);
                });
                    await Task.Delay(500);
                }
            }
            if(login_instance.ws.Call_List.Count>0)
            { 
                var call_list = login_instance.ws.Call_List;
                try
                {
                    foreach (string call_phone in call_list)
                    {
                        login_instance.ws.Call_List = login_instance.ws.Call_List.Remove(call_phone);
                        Task.Run(async()=>{
                            var call_phone_ob = JsonConvert.DeserializeObject<CallItem>(call_phone);

                            if (call_phone_ob != null)
                        {
                            string phone = call_phone_ob.data.Phone;
                            string call_to = call_phone_ob.data.Call_To;
                            string call_time_out = call_phone_ob.data.Timer;
                            string tran_id = call_phone_ob.data.Tran_Id;
                            string handle_response = $"{{\"type\":\"callback\",\"data\":{{\"phone\":\"{phone}\",\"call_to\":\"{call_to}\",\"timer\":\"{call_time_out}\",\"tran_id\":\"{tran_id}\",\"service\":\"call\"}}}}";
                            var handle_res_ob = JsonConvert.DeserializeObject(handle_response);
                            handle_response = JsonConvert.SerializeObject(handle_res_ob, Newtonsoft.Json.Formatting.Indented);
                            this.login_instance.ws.sendMessageToServer(handle_response);
                            GSMObject gsm = gsmList.SingleOrDefault(p => p.Phone == phone);
                            if (gsm != null)
                            {
                                bool call_status = await gsm.callHandling(call_to, call_time_out);
                                if (call_status)
                                {
                                    gsm.Note = $"Thực hiện cuộc gọi tới số {call_to} thành công.";
                                    if (login_instance.language == "English")
                                    {
                                        gsm.Note = $"Succes to make a call to {call_to}";
                                    }
                                    string response = $"{{\"type\":\"call\",\"success\":true,\"message\":\"Thực hiện cuộc gọi thành công.\",\"data\":{{\"phone\":\"{phone}\",\"call_to\":\"{call_to}\",\"timer\":\"{call_time_out}\",\"tran_id\":\"{tran_id}\"}}}}";
                                    Properties.Settings.Default.reportTransactions.Add(response);
                                    Properties.Settings.Default.Save();
                                    var response_ob = JsonConvert.DeserializeObject(response);
                                    response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                    this.login_instance.ws.sendMessageToServer(response);
                                }
                                else
                                {
                                    gsm.Note = $"Thực hiện cuộc gọi tới số {call_to} thất bại.";
                                    if (login_instance.language == "English")
                                    {
                                        gsm.Note = $"Fail to make a call to {call_to}.";
                                    }
                                    string response = $"{{\"type\":\"call\",\"success\":false,\"message\":\"Thực hiện cuộc gọi thất bại.\",\"data\":{{\"phone\":\"{phone}\",\"call_to\":\"{call_to}\",\"timer\":\"{call_time_out}\",\"tran_id\":\"{tran_id}\"}}}}";
                                    Properties.Settings.Default.reportTransactions.Add(response);
                                    Properties.Settings.Default.Save();
                                    var response_ob = JsonConvert.DeserializeObject(response);
                                    response = JsonConvert.SerializeObject(response_ob, Newtonsoft.Json.Formatting.Indented);
                                    this.login_instance.ws.sendMessageToServer(response);
                                }
                                gsm = null;
                            }
                        }
                            return;
                    });
                        await Task.Delay(500);
                    }
                    call_list = null;
                }
                catch(Exception er)
                {
                    LoggerManager.LogError("Call error:" + er.Message);
                }
            }
            await Task.Delay(5000);
            goto handle_event;
        }
         public string simDataPatternWithType(SimDetail data,string type)
        {
            string res = "";
            if (data != null)
            {
                res = $"{{\"type\":\"{type}\",\"phone\":\"{data.Phone}\",\"telco\":\"{data.Telco}\",\"imei\":\"{data.Imei}\",\"com\":\"{data.Com}\",\"tkc\":\"{data.Tkc}\",\"hsd\":\"{data.Hsd}\",\"serial\":\"{data.Serial}\"}}";
            }
            return res;
           
    }

        public async Task<string> reportTransactionsHandling(string trans_id)
        {
            string val = "";
            try
            {
                if (Properties.Settings.Default.reportTransactions != null)
                {
                    foreach(var report in Properties.Settings.Default.reportTransactions)
                    {
                        var report_json = JsonConvert.DeserializeObject<TransId>(report);
                        string transaction_id = report_json.Tran_Id;
                        if(trans_id==transaction_id)
                        {
                            return report;
                        }
                        await Task.Delay(100);
                    }
                }
            }
            catch(Exception er)
            {
                LoggerManager.LogError("Report Transaction error:"+er.Message);
            }
            return val;
        }


        public void setupPortHandling()
        {
           try
            {
                string[] ports = SerialPort.GetPortNames();
                int port_count=ports.Length;
                if(port_count<1)
                {
                    return;
                }
                if (this.login_instance.language == "English")
                {
                    num_of_port.Text = "The number of ports:" + port_count;
                }
                else
                {
                    num_of_port.Text = "Tổng số cổng:" + port_count;
                }

                if (port_count!=new_port_num)
                {   
                    dataGSM.Items.Clear();
                    new_port_num= port_count;
                    new Task(async () =>
                    {   
                        await Task.Delay(100);
                        for(int i=0;i<new_port_num;i++)
                        {   
                            string port = ports[i];
                            ReferenceRunPort ref_run_port = new ReferenceRunPort(runPort);
                            this.Invoke(ref_run_port, port);
                            ref_run_port = null;
                            port = null;
                            await Task.Delay(100);
                        }
                        ports = null;
                    }).Start();
                }
                else
                {
                    return;
                }
                Task.Run(async () =>
                {
                    while(num_of_sim==0)
                    {
                        num_of_sim = await countAvailableSim();
                        await Task.Delay(15000);
                    }
             
                    if (num_of_sim != 0)
                    {
                        DateTime sim_timeout = DateTime.Now;
                        while (phoneList.Count != num_of_sim)
                        {
                            if (DateTime.Now.Subtract(sim_timeout).TotalMinutes > 3)
                            {
                                break;
                            }
                            else
                            {
                                await Task.Delay(100);
                            }
                        }
                        await Task.Delay(15000);
                        foreach(string phone in phoneList)
                        {
                            temp_phone_list.Add(phone);
                        }
                        await Task.Delay(1000);
                        string list_sim = pushListSimCardInfo();
                        if(!string.IsNullOrEmpty(list_sim))
                        {
                            this.login_instance.ws.sendMessageToServer(list_sim);
                            LoggerManager.LogTrace(list_sim);
                        }
                    add_new_sim:
                        int add_new_sim_res = await updateSimData();
                        await Task.Delay(5000);
                        goto add_new_sim;
                    }
                });
            }
            catch(Exception ex) 
            {
                LoggerManager.LogError(ex.Message);
            }
        }

        public delegate void ReferenceRunPort(string port);

        public void runPort(string port)
        {
            try
            {
                GSMObject gsm_ob = new GSMObject(port);
                gsmList.Add(gsm_ob);
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
        }
        public async Task<int> updateSimData()
        {
            int res = 0;
            try
            {
                await Task.Delay(10000);
                foreach (string phone in phoneList)
                {
                    if(!temp_phone_list.Contains(phone))
                    {
                        string new_sim_push = pushSingleSimInfo(phone, 1);
                        if(!string.IsNullOrEmpty(new_sim_push))
                        {
                            this.login_instance.ws.sendMessageToServer(new_sim_push);
                        }
                        temp_phone_list.Add(phone);
                        await Task.Delay(100);
                    }
                }
                res = 1;
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            return res;
        }
        public int convertTelco(string telco)
        {
            int res = 0;
            switch(telco)
            {
                case "VIETTEL":res = 1;break;
                case "MOBIFONE":res = 2;break;
                case "VINAPHONE":res = 3;break;
                case "VIETNAMOBILE":res = 4;break;
            }
            return res;
        }

        public string balance_standard(string balance)
        {
            string res = balance;
            if (balance.Contains("₫"))
            {
                balance = balance.Replace("₫", "").Trim();
            }
            if (balance.Contains(",") && !balance.Contains("."))
            {
                string[] data_balance_split = balance.Split(',');
                string tail_data_balance = data_balance_split[1];
                string data_balance = "";
                if (tail_data_balance == "00" || tail_data_balance == "00 ")
                {
                    data_balance = data_balance_split[0];
                }
                else
                {
                    data_balance = balance.Replace(",", "");
                }
                res = data_balance;
            }
            else if (balance.Contains(".") && !balance.Contains(","))
            {
                string[] data_balance_split = balance.Split('.');
                string tail_balance = data_balance_split[1];
                string data_balance = "";
                if (tail_balance == "00" || tail_balance == "00 ")
                {
                    data_balance = data_balance_split[0];
                }
                else
                {
                    data_balance = balance.Replace(".", "");
                }
                res = data_balance;
            }
            else if (balance.Contains(".") && balance.Contains(","))
            {
                int index_dot = balance.IndexOf(".");
                int index_colon = balance.IndexOf(",");
                string data_balance = "";
                if (index_dot < index_colon)
                {
                    string[] data_balance_split = balance.Split(',');
                    data_balance = data_balance_split[0].Replace(".", "");
                }
                else
                {
                    string[] data_balance_split = balance.Split('.');
                    data_balance = data_balance_split[0].Replace(",", "");
                }
                res = data_balance;
            }
            if (string.IsNullOrEmpty(balance))
            {
                res = "0";
            }
            return res;
        }
        public string pushSingleSimInfo(string phone,int status)
        {
            string res = "";
            try
            {
                if(status==0)
                {
                    res = $"{{\"type\":\"remove_port\",\"data\":[";
                }
                else
                {
                    res = $"{{\"type\":\"add_port\",\"first\":\"1\",\"data\":[";
                }
                foreach (ListViewItem item in dataGSM.Items)
                {
                    string phone_item = login_instance.language=="English" ? item.SubItems[getColumnIndex("Phone")].Text : item.SubItems[getColumnIndex("Số điện thoại")].Text;
                    if(phone_item== phone) 
                    {
                        string telco = login_instance.language=="English"? item.SubItems[getColumnIndex("Network")].Text: item.SubItems[getColumnIndex("Nhà mạng")].Text;
                        int telco_signal = convertTelco(telco);
                        string imei = login_instance.language=="English"? item.SubItems[getColumnIndex("Imei Device")].Text.Replace("\r\n",string.Empty): item.SubItems[getColumnIndex("Imei")].Text.Replace("\r\n", string.Empty);
                        string com = login_instance.language=="English" ? item.SubItems[getColumnIndex("Port(COM)")].Text : item.SubItems[getColumnIndex("Cổng")].Text;
                        string tkc = login_instance.language == "English" ? item.SubItems[getColumnIndex("Main Balance")].Text : item.SubItems[getColumnIndex("Số dư")].Text;
                        string hsd = login_instance.language=="English"?item.SubItems[getColumnIndex("Expire Date")].Text: item.SubItems[getColumnIndex("HSD")].Text;
                        string serial = login_instance.language=="English"? item.SubItems[getColumnIndex("Serial_Sim")].Text: item.SubItems[getColumnIndex("Serial Sim")].Text;
                        SimDetail sim_detail = new SimDetail(phone, telco_signal, imei, com, tkc, hsd, serial);
                        res += simDataPattern(sim_detail);
                        break;
                    }
                }
                res += $"]}}";
                var res_ob = JsonConvert.DeserializeObject(res);
                res = JsonConvert.SerializeObject(res_ob,Newtonsoft.Json.Formatting.Indented);
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            return res;
        }
        private void materialButton1_Click(object sender, EventArgs e)
        {
            string selected_baudrate=baudrateComboBox.SelectedItem.ToString();
            modify_config.updateConfigSetting("baudrate", selected_baudrate,"displaySet");
            updateBaudRateSetting();
            if (login_instance.language == "English")
            {
                MessageBox.Show("The Config has been saved", "Save Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Đã lưu cấu hình.", "Lưu cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            string language = this.languagesComboBox.SelectedItem.ToString();
            modify_config.updateConfigSetting("language", language, "languageSet");
            DialogResult dialog;
            if (language != current_language)
            {
                if (language == "English")
                {
                    dialog = MessageBox.Show("Phần mềm sẽ được khởi động lại để áp dụng phiên bản ngôn ngữ mới.", "Phiên bản ngôn ngữ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dialog == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(Application.ExecutablePath);
                        this.Close();
                    }
                }
                else
                {
                    dialog = MessageBox.Show("The app will be restarted to apply the new language.", "Language Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dialog == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start(Application.ExecutablePath);
                        this.Close();
                    }
                }
            }
        }

        private void kryptonToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void kryptonContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void languagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
    }
}
