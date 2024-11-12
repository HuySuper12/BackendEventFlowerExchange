using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Message
{
    public int MessageId { get; set; }

    public string SenderId { get; set; } = null!;

    public string ReceiverId { get; set; } = null!;

    public string? Contents { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int NotificationId { get; set; }

    public virtual Notification Notification { get; set; } = null!;

    public virtual Account Sender { get; set; } = null!;
}
