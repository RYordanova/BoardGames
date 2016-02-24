namespace BoardGames.Web.Models.Room
{
    using System;
    using AutoMapper;
    using BoardGames.Web.Infrastructure.Mapping;

    public class RoomViewModel : IMapFrom<BoardGames.Models.Room>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int FullPlaces { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<BoardGames.Models.Room, RoomViewModel>()
                .ForMember(x => x.FullPlaces, opt => opt.MapFrom(x => x.Users.Count));
        }
    }
}