using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BoardGames.Models;
using BoardGames.Web.Infrastructure.Mapping;

namespace BoardGames.Web.Areas.Administration.ViewModels
{
    public class UserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>("Username")
                .ForMember(m => m.Username, opt => opt.MapFrom(t => t.UserName))
                .ReverseMap();
        }
    }
}