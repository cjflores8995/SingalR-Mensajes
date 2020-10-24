using Business.Repository;
using Entities.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers
{
    public class HomeController : Controller
    {
        MainEntity mainEntity = new MainEntity();
        string userId = string.Empty;

        public HomeController()
        {
            userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Message()
        {
            mainEntity.Usuarios =
                new UserRepository()
                .GetAllUsers()
                .Where(x => !x.Id.Equals(userId))
                .ToList();

            return View(mainEntity);
        }
    }
}