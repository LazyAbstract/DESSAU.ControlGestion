using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DESSAU.ControlGestion.Web.Helpers;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class VerTimeSheetViewModel
    {
        public int IdTipoPlanificacion { get; set; }
        public VerTimeSheetFormModel FORM { get; set; }
        public IEnumerable<TimeSheetDTO> TimeSheetFORM { get; set; }
        public SelectList Proyectos { get; set; }
        public SelectList Categorias { get; set; }
        public List<DateTime> FechaDeseHasta { get; set; }
        public IEnumerable<DiaEspecial> DiaEspecials { get; set; }

        private List<DateTime> _FechaDesdeHasta { get; set; }

        public List<DateTime> getFechaDesdeHasta
        {
            get
            {
                return _FechaDesdeHasta;
            }
        }

        public VerTimeSheetViewModel()
        {
            FORM = new VerTimeSheetFormModel();
            TimeSheetFORM = new List<TimeSheetDTO>();
            DateTime startMonday = FORM.Fecha.Value.StartOfWeek(DayOfWeek.Monday);
            _FechaDesdeHasta =  Enumerable.Range(0, 5)
                .Select(offset => startMonday.AddDays(offset)).ToList();
        }
        public IEnumerable<UsuarioCategoriaProyecto> UsuarioCategoriaProyectos { get; set; }
        public IEnumerable<TimeSheet> TimeSheets { get; set; }

        public VerTimeSheetViewModel(DESSAUControlGestionDataContext db):this()
        {
            Proyectos = new SelectList(db.Proyectos, "IdProyecto", "Nombre");
            Categorias = new SelectList(db.Categorias, "IdCategoria", "Nombre");
            DiaEspecials = db.DiaEspecials.Where(x=> getFechaDesdeHasta.Contains(x.Fecha.Date));
        }

        public VerTimeSheetViewModel(VerTimeSheetFormModel form, DESSAUControlGestionDataContext db) :this(db)
        {
            FORM = form;
        }
    }
}