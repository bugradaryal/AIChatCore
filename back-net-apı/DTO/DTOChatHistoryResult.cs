using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOChatHistoryResult
    {
        public ICollection<DTOChatHistory>? History { get; set; }
        public string? Title { get; set; }
    }
}