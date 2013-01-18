using System;
using System.Linq;
using System.Web.Mvc;
using DQF.Common.Account;
using DQF.Domain.Aggregates.User;
using DQF.Platform.ViewModels;

namespace DQF.Platform.Mvc
{
    public abstract class BaseController : Controller
    {
        protected new UserIdentity User { get { return Session[UserIdentitySessionKey] as UserIdentity; } }

        public const string UserIdentitySessionKey = "UserIdentity";

        public virtual T Bind<T>() where T : class, new()
        {
            var model = new T();
            UpdateModel(model);
            return model;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SaveUserDataToViewBag();
            base.OnActionExecuting(filterContext);
        }

        private void SaveUserDataToViewBag()
        {
            if (User != null)
            {
                ViewBag.UserName = User.UserName ?? User.Email;
            }
        }

        public RedirectResult RedirectToRefferer()
        {
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.LocalPath : "/");
        }

        protected string GetRouteValue(string paramName)
        {
            return RouteData.Values[paramName].ToString();
        }

        protected ActionResult PermissionsErrorResult()
        {
            return RedirectToAction("PermissionsError", "Account");
        }

        public new JsonResult Json(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}