using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;

namespace DTO
{
    public class DTOChatHistory
    {
        public string? message { get; set; }
        public Roles role { get; set; }
        public DateTime date { get; set; }
    }
}
