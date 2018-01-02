using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.EWPModels
{
    public class CrearEditarSubEWPViewModel
    {
        public CrearEditarSubEWPFormModel Form { get; set; }
        public IEnumerable<SelectListItem> EWPs { get; set; }
        private EWPSelectListProvider eslp = new EWPSelectListProvider();
        public CrearEditarSubEWPViewModel()
        {
            Form = new CrearEditarSubEWPFormModel();
            EWPs = eslp.Provide();
        }

        public CrearEditarSubEWPViewModel(CrearEditarSubEWPFormModel f) : this()
        {
            Form = f;
        }
    }
}