using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public class Account : IdentityUser
{
    //public int UserId { get; set; }//

    public string? Name { get; set; }

    //public string? Email { get; set; }//

    //public string? Password { get; set; }//

    //public string? Phone { get; set; }//

    //public string? Role { get; set; }
    public string? Address { get; set; }

    public double? Salary { get; set; }

    public decimal? Balance { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? Status { get; set; }
    public string? OtpCode { get; set; }
    public DateTime? OtpExpiration { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<DeliveryLog> DeliveryLogs { get; set; } = new List<DeliveryLog>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> OrderBuyers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderDeliveryPeople { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSellers { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<ShopNotification> ShopNotificationFollowers { get; set; } = new List<ShopNotification>();

    public virtual ICollection<ShopNotification> ShopNotificationSellers { get; set; } = new List<ShopNotification>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
