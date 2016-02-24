namespace BoardGames.Web.Models.User
{
    using BoardGames.Web.Infrastructure.Mapping;

    public class UserViewModel : IMapFrom<BoardGames.Models.User>
    {
        public string UserName { get; set; }
        
        public int Rating { get; set; }
    }
}