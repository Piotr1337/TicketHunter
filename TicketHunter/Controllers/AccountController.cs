using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;
using TicketHunter.Models;
using Facebook;

namespace TicketHunter.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Account(string returnUrl)
        {
            var model = new AuthModelView()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MemberLoginSummary(AuthModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.LoginModel.Email);

            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ModelState.AddModelError("", "Musisz potwierdzić swój email aby się zalogować.");
                    return View("Account", model);
                }else if(UserManager.IsInRole(user.Id, "Admin"))
                {
                    ModelState.AddModelError("", "Nieprawidłowy email lub hasło");
                    return View("Account", model);
                }
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result =
                await
                    SignInManager.PasswordSignInAsync(model.LoginModel.Email, model.LoginModel.Password,
                        model.LoginModel.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if(model.ReturnUrl != null)
                    {
                        return RedirectToLocal(model.ReturnUrl);
                    }
                    return RedirectToAction("List", "Event");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode",
                        new {ReturnUrl = model.ReturnUrl, RememberMe = model.LoginModel.RememberMe});
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Nieprawidłowy email lub hasło");
                    return View("Account", model);
            }
        }

        

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe,
                        rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MemberRegisterSummary(AuthModelView model)
        {
             if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.RegisterModel.Email, Email = model.RegisterModel.Email};
                var result = await UserManager.CreateAsync(user, model.RegisterModel.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code = code},
                        protocol: Request.Url.Scheme);

                    UserManager.AddToRole(user.Id, "User");

                    StringBuilder builder = new StringBuilder();
                    builder.Append("<h2>Potwierdź swój adres email</h2>");
                    builder.Append("Potwierdź swoje konto klikając na ten link:<a href=\"" + callbackUrl + "\">here</a>");
                    await
                        UserManager.SendEmailAsync(user.Id, "Dziękujemy za zarejestrowanie się w serwisie TicketHunter!",
                            builder.ToString());
                    return RedirectToAction("ConfirmEmailSended", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View("Account", model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmailSended()
        {
            return View();
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode",
                new {Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe});
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            if(loginInfo.Login.LoginProvider == "Facebook")
            {
                var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var access_token = identity.FindFirstValue("FacebookAccessToken");
                var fb = new FacebookClient(access_token);
                dynamic myInfo = fb.Get("/me?fields=email"); 
                loginInfo.Email = myInfo.email;
            }
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    var user = new ApplicationUser { UserName = loginInfo.DefaultUserName , Email = loginInfo.Email };
                    if (UserManager.FindByEmail(user.Email) == null)
                    {
                        var resultt = await UserManager.CreateAsync(user);
                        if (resultt.Succeeded)
                        {
                            resultt = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                            if (resultt.Succeeded)
                            {
                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                return RedirectToLocal(returnUrl);
                            }
                        }
                    }
                    else
                    {
                        var resultt = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                        if (resultt.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    return RedirectToLocal(returnUrl);
            }
            //else
            //{
            //    ViewBag.ReturnUrl = returnUrl;
            //    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            //    var newUser = new ApplicationUser { UserName = loginInfo.Email, Email = loginInfo.Email };
            //    var newResult = await UserManager.CreateAsync(newUser, "Nhy6&*()");
            //    await SignInManager.PasswordSignInAsync(loginInfo.Email, "Nhy6&*()",
            //        false, shouldLockout: false);
            //    return RedirectToAction("List", "Event");
            //}
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
            //    case SignInStatus.Failure:
            //    default:
            //        // If the user does not have an account, then prompt the user to create an account
            //        ViewBag.ReturnUrl = returnUrl;
            //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            //        var newUser = new ApplicationUser { UserName = loginInfo.Email, Email = loginInfo.Email };
            //        var newResult = await UserManager.CreateAsync(newUser, "Nhy6&*()");
            //        await SignInManager.PasswordSignInAsync(loginInfo.Email, "Nhy6&*()",
            //            false, shouldLockout: false);
            //        return RedirectToLocal(returnUrl);
            //        //return View("ExternalLoginConfirmation",
            //        //    new ExternalLoginConfirmationViewModel { UserName = loginInfo.ExternalIdentity.Name ,Email = loginInfo.Email });
            //}
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser {UserName = model.UserName , Email = model.Email};
                if(UserManager.FindByEmail(user.Email) == null)
                {
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                }
                else
                {
                    //98bdedd1-cbdc-4004-bee3-9080f40e328d
                    var result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                //AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("List", "Event");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

        public ActionResult UserProfile()
        {
            UserProfileDetails details = _userRepository.GetDetails(User.Identity.GetUserId());

            if (details != null)
            {
                UserDetailsViewModel vm = new UserDetailsViewModel()
                {
                    UserDataModel = new UserDataViewModel()
                    {
                        Id = User.Identity.GetUserId(),
                        UserProfileDetailsID = details.UserProfileDetailsID,
                        FirstName = details.FirstName,
                        LastName = details.LastName,
                        TelephoneNumber = details.TelephoneNumber,
                    },
                    UserAddressModel = new UserAddressViewModel()
                    {
                        Id = User.Identity.GetUserId(),
                        UserAddresses = _userRepository.UserAddresses.Where(x => x.Id == User.Identity.GetUserId()),
                        Countries = _userRepository.Countries,
                        CountiresDropDown = _userRepository.CountriesDropList
                    }
                };
                return View(vm);
            }
            else
            {
                UserDetailsViewModel vm = new UserDetailsViewModel()
                {
                    UserDataModel = new UserDataViewModel()
                    {
                        Id = User.Identity.GetUserId(),
                    },
                    UserAddressModel = new UserAddressViewModel()
                    {
                        Id = User.Identity.GetUserId(),
                        UserAddresses = _userRepository.UserAddresses.Where(x => x.Id == User.Identity.GetUserId()),
                        Countries = _userRepository.Countries,
                        CountiresDropDown = _userRepository.CountriesDropList
                    }
                };
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveFilledUserData(UserDetailsViewModel data)
        {
            var model = Mapper.Map<UserDataViewModel, UserProfileDetails>(data.UserDataModel);


            if (ModelState.IsValid)
            {
                _userRepository.SaveUserData(model);
            }
            return RedirectToAction("UserProfile", "Account");
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveFilledUserAddress(UserDetailsViewModel data)
        {
            var country = _userRepository.Countries.FirstOrDefault(x => x.CountryID == int.Parse(data.UserAddressModel.Country));

            var model = Mapper.Map<UserAddressViewModel, UserAddress>(data.UserAddressModel);
            if (country != null)
            {
                model.Country = country.CountryName;
            }
            if (ModelState.IsValid)
            {
                _userRepository.SaveUserAddress(model);
            }
            return RedirectToAction("UserProfile", "Account");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteAddress(int addressId)
        {
            _userRepository.RemoveUserAddress(addressId);

            return RedirectToAction("UserProfile", "Account");
        }
    }
}