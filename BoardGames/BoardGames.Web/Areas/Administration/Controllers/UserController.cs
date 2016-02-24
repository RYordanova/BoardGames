using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using BoardGames.Models;
using BoardGames.Data;
using BoardGames.Data.Common.Repository;
using BoardGames.Web.Areas.Administration.ViewModels;
using AutoMapper.QueryableExtensions;

namespace BoardGames.Web.Areas.Administration.Controllers
{
    public class UserController : Controller
    {
        IDeletableEntityRepository<User> users;

        public UserController(IDeletableEntityRepository<User> users)
        {
            this.users = users;
        }

        private BoardGamesDbContext db = new BoardGamesDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = this.users.All()
                .Project().To<UserViewModel>()
                .ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Destroy([DataSourceRequest]DataSourceRequest request, User user)
        {
            user.DeletedOn = DateTime.Now;
            user.CreatedOn = DateTime.Now;
            this.users.Delete(user);
            this.users.SaveChanges();

            return Json(new[] { users }.ToDataSourceResult(request, ModelState));
        }
        
    }
}
