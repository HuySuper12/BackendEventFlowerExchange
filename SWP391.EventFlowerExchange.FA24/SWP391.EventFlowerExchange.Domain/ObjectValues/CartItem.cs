using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class CartItem
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public string BuyerId { get; set; } = null!;

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ProductImage { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
