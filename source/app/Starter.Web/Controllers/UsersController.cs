using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using DQF.Domain.Aggregates.User;
using DQF.Domain.Aggregates.User.Commands;
using DQF.Platform.Domain;
using DQF.Platform.Extensions;
using DQF.Platform.Mvc;
using DQF.ViewServices;
using DQF.Web.Models.User;

namespace DQF.Web.Controllers
{
    [RoutePrefix("users")]
    [Auth]
    public class UsersController : BaseController
    {
        private readonly UsersViewService _users;
        private readonly ICommandBus _bus;

        public UsersController(UsersViewService users, ICommandBus bus)
        {
            _users = users;
            _bus = bus;
        }

        [GET("")]
        public ActionResult Index()
        {
            var users = _users.GetAll();
            var model = users.Select(x => new UserListItem(x)).ToList();
            return View(model);
        }

        public ActionResult AddUser()
        {
            var model = new AddUserViewModel();
            return View("New", model);
        }

        public ActionResult AddUser(AddUserViewModel model)
        {
            if (model.IsValid)
            {
                var cmd = model.ToCommand();
                _bus.Send(cmd);
                return RedirectToAction("Index", "Home");
            }
            return View("New", model);
        }

        [GET("edit/{id}")]
        public ActionResult Edit(string id)
        {
            var user = _users.GetById(id);
            var model = new UserViewModel(user);
            return View(model);
        }

        [POST("edit/{id}")]
        public ActionResult Edit(UserViewModel model)
        {
            if (model.IsValid)
            {
                var cmd = model.ToUpdateCommand();
                _bus.Send(cmd);
                model.RedirectToRefferer();
            }
            return Json(model);
        }

        [POST("delete/{id}")]
        public ActionResult Delete(string id)
        {
            var cmd = new DeleteUser
            {
                Id = id,
                DeletedByUserId = User.UserId
            };
            _bus.Send(cmd);
            return RedirectToRefferer();
        }
    }
}
