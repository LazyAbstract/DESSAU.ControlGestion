using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EWPModels
{
    public class CrearEditarEWPViewModel
    {
        public CrearEditarEWPFormModel Form { get; set; }

        public CrearEditarEWPViewModel()
        {
            Form = new CrearEditarEWPFormModel();
        }

        public CrearEditarEWPViewModel(CrearEditarEWPFormModel f) : this()
        {
            Form = f;
        }
    }
}