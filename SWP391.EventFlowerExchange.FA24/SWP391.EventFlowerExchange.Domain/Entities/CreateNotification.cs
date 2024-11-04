using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Domain.Entities
{
    public class CreateNotification
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        [MaxLength(700)]  // Giới hạn độ dài của nội dung thông báo
        public string Content { get; set; }
    }
}
