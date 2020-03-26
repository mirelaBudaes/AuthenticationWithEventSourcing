﻿using Authentication.Library;
using Authentication.Library.Exceptions;
using Authentication.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Register(RegisterViewModel viewModel)
        {
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Register(string emailAddress)
        {
            //todo: email address validation

            try
            {
                _authenticationService.RegisterUser(emailAddress);
            }
            catch (EmailExistsException e)
            {
                return Register(new RegisterViewModel()
                {
                    EmailAddress = emailAddress,
                    ValidationError = $"User {emailAddress} already exists"
                });
            }

            return RedirectToAction("Register");
        }
    }
}