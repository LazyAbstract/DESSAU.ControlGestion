using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DESSAU.ControlGestion.Web.Helpers;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class VerTimeSheetViewModel
    {
        public VerTimeSheetFormModel FORM { get; set; }
        public List<DateTime> FechaDesdeHasta
        {
            get
            {
                DateTime startMonday = FORM.Fecha.Value.StartOfWeek(DayOfWeek.Monday);
                return Enumerable.Range(0, 4)
                    .Select(offset => startMonday.AddDays(offset)).ToList();
            }
        }
        public IEnumerable<UsuarioCategoriaProyecto> UsuarioCategoriaProyectos { get; set; }
        public IEnumerable<TimeSheet> TimeSheets { get; set; }

        public VerTimeSheetViewModel()
        {
            FORM = new VerTimeSheetFormModel();
        }

        public VerTimeSheetViewModel(VerTimeSheetFormModel form) : this()
        {
            FORM = form;
        }
    }
}