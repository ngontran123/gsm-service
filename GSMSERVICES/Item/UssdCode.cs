using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class UssdCode
    {
        public UssdCodeDetail Data { get; set; }
    }
    public class UssdCodeDetail
    {
        public string Phone { get; set; }
        public string Ussd_Content { get; set; }
        public string Tran_Id { get; set; }
        public string Unlock_Topup { get; set; }
        public string Extra { get; set; }
    }

}
