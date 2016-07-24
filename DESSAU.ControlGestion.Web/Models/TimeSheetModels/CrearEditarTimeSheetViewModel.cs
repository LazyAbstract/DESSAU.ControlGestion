using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class CrearEditarTimeSheetViewModel
    {
        public CrearEditarTimeSheetFormModel FORM { get; set; }

        public CrearEditarTimeSheetViewModel()
        {
            FORM = new CrearEditarTimeSheetFormModel();
        }
                
    }
}