using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AiMessage
    {
        public int id { get; set; }
        public string? ai_message { get; set; }
        public DateTime ai_message_date { get; set; }
        public int UserMessageId { get; set; }
        public UserMessage? userMessage { get; set; }
    }
}
