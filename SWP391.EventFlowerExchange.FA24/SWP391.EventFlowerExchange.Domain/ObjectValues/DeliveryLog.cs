using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class DeliveryLog
{
    public int LogId { get; set; }

    public int? OrderId { get; set; }

    public string? DeliveryPersonId { get; set; }

    public string? Status { get; set; }

    public string? Reason { get; set; }

    public string? PhotoUrl { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? TakeOverAt { get; set; }
    public DateTime? DeliveryAt { get; set; }

    public virtual Account? DeliveryPerson { get; set; }

    public virtual Order? Order { get; set; }
}
