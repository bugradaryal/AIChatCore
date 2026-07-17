using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAiResponse
    {
        public string? ai_message { get; set; }
        public DateTime ai_message_date { get; set; }
        public string? title { get; set; }
    }
}
