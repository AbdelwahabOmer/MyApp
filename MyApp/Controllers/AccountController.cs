using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Repository;

namespace MyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [Route("Signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [Route("Signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
            }
            return View();
        }



        [Route("Signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [Route("Signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInUserModel userModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(userModel);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Not allow to signin, confirm your email");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Account blocked ... try after some times");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password incorrect...");
                }
            }
            return View(userModel);
        }



        [Route("Signout")]
        public async Task<IActionResult> SignOut()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("SignIn", "Account");
        }



        [Route("Change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("Change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(model);
                if (result.Succeeded)
                {
                    ViewBag.isSuccess = true;
                    ModelState.Clear();
                    return View();
                }

                foreach (var errorMessage in result.Errors)
                {
                    ModelState.AddModelError("", errorMessage.Description);
                }
            }
            return View(model);
        }



        [HttpGet("Confirm-emil")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmModel model = new EmailConfirmModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }

        [HttpPost("Confirm-emil")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

                await _accountRepository.GenerateEmailTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went error");
            }
            return View(model);
        }



        [AllowAnonymous, HttpGet("Forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("Forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await _accountRepository.GenerateForgetTokenAsync(user);
                    model.EmailSent = true;
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "This email ins not registered");
                }
            }
            return View(model);
        }



        [AllowAnonymous, HttpGet("Reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel model = new ResetPasswordModel
            {
                UserId = uid,
                Token = token
            };
            return View(model);
        }

        [AllowAnonymous, HttpPost("Reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(model);

                if (result.Succeeded)
                {
                    model.IsSuccess = true;
                    ModelState.Clear();
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

    }
}
