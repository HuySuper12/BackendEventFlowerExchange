using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateFollower
    {
        [Required(ErrorMessage = "FollowerId is required.")]
        public string? FollowerEmail { get; set; }

        [Required(ErrorMessage = "SellerId is required.")]
        public string? SellerEmail { get; set; }
    }
}
