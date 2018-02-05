using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.RevisionModels
{
    public class CrearEditarRevisionViewModel
    {
        public CrearEditarRevisionFormModel Form { get; set; }

        public CrearEditarRevisionViewModel()
        {
            Form = new CrearEditarRevisionFormModel();
        }

        public CrearEditarRevisionViewModel(CrearEditarRevisionFormModel F) : this()
        {
            Form = F;
        }
    }
}