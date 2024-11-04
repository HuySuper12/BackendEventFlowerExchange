using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Order
{
    public int OrderId { get; set; }

    public string? BuyerId { get; set; }

    public string? SellerId { get; set; }

    public string? DeliveryPersonId { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public string? DeliveredAt { get; set; }

    public string? IssueReport { get; set; }
    public string? PhoneNumber { get; set; }

    public int? VoucherId { get; set; }

    public virtual Account? Buyer { get; set; }

    public virtual ICollection<DeliveryLog> DeliveryLogs { get; set; } = new List<DeliveryLog>();

    public virtual Account? DeliveryPerson { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Account? Seller { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Voucher? Voucher { get; set; }
}
