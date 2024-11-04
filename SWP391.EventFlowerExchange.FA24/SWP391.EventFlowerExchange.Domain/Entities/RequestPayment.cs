using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class RequestPayment
    {
        public string? UserEmail { get; set; }

        public string? RequestType { get; set; }

        public decimal? Amount { get; set; }

    }
}
