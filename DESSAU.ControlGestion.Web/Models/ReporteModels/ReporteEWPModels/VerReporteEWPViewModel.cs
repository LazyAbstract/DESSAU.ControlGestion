using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DESSAU.ControlGestion.Core;

namespace DESSAU.ControlGestion.Web.Models.ReporteModels.ReporteEWPModels
{
    public class VerReporteEWPViewModel
    {
        public VerReporteEWPFormModel Form { get; set; }
        public IEnumerable<fn_ReportePorEWPResult> Resultados { get; set; }
        public int Total { get; set; }
        public string Profesional { get; set; }
        public IEnumerable<SelectListItem> Usuarios { get; set; }
        private UsuarioSelectListProvider uslp = new UsuarioSelectListProvider();

        public VerReporteEWPViewModel()
        {
            Form = new VerReporteEWPFormModel();
            Usuarios = uslp.Provide();
            Total = 0;
            Profesional = String.Empty;
        }

        public VerReporteEWPViewModel(VerReporteEWPFormModel F) : this()
        {
            Form = F;
        }
    }
}