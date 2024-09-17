using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.ResponseItem
{
    public class LoginItem
    {
       public Extra_Data Data { get; set; }
    }
    public class Extra_Data
    {
        public string Token { get; set; }
        public UserInfo User { get; set; }

    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Balance { get; set; }
        public string Created_Format { get; set; }
    }
}
