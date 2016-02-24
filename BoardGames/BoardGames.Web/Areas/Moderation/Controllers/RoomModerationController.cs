using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using BoardGames.Models;
using BoardGames.Data;
using BoardGames.Data.Common.Repository;
using AutoMapper.QueryableExtensions;
using BoardGames.Web.Areas.Moderation.Models;

namespace BoardGames.Web.Areas.Moderation.Controllers
{
    public class RoomModerationController : Controller
    {
        IDeletableEntityRepository<Room> rooms;

        public RoomModerationController(IDeletableEntityRepository<Room> rooms)
        {
            this.rooms = rooms;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rooms_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = this.rooms.All()
                .Project().To<RoomViewModel>()
                .ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Rooms_Destroy([DataSourceRequest]DataSourceRequest request, Room room)
        {
            room.DeletedOn = DateTime.Now;
            room.CreatedOn = DateTime.Now;
            this.rooms.Delete(room);
            this.rooms.SaveChanges();

            return Json(new[] { room }.ToDataSourceResult(request, ModelState));
        }
    }
}
