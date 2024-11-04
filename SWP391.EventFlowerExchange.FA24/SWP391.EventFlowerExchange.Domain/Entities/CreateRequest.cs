using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateRequest
    {
        public int? RequestId { get; set; } //withdraw

        public string? UserId { get; set; }// chung

        public string? RequestType { get; set; } //chung

        public int? PaymentId { get; set; } //post

        public decimal? Amount { get; set; } //withdraw

        public int? ProductId { get; set; } //post

        public string? Status { get; set; } //chung

        public DateTime? CreatedAt { get; set; } //chung

        public string? Reason { get; set; }//withdraw

    }
}
