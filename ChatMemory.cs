using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class ChatMemory
    {
        public Dictionary<string,string> UserMemory= new Dictionary<string,string>();
        public string LastTopic { get; set; }
    }
}
