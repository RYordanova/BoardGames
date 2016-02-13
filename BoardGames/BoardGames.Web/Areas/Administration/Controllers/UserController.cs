using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BoardGames.Data;
using BoardGames.Web.Infrastructure.Caching;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using BoardGames.Web.Areas.Administration.ViewModels;
using BoardGames.Models;
using System.Web.Security;

namespace BoardGames.Web.Areas.Administration.Controllers
{
    public class UserController : KendoGridAdministrationController
    {
        private readonly ICacheService service;

        public UserController(IBoardGamesData data, ICacheService service)
            : base(data)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable GetData()
        {
            return this.Data
                .Users
                .All()
                .ProjectTo<UserViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return this.Data.Users.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            var dbModel = base.Create<User>(model);
            if (dbModel != null) model.Id = dbModel.Id;
            this.ClearUserCache();
            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {

            var role = Roles.GetRolesForUser("Administrator");
            base.Update<User, UserViewModel>(model, model.Id);
            this.ClearUserCache();
            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var user = this.Data.Users.GetById(model.Id);
                this.Data.Users.Delete(user);
                this.Data.SaveChanges();
            }

            this.ClearUserCache();
            return this.GridOperation(model, request);
        }

        private void ClearUserCache()
        {
            this.service.Clear("user");
        }
    }
}
