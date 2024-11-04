using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CheckOutAfter
    {
        public decimal? SubTotal { get; set; }
        public decimal? Ship { get; set; }
        public decimal? Total { get; set; }
        public decimal? Discount { get; set; }
    }
}
