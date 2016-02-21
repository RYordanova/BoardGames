using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BoardGames.Data.Common.Repository;
using BoardGames.Models;
using Microsoft.AspNet.Identity;

namespace BoardGames.Web.Controllers
{
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

            var userId = User.Identity.GetUserId();
            var user = this.users.GetById((object)userId);

            if (user.RoomId == null)
            {
                if (currentRoom.Capacity > currentRoom.Users.Count)
                {
                    try
                    {
                        user.RoomId = currentRoom.Id;
                        this.users.SaveChanges();
                        currentRoom.Users.Add(user);
                        this.rooms.SaveChanges();
                    }
                    catch
                    {
                        return Redirect(currentRoom.Name);
                    }
                }
                else {
                    this.TempData["Notification"] = "This room is full!";
                    return RedirectToAction("Index", "Room");
                }

            }
            else if(user.RoomId != currentRoom.Id)
            {
                this.TempData["Notification"] = "Can't join two rooms. Please leave room" + user.Room.Name + "first!";
                return RedirectToAction("Index", "Room");
            }
            else
            {
                if (currentRoom.Capacity == currentRoom.Users.Count)
                {
                    this.TempData["Notification"] = "This room is full!";
                    return RedirectToAction("Index", "Room");
                }
            }

            return View();
        }
    }
}