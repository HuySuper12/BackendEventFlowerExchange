using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class ShopNotification
{
    public int ShopNotificationId { get; set; }

    public string? FollowerId { get; set; }

    public string? SellerId { get; set; }

    public int? ProductId { get; set; }

    public string? Content { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Follow? Follow { get; set; }

    public virtual Account? Follower { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Account? Seller { get; set; }

}
