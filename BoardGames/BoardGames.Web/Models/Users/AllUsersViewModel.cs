using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoardGames.Web.Infrastructure.Mapping;

namespace BoardGames.Web.Models.User
{
    public class AllUsersViewModel
    {

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}