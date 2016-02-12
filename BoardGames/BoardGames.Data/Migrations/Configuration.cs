namespace BoardGames.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Common;
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
                UserName = "Administrator"
            };

            this.userManager.Create(adminUser, "admin123456");

            this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdminRole);
        }
    }
}
