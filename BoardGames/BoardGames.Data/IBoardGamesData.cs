namespace BoardGames.Data
{
    using System.Data.Entity;
    using BoardGames.Models;

    public interface IBoardGamesData
    {
        DbContext Context { get; }

        IRepository<Room> Rooms { get; }

        IRepository<User> Users { get; }

        void Dispose();

        int SaveChanges();
    }
}
