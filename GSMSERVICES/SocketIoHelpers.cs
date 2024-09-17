using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using Quobject.SocketIoClientDotNet.Client;
using WebSocketSharp;
using Newtonsoft;
using Newtonsoft.Json;
using GSMSERVICES.ResponseItem;
using Quobject.Collections.Immutable;
using GSMSERVICES.Item;
using System.ComponentModel.Design;
using GSMSERVICES.Services;
using System.Security.Permissions;

namespace GSMSERVICES
{
    public class SocketIoHelpers
    {
      public string access_token = "";
      public string device_id = "";
      public string type = "";
      public WebSocket ws;
      public bool is_connect = false;
      private System.Timers.Timer recheck_connect_timer = new System.Timers.Timer();
      public bool is_login = false;
      public bool is_resend_login = false;
      public int login_status;
      public string login_message = "Cannot make a connection to server.Please recheck your internet connection.";
      private string data_recevice = "";
      private int current_agent_version = 2 ;
      public int canAccess = -1;
      public bool is_external_login = false;
      public bool resend_list_phone = false;
      public ImmutableList<string> Report_Port_List = ImmutableList<string>.Empty;
      public ImmutableList<string> Change_Imei_List = ImmutableList<string>.Empty;
      public ImmutableList<string> Send_Sms_List = ImmutableList<string>.Empty;
      public ImmutableList<string> Ussd_Code_List = ImmutableList<string>.Empty;
      public ImmutableList<string> Call_List = ImmutableList<string>.Empty;
      
        public SocketIoHelpers()
        {
           
        }
        public SocketIoHelpers(string language)
        {
          
            if (language!="English")
            {
                login_message = "Mất kết nối với internet.Vui lòng kiểm tra lại kết nối của bạn.";
            }
        }
       
        public void connectWithServer(string url)
        {
            try
            {
                ws = new WebSocket(url);
                ws.OnOpen += Ws_OnOpen;
                ws.OnMessage += Ws_OnMessage;
                ws.OnError += Ws_OnError;
                ws.OnClose += Ws_OnClose;
                ws.ConnectAsync();
                
            }
            catch(WebSocketException wse)
            {
                LoggerManager.LogError(wse.Message);
            }
        }
        private void Ws_OnOpen(object sender, EventArgs e)
        {
            is_connect = true;
            LoggerManager.LogInfo("Connection has been established.");
        }
        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            is_connect = false;
            LoggerManager.LogInfo("Connection has been closed.");
        }
        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            is_connect = false;
            LoggerManager.LogError("There is error during the connection:" + e.Message);
        }
        private void Ws_OnMessage(object sender, MessageEventArgs e) 
        {
            try
            {
                data_recevice = e.Data;
            }
            catch (Exception er)
            {
                LoggerManager.LogError(er.Message); 
            }
            try
            {    
                if(!string.IsNullOrEmpty(data_recevice))
                {

                    if (data_recevice.Contains("error_code"))
                    {
                        if (data_recevice.Contains("type"))
                        {
                            if (Properties.Settings.Default.repushTransactions == null)
                            {
                                Properties.Settings.Default.repushTransactions = new System.Collections.Specialized.StringCollection();
                            }
                            var response_message = JsonConvert.DeserializeObject<CommandType>(data_recevice);
                           
                            
                            if (response_message!=null)
                            {
                                string command = response_message.Type.Trim();
                                if(!command.Contains("add_port") || command.Contains("check_version"))
                                {
                                    var response_trans_id = JsonConvert.DeserializeObject<TransactionItem>(data_recevice);
                                    if (response_trans_id != null)
                                    {
                                        string trans_id = response_trans_id.Data.Trans_Id;

                                        int response = response_trans_id.Data.Response;
                                        if (response == 1)
                                        {
                                            Properties.Settings.Default.repushTransactions.Add(trans_id);
                                        }
                                    }
                                }
                                if(command.Equals("check_port"))
                                {
                                    handleEvent(TypeList.CHECK_PORT, data_recevice);
                                    
                                }
                               else if(command.Contains("add_port"))
                                {
                                    var error_ob = JsonConvert.DeserializeObject<ErrorCodeItem>(data_recevice);
                                    if(error_ob != null)
                                    {
                                        int error_code = error_ob.Error_Code;
                                        if(error_code == 0)
                                        {
                                           string error_message = error_ob.Message;
                                           DialogResult mess=MessageBox.Show(error_message,"Lỗi đăng nhập",MessageBoxButtons.OK,MessageBoxIcon.Error);
                                           if(mess == DialogResult.OK)
                                            {
                                                Environment.Exit(Environment.ExitCode);
                                            }
                                        }
                                    }
                                }
                                else if(command.Equals("change_imei"))
                                {
                                    handleEvent(TypeList.CHANGE_IMEI, data_recevice);
                                }
                                else if(command.Equals("send_sms"))
                                {
                                    handleEvent(TypeList.SEND_SMS, data_recevice);
                                }
                                else if(command.Equals("ussd_code"))
                                {
                                    handleEvent(TypeList.USSD_CODE, data_recevice);
                                }
                                else if(command.Equals("check_version"))
                                {
                                    handleEvent(TypeList.CHECK_VERSION, data_recevice);
                                }
                                else if(command.Equals("call"))
                                {
                                    handleEvent(TypeList.CALL, data_recevice);
                                }
                            }
                        }
                        else
                        {
                            var response_message = JsonConvert.DeserializeObject<ErrorCodeItem>(data_recevice);
                            if (response_message != null)
                            {
                                int error_code = response_message.Error_Code;
                                string message = response_message.Message;
                                message = message.ToLower();
                          
                                if (error_code == 0 && message.Contains("thiết bị khác"))
                                {
                                    is_external_login = true;                       
                                /*    DialogResult mess = MessageBox.Show(message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    if (mess == DialogResult.OK)
                                    {
                                        Environment.Exit(Environment.ExitCode);

                                    }*/
                                }
                                this.login_status = error_code;
                                this.login_message = message;
                            }
                            response_message = null;
                        }
                    }                    
                }
                data_recevice = null;
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
        }

        public bool checkConnectAlive()
        {   
            if (ws == null)
            {
                return false;
            }
            return ws.IsAlive && ws.ReadyState == WebSocketState.Open;
        }
        public void handlingDisconnect()
        {
            recheck_connect_timer.Interval = 15000;
            recheck_connect_timer.Elapsed += async (send, env) =>
            {
                await Task.Run(async () =>
                {
                    if (!checkConnectAlive() && ws !=null)
                    {
                        ws.ConnectAsync();
                    }
                   else
                    {
                            is_connect = true;
                            resend_list_phone = true;
                            string token = Environment.GetEnvironmentVariable("TOKEN");
                            string connect_callback = $"{{\"type\":\"connect\",\"message\":\"Đang kết nối\"}}";
                            var json_object = JsonConvert.DeserializeObject(connect_callback);
                            connect_callback=JsonConvert.SerializeObject(json_object,Formatting.Indented);
                            sendMessageToServer(connect_callback);
                            await Task.Delay(300);                                                          
                    }
                });
            };     
            recheck_connect_timer.Enabled = true;
            recheck_connect_timer.Start();
                 
        }          
        public void sendMessageToServer(string message)
        {
            try
            {  if (ws != null)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        this.ws.Send(message);
                    }
                }
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
        }

     private enum TypeList
        {
            CHECK_PORT,
            CHANGE_IMEI,
            SEND_SMS,
            USSD_CODE,
            CHECK_VERSION,
            CALL
        }
     private void handleEvent(TypeList command,string data_receive)
        {
            try
            {
                switch (command)
                {
                    case TypeList.CHECK_PORT:
                        Report_Port_List=Report_Port_List.Add(data_receive);
                        break;
                    case TypeList.CHANGE_IMEI:
                        Change_Imei_List=Change_Imei_List.Add(data_receive);
                        break;
                    case TypeList.SEND_SMS:
                        Send_Sms_List=Send_Sms_List.Add(data_receive);
                        break;
                    case TypeList.USSD_CODE:
                        Ussd_Code_List = Ussd_Code_List.Add(data_receive);
                        break;
                    case TypeList.CHECK_VERSION:
                        var version_ob = JsonConvert.DeserializeObject<VersionDetail>(data_receive);
                        if(version_ob != null)
                        {
                          

                            Environment.SetEnvironmentVariable("MIN_VERSION", version_ob.Data.Min_Version);
                            Environment.SetEnvironmentVariable("CURRENT_VERSION", version_ob.Data.Current_Version);
                            CheckVersionService version_service=new CheckVersionService();
                            if (version_service.checkVersion(current_agent_version))
                            {
                                canAccess = 1;
                            }
                            else
                            {
                                canAccess = 0;
                            }
                        }
                        break;
                    case TypeList.CALL:
                        Call_List=Call_List.Add(data_receive); 
                        break;
                }
            }
            catch(Exception er)
            {
                LoggerManager.LogError(er.Message);
            }
        }
    }
}
