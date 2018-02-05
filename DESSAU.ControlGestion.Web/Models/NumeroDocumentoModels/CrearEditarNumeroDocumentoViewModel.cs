using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.NumeroDocumentoModels
{
    public class CrearEditarNumeroDocumentoViewModel
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public CrearEditarNumeroDocumentoFormModel Form { get; set; }
        public IEnumerable<SelectListItem> EWPs { get; set; }
        //private SubEWPSelectListProvider sewpslp = new SubEWPSelectListProvider();

        public CrearEditarNumeroDocumentoViewModel()
        {
            Form = new CrearEditarNumeroDocumentoFormModel();
            EWPs = new SelectList(db.EWPs, "IdEWP", "Codigo");
        }

        public CrearEditarNumeroDocumentoViewModel(CrearEditarNumeroDocumentoFormModel f) : this()
        {
            Form = f;
        }
    }
}