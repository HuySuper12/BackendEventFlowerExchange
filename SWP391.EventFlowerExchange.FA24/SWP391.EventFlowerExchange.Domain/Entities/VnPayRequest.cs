using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class VnPayRequest
    {
        public string? UserId { get; set; }

        public string? Email { get; set; }

        public double Amount { get; set; }

        public string Type { get; set; }

        public int RequestId { get; set; }

        public DateTime CreateDate { get; set; }


    }
}
