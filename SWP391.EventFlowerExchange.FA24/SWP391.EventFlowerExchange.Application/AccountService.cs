using Microsoft.AspNetCore.Identity;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;

namespace SWP391.EventFlowerExchange.Application
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _repo;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<Account> _userManager;

        public AccountService(IAccountRepository repo, UserManager<Account> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repo = repo;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //SignIn
        public async Task<string> SignInFromAPIAsync(SignIn signIn)
        {
            return await _repo.SignInAsync(signIn);
        }

        public async Task<string> SignInEmailFromAPIAsync(SignIn signIn)
        {
            return await _repo.SignInEmailAsync(signIn);
        }

        //SignUp
        public async Task<IdentityResult> SignUpBuyerFromAPIAsync(SignUpBuyer signUp)
        {
            return await _repo.SignUpBuyerAsync(signUp);
        }
        public async Task<IdentityResult> SignUpSellerFromAPIAsync(SignUpSeller signUp)
        {
            return await _repo.SignUpSellerAsync(signUp);
        }
        public async Task<IdentityResult> CreateStaffAccountFromAPIAsync(SignUpStaff staff)
        {
            return await _repo.CreateStaffAccountAsync(staff);
        }
        public async Task<IdentityResult> CreateShipperAccountFromAPIAsync(SignUpShipper shipper)
        {
            return await _repo.CreateShipperAccountAsync(shipper);
        }

        //SendOTP
        public async Task<bool> SendOTPFromAPIAsync(string email)
        {
            return await _repo.SendOTPAsync(email);
        }

        public async Task<bool> VerifyOTPFromAPIAsync(string email, string otp)
        {
            return await _repo.VerifyOTPAsync(email, otp);
        }

        //Reset password
        public async Task<bool> ResetPasswordFromAPIAsync(string email, string newPassword)
        {
            return await _repo.ResetPasswordAsync(email, newPassword);
        }

        //CRUD Account
        public async Task<Account> GetUserByEmailFromAPIAsync(Account account)
        {
            return await _repo.GetUserByEmailAsync(account);
        }

        public async Task<Account> GetUserByIdFromAPIAsync(Account account)
        {
            return await _repo.GetUserByIdAsync(account);
        }

        public async Task<IdentityResult> UpdateAccountFromAPIAsync(Account account)
        {
            return await _repo.UpdateAccountAsync(account);
        }
        public async Task<IdentityResult> DeleteAccountFromAPIAsync(Account account)
        {
            var userRoles = await _userManager.GetRolesAsync(account);

            foreach (var role in userRoles)
            {
                if (role.ToLower().Contains("buyer")
                    || role.ToLower().Contains("seller"))
                {
                    await _repo.DeleteAccountAsync(account);
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> RemoveAccountFromAPIAsync(Account account)
        {
            var userRoles = await _userManager.GetRolesAsync(account);

            foreach (var role in userRoles)
            {
                if (role.ToLower().Contains("staff")
                    || role.ToLower().Contains("shipper"))
                {
                    await _repo.RemoveAccountAsync(account);
                    return IdentityResult.Success;
                }
            }
            return IdentityResult.Failed();
        }

        public async Task<List<Account>> SearchAccountsByAddressFromAPIAsync(string address)
        {
            return await _repo.SearchAccountsByAddressAsync(address);
        }

        public async Task<List<Account>> SearchAccountsBySalaryFromAPIAsync(float minSalary, float maxSalary)
        {
            return await _repo.SearchAccountsBySalaryAsync(minSalary, maxSalary);
        }
        public async Task<List<Account>> ViewAllAccountFromAPIAsync()
        {
            return await _repo.ViewAllAccountAsync();
        }

        public async Task<List<Account>> ViewAllAccountByRoleFromAPIAsync(string role)
        {
            return await _repo.ViewAllAccountByRoleAsync(role);
        }

        public async Task<List<Account>> SearchShipperByAddressFromAPIAsync(string address)
        {
            return await _repo.SearchShipperByAddressAsync(address);
        }

        public async Task<List<GetRegisterCustomerStatistic>> GetMonthlyRegisterCustomerStatisticsFromAPIAsync()
        {
            return await _repo.GetMonthlyRegisterCustomerStatisticsAsync();
        }
    }
}
