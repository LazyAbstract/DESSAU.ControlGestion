using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class VerTimeSheetFormModel
    {
        public VerTimeSheetFormModel()
        {
            if (!Fecha.HasValue)
            {
                Fecha = DateTime.Now;
            }
        }
        public int? IdUsuario { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdCategoria { get; set; }
    }
}