using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Session
    {
        public int id { get; set; }
        public string? title { get; set; }
        public DateTime created_date { get; set; }
    }
}
