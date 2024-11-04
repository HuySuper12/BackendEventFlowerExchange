using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class ProductStatistics
    {
        public int Order { get; set; }

        public double Rating { get; set; }

        public int EnableProducts { get; set; }

        public int SoldOut { get; set; }

        public int AllProduct { get; set; }
    }
}
