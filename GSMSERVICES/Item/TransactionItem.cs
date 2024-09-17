using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class TransactionItem
    {
      public ResponseData Data { get; set; }
      public string Message { get; set; }
    }
    public class ResponseData
    {
        public string Trans_Id { get; set; }
        public int Response { get; set; }
    }
}
