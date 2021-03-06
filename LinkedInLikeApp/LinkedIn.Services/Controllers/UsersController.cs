﻿namespace LinkedIn.Services.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    using LinkedIn.Data;
    using LinkedIn.Models;
    using LinkedIn.Services.Models.Users;
    using LinkedIn.Services.UserSessionUtils;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Testing;

    [SessionAuthorize]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;

        public UsersController()
        {
            this.userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new LinkedInContext()));
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        private IAuthenticationManager Authentication
        {
            get
            {
                return this.Request.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterUser(RegisterBindingModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest("There is already logged user. Log off to register new user.");
            }

            if (model == null)
            {
                return this.BadRequest("No registration data provided.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var emailExists = this.Data.Users.All()
                .Any(x => x.Email == model.Email);
            if (emailExists)
            {
                return this.BadRequest("Email is already taken.");
            }

            ApplicationUser newUser = new ApplicationUser
            {
                Name = model.Name,
                UserName = model.Username,
                Email = model.Email
            };

            var identityResult = await this.UserManager.CreateAsync(newUser, model.Password);

            if (!identityResult.Succeeded)
            {
                return this.GetErrorResult(identityResult);
            }

            return this.Ok(new UserRegisterViewModel
            {
                Username = newUser.UserName,
                Email = newUser.Email
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> LoginUser(LoginUserBindingModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            // Invoke the "token" OWIN service to perform the login (POST /api/token)
            // Use Microsoft.Owin.Testing.TestServer to perform in-memory HTTP POST request
            var testServer = TestServer.Create<Startup>();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };

            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await testServer.HttpClient.PostAsync(
                Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                // Sucessful login --> create user session in the database
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData =
                    jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authToken = responseData["access_token"];
                var username = responseData["userName"];
                var owinContext = this.Request.GetOwinContext();
                var userSessionManager = new UserSessionManager(owinContext);
                userSessionManager.CreateUserSession(username, authToken);

                // Cleanup: delete expired sessions from the database
                userSessionManager.DeleteExpiredSessions();
            }

            return this.ResponseMessage(tokenServiceResponse);
        }

        // POST api/Users/Logout
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            // This does not actually perform logout! The OWIN OAuth implementation
            // does not support "revoke OAuth token" (logout) by design.
            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

            // Delete the user's session from the database (revoke its bearer token)
            var owinContext = this.Request.GetOwinContext();
            var userSessionManager = new UserSessionManager(owinContext);
            userSessionManager.InvalidateUserSession();

            return this.Ok(new
            {
                message = "Logout successful."
            });
        }

        [HttpGet]
        [Route("{username}")]
        public IHttpActionResult GetUserInfo(string username)
        {
            var user = this.Data.Users.All()
                .Where(u => u.UserName == username)
                .Select(GetUserInfoViewModel.Create)
                .FirstOrDefault();
            
            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }

        [HttpGet]
        public async Task<IHttpActionResult> SearchForUser([FromUri]string search)
        {
            var users = await this.Data.Users.All()
                .Where(u => u.UserName.Contains(search) || u.Name.Contains(search))
                .Select(GetUserInfoViewModel.Create)
                .ToListAsync();

            return this.Ok(users);
        }

        [HttpPut]
        [Route("me/EditProfile")]
        public IHttpActionResult EditUserProfile(EditUserProfileBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.All()
                .FirstOrDefault(u => u.Id == loggedUserId);
            if (loggedUser == null)
            {
                return this.BadRequest("Invalid user token");
            }

            var userWithSameEmailLikeModel = this.Data.Users.All()
                .FirstOrDefault(u => u.Email == model.Email);
            if (userWithSameEmailLikeModel != null && userWithSameEmailLikeModel.Id != loggedUserId)
            {
                return this.BadRequest("Email already taken.");
            }

            loggedUser.Name = model.Name;
            loggedUser.Email = model.Email;
            loggedUser.Address = model.Address;
            loggedUser.Website = model.Website;
            loggedUser.PhoneNumber = model.PhoneNumber;

            this.Data.SaveChanges();
            return this.Ok(new
            {
                message = "Profile edited successfully."
            });
        }

        [HttpPut]
        [Route("me/ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var changePasswordResult = await this.UserManager.ChangePasswordAsync(
                this.User.Identity.GetUserId(),
                model.OldPassword,
                model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                return this.GetErrorResult(changePasswordResult);
            }

            return this.Ok(new
            {
                message = "Password changed successfully."
            });
        }
    }
}