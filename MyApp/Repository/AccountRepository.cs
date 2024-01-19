using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;
using MyApp.Service;

namespace MyApp.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly myDbContext _dbContext;

        public AccountRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IUserService userService,
            IEmailService emailService, IConfiguration configuration, myDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task GenerateEmailTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendConfirmationEmail(user, token);
            }
        }

        public async Task GenerateForgetTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgetEmail(user, token);
            }
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result;
        }

        public async Task<SignInResult> PasswordSignInAsync(SignInUserModel userModel)
        {
            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, true);
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            var user = await _userManager.FindByIdAsync(uid);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            return result;
        }

        private async Task SendConfirmationEmail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{userName}}", user.UserName),
                    new KeyValuePair<string, string>("{{link}}", string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };
            await _emailService.SendConfirmationEmail(options);
        }

        private async Task SendForgetEmail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgetPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{userName}}", user.UserName),
                    new KeyValuePair<string, string>("{{link}}", string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };
            await _emailService.SendForgetEmail(options);
        }








        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {
            string newCMP = RandomString(6);


            var user = new ApplicationUser()
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                CMP = newCMP
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {

                var data = new CompanyInfo()
                {
                    CMP = newCMP,
                    Name = userModel.Name,
                    Web = userModel.Web,
                    Email = userModel.Email,
                    Phone = userModel.Phone,
                    Address = userModel.Address,
                    CompanyType = userModel.CompanyType,
                    TRN = null,
                    LogoURL = null,
                    StampURL = null,
                    SignURL = null
                };

                await _dbContext.CompanyInfo.AddAsync(data);
                await _dbContext.SaveChangesAsync();


                await GenerateEmailTokenAsync(user);
            }
            return result;
        }


        /// <summary>
        /// Adding Company info
        /// </summary>
        /// <param name="model"></param>
        /// <returns>model.ID</returns>
        public async Task<long> AddAccountInfo(CompanyInfoModel model)
        {
            string newCMP = RandomString(6);

            var data = new CompanyInfo()
            {
                CMP = model.CMP,
                Name = model.Name,
                Web = model.Web,
                Email = model.Email,
                Phone = model.Phone,
                TRN = model.TRN,
                Address = model.Address,
                CompanyType = model.CompanyType,
                LogoURL = model.LogoURL,
                StampURL = model.StampURL,
                SignURL = model.SignURL
            };

            await _dbContext.CompanyInfo.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return data.ID;
        }

        /// <summary>
        /// Edit company Info
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<long> EditAccountInfo(long Id, CompanyInfoModel model)
        {
            var data = await _dbContext.CompanyInfo.Where(x => x.ID == Id).Select(info => new CompanyInfo()
            {
                ID = info.ID,
                CMP = info.CMP,
                Name = info.Name,
                Web = info.Web,
                Email = info.Email,
                Phone = info.Phone,
                TRN = info.TRN,
                Address = info.Address,
                CompanyType = info.CompanyType,
                LogoURL = info.LogoURL,
                StampURL = info.StampURL,
                SignURL = info.SignURL
            }).FirstOrDefaultAsync();
            if (data != null)
            {
                _dbContext.CompanyInfo.Update(data);
                await _dbContext.SaveChangesAsync();
                return data.ID;
            }
            else { return 0; }
        }




        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomNumbers(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
