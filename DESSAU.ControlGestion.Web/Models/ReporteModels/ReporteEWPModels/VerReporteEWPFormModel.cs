using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.ReporteModels.ReporteEWPModels
{
    public class VerReporteEWPFormModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int? IdUsuario { get; set; }

        public VerReporteEWPFormModel()
        {
            DateTime hoy = DateTime.Now;
            if (hoy.Day <= 20)
            {
                FechaDesde = new DateTime(hoy.AddMonths(-1).Year, hoy.AddMonths(-1).Month, 21);
                FechaHasta = new DateTime(hoy.Year, hoy.Month, 20);
            }
            else
            {
                FechaDesde = new DateTime(hoy.Year, hoy.Month, 21);
                FechaHasta = new DateTime(hoy.AddMonths(1).Year, hoy.AddMonths(1).Month, 20);
            }
        }
    }
}