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
    public interface IFollowRepository
    {
        public Task<List<Follow>> GetFollowerListAsync(Account account);
        public Task<IdentityResult> AddNewFollowerAsync(CreateFollower follower);
        public Task<IdentityResult> RemoveFollowerAsync(ShopNotification follower);
        public Task<int> GetCountFollowByUserEmailAsync(Account account);
        public Task<IdentityResult> CheckFollowByUserEmailAsync(Account follower, Account seller);
    }
}
