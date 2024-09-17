using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class CheckPort
    {
        public PortDetail Data { get; set; }
        public int Error_Code { get; set; }
        public string Message { get; set; }
    }
    public class PortDetail
    {
        public string Phone { get; set; }
        public string Tran_Id { get; set; }

    }
}
