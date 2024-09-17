using GsmComm.PduConverter;
using GSMSERVICES.Properties;
using GSMSERVICES.Services;
using Guna.UI2.WinForms.Enums;
using Krypton.Toolkit;
using Newtonsoft.Json;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GSMSERVICES.GSM
{
    public class GSMObject
    {
        public string data = "";
        public SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public SerialPort sp = new SerialPort();
        private string port;
        private string network;
        private string phone;
        private string status;
        private string balance;
        private string expire_date;
        private string ht;
        private string sub_balance;
        private string serial_sim;
        private string imei;
        private string note;
        private string modem;
        private bool _isSIM;
        private bool _isPhone;
        private bool _isInfor;
        private bool _isSendPort;
        public bool isPostPaidVnpt = false;
        private bool is_modem = false;

        private string text = "";
        public string readPortSmS = "";
        private bool check_11_dig = false;
        private bool CheckSimReadyHasResult = false;
        private bool CarrierHasResult = false;
        private string _otp;
        private object lockReadPort = new object();
        private SemaphoreSlim lockWritePort = new SemaphoreSlim(1, 5);
        private SemaphoreSlim lockChangeImeiPort = new SemaphoreSlim(1, 1);
        private SemaphoreSlim lockSMSPort = new SemaphoreSlim(1, 1);
        private SemaphoreSlim lockRegisterPort = new SemaphoreSlim(10);
        private SemaphoreSlim registerSemaphore = new SemaphoreSlim(1);
        private SemaphoreSlim changeImeiSemaphore = new SemaphoreSlim(1, 1);
        private DateTime lastReportPhone = DateTime.MinValue;
        private DateTime lastReportInfo = DateTime.MinValue;
        private DateTime lastReportNetwork = DateTime.MinValue;
        private DateTime lastReportSerialData = DateTime.MinValue;
        private DateTime lastSmSReport = DateTime.MinValue;
        private DateTime timeOutExist = DateTime.Now;
        private DateTime lastSerialSimReport = DateTime.MinValue;
        private DateTime lastReportImei = DateTime.MinValue;
        private DateTime lastModemReport = DateTime.MinValue;
        private string transaction_id;
        private string time_process;
        private string status_my;
        private string ussd_code_message = "";
        public string sms;
        public string send_from;
        public string receive_date;
        private string otp_receive_times;
        public ListViewItem rowGSMSelect = new ListViewItem();
        public Form1 instance = Form1.GetInstance();
        public LoginForm login_instance = LoginForm.ReturnLoginInstance();
        private string current_sms;
        private int oldindexsms;
        private List<SmsPdu> _smsPduList = new List<SmsPdu>();
        private bool isFailSendSMS;
        private bool smsSuccess;
        private bool delallsms = true;
        private bool isTKKM;
        private bool is_run_ussd_code;
        private DateTime lastTKKMReport = DateTime.MinValue;
        private bool stop_loop = false;
        private bool is_change_imei=false;
        private bool is_dtfm = false;
        public string phone_temp = "";
        public string dtfm_response = "";
        public string phone_secondary_temp = "";
        public string phoneCallingStatus = "";
        public bool isPhoneCalling;
        private CancellationTokenSource cts;
        private bool running_loop = true;
        private bool isSerial;
        private bool is_imei;
        Regex imei_regex = new Regex(@"[^\d]+$");
        private bool lock_task = false;
        private static Dictionary<string, SemaphoreSlim> instanceSemaphores = new Dictionary<string, SemaphoreSlim>();
        private ChangeImeiService imei_ob = new ChangeImeiService();
        
        public Task task { get; private set; }

        public GSMObject(string port)
        {
            this.rowGSMSelect = this.instance.newRow();
            this.Port = port;
            this.NetWork = "";
            this.Phone = "";
            this.Status = "Waiting";
            this.Balance = "";
            this.Expire_Date = "";
            this.Note = this.loadMsg("Retrieving Info");
            this.Sub_Balance = "";
            this.Imei = "";
            this.Modem = "";
            cts = new CancellationTokenSource();
            this.sp = new SerialPort()
            {
                PortName = port,
                BaudRate =int.Parse(this.instance.baudrate),
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.RequestToSend,
                NewLine = "\r\n",
                WriteTimeout = 1000,
                ReadTimeout = 1000
            };

            this.sp.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            this.task = Task.Run(() => this.Work(), cts.Token);
        }

        public string Port
        {
            get => this.port;
            set
            {
                this.port = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Port(COM)", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Cổng", value);
                }
            }
        }
        
        public string NetWork
        {
            get => this.network;
            set
            {
                this.network = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Network", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Nhà mạng", value);
                }
            }
        }

        public string Phone
        {
            get => this.phone;
            set
            {
                this.phone = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Phone", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Số điện thoại", value);
                }
            }
        }
        
        public string Balance
        {
            get => this.balance;
            set
            {
                this.balance = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Main Balance", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Số dư", value);
                }
            }
        }
        
        public string Sub_Balance
        {
            get=>this.sub_balance;
            set
            {   this.sub_balance = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Sub Balance", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "TKKM", value);

                }
            }
        }
        public string Status
        {
            get => this.status;
            set
            {
                this.status = value;

                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Status", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Trạng thái", value);

                }
            }
        }
        public string Expire_Date
        {
            get => this.expire_date;
            set
            {
                this.expire_date = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Expire Date", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "HSD", value);

                }
            }
        }
        public string Imei
        {
            get => this.imei;
            set
            {
                this.imei = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Imei Device", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Imei", value);
                }
            }
        }
        public string Serial_Sim
        {
            get => this.serial_sim;
            set
            {
                this.serial_sim = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Serial_Sim", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Serial Sim", value);
                }
            }
        }
        public string Note
        {
            get => this.note;
            set
            {
                this.note = value;
                if (login_instance.language == "English")
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Note", value);
                }
                else
                {
                    UpdateGUI.ChangeRow(rowGSMSelect, "Thông báo", value);
                }
            }
        }
        public string Otp
        {
            get => this._otp;
            set
            {
                this._otp = value;
            }
        }

        public string Modem
        {
            get => this.modem;
            set
            {
                this.modem = value;
            }
        }
        public string loadMsg(string message)
        {
            this.Note = "";
            string res = "[" + DateTime.Now + "]" + message;
            return res;
        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if (!Monitor.TryEnter(this.lockReadPort, 2000))
            {
                return;
            }
            try
            {
                this.data += this.sp.ReadExisting();
                if (this.data.EndsWith("\n") || this.data.EndsWith("\r"))
                {
                    this.lastReportSerialData = DateTime.Now;
                    if (this.data.Contains("+CUSD:"))
                    {
                        if (this.data.EndsWith("OK\r\n"))
                        {
                            this.HandleSerialData(data);
                            data = "";
                        }
                    }
                    else
                    {
                        this.HandleSerialData(data);
                        data = "";
                    }
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er);
            }
            finally
            {
                Monitor.Exit(this.lockReadPort);
            }
        }
        public async Task sendAT(string command)
        {
            await this.lockWritePort.WaitAsync();
            try
            {
                this.sp.WriteLine(command);
            }
            catch (Exception e)
            {
                LoggerManager.LogError(e.Message);
            }
            finally
            {
                this.lockWritePort.Release();
            }
        }

        public bool TryOpenPort()
        {
            try
            {
                if (!sp.IsOpen)
                {
                    this.sp.Open();
                }
                this.sp.DiscardInBuffer();
                this.sp.DiscardOutBuffer();
                return this.sp.IsOpen;
            }
            catch (Exception e)
            {
                LoggerManager.LogError(e.Message);
                return false;
            }
        }
        public async Task CheckSimReady()
        {

            await this.semaphoreSlim.WaitAsync();
            try
            {
                this.CheckSimReadyHasResult = false;
                this._isSIM = true;
                await this.sendAT("AT+CPIN?");
                await this.sendAT("AT+CGMI?");
            }
            catch (Exception e)
            {
                LoggerManager.LogError(e.Message);
            }
        }
        public string ConvertStringToDigit(string iccid)
        {
            if (string.IsNullOrEmpty(iccid))
            {
                return "";
            }
            Regex reg = new Regex("[^0-9]");
            string res = reg.Replace(iccid, string.Empty);
            return res;
        }
        public async Task RunUSSD(string command)
        {
            try
            {
                await this.sendAT("AT+CMGF=0");
                await Task.Delay(100);
                await this.sendAT("AT+CUSD=2");
                await Task.Delay(100);
                await this.sendAT("AT+CUSD=1,\"" + command + "\",15\r");
                await Task.Delay(100);
            }
            catch (Exception e)
            {
                LoggerManager.LogError(e.Message);
            }
        }
        public async Task<int> changeImei(string imei)
        {
            int res = 0;
            try
            {
                if (string.IsNullOrEmpty(imei))
                {
                    Random rand = new Random();
                    int index = rand.Next(1, this.instance.Imei_List.Count);
                    //int check_sum = rand.Next(1, 10);
                    // string new_imei = this.imei_ob.generatePhoneImeiNumber(this.instance.Imei_List[index], check_sum);
                    string new_imei = this.instance.Imei_List[index];
                    LoggerManager.LogInfo($"New Imei:{this.Phone}+{new_imei}");
                    int exec_val = await imeiExecute(new_imei);
                    res = 1;
                }
                else
                {
                    int exec_val = await imeiExecute(imei);
                    res = 1;
                }
            }
            catch(Exception er)
            {
                res = 0;
                LoggerManager.LogError(er.Message);
            }
            return res;
        }
        public async Task<int> imeiExecute(string imei)
        {
            int res = -1;
            //await changeImeiSemaphore.WaitAsync();
            try
            {
                res = 0;
                is_change_imei = true;
                LoggerManager.LogInfo($"{this.Phone}:{imei}");
                await this.sendAT($"AT+EGMR=1,7,\"{imei}\"");
                this.Imei = "";
                for(int i=0;(i<10000 && is_change_imei==true);i+=100)
                {
                    await Task.Delay(100);
                }
                if(!is_change_imei)
                {
                    res = 1;
                }
                is_change_imei = false;
                return res;
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            //finally { changeImeiSemaphore.Release(); }
            return res;
        }

        public async Task<bool> SendMessage(string message,string phone_sent)
        {
            bool res_value = false;
            try
            {
                this.isFailSendSMS = true;
                this._isSendPort = true;
                this.sp.Write("AT+CMGF=1" + Environment.NewLine);
                this.sp.Write($"AT+CMGS=\"{phone_sent}\""+Environment.NewLine);
                this.sp.Write(message);
                this.sp.Write(new byte[1] { 26 }, 0, 1);
                for(int timeout=100;timeout<20000 && _isSendPort;timeout+=100)
                {
                    await Task.Delay(100);
                    timeOutExist = DateTime.Now;
                }
                
                if(!_isSendPort && !isFailSendSMS)
                {
                    res_value = true;
                }
                _isSendPort = false;
                isFailSendSMS = false;
                phone_sent = null;
                message = null;
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            return res_value;
        }
        public void deleteMessageByIndex()
        {
            Task.Run(async () =>
            {
                try
                {
                    await this.sendAT(string.Format("AT+CMGD={0}\r", this.oldindexsms));
                }
                catch (Exception e)
                {
                    LoggerManager.LogError(e.Message);
                }
            });
        }
        public string otpFilter(string txt1, string txt2)
        {
            string str = "";
            if ((txt2 == "MyViettel" && txt1.Contains("OTP")) || (txt2 == "VTMONEY" && txt1.Contains("OTP")))
            {
                str = new Regex("OTP\\s(\\d{4}).*").Match(txt1).Groups[1].ToString();
            }
            else
            {
                for (int i = 8; i > 3; i--)
                {
                    Match match = new Regex("(\\d{i})".Replace("i", i.ToString())).Match(txt1);
                    if (match.Groups.Count > 1)
                    {
                        str = match.Groups[1].ToString();
                        break;
                    }
                }
            }
            return str;
        }
        public void reset(string port, ListViewItem row)
        { 
            this.rowGSMSelect = row;
            this.Port = port;
            this.NetWork = "";
            this.Phone = "";
            if(this.login_instance.language=="English")
            {
                this.Status = "Đang đợi tín hiệu";
                this.Note = this.loadMsg("Đang lấy thông tin");
            }
            else
            {
                this.Status = "Waiting";
                this.Note = this.loadMsg("Retrieving info");
            }
            this.Balance = "";
            this.Expire_Date = "";
            this.Sub_Balance = "";
            this.Note = "";
            this.CarrierHasResult = false;
            cts.Cancel();
            this.sp.BaudRate = int.Parse(this.instance.baudrate);
            this.sp.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            this.task = Task.Run(() => this.Work(), cts.Token);
        }
        public void ReadSmsInbox()
        {
            Task.Run(async () =>
            {
                try
                {
                    await this.sendAT("AT+CMGF=0");
                    await this.sendAT(string.Format("AT+CMGR={0}", this.oldindexsms));
                }
                catch (Exception ex)
                {
                    LoggerManager.LogError(ex.Message);
                }
            });
        }

        private string removeSpecialCharacters(string s)
        {
            return Regex.Replace(s, "[^a-zA-Z0-9_.\"\\s\\n ]+", "", RegexOptions.Compiled);
        }

        public async Task<bool> callHandling(string to_phone,string time_out)
        {
            bool res = false;
            try
            {
                this.lock_task = true;
                int call_time_out = int.Parse(time_out);
                await this.sendAT("AT+COLP=1");
                await Task.Delay(1000);
                await this.sendAT("ATH");
                await Task.Delay(1000);
                await this.sendAT($"ATD{to_phone}i;");
                await Task.Delay(5000);
                await this.sendAT("AT+CPAS");
                DateTime waitPhoneTimeOut=DateTime.Now;
                while((string.IsNullOrEmpty(phoneCallingStatus) || !phoneCallingStatus.Equals("4")) && DateTime.Now.Subtract(waitPhoneTimeOut).TotalSeconds<30)
                {
                    if(!string.IsNullOrEmpty(phoneCallingStatus))
                    {
                        phoneCallingStatus = "";
                        await this.sendAT("AT+CPAS");
                        await Task.Delay(100);
                    }
                    await Task.Delay(100);
                }

                if(!string.IsNullOrEmpty(phoneCallingStatus))
                {
                    if(phoneCallingStatus.Equals("4")) 
                    {
                        res = true;
                        isPhoneCalling = true;
                        DateTime waitPhoneCalling = DateTime.Now;
                        while(isPhoneCalling && DateTime.Now.Subtract(waitPhoneCalling).TotalSeconds<=call_time_out)
                        {   
                            await Task.Delay(50);
                        }
                     await Task.Delay(100);
                    }
                }
                await this.sendAT("ATH");
            }
            catch (Exception er)
            {
                LoggerManager.LogError("Call handling error:" + er.Message);
            }
            lock_task = false;
            phoneCallingStatus = "";
            isPhoneCalling = false;
            return res;
        }
        public async Task<string> handlingUssdCode(string ussd_code)
        {
            string ussd_response = string.Empty;
            string ussd_real_response=string.Empty;
            try
            {
                is_run_ussd_code = true;
                await RunUSSD(ussd_code);
                for(int timeout=100; timeout<20000 && is_run_ussd_code;timeout+=100)
                {
                    await Task.Delay(100);
                    timeOutExist = DateTime.Now;
                }
                if(!is_run_ussd_code)
                {  
                    string ussd_message_regex =("\"(.*?)\"");
                    ussd_response = ussd_code_message;
                    ussd_response = ussd_response.Replace("\n", "").Replace("OK", "");
                    string[] val = ussd_response.Split(',');
                    for(int i=1;i<val.Length-1;i++)
                    {
                        if(i<val.Length-2)
                        {
                            ussd_real_response += val[i] + ",";
                        }
                        else
                        {
                            ussd_real_response += val[i];
                        }
                    }
                    ussd_real_response = removeSpecialCharacters(ussd_real_response);
              
                }
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
            return ussd_real_response;
        }
        public async Task DeleteAllSmsInbox()
        {
            try
            {  
                await this.sendAT("AT+CMGF=1");
                await Task.Delay(100);
                await this.sendAT("AT+CMGD=1,4");
                await Task.Delay(100);
                await this.sendAT("AT+CNMI=1,1");
            }
            catch (Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
        }

        public async Task<string> ussdWithDtmf(string ussd_code,string dtmf)
        {   string response="";
            try
            {
                await this.RunUSSD(ussd_code);
                await Task.Delay(8000);
                await this.sendAT($"AT+CUSD=1,\"${dtmf}\",15\r");
                is_dtfm = true;
                dtfm_response = "";
                DateTime dtfm_response_timeout = DateTime.Now;
                while(is_dtfm && DateTime.Now.Subtract(dtfm_response_timeout).TotalSeconds<90)
                {
                    await Task.Delay(100);
                }
                if(is_dtfm)
                {
                    is_dtfm = false;
                    response = "Không nhận được response trả về.";
                }
                else
                {
                    response = dtfm_response;
                }
            }
            catch(Exception er)
            {
                LoggerManager.LogError("There is error in requesting ussd code with dtfm:"+er.Message);
            }
            return response;
        }

        public async void activateSim(string phone_number,string dtmf)
        {  
            try
            {
                await this.sendAT("ATH\r");
                await Task.Delay(2000);
                await this.sendAT("AT+SPEAKER=1");
                await Task.Delay(500);
                await this.sendAT($"ATD{phone_number};");
                await Task.Delay(15000);
                await this.sendAT($"AT+VTD=6;+VTS={dtmf}");
                await Task.Delay(7000);
                await this.sendAT("ATH\r");
                await Task.Delay(1000);
            }
            catch(Exception er)
            {
                LoggerManager.LogError("There is error in activating Sim" + er.Message);
            }
        }
        public async void HandleSerialData(string input)
        {
            try
            {
                this.lastReportSerialData = DateTime.Now;
                if (input.Contains("+CMGS: ") && this._isSendPort && this.isFailSendSMS)
                {
                    this._isSendPort = false;
                    this.isFailSendSMS = false;
                    this.Balance = "";
                }
                else if (input.Contains("CMS ERROR") && this._isSendPort && this.isFailSendSMS)
                {

                    this._isSendPort = false;
                    this.isFailSendSMS = true;
                }
                if (input.Contains("+CPIN: READY") && !this._isSIM)
                {
                    this.Status = "READY";
                    this._isSIM = true;
                    this.CheckSimReadyHasResult = true;
                    this.Note = loadMsg("SIM IS READY");
                    return;
                }
                else if (input.Contains("+CPIN: READY") && this._isSIM)
                {
                    this.timeOutExist = DateTime.Now;
                }
                if (input.Contains("+CPIN: NOT READY") || (input.Contains("+CME ERROR: 10") && !input.Contains("+CME ERROR: 100")))
                {
                    this._isSIM = true;
                    this.CheckSimReadyHasResult = false;
                    this.CarrierHasResult = false;
                    this.lastReportInfo = DateTime.MinValue;
                    this.lastReportNetwork = DateTime.MinValue;
                    this.lastReportPhone = DateTime.MinValue;
                    this.Phone = "";
                    this.NetWork = "";
                    this.Expire_Date = "";
                    if (this.login_instance.language == "English")
                    {
                        this.Status = "No Sim";
                        this.Note = loadMsg("Sim is unrecognizable");
                    }
                    else
                    {
                        this.Status = "Không có sim.";
                        this.Note = loadMsg("Không thể nhận dạng sim.");
                    }
                    this.Balance = "";
                    this.Sub_Balance = "";
                }

                if (is_modem && (input.Trim().Contains("EC") || input.Trim().Contains("M26")) && !input.Contains("+CMGR"))
                {
                    is_modem = false;
                    this.Modem = input.Trim().Replace("OK", "");
                    if (this.Modem.Contains("M26"))
                    {
                        await this.sendAT("AT+CPMS=\"SM\",\"SM\",\"SM\"");
                        await Task.Delay(100);
                        await this.DeleteAllSmsInbox();
                    }
                    else
                    {
                        await this.sendAT("AT+CPMS=\"ME\",\"ME\",\"ME\"");
                        await Task.Delay(100);
                        await this.DeleteAllSmsInbox();
                    }
                }

                /* if (input.Trim().Length == 2 && input.Trim() == "OK" && is_change_imei == true)
                 {

                     is_change_imei = false;
                     this.Imei = "";
                 }*/
                if (this.imei_regex.Replace(input, "").Trim().Length == 15 && (is_change_imei))
                {
                    is_change_imei = false;
                    this.Imei = this.imei_regex.Replace(input, "").Trim();
                }
                if (input.Contains("+ICCID:") || input.Contains("+CCID:"))
                {
                    string[] iccid_values = input.Replace("OK", "").Replace("\n", "").Replace("\r", "").Replace("f", "").Split(':');
                    string iccid = iccid_values[1];
                    if (!string.IsNullOrEmpty(iccid))
                    {
                        this.Serial_Sim = ConvertStringToDigit(iccid);
                    }
                }
                if (input.Contains("+COPS:"))
                {
                    string str = input.Replace(" ", "").Replace("\n\r", "");

                    this.NetWork = !str.ToUpper().Contains("VIETTEL") ? (!str.ToUpper().Contains("MOBIFONE") ? (!str.ToUpper().Contains("VINAPHONE") ? (!str.ToUpper().Contains("VIETNAMOBILE") ? (!str.ToUpper().Contains("VNSKY") ? "Waiting Carrier" : "VNSKY") : "VIETNAMOBILE") : "VINAPHONE") : "MOBIFONE") : "VIETTEL";
                    if (this.NetWork == "Waiting Carrier")
                    {  if (this.login_instance.language == "English")
                        {
                            this.Note = loadMsg("Cannot read network signal");
                            this.Status = "Waiting for carrier";
                        }
                        else
                        {
                            this.Note = loadMsg("Không thể đọc được tín hiệu từ nhà mạng.");
                            this.Status = "Đang nhận diện nhà mạng.";
                        }
                    }
                    await this.sendAT("ATE0");
                    await Task.Delay(100);
                }
                if (input.Contains("+CUSD:"))
                {
                    if (this._isPhone)
                    {
                        this._isPhone = false;
                        if (this.NetWork == "VIETTEL")
                        {
                            string phone_reg = input.Replace(" ", "").Replace("\n", ",").Replace("\r", "").Replace("\t", "");
                            string[] phone_split = phone_reg.Split(',');
                            phone_split = phone_split.Where(x => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrEmpty(x)).ToArray();
                            string text_clone = "\"YeucaucuaQuykhachkhongduocdapungtaithoidiemnay\"";
                            if (phone_split[1] == text_clone)
                            {
                                this.text = "\"YeucaucuaQuykhachkhongduocdapungtaithoidiemnay\"";
                            }
                            if (check_11_dig)
                            {
                                Regex reg = new Regex("[^0-9]");
                                string[] newip = phone_split[1].Split(':');
                                string phone_eleven = reg.Replace(newip[0], "");
                                this.Phone = phone_eleven;
                            }
                            else
                            {
                                Match match = new Regex(".*?" + char.ConvertFromUtf32(34) + "(\\d+).*?:([0-9\\.]+)d[a-zA-Z0?,:]+(\\d+\\/\\d+\\/\\d+).*").Match(phone_reg);
                                this.Phone = "0" + match.Groups[1].ToString().Substring(2);
                                if (phone_split[1].Contains("."))
                                {
                                    string[] tkc = phone_split[1].Split(':');
                                    Regex reg_digit = new Regex(@"(\d+|\d+\.\d+)d");
                                    Match match_digit = reg_digit.Match(tkc[1]);
                                    if (match_digit.Success)
                                    {
                                        this.Balance = match_digit.Value.Replace("d", "");
                                    }
                                }
                                else
                                {
                                    Regex reg_digit = new Regex(@"(\d+|\d+\.\d+)d");
                                    Match match_digit = reg_digit.Match(phone_split[3]);
                                    if (match_digit.Success)
                                    {
                                        this.Balance = match_digit.Value.Replace("d", "");
                                    }
                                }

                                this.Expire_Date = match.Groups[3].ToString();
                            }
                            if(!this.instance.phoneList.Contains(this.Phone))
                            {
                                this.instance.phoneList.Add(this.Phone);
                            }
                        }
                        else if (this.NetWork == "VINAPHONE")
                        {
                            if ((input.Contains("Dich vu chi ap dung cho thue bao tra truoc") || input.Contains("Da qua so lan cho phep thuc hien tra cuu trong ngay")) && !isPostPaidVnpt)
                            {
                                isPostPaidVnpt = true;
                            }
                            else
                            {
                                this.Phone = "0" + new Regex(".*(\\d{9}).*").Match(input.Replace(" ", "").Replace("\r\n", "").Replace("\n", "")).Groups[1].ToString();
                             
                                phone_secondary_temp = this.Phone;
                                if (isPostPaidVnpt)
                                {  
                                    this.Balance = "0";
                                    this.Expire_Date = "0";
                                    isPostPaidVnpt = false;
                                }
                                else
                                {
                                    this.Balance = "";
                                    this.Expire_Date = "";
                                }
                                if (!this.instance.phoneList.Contains(this.Phone))
                                {
                                    this.instance.phoneList.Add(this.Phone);
                                }
                            }
                        }
                        else if (this.NetWork == "MOBIFONE" || this.NetWork == "VNSKY")
                        {
                            string[] phones = input.Split(',');
                            Regex reg_phone = new Regex(@"\b84\d+\b");
                            string phone_value = phones[1].Replace("\"", "");
                            Match phone_match = reg_phone.Match(phone_value);
                            string phone = "";
                            if (phone_match.Success)
                            {
                                string phone_temp_value = phone_match.Value;
                                phone = "0" + phone_temp_value.Substring(2);
                            }
                            if (!string.IsNullOrEmpty(phone))
                            {
                                this.Phone = phone;
                            }
                            phone_secondary_temp = this.Phone;
                            this.Balance = "";
                            this.Expire_Date = "";
                            if(!this.instance.phoneList.Contains(this.Phone))
                            {
                                this.instance.phoneList.Add(this.Phone);
                            }
                        }
                        else if (this.NetWork == "VIETNAMOBILE")
                        {
                            this.Phone = "0" + new Regex(".*(\\d{11}).*").Match(input.Replace(" ", "").Replace("\r\n", "").Replace("\n", "")).Groups[1].ToString().Substring(2);
                            phone_secondary_temp = this.Phone;
                            this.Balance = "";
                            this.Expire_Date = "";
                            if(!this.instance.phoneList.Contains(this.Phone))
                            {
                                this.instance.phoneList.Add(this.Phone);
                            }
                        }
                        if (this.Phone != "" && this.Phone != "Loading" && (this.Phone.Length == 10 || this.Phone.Length == 11))
                        {
                            this.CarrierHasResult = true;
                            if (this.login_instance.language == "English")
                            {
                                this.Note = loadMsg(this.Port + " has been recognized successfully: Phone:" + this.Phone + " Network:" + this.NetWork);
                            }
                            else
                            {
                                this.Note = loadMsg(this.Port + " đã được nhận dạng thành công: SĐT:" + this.Phone + " Nhà mạng:" + this.NetWork);

                            }
                        }
                        else
                        {  
                            if (this.login_instance.language == "English")
                            {
                                this.Phone = "Loading";
                                this.Note = loadMsg(this.Port + " cannot be recognized.");
                            }
                            else
                            {
                                this.Phone = "Đang tải.";
                                this.Note = loadMsg(this.Port + " không thể nhận diện.");
                            }
                        }
                    }
                    if (this._isInfor)
                    {
                        this._isInfor = false;
                        if (this.NetWork == "MOBIFONE" || this.NetWork == "VNSKY")
                        {
                            if (input.Contains("VNSKY"))
                            {
                                this.NetWork = "VNSKY";
                            }
                            Match match = new Regex("TKC (\\d+) d.* ([\\d+\\/\\d+\\/\\d+]+)").Match(input.Replace("\r\n", "").Replace("\n", "").Replace("-", "/"));
                            this.Balance = match.Groups[1].ToString();
                            this.Expire_Date = match.Groups[2].ToString();
                           string info = this.instance.pushSingleSimInfo(this.Phone, 1);
                           if(!string.IsNullOrEmpty(info))
                            {
                                this.login_instance.ws.sendMessageToServer(info);
                            }
                        }
                        else if (this.NetWork == "VINAPHONE")
                        {
                            if (input.Contains("Tai khoan chinh"))
                            {
                                Regex main_balance_regex = new Regex(@"[\d,]+\s+VND");
                                Match match_main_balance = main_balance_regex.Match(input);
                                if (match_main_balance.Success)
                                {
                                    this.Balance = match_main_balance.Value.Replace("VND", "");
                                }
                                Regex expire_regex = new Regex(@"(\b\d{1,2}\/\d{1,2}\/\d{4}\b)");
                                Match match_expire = expire_regex.Match(input);
                                if (match_expire.Success)
                                {
                                    this.Expire_Date = match_expire.Value;
                                }
                            }
                            else
                            {
                                Match match = new Regex("=(\\d+) VND.* ([\\d+\\/\\d+\\/\\d+]+)").Match(input.Replace("\r\n", "").Replace("\n", ""));
                                this.Balance = match.Groups[1].ToString();
                                string date = match.Groups[0].ToString();
                                string[] dates = date.Split('.');
                                string[] expiration_date = dates[0].Split(',');
                                string expire = expiration_date[1].Replace("HSD", "");
                                this.Expire_Date = expire;
                            }
                           string info= this.instance.pushSingleSimInfo(this.Phone, 1);
                            if(!string.IsNullOrEmpty(info))
                            {
                                this.login_instance.ws.sendMessageToServer(info);
                            }
                        }
                        else if (this.NetWork == "VIETTEL")
                        {
                            string input_refact = input.Replace(" ", "").Replace("\r\n", "").Replace("\n", "");
                            Match match = new Regex(".*?" + char.ConvertFromUtf32(34) + "(\\d+).*?:([0-9\\.]+)d[a-zA-Z0?,:]+(\\d+\\/\\d+\\/\\d+).*").Match(input_refact);
                            this.Phone = "0" + match.Groups[1].ToString().Substring(2);
                            this.Balance = match.Groups[2].ToString();
                            this.Expire_Date = match.Groups[3].ToString();
                        }
                        else if (this.NetWork == "VIETNAMOBILE")
                        {
                            string modified_str = input.Replace("\n", " ");
                            string[] split_values = modified_str.Split(' ');
                            string tkc = "";
                            foreach (string value in split_values)
                            {
                                if (value.Contains("d"))
                                {
                                    tkc = value;
                                    break;
                                }
                            }
                            Regex reg = new Regex("[^0-9.]");
                            tkc = reg.Replace(tkc, "");
                            this.Balance = tkc;
                           string info= this.instance.pushSingleSimInfo(this.Phone, 1);
                            if(!string.IsNullOrEmpty(info))
                            {
                                this.login_instance.ws.sendMessageToServer(info);
                            }
                        }
                    }
                    if (this.isTKKM)
                    {
                        this.isTKKM = false;
                        Regex reg = new Regex(@"(\d+|\d+\.\d+)d");
                        Match match = reg.Match(input);
                        if (match.Success)
                        {
                            string tkkm = match.Value.Replace("d", "");
                            this.Sub_Balance = tkkm;
                        }
                       string info=this.instance.pushSingleSimInfo(this.Phone, 1);
                        if(!string.IsNullOrEmpty(info))
                        {
                            this.login_instance.ws.sendMessageToServer(info);
                        }
                    }
                    if(is_run_ussd_code)
                    {
                        ussd_code_message = input;
                        is_run_ussd_code = false;
                    }
                    if(is_dtfm)
                    {
                        is_dtfm = false;
                        string ussd_response = "";
                        string ussd_message_regex = ("\"(.*?)\"");
                        ussd_response= input;
                        ussd_response = ussd_response.Replace("\n", "").Replace("OK", "");
                        string[] val = ussd_response.Split(',');
                        for (int i = 1; i < val.Length - 1; i++)
                        {
                            if (i < val.Length - 2)
                            {
                                dtfm_response += val[i] + ",";
                            }
                            else
                            {
                                dtfm_response += val[i];
                            }
                        }
                        dtfm_response = removeSpecialCharacters(dtfm_response);
                    }
                }
                if (input.Contains("+CMGR: "))
                {
                    this.current_sms = input;
                    this.smsSuccess = false;
                    if (this.current_sms.Replace("\n", "").Replace("\r", "").EndsWith("OK"))
                    {
                        input = this.current_sms;
                        this.current_sms = string.Empty;
                        this.smsSuccess = true;
                    }
                }
                else if (!string.IsNullOrEmpty(this.current_sms) && !this.smsSuccess)
                {
                    this.current_sms += input;
                    if (input.Replace("\n", "").Replace("\r", "").EndsWith("OK"))
                    {
                        input = this.current_sms;
                        this.current_sms = string.Empty;
                        this.smsSuccess = true;
                    }
                }
                if (input.Contains("+CPAS"))
                {
                    Regex status_number = new Regex("[^\\d]");
                    phoneCallingStatus = status_number.Replace(input, "");
                }
                if (isPhoneCalling && input.Contains("NO CARRIER"))
                {
                    isPhoneCalling = false;
                }
                if (input.Contains("+CMGR:") && this.smsSuccess)
                {
                    this.deleteMessageByIndex();

                    if (this.Modem.Contains("EC2"))
                    {
                        int start_index_1 = input.IndexOf("+CMGR: ") + 7;
                        int start_index_2 = input.IndexOf("\r\n", start_index_1) + 2;
                        int num = input.IndexOf("\r\n", start_index_2 + 1);
                        if (start_index_2 == -1 || num == -1 || num <= start_index_2)
                        {
                            return;
                        }
                        string pdu = input.Substring(start_index_2, num - start_index_2);
                        string msg_content = string.Empty;
                        string origin_address = string.Empty;
                        string date_time_sent = string.Empty;

                        SmsDeliverPdu smsDeliverPdu = (SmsDeliverPdu)IncomingSmsPdu.Decode(pdu, true);

                        msg_content = smsDeliverPdu.UserDataText;

                        origin_address = smsDeliverPdu.OriginatingAddress;

                        date_time_sent = string.Format("{0:dd/MM/yyyy}", smsDeliverPdu.SCTimestamp.ToDateTime()) + " " + string.Format("{0:HH:mm:ss}", smsDeliverPdu.SCTimestamp.ToDateTime());

                        DateTime sms_sent_time = smsDeliverPdu.SCTimestamp.ToDateTime();

                        //int start_index = input.IndexOf("READ\",");

                        //string msg = input.Substring(start_index).Replace("READ\",", "").Trim();

                        //string[] msg_values = msg.Split('\n');

                        //string new_info_msg = msg_values[0];

                        //string msg_content = "";

                        //string[] msg_info = new_info_msg.Split(',');

                        //for (int i = 1; i < msg_values.Length; i++)
                        //{
                        //    msg_content += msg_values[i];
                        //}
                        //msg_content = msg_content.Replace("OK", "").Replace("\r", "").Replace("\n", "").Trim();
                        //string origin_address = msg_info[0].Replace("\"", "").Trim();
                        ///*  SmsDeliverPdu smsDeliverPdu = (SmsDeliverPdu)IncomingSmsPdu.Decode(origin_address, true);
                        //  string address = smsDeliverPdu.OriginatingAddress;
                        //  MessageBox.Show(address);*/
                        //if (Regex.IsMatch(msg_content, @"^[0-9A-Fa-f]+$"))
                        //{
                        //    string hex = msg_content;
                        //    byte[] bytes = Enumerable.Range(0, hex.Length)
                        //        .Where(x => x % 2 == 0)
                        //        .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                        //        .ToArray();
                        //    string content = Encoding.GetEncoding("utf-16BE").GetString(bytes);

                        //    if (!string.IsNullOrEmpty(content))
                        //    {
                        //        msg_content = content.Replace("\n", "").Replace("\r", "");
                        //        //dummy = content.Replace("\n", "").Replace("\r", "");
                        //    }
                        //}

                        //string date_time_sent = (msg_info[2] + "," + msg_info[3]).Replace("\"", "");
                        int first_place = this.Phone.IndexOf("0");
                        string phone_push = this.Phone.Remove(first_place, 1).Insert(first_place, "84");
                        string com = this.Port;
                        Regex regex = new Regex("^[^a-zA-Z0-9]*");
                        string txt1_1 = regex.Replace(msg_content, "");
                        string res = $"{{\"type\":\"add_sms\",\"data\":{{\"phone_sent\":\"{origin_address}\",\"phone_receive\":\"{this.Phone}\",\"sms\":\"{txt1_1}\"}}}}";
                        var res_ob = JsonConvert.DeserializeObject(res);
                        res = JsonConvert.SerializeObject(res_ob, Formatting.Indented);
                        this.login_instance.ws.sendMessageToServer(res);
                        if (msg_content.Contains("OTP"))
                        {
                            string otp_number = otpFilter(txt1_1, origin_address);
                            string re = origin_address + "~" + txt1_1 + "~" + date_time_sent + "~" + phone_push + "~" + com + "~" + otp_number;
                            string re_ver1 = origin_address + "@" + txt1_1 + "@" + date_time_sent + "@" + phone_push + "@" + com + "@" + otp_number;
                          
                            this.Otp = re;
                            
                            this.instance.addNewMessageRow(re);

                            instance.List_Receive.Add(re_ver1);
                    
                        }
                        else
                        {
                            if (origin_address.Equals("123") || origin_address.Equals("+123"))
                            {
                                Regex expire_regex = new Regex(@"(\b\d{1,2}\/\d{1,2}\/\d{4}\b)");
                                Match expire_match = expire_regex.Match(msg_content);

                                if (expire_match.Success)
                                {
                                    this.Expire_Date = expire_match.Value;
                                }
                            }
                            string re = origin_address + "~" + txt1_1 + "~" + date_time_sent + "~" + phone_push + "~" + com;
                            string re_ver1 = origin_address + "@" + txt1_1 + "@" + date_time_sent + "@" + phone_push + "@" + com;
                            this.instance.addNewMessageRow(re);
                            instance.List_Receive.Add(re_ver1);
                        }
                    }
                    else
                    {
                        int start_index_1 = input.IndexOf("+CMGR: ") + 7;
                        int start_index_2 = input.IndexOf("\r\n", start_index_1) + 2;
                        int num = input.IndexOf("\r\n", start_index_2 + 1);
                        if (start_index_2 == -1 || num == -1 || num <= start_index_2)
                        {
                            return;
                        }
                        string pdu = input.Substring(start_index_2, num - start_index_2);
                        string txt1 = string.Empty;
                        string txt2 = string.Empty;
                        string txt3 = string.Empty;
                        SmsDeliverPdu smsDeliverPdu = (SmsDeliverPdu)IncomingSmsPdu.Decode(pdu, true);
                        string otp = smsDeliverPdu.UserDataText;
                        if (otp.Contains("OTP"))
                        {
                            txt1 = smsDeliverPdu.UserDataText;
                            txt2 = smsDeliverPdu.OriginatingAddress;
                            txt3 = string.Format("{0:dd/MM/yyyy}", smsDeliverPdu.SCTimestamp.ToDateTime()) + " " + string.Format("{0:HH:mm:ss}", smsDeliverPdu.SCTimestamp.ToDateTime());
                            Regex regex = new Regex("^[^a-zA-Z0-9]*");
                            string txt1_1 = regex.Replace(txt1, "");
                            string res = $"{{\"type\":\"add_sms\",\"data\":{{\"phone_sent\":\"{txt2}\",\"phone_receive\":\"{this.Phone}\",\"sms\":\"{txt1_1}\"}}}}";
                            var res_ob = JsonConvert.DeserializeObject(res);
                            res = JsonConvert.SerializeObject(res_ob, Formatting.Indented);
                            this.login_instance.ws.sendMessageToServer(res);
                            int first_place = this.Phone.IndexOf("0");
                            string phone_push = this.Phone;
                            if (first_place == 0)
                            {
                                phone_push = this.Phone.Remove(first_place, 1).Insert(first_place, "84");
                            }
                            string com = this.Port;
                            string otp_number = otpFilter(txt1_1, txt2);
                            string re = txt2 + "~" + txt1_1 + "~" + txt3 + "~" + phone_push + "~" + com + "~" + otp_number;
                            string re_ver1 = txt2 + "@" + txt1_1 + "@" + txt3 + "@" + phone_push + "@" + com + "@" + otp_number;
                            this.Otp = re;
                            this.instance.addNewMessageRow(re);
                            instance.List_Receive.Add(re_ver1);
                            LoggerManager.LogTrace(res);
                            return;
                        }
                        else
                        {
                            txt1 = smsDeliverPdu.UserDataText;
                            txt2 = smsDeliverPdu.OriginatingAddress;
                            txt3 = string.Format("{0:dd/MM/yyyy}", smsDeliverPdu.SCTimestamp.ToDateTime()) + " " + string.Format("{0:HH:mm:ss}", smsDeliverPdu.SCTimestamp.ToDateTime());
                            Regex regex = new Regex("^[^a-zA-Z0-9]*");
                            string txt1_1 = regex.Replace(txt1, "");
                            string res = $"{{\"type\":\"add_sms\",\"data\":{{\"phone_sent\":\"{txt2}\",\"phone_receive\":\"{this.Phone}\",\"sms\":\"{txt1}\"}}}}";
                            var res_ob = JsonConvert.DeserializeObject(res);
                            res = JsonConvert.SerializeObject(res_ob, Formatting.Indented);
                            this.login_instance.ws.sendMessageToServer(res);
                            if (string.IsNullOrEmpty(this.Phone))
                            {
                                return;
                            }
                            if (txt2.Equals("123") || txt2.Equals("+123"))
                            {
                                Regex expire_regex = new Regex(@"(\b\d{1,2}\/\d{1,2}\/\d{4}\b)");
                                Match expire_match = expire_regex.Match(txt1);
                                if (expire_match.Success)
                                {
                                    this.Expire_Date = expire_match.Value;
                                }
                            }
                            int first_place = this.Phone.IndexOf("0");
                            string phone_push = "";
                            if (first_place == 0)
                            {
                                phone_push = this.Phone.Remove(first_place, 1).Insert(first_place, "84");
                            }
                            string com = this.Port;
                            string re = txt2 + "@" + txt1_1 + "@" + txt3 + "@" + phone_push + "@" + com;
                            string re_ver1 = txt2 + "@" + txt1_1 + "@" + txt3 + "@" + phone_push + "@" + com;
                            this.instance.addNewMessageRow(re);
                            instance.List_Receive.Add(re_ver1);
                            LoggerManager.LogTrace(res);
                            return;
                        }
                    }
                }
                if (this.Modem.Contains("EC2"))
                {
                    if (!input.Contains("+CMTI: \"ME\","))
                    {
                        return;
                    }
                    this.oldindexsms = int.Parse((input.Split('\n')).FirstOrDefault((y => y.Contains("+CMTI: \"ME\","))).Replace("+CMTI: \"ME\",", ""));
                }
                else
                {
                    if (!input.Contains("+CMTI: \"SM\","))
                    {
                        return;
                    }

                    this.oldindexsms = int.Parse((input.Split('\n')).FirstOrDefault((y => y.Contains("+CMTI: \"SM\","))).Replace("+CMTI: \"SM\",", ""));
                }
                this.ReadSmsInbox();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public Task Work()
        {
            this.delallsms = true;
            return Task.Run(async () =>
            {
                while (!cts.IsCancellationRequested)
                {
                    if (this.rowGSMSelect == null)
                    {
                        this.Note = loadMsg("Port " + this.Port + "cannot connect");
                    }
                    else
                    {  
                        this._isSIM = false;
                        if (this.login_instance.language == "English")
                        {
                            this.Status = "Waiting Sim Card";
                            this.Note = this.Port + " is waiting for Sim";
                        }
                        else
                        {
                            this.Status = "Đang mở cổng";
                            this.Note = this.Port + " đang đợi sim.";
                        }
                        bool try_open_port = this.TryOpenPort();
                        if (!try_open_port)
                        {
                            if (this.login_instance.language == "English")
                            {
                                this.Status = "Port closed";
                                this.Note = this.Port + " has been closed.";
                            }
                            else
                            {
                                this.Status = "Đóng cổng.";
                                this.Note = this.Port + " đã đóng.";
                            }
                        }
                        else
                        {

                            try
                            {
                                while (true)
                                {  if (!lock_task)
                                    {
                                        try
                                        {

                                            if (!this._isSIM)
                                            {
                                                await CheckSimReady();
                                                for (int msWait = 0; !this.CheckSimReadyHasResult && msWait < 3000; msWait += 100)
                                                {
                                                    await Task.Delay(100);
                                                }
                                            }
                                            else
                                            {
                                                if (DateTime.Now.Subtract(this.timeOutExist).TotalSeconds > 3)
                                                {
                                                    if (this.instance.phoneList.Contains(this.Phone))
                                                    {
                                                        this.instance.phoneList.Remove(this.Phone);
                                                        this.instance.temp_phone_list.Remove(this.Phone);
                                                        string remove_sim = this.instance.pushSingleSimInfo(this.Phone, 0);
                                                        if (!string.IsNullOrEmpty(remove_sim))
                                                        {
                                                            this.login_instance.ws.sendMessageToServer(remove_sim);
                                                            LoggerManager.LogTrace(remove_sim);
                                                        }
                                                    }
                                                    this.Port = port;
                                                    this.NetWork = "";
                                                    this.Phone = "";
                                                    if (this.login_instance.language == "English")
                                                    {
                                                        this.Status = "Waiting Sim Card";
                                                        this.Note = port + " is waiting for Sim";
                                                    }
                                                    else
                                                    {
                                                        this.Status = "Đang mở cổng.";
                                                        this.Note = port + " đang đợi sim.";
                                                    }

                                                    this.Balance = "";
                                                    this.Expire_Date = "";
                                                    this.Imei = "";
                                                    this.Balance = "";
                                                    this.Otp = "";
                                                    this.Note = "";
                                                    this.Serial_Sim = "";
                                                    this.Modem = "";
                                                    this.delallsms = true;
                                                    this.CarrierHasResult = false;
                                                    await Task.Delay(1000);
                                                }
                                                if (this.delallsms)
                                                {
                                                    await this.DeleteAllSmsInbox();
                                                    await Task.Delay(100);
                                                    this.delallsms = false;
                                                }
                                                await this.sendAT("AT+CPIN?");
                                                await Task.Delay(100);

                                                if(!this.is_modem && string.IsNullOrEmpty(this.Modem) && DateTime.Now.Subtract(this.lastModemReport).TotalSeconds>5)
                                                {
                                                    this.lastModemReport = DateTime.Now;
                                                    this.is_modem = true;
                                                    await this.sendAT("AT+CGMM");
                                                }
                                                await Task.Delay(100);

                                                if ((this.NetWork == "" || this.NetWork == "Waiting Carrier") && DateTime.Now.Subtract(this.lastReportNetwork).TotalSeconds > 5.0)
                                                {
                                                    this.lastReportNetwork = DateTime.Now;
                                                    await this.sendAT("AT+COPS?");
                                                    await Task.Delay(100);
                                                }
                                                if (string.IsNullOrEmpty(this.Imei) && (!string.IsNullOrEmpty(this.NetWork) && this.NetWork != "Waiting Carrier") && DateTime.Now.Subtract(this.lastReportImei).TotalSeconds > 5.0)
                                                {
                                                    this.lastReportImei = DateTime.Now;
                                                    this.is_change_imei= true;
                                                    await this.sendAT("AT+GSN");
                                                    await Task.Delay(100);
                                                }
                                                if (string.IsNullOrEmpty(this.Serial_Sim) && this.Modem!="" &&  this.NetWork != "" && DateTime.Now.Subtract(this.lastSerialSimReport).TotalSeconds > 5.0)
                                                {
                                                    this.lastSerialSimReport = DateTime.Now;
                                                    if (this.Modem.Contains("EC"))
                                                    {
                                                        await this.sendAT("AT+CCID");
                                                    }
                                                    else
                                                    {
                                                        await this.sendAT("AT+ICCID?");
                                                    }
                                                    await Task.Delay(100);
                                                }
                                                if (!this.CarrierHasResult && this.NetWork != "" && this.NetWork != "Waiting Carrier" && DateTime.Now.Subtract(this.lastReportPhone).TotalSeconds > 10)
                                                {
                                                    if (this.login_instance.language == "English")
                                                    {
                                                        this.Status = "Ready";
                                                        this.Phone = "Loading";
                                                        this.Note = loadMsg(this.Port + " is getting phone info");
                                                    }
                                                    else
                                                    {
                                                        this.Status = "Sẵn sàng";
                                                        this.Phone = "Đang tải";
                                                        this.Note = loadMsg(this.Port + " đang lấy thông tin số điện thoại.");
                                                    }
                                                    this.lastReportPhone = DateTime.Now;
                                                    this._isPhone = true;
                                                    if (this.NetWork == "VIETTEL" && text != "\"YeucaucuaQuykhachkhongduocdapungtaithoidiemnay\"")
                                                    {
                                                        await this.RunUSSD("*101#");
                                                    }
                                                    else if (this.NetWork == "VIETTEL" && text == "\"YeucaucuaQuykhachkhongduocdapungtaithoidiemnay\"")
                                                    {
                                                        check_11_dig = true;
                                                        await this.RunUSSD("*098#");
                                                    }
                                                    else if (this.NetWork == "VINAPHONE")
                                                    {
                                                        if (!isPostPaidVnpt)
                                                        {
                                                         
                                                            await this.RunUSSD("*110#");

                                                        }
                                                        else
                                                        {
                                                            
                                                            await this.RunUSSD("*111#");

                                                        }
                                                    }
                                                    else if (this.NetWork == "MOBIFONE" || this.NetWork == "VNSKY")
                                                    {
                                                        await this.RunUSSD("*0#");
                                                    }
                                                    else if (this.NetWork == "VIETNAMOBILE")
                                                    {
                                                        await this.RunUSSD("*123#");
                                                    }
                                                    for (int wait = 0; !this.CarrierHasResult && wait < 1000; wait += 100)
                                                    {
                                                        await Task.Delay(100);
                                                    }
                                                }
                                                if ((this.Balance == "") && this.NetWork != "" && this.NetWork != "Sim tạm thời bị khóa hoặc không có sóng" && DateTime.Now.Subtract(this.lastReportInfo).TotalSeconds > 10 && this.Phone != "" && this.Phone != "Loading" && this.Phone != "Đang tải")
                                                {
                                                    this.lastReportInfo = DateTime.Now;
                                                    this._isInfor = true;
                                                    if (this.NetWork == "MOBIFONE" || this.NetWork == "VINAPHONE" || this.NetWork == "VIETNAMOBILE" || this.NetWork == "VIETTEL" || this.NetWork == "VNSKY")
                                                    {
                                                        await this.RunUSSD("*101#");
                                                        if (this.NetWork == "VIETNAMMOBILE")
                                                        {
                                                            await this.SendMessage("1414", "TTTB");
                                                        }
                                                    }
                                                    await Task.Delay(100);
                                                }
                                                if (!string.IsNullOrEmpty(this.Balance) && !string.IsNullOrEmpty(this.Expire_Date) && !string.IsNullOrEmpty(this.NetWork) && !string.IsNullOrEmpty(this.Phone) && (this.Phone != "Loading" || this.Phone != "Đang tải") && string.IsNullOrEmpty(this.Sub_Balance) && DateTime.Now.Subtract(this.lastTKKMReport).TotalSeconds > 10)
                                                {
                                                    this.lastTKKMReport = DateTime.Now;
                                                    this.isTKKM = true;
                                                    if (this.NetWork == "VIETTEL")
                                                    {
                                                        await this.RunUSSD("*102#");
                                                    }
                                                    await Task.Delay(100);
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }
                                    else
                                    {
                                        timeOutExist = DateTime.Now;
                                    }
                                }
                            }
                            catch (Exception er)
                            {
                                LoggerManager.LogError(er.Message);
                            }

                        }
                    }
                }
            });
        }
    }
}
