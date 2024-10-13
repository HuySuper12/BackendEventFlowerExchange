using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues
{
    public class ImageProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string? LinkImage { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
