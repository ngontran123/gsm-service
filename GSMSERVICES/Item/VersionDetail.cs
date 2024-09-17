using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMSERVICES.Item
{
    public class VersionDetail
    {
        public VersionData Data { get; set; }
    }
    public class VersionData
    {
        public string Min_Version { get; set; }
        public string Current_Version { get; set;}
    }
}
