using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class ChangeImei
    {
        public ImeiDetail Data { get; set; }
    }
    public class ImeiDetail
    {
        public string Phone { get; set; }
        public string Imei { get; set; }
        public string Tran_Id { get; set; }
    }
}
