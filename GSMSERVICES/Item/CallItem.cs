using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class CallItem
    {
       public CallDetail data { get; set; }
    }
    public class CallDetail
    {
        public string Phone { get; set; }
        public string Call_To { get; set; }
        public string Timer { get; set; }
        public string Tran_Id { get; set; }
    }
    
}
