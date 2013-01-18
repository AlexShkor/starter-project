using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using DQF.Common.Account;
using DQF.Helpers;
using DQF.Platform.Mvc;
using DQF.ViewServices;
using DQF.Web.Models.Account;

namespace DQF.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly AuthenticationService _auth;
        private readonly DatabaseGenerator _generator;
        private readonly UsersViewService _users;

        public AccountController(AuthenticationService auth, DatabaseGenerator generator, UsersViewService users)
        {
            _auth = auth;
            _generator = generator;
            _users = users;
        }

        
        [GET("login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View(new LoginModel(){ReturnUrl = returnUrl});
        }

        
        [POST("login")]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && _auth.Logon(model.Email,model.Password,model.RememberMe))
            {
                var user = _users.GetByEmail(model.Email);
                Session[BaseController.UserIdentitySessionKey] = new UserIdentity(user);
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        [GET("logout")]
        public ActionResult LogOff()
        {
            _auth.LogOut();
            Session[BaseController.UserIdentitySessionKey] = null;
            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult GenerateAdmin()
        {
            _generator.SetupInitialData();

            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult PermissionsError()
        {
            return View();
        }

        #endregion
    }
}
