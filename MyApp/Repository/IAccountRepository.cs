using Microsoft.AspNetCore.Identity;
using MyApp.Models;

namespace MyApp.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task GenerateEmailTokenAsync(ApplicationUser user);
        Task GenerateForgetTokenAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<SignInResult> PasswordSignInAsync(SignInUserModel userModel);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
        Task SignOutAsync();
    }
}