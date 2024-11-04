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
    public interface IFollowService
    {
        public Task<List<Follow>> GetFollowerListFromApiAsync(Account account);
        public Task<IdentityResult> AddNewFollowerFromApiAsync(CreateFollower follower);
        public Task<IdentityResult> RemoveFollowerFromApiAsync(ShopNotification follower);
        public Task<int> GetCountFollowByUserEmailFromApiAsync(Account account);
        public Task<IdentityResult> CheckFollowByUserEmailFromApiAsync(Account follower, Account seller);
    }
}
