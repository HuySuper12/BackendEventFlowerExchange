using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateMessage
    {
        public string SenderEmail { get; set; }

        public string ReveiverEmail { get; set; }

        public string? Contents { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
