using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.NotificacionModels
{
    public class GetNotificacionViewModel
    {
        public IEnumerable<Notificacion> Notificaciones { get; set; }

        public GetNotificacionViewModel()
        {
        }

        public GetNotificacionViewModel(Usuario usuarioActual) : this()
        {
            if (usuarioActual != null)
            {
                Notificaciones = usuarioActual.Notificacions.Where(x => x.IdTipoNotificacion == TipoNotificacion.BarraSuperior
                    && (x.IdTipoEstadoNotificacion == TipoEstadoNotificacion.Creado || (
                    x.IdTipoEstadoNotificacion == TipoEstadoNotificacion.PostPuesto &&
                    x.FechaPostpuesto.HasValue && x.FechaPostpuesto <= DateTime.Now
                    )));
            }
            else
            {
                Notificaciones = new List<Notificacion>();
            }
        }
    }
}