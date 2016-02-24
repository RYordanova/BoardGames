using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BoardGames.Models;
using BoardGames.Web.Infrastructure.Mapping;

namespace BoardGames.Web.Areas.Moderation.Models
{
    public class RoomViewModel : IMapFrom<Room>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UsersCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Room, RoomViewModel>()
                .ForMember(x => x.UsersCount, opt => opt.MapFrom(x => x.Users.Any() ? x.Users.Count : 0));
        }
    }
}