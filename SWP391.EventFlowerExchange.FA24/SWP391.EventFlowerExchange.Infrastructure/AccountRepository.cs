using SWP391.EventFlowerExchange.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SWP391.EventFlowerExchange.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace SWP391.EventFlowerExchange.Infrastructure
{

    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SmtpSetting smtpSetting;
        private Swp391eventFlowerExchangePlatformContext _context;

        public AccountRepository(UserManager<Account> userManager, SignInManager<Account> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IOptionsMonitor<SmtpSetting> smtpSetting)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.smtpSetting = smtpSetting.CurrentValue;
        }

        // Login available account 
        public async Task<string> SignInAsync(SignIn model)
        {

            var user = await userManager.FindByEmailAsync(model.Email);//Tim email
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);//Check password cua user
            if (user == null || !passwordValid)
            {
                return string.Empty;
            }

            if (user.Status == false)
            {
                return string.Empty;
            }

            var authenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authenClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var tokenDescription = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authenClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);
        }

        // Login available email (Login Google)
        public async Task<string> SignInEmailAsync(SignIn model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);//Tim email
            if (user == null)
            {
                return string.Empty;
            }

            if(user.Status == false)
            {
                return string.Empty;
            }

            var authenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authenClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var tokenDescription = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authenClaims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);
        }


        //SignUp
        public async Task<IdentityResult> SignUpBuyerAsync(SignUpBuyer model)
        {
            var user = new Account
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                Balance = 0,
                Address = model.Address,
                PhoneNumber = model.Phone,
                CreatedAt = DateTime.UtcNow,
                Status = true
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Gan Role Customer
                await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Buyer));
                await userManager.AddToRoleAsync(user, ApplicationRoles.Buyer);
            }
            return result;
        }

        public async Task<IdentityResult> SignUpSellerAsync(SignUpSeller model)
        {

            var user = new Account
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                Address = model.Address,
                PhoneNumber = model.Phone,
                CreatedAt = DateTime.UtcNow,
                Status = true
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Gan Role Seller
                await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Seller));
                await userManager.AddToRoleAsync(user, ApplicationRoles.Seller);
            }

            return result;

        }

        public async Task<IdentityResult> CreateStaffAccountAsync(SignUpStaff model)
        {

            var user = new Account
            {
                Name = model.Name,
                Salary = model.Salary,
                UserName = model.Email,
                //khi login thì identity tiến hành xác thực email qua NormalizedUserName và NormalizedEmail thì nó mới 
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow,
                Status = true
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Gan Role Customer
                await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Staff));
                await userManager.AddToRoleAsync(user, ApplicationRoles.Staff);
            }

            return result;

        }

        public async Task<IdentityResult> CreateShipperAccountAsync(SignUpShipper model)
        {

            var user = new Account
            {
                Name = model.Name,
                Salary = model.Salary,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow,
                Status = true
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Gan Role Customer
                await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Shipper));
                await userManager.AddToRoleAsync(user, ApplicationRoles.Shipper);
            }

            return result;

        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(smtpSetting.SenderName, smtpSetting.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart()
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpSetting.Server, smtpSetting.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpSetting.Username, smtpSetting.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<bool> SendOTPAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var otp = new Random().Next(100000, 999999).ToString();

            user.OtpCode = otp;
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(2);
            await userManager.UpdateAsync(user);

            string subject = "This is your OTP";
            string message = $"Your OTP is {otp}. This OTP is valid for 2 minutes.";
            await SendEmailAsync(email, subject, message);

            return true;
            
        }

        public async Task<bool> VerifyOTPAsync(string email, string otp)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            if (user.OtpCode == otp && user.OtpExpiration > DateTime.UtcNow)
            {
                return true;
            }

            return false;

        }

        //Reset password
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword); //Khi reset password, Identity yeu cau Generate ra chuoi Token

            if (result.Succeeded) 
            {
                user.OtpCode = null;
                user.OtpExpiration = null;
                await userManager.UpdateAsync(user);
                return true;
            }

            return false;

        }

        public async Task<IdentityResult> UpdateAccountAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<Account> GetUserByEmailAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == account.Email);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<Account> GetUserByIdAsync(Account account)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<List<Account>> ViewAllAccountAsync()
        {

            _context = new Swp391eventFlowerExchangePlatformContext();

            return await _context.Accounts.ToListAsync();

        }

        public async Task<List<Account>> ViewAllAccountByRoleAsync(string role)
        {
            _context = new Swp391eventFlowerExchangePlatformContext();

            var result = new List<Account>();
            var accounts = await _context.Accounts.ToListAsync();

            foreach (var acc in accounts)
            {
                var userRoles = await userManager.GetRolesAsync(acc);
                foreach (var userRole in userRoles)
                {
                    if (userRole.ToLower().Contains(role.ToLower()))
                    {
                        result.Add(acc);
                        break;
                    }
                }
            }

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }


        public async Task<IdentityResult> RemoveAccountAsync(Account account)
        {

            _context = new Swp391eventFlowerExchangePlatformContext();

            _context.Accounts!.Remove(account);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;

        }

        public async Task<IdentityResult> DeleteAccountAsync(Account account)
        {

            _context = new Swp391eventFlowerExchangePlatformContext();

            var disableAccount = await this.GetUserByIdAsync(account);

            disableAccount.Status = false;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;

        }

        public async Task<List<Account>> SearchAccountsByAddressAsync(string address)
        {

            _context = new Swp391eventFlowerExchangePlatformContext();

            var accounts = await _context.Accounts
                .Where(b => b.Address.ToLower().Contains(address.ToLower()))
                .ToListAsync();

            if (accounts != null)
            {
                return accounts;
            }

            return null;

        }

        public async Task<List<Account>> SearchAccountsBySalaryAsync(float minSalary, float maxSalary)
        {

            _context = new Swp391eventFlowerExchangePlatformContext();

            var accounts = await _context.Accounts
                .Where(s => s.Salary >= minSalary && s.Salary <= maxSalary)
                .ToListAsync();

            if (accounts != null)
            {
                return accounts;
            }

            return null;

        }

        public async Task<List<Account>> SearchShipperByAddressAsync(string address)
        {

            _context = new Swp391eventFlowerExchangePlatformContext();

            var result = new List<Account>();
            var accounts = await _context.Accounts
                .Where(b => b.Address.ToLower().Contains(address.ToLower()))
                .ToListAsync();

            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    var userRoles = await userManager.GetRolesAsync(account);
                    foreach (var userRole in userRoles)
                    {
                        if (userRole.ToLower().Contains("shipper"))
                        {
                            result.Add(account);
                        }
                    }
                }
            }

            return null;

        }
    }
}
