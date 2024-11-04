using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public class FollowRepository : IFollowRepository
    {
        private Swp391eventFlowerExchangePlatformContext _context;

        public FollowRepository(Swp391eventFlowerExchangePlatformContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> AddNewFollowerAsync(CreateFollower follower)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var foAccount = _context.Accounts.FirstOrDefault(x => x.Id == follower.FollowerEmail);

            if (foAccount == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Follower not found." });
            }

            /*var noti = new ShopNotification
            {
                SellerId = follower.SellerEmail,        //FollowerEmail chứa giá trị id
                FollowerId = follower.FollowerEmail,    //SellerEmail chứa giá trị id
                Content = $"{foAccount.Name} has followed you.",
                CreatedAt = DateTime.UtcNow,
                Status = "enable"
            };*/

            var fo = new Follow()
            {
                FollowerId = follower.FollowerEmail,        // FollowerEmail chứa giá trị id
                SellerId = follower.SellerEmail             // SellerEmail chứa giá trị id
            };

            /*await _context.ShopNotifications.AddAsync(noti);*/
            await _context.Follows.AddAsync(fo);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<List<Follow>> GetFollowerListAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var list = await _context.Follows
                .Where(x => x.SellerId == account.Id).ToListAsync();
            if (list != null)
            {
                return list;
            }

            return null;
        }

        public async Task<IdentityResult> RemoveFollowerAsync(ShopNotification follower)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var removeFollower = await _context.Follows
                .FirstOrDefaultAsync(x => x.SellerId == follower.SellerId && x.FollowerId == follower.FollowerId);

            if (removeFollower == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Follower not found." });
            }

            var notificationsToRemove = _context.ShopNotifications
        .Where(n => n.FollowerId == follower.FollowerId && n.SellerId == follower.SellerId);

            _context.ShopNotifications.RemoveRange(notificationsToRemove);

            _context.Follows.Remove(removeFollower);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = "Failed to remove follower." });
        }

        public async Task<int> GetCountFollowByUserEmailAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var count = await _context.Follows
                .Where(x => x.SellerId == account.Id).CountAsync();
            return count;
        }

        public async Task<IdentityResult> CheckFollowByUserEmailAsync(Account follower, Account seller)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var follow = await _context.Follows
                .FirstOrDefaultAsync(x => x.FollowerId == follower.Id && x.SellerId == seller.Id);

            if (follow != null)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }
    }
}
