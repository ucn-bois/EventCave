﻿using EventCaveWeb.Database;
using EventCaveWeb.Entities;
using EventCaveWeb.Utils;
using EventCaveWeb.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using System.Web.Mvc;

namespace EventCaveWeb.Controllers
{
    [RoutePrefix("Users")]
    public class UsersController : Controller
    {
        [Route("{username}")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult UserProfile(string username)
        {
            DatabaseContext db = HttpContext.GetOwinContext().Get<DatabaseContext>();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName == username);
            DetailUserProfileViewModel detailUserProfileViewModel = new DetailUserProfileViewModel()
            {
                Username = user.UserName,
                Picture = Imgur.Instance.GetImage(user.Picture),
                Bio = user.Bio,
                RegisteredAt = user.RegisteredAt,
                HostedEvents = user.EventsHosted,
                EventsEnrolledIn = user.EventsEnrolledIn.Select(ue => ue.Event).ToList()
            };
            return View(detailUserProfileViewModel);
        }

        [Route("{username}/Edit")]
        [HttpGet]
        [Authorize]
        public ActionResult Edit(string username)
        {
            if (username != User.Identity.Name)
            {
                return RedirectToAction("Index", "Home");
            }
            DatabaseContext db = HttpContext.GetOwinContext().Get<DatabaseContext>();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName == username);
            EditUserProfileViewModel editUserProfileViewModel = new EditUserProfileViewModel()
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Picture = user.Picture,
                Bio = user.Bio
            };
            return View(editUserProfileViewModel);
        }

        [Route("{username}/Edit")]
        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Username, FirstName, LastName, Email, Picture, Bio")] EditUserProfileViewModel model)
        {
            if (model.Username != User.Identity.Name)
            {
                return RedirectToAction("Index", "Home");
            }
            DatabaseContext db = HttpContext.GetOwinContext().Get<DatabaseContext>();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Picture = model.Picture;
                user.Bio = model.Bio;
                db.SaveChanges();
            }
            Message.Create(Response, "Profile was successfully edited.");
            return RedirectToAction("UserProfile", "Users", new { username = user.UserName });
        }

        [Route("{username}/Edit/ChangePassword")]
        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword(string username)
        {
            return View(new ChangeUserPasswordViewModel() { Username = username });
        }

        [Route("{username}/Edit/ChangePassword")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword([Bind(Include = "Username,OldPassword,NewPassword,RepeatedPassword")] ChangeUserPasswordViewModel model)
        {
            if (model.Username != User.Identity.Name)
            {
                return RedirectToAction("UserProfile", "Users", new { username = model.Username });
            }
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                await userManager.ChangePasswordAsync(
                    user.Id,
                    model.OldPassword,
                    model.NewPassword
                );
            }
            Message.Create(Response, "Password was successfully changed.");
            return RedirectToAction("UserProfile", "Users", new { username = model.Username });
        }
    }
}