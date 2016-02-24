using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using BoardGames.Data.Common.Repository;
using BoardGames.Models;
using BoardGames.Web.Models.User;

namespace BoardGames.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private const int ItemsPerPage = 3;

        private readonly IDeletableEntityRepository<User> users;

        public UsersController(IDeletableEntityRepository<User> users)
        {
            this.users = users;
        }

        // GET: User
        public ActionResult Index(int id = 1)
        {
            AllUsersViewModel viewModel;
            //if (this.HttpContext.Cache["Feedback page_" + id] != null)
            //{
            //    viewModel = (AllUsersViewModel)this.HttpContext.Cache["Feedback page_" + id];
            //}
            //else
            //{
                var page = id;
                var allItemsCount = this.users.All().Count();
                var totalPages = (int)Math.Ceiling(allItemsCount / (decimal)ItemsPerPage);
                var itemsToSkip = (page - 1) * ItemsPerPage;
                var model = this.users.All()
                    .OrderByDescending(x => x.Rating)
                    .Skip(itemsToSkip)
                    .Take(ItemsPerPage)
                    .Project()
                    .To<UserViewModel>().ToList();

                viewModel = new AllUsersViewModel()
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    Users = model
                };

            //    this.HttpContext.Cache["Feedback page_" + id] = viewModel;

            //}

            return this.View(viewModel);
        }
    }
}