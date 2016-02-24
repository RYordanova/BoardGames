using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BoardGames.Data;
using BoardGames.Data.Common.Repository;
using BoardGames.Models;
using Microsoft.AspNet.Identity;

namespace BoardGames.Web.Controllers
{
    [Authorize]
    public class JoinController : Controller
    {
        private readonly IDeletableEntityRepository<Room> rooms;
        private readonly IDeletableEntityRepository<User> users;

        public JoinController(IDeletableEntityRepository<Room> rooms, IDeletableEntityRepository<User> users)
        {
            this.rooms = rooms;
            this.users = users;
        }

        public ActionResult Index(int id)
        {
            var currentRoom = this.rooms.GetById((object)id);
            // TODO: Error handling for null currentRoom
            var userId = User.Identity.GetUserId();
            var user = this.users.GetById((object)userId);

            if (user.Room == null)
            {
                if (currentRoom.IsFull())
                {
                    this.TempData["Notification"] = "This room is full!";
                    return RedirectToAction("Index", "Room");
                }
                else
                {
                    user.Room = currentRoom;
                    currentRoom.Users.Add(user);
                    this.rooms.SaveChanges();
                    return View();
                }
            }
            else
            {
                if (user.Room.Id == currentRoom.Id)
                {
                    return View();
                }
                else
                {
                    this.TempData["Notification"] = "Can't join two rooms. Please leave room " + user.Room.Name + " first!";
                    return RedirectToAction("Index", "Room");
                }
            }
        }

        public ActionResult Leave()
        {
            var userId = User.Identity.GetUserId();
            var user = this.users.GetById((object)userId);
            var room = this.rooms.All().Where(r => r.Id == user.RoomId).FirstOrDefault();

            room.Users.Remove(user);
            this.rooms.SaveChanges();

            return RedirectToAction("Index", "Room");
        }
    }
}