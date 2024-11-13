using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public interface IRatingService
    {
        public Task<List<Review>> ViewAllRatingByUserIdFromApiAsync(Account account);
        public Task<IdentityResult> PostRatingFromApiAsync(CreateRating rating);
        public Task<Review> ViewRatingByOrderIdFromAPIAsync(Order order);

        public Task<bool> CheckRatingOrderByOrderIdFromAPIAsync(Account buyer, Order order);
    }
}
