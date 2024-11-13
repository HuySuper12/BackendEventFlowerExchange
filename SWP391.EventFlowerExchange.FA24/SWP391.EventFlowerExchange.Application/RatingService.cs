using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.EventFlowerExchange.Application
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repo;

        public RatingService(IRatingRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CheckRatingOrderByOrderIdFromAPIAsync(Account buyer, Order order)
        {
            return await _repo.CheckRatingOrderByOrderIdAsync(buyer, order);
        }

        public async Task<IdentityResult> PostRatingFromApiAsync(CreateRating rating)
        {
            return await _repo.PostRatingAsync(rating);
        }

        public async Task<List<Review>> ViewAllRatingByUserIdFromApiAsync(Account account)
        {
            return await _repo.ViewAllRatingByUserIdAsync(account);
        }

        public async Task<Review> ViewRatingByOrderIdFromAPIAsync(Order order)
        {
            return await _repo.ViewRatingByOrderIdAsync(order);
        }


    }
}
