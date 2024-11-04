using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public string? TransactionCode { get; set; }

    public int? OrderId { get; set; }

    public string? UserId { get; set; }

    public int? TransactionType { get; set; }

    public string? TransactionContent { get; set; }

    public decimal? Amount { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Account? User { get; set; }
}
