using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSMSERVICES.GSM
{
    public class UpdateGUI
    {
        public static event UIViewRow dataRow;
        public static void ChangeRow(ListViewItem item, string name, string value) => dataRow(item,name,value);
    }
}
