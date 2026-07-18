using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAiResponse
    {
        public int user_id { get; set; }
        public int ai_id { get; set; }
        public int session_id { get; set; }
        public string? ai_message { get; set; }
        public DateTime ai_message_date { get; set; }
        public string? title { get; set; }
    }
}
