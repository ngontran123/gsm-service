using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class SimDetail
    {
        public string Phone { get; set; }
        public int Telco { get; set; }
        public string Imei { get; set; }
        public string Com { get; set; }
        public string Tkc { get; set; }
        public string Hsd { get; set; }
        public string Serial { get; set; }
        public SimDetail(string phone,int telco,string imei,string com,string tkc,string hsd,string serial)
        {
            this.Phone = phone;
            this.Telco = telco;
            this.Imei = imei;
            this.Com = com;
            this.Tkc = tkc;
            this.Hsd = hsd;
            this.Serial = serial;
        }
    }
    
}
