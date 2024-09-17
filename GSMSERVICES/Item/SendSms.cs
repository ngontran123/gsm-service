using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class SendSms
    {
        public SmsDetail Data { get; set; }
    }
    public class SmsDetail
    {
        public string Phone { get; set; }
        public string Phone_Sent { get; set; }
        public string Content { get; set; }
        public string Tran_Id { get; set; }
    }
}
