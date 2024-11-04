using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Product
{
    public int ProductId { get; set; }

    public string? SellerId { get; set; }

    public string? ProductName { get; set; }

    public int? FreshnessDuration { get; set; }

    public string? ComboType { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Account? Seller { get; set; }

    public virtual ICollection<ShopNotification> ShopNotifications { get; set; } = new List<ShopNotification>();
}
