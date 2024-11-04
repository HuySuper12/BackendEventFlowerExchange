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
    public class FollowService : IFollowService
    {
        private IFollowRepository _repo;
        private IAccountRepository _accountRepository;

        public FollowService(IFollowRepository repo, IAccountRepository accountRepository)
        {
            _repo = repo;
            _accountRepository = accountRepository;
        }

        public async Task<IdentityResult> AddNewFollowerFromApiAsync(CreateFollower follower)
        {
            Account check = new Account() { Id = follower.FollowerEmail };

            var result = _accountRepository.GetUserByIdAsync(check);
            if (result == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = "User not found" });
            }

            return await _repo.AddNewFollowerAsync(follower);
        }

        public async Task<List<Follow>> GetFollowerListFromApiAsync(Account account)
        {
            Account check = new Account() { Id = account.Id };

            var result = _accountRepository.GetUserByIdAsync(check);
            if (result == null)
            {
                return null;
            }

            return await _repo.GetFollowerListAsync(account);
        }

        public async Task<IdentityResult> RemoveFollowerFromApiAsync(ShopNotification follower)
        {
            return await _repo.RemoveFollowerAsync(follower);
        }

        public Task<int> GetCountFollowByUserEmailFromApiAsync(Account account)
        {
            return _repo.GetCountFollowByUserEmailAsync(account);
        }

        public Task<IdentityResult> CheckFollowByUserEmailFromApiAsync(Account follower, Account seller)
        {
            return _repo.CheckFollowByUserEmailAsync(follower, seller);
        }
    }
}
