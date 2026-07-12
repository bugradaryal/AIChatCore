using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserMessage
    {
        public int id { get; set; }
        public string? user_message { get; set; }
        public DateTime user_message_date { get; set; }
        public int? SessionId { get; set; }
        public Session? session { get; set; }
        public AiMessage? aiMessage { get; set; }
    }
}
