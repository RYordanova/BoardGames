using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AutoMapper;
using BoardGames.Models;
using BoardGames.Web.Infrastructure.Mapping;

namespace BoardGames.Web.Areas.Administration.ViewModels
{
    public class UserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}