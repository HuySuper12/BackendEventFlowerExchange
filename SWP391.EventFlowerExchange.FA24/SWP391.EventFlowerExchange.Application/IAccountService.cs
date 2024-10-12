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
    public interface IAccountService
    {
        //Login
        public Task<IdentityResult> SignUpBuyerFromAPIAsync(SignUpBuyer signUp);
        public Task<IdentityResult> SignUpSellerFromAPIAsync(SignUpSeller signUp);
        public Task<IdentityResult> CreateStaffAccountFromAPIAsync(SignUpStaff staff);
        public Task<IdentityResult> CreateShipperAccountFromAPIAsync(SignUpShipper shipper);
        public Task<string> SignInFromAPIAsync(SignIn signIn);
        public Task<string> SignInEmailFromAPIAsync(SignIn signIn);

        //Send OTP
        public Task<bool> SendOTPFromAPIAsync(string email);
        public Task<bool> VerifyOTPFromAPIAsync(string email, string otp);

        //Reset password
        public Task<bool> ResetPasswordFromAPIAsync(string email, string otp);

        //CRUD ACCOUNT
        public Task<Account> GetUserByEmailFromAPIAsync(Account account);
        public Task<Account> GetUserByIdFromAPIAsync(Account account);
        public Task<IdentityResult> UpdateAccountFromAPIAsync(Account account);
        public Task<List<Account>> ViewAllAccountFromAPIAsync();
        public Task<List<Account>> ViewAllAccountByRoleFromAPIAsync(string role);
        public Task<IdentityResult> RemoveAccountFromAPIAsync(Account account);
        public Task<IdentityResult> DeleteAccountFromAPIAsync(Account account);
        public Task<List<Account>> SearchAccountsByAddressFromAPIAsync(string address);
        public Task<List<Account>> SearchAccountsBySalaryFromAPIAsync(float minSalary, float maxSalary);
        public Task<List<Account>> SearchShipperByAddressFromAPIAsync(string address);
        
    }
}
