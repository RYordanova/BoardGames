namespace BoardGames.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BoardGames.Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<BoardGamesDbContext>
    {
        private UserManager<User> userManager;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BoardGamesDbContext context)
        {
            this.userManager = new UserManager<User>(new UserStore<User>(context));
            this.SeedRoles(context);
            this.SeedUsers(context);
        }

        private void SeedRoles(BoardGamesDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.AdminRole));
            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.ModeratorRole));
            context.SaveChanges();
        }

        private void SeedUsers(BoardGamesDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }          

            var adminUser = new User
            {
                Email = "admin@mysite.com",
                UserName = "Administrator",
                CreatedOn = DateTime.Now
            };

            this.userManager.Create(adminUser, "admin123456");

            this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdminRole);

            var modUser = new User
            {
                Email = "mod@mysite.com",
                UserName = "Moderator",
                CreatedOn = DateTime.Now
            };

            this.userManager.Create(modUser, "mod123456");

            this.userManager.AddToRole(modUser.Id, GlobalConstants.ModeratorRole);
        }
    }
}
