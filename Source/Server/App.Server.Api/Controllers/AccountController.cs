﻿namespace App.Server.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using App.Data.Models;
    using App.Server.DataTransferModels.User;
    using App.Services.Logic.Mapping;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;

    public class AccountController : BaseController
    {
        private readonly IMappingService mappingService;

        public AccountController(IMappingService mappingService)
        {
            this.mappingService = mappingService;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/GetAll
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var users = await base.UserManager.Users.ToListAsync();
            var response = this.mappingService.Map<IList<UserResponseModel>>(users);

            if (!response.Any())
            {
                return this.BadRequest("The user list is empty!");
            }

            return this.Ok(response);
        }

        // GET api/Account/Get/{username}
        [HttpGet]
        public async Task<IHttpActionResult> Get(string username)
        {
            var user = await base.UserManager.FindByNameAsync(username);

            if (user == null)
            {
                return this.BadRequest("User not found!");
            }

            var responseModel = this.mappingService.Map<UserResponseModel>(user);

            return this.Ok(responseModel);
        }

        // POST api/Account/Login
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginBindingModel model)
        {
            var user = await base.UserManager.FindAsync(model.UserName, model.Password);

            if (user == null)
            {
                return this.BadRequest("Wrong username or password!");
            }

            var responseModel = this.mappingService.Map<UserResponseModel>(user);

            return this.Ok(responseModel);
        }

        // POST api/Account/Register
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.mappingService.Map<User>(model);
            var result = await base.UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            var responseModel = this.mappingService.Map<UserResponseModel>(user);

            return this.Ok(responseModel);
        }

        // POST api/Account/Logout
        [HttpPost]
        [Authorize]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok();
        }

        // POST api/Account/ChangePassword
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await base.UserManager
                .ChangePasswordAsync(this.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // POST api/Account/SetPassword
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await base.UserManager
                .AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return this.Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int BitsPerByte = 8;

                if (strengthInBits % BitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / BitsPerByte;

                byte[] data = new byte[strengthInBytes];
                random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
