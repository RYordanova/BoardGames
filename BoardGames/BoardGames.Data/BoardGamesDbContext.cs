namespace BoardGames.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Common.Models;
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

        public IDbSet<BoardGames.Models.Room> Rooms { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P

            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
