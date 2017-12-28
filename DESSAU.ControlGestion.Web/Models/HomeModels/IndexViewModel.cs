using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.HomeModels
{
    public class IndexViewModel
    {
        public DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public IEnumerable<object> DiasPendientesPlanificacion { get; set; }
        public IEnumerable<object> DiasPendientesDeclaracion { get; set; }
        public IEnumerable<object> DiasPendientesDeclaracionEWP { get; set; }
        public Usuario Usuario { get; set; }
        //public string MesPlanificacion { get; set; }

        public IndexViewModel()
        {

        }

        public IndexViewModel(int IdUsuario) : this()
        {
            var buffer = db.sp_GetDiasPendientesPlanificacionByIdUsuario(IdUsuario);
            if (buffer != null)
                DiasPendientesPlanificacion = buffer.Select(x => new
                {
                    date = x.Fecha.HasValue ? x.Fecha.Value.ToString("yyyy-MM-dd")
                        : String.Empty,
                    title = "Planificación Pendiente"
                }).ToList();

            var buffer2 = db.sp_GetDiasPendientesDeclaracionByIdUsuario(IdUsuario);
            if (buffer2 != null)
                DiasPendientesDeclaracion = buffer2.Select(x => new
                {
                    date = x.Fecha.HasValue ? x.Fecha.Value.ToString("yyyy-MM-dd")
                        : String.Empty,
                    title = "Declaración Pendiente"
                }).ToList();
            var buffer3 = db.sp_GetDiasPendientesDeclaracionEWPByIdUsuario(IdUsuario);
            if (buffer3 != null)
                DiasPendientesDeclaracionEWP = buffer3.Select(x => new
                {
                    date = x.Fecha.HasValue ? x.Fecha.Value.ToString("yyyy-MM-dd")
                        : String.Empty,
                    title = "Declaración Pendiente"
                }).ToList();
        }
    }
}