using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Follow
{
    public int FollowId { get; set; }

    public string SellerId { get; set; } = null!;

    public string FollowerId { get; set; } = null!;

    public virtual Account Seller { get; set; } = null!;

    public virtual ICollection<ShopNotification> ShopNotifications { get; set; } = new List<ShopNotification>();
}
