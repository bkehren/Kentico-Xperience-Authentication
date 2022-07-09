using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using CMS.Base;
using CMS.Core;

using Kentico.Membership;
using KXAuthentication.Models.Users.Account;

namespace KXAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ISiteService siteService;
        private readonly IEventLogService eventLogService;

        public AccountController(ApplicationUserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ISiteService siteService,
                                 IEventLogService eventLogService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.siteService = siteService;
            this.eventLogService = eventLogService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Handles authentication when the sign-in form is submitted. Accepts parameters posted from the sign-in form via the SignInViewModel.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Attempts to authenticate the user against the Xperience database
            var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Failed;
            try
            {
                //// use this if you want the "remember me" feature to work
                // signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.StaySignedIn, false);
                signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            }
            catch (Exception ex)
            {
                eventLogService.LogException("AuthenticationApplication", "AUTHENTICATION", ex);
            }

            // If the authentication was successful, redirects to the return URL when possible or to a different default action
            if (signInResult.Succeeded)
            {
                string decodedReturnUrl = WebUtility.UrlDecode(returnUrl);
                if (!string.IsNullOrEmpty(decodedReturnUrl) && Url.IsLocalUrl(decodedReturnUrl))
                {
                    return Redirect(decodedReturnUrl);
                }
                // Redirects the user to the homepage
                return RedirectToAction("Index", "Home");
            }

            // Optional addition if a user is waiting for approval.
            if (signInResult.IsNotAllowed)
            {
                // If the 'Registration requires administrator's approval' setting is enabled and the user account
                // is pending activation, displays an appropriate message
                ApplicationUser user = await userManager.FindByNameAsync(model.UserName);
                if (user != null && user.WaitingForApproval)
                {
                    ModelState.AddModelError(String.Empty, "You account is pending administrator approval.");

                    return RedirectToAction(nameof(WaitingForApproval));
                }

                // The other setting that causes 'IsNotAllowed' is 'Require email confirmation'
                ModelState.AddModelError(String.Empty, "Please confirm your email.");

                return View();
            }

            // If the authentication was not successful due to any other reason, displays the sign-in form with an "Authentication failed" message
            ModelState.AddModelError(String.Empty, "Authentication failed, please verify you entered the correct credentials.");
            return View();
        }

        public IActionResult WaitingForApproval()
        {
            return View();
        }

        public IActionResult PermissionDenied()
        {
            return View();
        }

        /// <summary>
        /// Action for signing out users. The Authorize attribute allows the action only for users who are already signed in.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignOut()
        {
            // Signs out the current user
            signInManager.SignOutAsync();

            // Redirects to Sign in page after sign out, may have a redirect loop if directing to Home and Home requires authentication.
            return RedirectToAction("SignIn", "Account");
        }
    }
}
