using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.ObjectValues
{
    public partial class Payment
    {
        public int PaymentId { get; set; }

        public string? PaymentCode { get; set; }

        public string? UserId { get; set; }

        public int? PaymentType { get; set; }

        public string? PaymentContent { get; set; }

        public decimal? Amount { get; set; }

        public bool? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

        public virtual Account? User { get; set; }
    }
}