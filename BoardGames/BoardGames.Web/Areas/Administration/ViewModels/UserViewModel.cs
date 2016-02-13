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
    public class UserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        //public string Role { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>("Username")
                .ForMember(m => m.Username, opt => opt.MapFrom(t => t.UserName))
                .ForMember(m => m.Email, opt => opt.MapFrom(t => t.Email))
                //.ForMember(m => m.Role, opt => opt.MapFrom(t => Roles.Where(r => user.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name)))
                .ReverseMap();
        }
    }
}