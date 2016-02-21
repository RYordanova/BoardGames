using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BoardGames.Data;
using BoardGames.Data.Common.Repository;
using BoardGames.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using AutoMapper.QueryableExtensions;
using BoardGames.Web.Models.Room;
using System;

namespace BoardGames.Web.Controllers
{
    public class RoomController : Controller
    {
        IDeletableEntityRepository<Room> rooms;

        public RoomController(IDeletableEntityRepository<Room> rooms)
        {
            this.rooms = rooms;
        }

        private BoardGamesDbContext db = new BoardGamesDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rooms_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Room> rooms = db.Rooms;
            DataSourceResult result = rooms.ToDataSourceResult(request, room => new {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                CreatedOn = room.CreatedOn
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Rooms_Create([DataSourceRequest]DataSourceRequest request, RoomInputModel room)
        {
            var newId = 0;
            if (ModelState.IsValid)
            {
                var entity = new Room
                {
                    Name = room.Name,
                    Capacity = room.Capacity
                };

                entity.CreatedOn = DateTime.Now;
                this.rooms.Add(entity);
                this.rooms.SaveChanges();
                newId = entity.Id;
            }

            var roomToDisplay = this.rooms.All().Project()
                .To<RoomViewModel>()
                .FirstOrDefault(x => x.Id == newId);
            return Json(new[] { roomToDisplay }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Rooms_Update([DataSourceRequest]DataSourceRequest request, Room room)
        {
            if (ModelState.IsValid)
            {
                var entity = new Room
                {
                    Id = room.Id,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    CreatedOn = room.CreatedOn
                };

                db.Rooms.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { room }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Rooms_Destroy([DataSourceRequest]DataSourceRequest request, Room room)
        {
            if (ModelState.IsValid)
            {
                var entity = new Room
                {
                    Id = room.Id,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    CreatedOn = room.CreatedOn
                };

                db.Rooms.Attach(entity);
                db.Rooms.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { room }.ToDataSourceResult(request, ModelState));
        }

        public void Join([DataSourceRequest]DataSourceRequest request)
        {
            Redirect("/");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
