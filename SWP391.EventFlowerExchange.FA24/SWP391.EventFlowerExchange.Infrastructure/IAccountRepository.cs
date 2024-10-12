using SWP391.EventFlowerExchange.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWP391.EventFlowerExchange.Domain.ObjectValues;

namespace SWP391.EventFlowerExchange.Infrastructure
{
    public interface IAccountRepository
    {
        //SignUp
        public Task<IdentityResult> SignUpBuyerAsync(SignUpBuyer signUp);
        public Task<IdentityResult> SignUpSellerAsync(SignUpSeller signUp);
        public Task<IdentityResult> CreateStaffAccountAsync(SignUpStaff staff);
        public Task<IdentityResult> CreateShipperAccountAsync(SignUpShipper shipper);
        
        //Login
        public Task<string> SignInAsync(SignIn signIn);
        public Task<string> SignInEmailAsync(SignIn signIn);

        //Send OTP
        public Task SendEmailAsync(string toEmail, string subject, string message);
        public Task<bool> SendOTPAsync(string email);
        public Task<bool> VerifyOTPAsync(string email, string otp);

        //Reset Password
        public Task<bool> ResetPasswordAsync(string email, string newPassword);

        //CRUD Account
        public Task<Account> GetUserByEmailAsync(Account account);
        public Task<Account> GetUserByIdAsync(Account account);
        public Task<IdentityResult> UpdateAccountAsync(Account account);
        public Task<IdentityResult> RemoveAccountAsync(Account account);
        public Task<IdentityResult> DeleteAccountAsync(Account account);
        public Task<List<Account>> ViewAllAccountAsync();
        public Task<List<Account>> ViewAllAccountByRoleAsync(string role);
        public Task<List<Account>> SearchAccountsByAddressAsync(string address);
        public Task<List<Account>> SearchAccountsBySalaryAsync(float minSalary, float maxSalary);
        public Task<List<Account>> SearchShipperByAddressAsync(string address);

    }
}
