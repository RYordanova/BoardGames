namespace BoardGames.Data
{
    using BoardGames.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class BoardGamesDbContext : IdentityDbContext<User>
    {
        public BoardGamesDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static BoardGamesDbContext Create()
        {
            return new BoardGamesDbContext();
        }
    }
}
