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
            IQueryable<User> users = db.Users;
            DataSourceResult result = users.ToDataSourceResult(request, user => new {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Create([DataSourceRequest]DataSourceRequest request, User user)
        {
            var newId = "";
            if (ModelState.IsValid)
            {
                var entity = new User
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    CreatedOn = DateTime.Now
                };
                
                this.users.Add(entity);
                this.users.SaveChanges();
                newId = entity.Id;
            }

            var postToDisplay = this.users.All().Project()
                .To<UserViewModel>()
                .FirstOrDefault(x => x.Id == newId);
            return Json(new[] { postToDisplay }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Update([DataSourceRequest]DataSourceRequest request, User user)
        {
            if (ModelState.IsValid)
            {
                var entity = new User
                {
                    Id = user.Id,
                    Rating = user.Rating,
                    RoomId = user.RoomId,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PasswordHash = user.PasswordHash,
                    SecurityStamp = user.SecurityStamp,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    LockoutEndDateUtc = user.LockoutEndDateUtc,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    UserName = user.UserName
                };

                db.Users.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Destroy([DataSourceRequest]DataSourceRequest request, User user)
        {
            if (ModelState.IsValid)
            {
                var entity = new User
                {
                    Id = user.Id,
                    Rating = user.Rating,
                    RoomId = user.RoomId,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PasswordHash = user.PasswordHash,
                    SecurityStamp = user.SecurityStamp,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    LockoutEndDateUtc = user.LockoutEndDateUtc,
                    LockoutEnabled = user.LockoutEnabled,
                    AccessFailedCount = user.AccessFailedCount,
                    UserName = user.UserName
                };

                db.Users.Attach(entity);
                db.Users.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
