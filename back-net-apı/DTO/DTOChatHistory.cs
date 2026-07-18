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
        public int? id { get; set; }
        public string? message { get; set; }
        public ERoles role { get; set; }
        public DateTime date { get; set; }
    }
}
