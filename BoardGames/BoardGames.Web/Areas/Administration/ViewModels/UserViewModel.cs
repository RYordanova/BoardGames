namespace BoardGames.Web.Areas.Administration.ViewModels
{
    using BoardGames.Models;
    using BoardGames.Web.Infrastructure.Mapping;

    public class UserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}