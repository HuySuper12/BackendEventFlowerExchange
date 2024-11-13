using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IRatingRepository
    {
        public Task<List<Review>> ViewAllRatingByUserIdAsync(Account account);
        public Task<IdentityResult> PostRatingAsync(CreateRating rating);
        public Task<Review> ViewRatingByOrderIdAsync(Order order);
        public Task<bool> CheckRatingOrderByOrderIdAsync(Account buyer, Order order);
    }
}
