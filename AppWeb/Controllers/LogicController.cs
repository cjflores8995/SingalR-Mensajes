using AppWeb.Hubs;
using AppWeb.Models;
using Business.Repository;
using DataAccess;
using Entities.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace AppWeb.Controllers
{
    public class LogicController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Logic
        public ActionResult Index()
        {
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult SendMessage(MessageJsObj msj)
        {
            string fromUserId = User.Identity.GetUserId();
            string toUserId = new UserRepository().GetUserIdByEmail(msj.UserId);

            MainHub mainHub = new MainHub();
            Mensaje mensaje = new Mensaje()
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Texto = msj.Message
            };

            db.Configuration.ProxyCreationEnabled = false;
            db.Entry(mensaje).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            //solo se ejecuta una vez y muestra en pantalla la alerta de nueva notificación
            mainHub.DisplayNotification(mensaje.ToUserId,  mensaje.Texto.Truncar(100));

            mainHub.SendNotification(mensaje.ToUserId);

            return Json(new { OK = true, Message = Messages.TXN_REALIZADA_OK}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendNotificacionGlobal()
        {
            MainHub mainHub = new MainHub();

            string message = "Hola a todos, esta es una notificacion enviada globalmente a todos los usuarios!";

            mainHub.SendGlobalNotification(message);

            return Json(new { OK = true, Message = Messages.TXN_REALIZADA_OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendNotificacionGlobalExcept()
        {
            string currentUserId = User.Identity.GetUserId();

            MainHub mainHub = new MainHub();

            string message = "Hola a todos, esta es una notificacion enviada globalmente a todos los usuarios!, menos al que emitio";

            mainHub.SendGlobalNotificationExcept(currentUserId, message);

            return Json(new { OK = true, Message = Messages.TXN_REALIZADA_OK }, JsonRequestBehavior.AllowGet);
        }

    }
}
