﻿using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DESSAU.ControlGestion.Web.Helpers;
using System.Web.Mvc;
using DESSAU.ControlGestion.Web.SelectListProviders;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class VerTimeSheetViewModel
    {
        public VerTimeSheetFormModel FORM { get; set; }
        public IEnumerable<TimeSheetCategoriaProyectoDTO> TimeSheetFORM { get; set; }
        public SelectList Proyectos { get; set; }
        public SelectList Categorias { get; set; }
        public IEnumerable<SelectListItem> Usuarios { get; set; }
        private UsuarioSelectListProvider uslp = new UsuarioSelectListProvider();
        public List<DateTime> FechaDeseHasta { get; set; }
        public IEnumerable<DiaEspecial> DiaEspecials { get; set; }
        public CalculoHoraMensual calc { get; set; }
        private List<DateTime> _FechaDesdeHasta { get; set; }
        public List<DateTime> getFechaDesdeHasta
        {
            get
            {
                return _FechaDesdeHasta;
            }
        }
        public bool EsReporte { get; set; }
        public VerTimeSheetViewModel()
        {
            FORM = new VerTimeSheetFormModel();
            TimeSheetFORM = new List<TimeSheetCategoriaProyectoDTO>();
            DateTime startMonday = FORM.Fecha.Value.StartOfWeek(DayOfWeek.Monday);
            _FechaDesdeHasta =  Enumerable.Range(0, 5)
                .Select(offset => startMonday.AddDays(offset)).ToList();
            Usuarios = uslp.Provide();
            EsReporte = false;
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
            DateTime startMonday = FORM.Fecha.Value.StartOfWeek(DayOfWeek.Monday);
            _FechaDesdeHasta = Enumerable.Range(0, 5)
                .Select(offset => startMonday.AddDays(offset)).ToList();
        }
    }
}