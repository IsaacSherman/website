﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;

namespace live.asp.net.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return new ChallengeResult(
                OpenIdConnectAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = "/" }
            );
        }

        [HttpGet("signout")]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action("SignOutCallback", "Account", values: null, protocol: Request.Scheme);
            Context.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
            Context.Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = callbackUrl }
            );

            return new EmptyResult();
        }

        [HttpGet("signoutcallback")]
        public IActionResult SignOutCallback()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }
    }
}
