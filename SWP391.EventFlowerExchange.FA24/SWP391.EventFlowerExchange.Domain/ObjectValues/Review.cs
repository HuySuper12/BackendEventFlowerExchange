using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? OrderId { get; set; }

    public string? BuyerId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Buyer { get; set; }

    public virtual Order? Order { get; set; }
}
