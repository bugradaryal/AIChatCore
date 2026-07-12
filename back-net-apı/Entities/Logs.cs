using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Logs
    {
        public int id { get; set; }
        public IPAddress? ip_adress { get; set; }
        public int? UserMessageId { get; set; }
        public int? AiMessageId { get; set; }
        public DateTime date { get; set; }
        public string? prop { get; set; }   // artık "durum" (Success/Timeout) için, title değil
    }
}
