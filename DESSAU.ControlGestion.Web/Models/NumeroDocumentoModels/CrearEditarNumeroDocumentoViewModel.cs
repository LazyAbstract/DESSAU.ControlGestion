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
        public CrearEditarNumeroDocumentoFormModel Form { get; set; }
        public IEnumerable<SelectListItem> SubEWPs { get; set; }
        private SubEWPSelectListProvider sewpslp = new SubEWPSelectListProvider();

        public CrearEditarNumeroDocumentoViewModel()
        {
            Form = new CrearEditarNumeroDocumentoFormModel();
            SubEWPs = sewpslp.Provide();
        }

        public CrearEditarNumeroDocumentoViewModel(CrearEditarNumeroDocumentoFormModel f) : this()
        {
            Form = f;
        }
    }
}