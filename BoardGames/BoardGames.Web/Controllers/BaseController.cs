namespace BoardGames.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using BoardGames.Data;
    using BoardGames.Models;

    [HandleError]
    public class BaseController : Controller
    {
        protected IBoardGamesData Data { get; private set; }

        protected User UserProfile { get; private set; }

        public BaseController(IBoardGamesData data)
        {
            this.Data = data;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.Data.Users.All().Where(u => u.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}