using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BoardGames.Web.Infrastructure.Mapping;

namespace BoardGames.Web.Models.Room
{
    public class RoomInputModel : IMapFrom<BoardGames.Models.Room>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(2, 4)]
        public int Capacity { get; set; }
    }
}