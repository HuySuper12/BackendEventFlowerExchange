using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateShopNotification
    {
        [Required(ErrorMessage = "FollowerId is required.")]
        public string? FollowerẸmail { get; set; }

        [Required(ErrorMessage = "SellerId is required.")]
        public string? SellerEmail { get; set; }

        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
        public string? Content { get; set; }
    }
}
