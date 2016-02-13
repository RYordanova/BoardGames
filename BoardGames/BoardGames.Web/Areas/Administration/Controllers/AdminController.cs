namespace BoardGames.Web.Areas.Administration.Controllers
{
    using BoardGames.Data;
    using BoardGames.Web.Controllers;

    public class AdminController: BaseController
    {
        public AdminController(IBoardGamesData data)
           : base(data)
        {

        }
    }
}