using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Models.NotificacionModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Controllers
{
    public class BaseController : Controller
    {
        public DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public Usuario _CurrentUsuario
        {
            get{
                return db.Usuarios.SingleOrDefault(x => x.Correo == User.Identity.Name);
            }
        }

        public string Mensaje
        {
            set
            {
                TempData["Mensaje"] = value;
            }
        }

        protected Usuario UsuarioActual
        {
            get
            {
                return db.Usuarios.SingleOrDefault(x => x.Correo.Trim() == User.Identity.Name);
            }
        }

        public ActionResult GetNotificaciones(bool soloNumero)
        {
            GetNotificacionViewModel model = new GetNotificacionViewModel(UsuarioActual);
            return Json(new
            {
                numero = model.Notificaciones.Count(),
                contenido = soloNumero ?
                    String.Empty :
                    RenderRazorViewToString("GetNotificaciones", model)
            },
                    JsonRequestBehavior.AllowGet);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult CambiarEstadoNotificacion(int idNotificacion, int tiempo)
        {
            var notificacion = db.Notificacions.SingleOrDefault(x => x.IdNotificacion == idNotificacion);
            notificacion.IdTipoEstadoNotificacion = tiempo == 0 ? TipoEstadoNotificacion.Leida : TipoEstadoNotificacion.PostPuesto;
            notificacion.FechaPostpuesto = DateTime.Now.AddHours(tiempo);
            db.SubmitChanges();
            return Json(
                new
                {
                    numero = UsuarioActual.Notificacions.Count(x => x.IdTipoNotificacion == TipoNotificacion.BarraSuperior
                        && (x.IdTipoEstadoNotificacion == TipoEstadoNotificacion.Creado || (
                        x.IdTipoEstadoNotificacion == TipoEstadoNotificacion.PostPuesto &&
                        x.FechaPostpuesto.HasValue && x.FechaPostpuesto <= DateTime.Now
                )))
                }, JsonRequestBehavior.AllowGet);
        }
    }
}