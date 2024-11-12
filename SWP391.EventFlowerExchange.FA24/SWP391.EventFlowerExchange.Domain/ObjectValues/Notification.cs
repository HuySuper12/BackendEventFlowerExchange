using System;
using System.Collections.Generic;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string? UserId { get; set; }

    public string? Content { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? User { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
