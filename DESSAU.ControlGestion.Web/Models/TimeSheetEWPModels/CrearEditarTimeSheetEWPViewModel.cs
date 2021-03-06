﻿using DESSAU.ControlGestion.Core;
using DESSAU.ControlGestion.Web.Helpers;
using DESSAU.ControlGestion.Web.SelectListProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetEWPModels
{
    public class CrearEditarTimeSheetEWPViewModel
    {
        private DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public CrearEditarTimeSheetEWPFormModel Form { get; set; }
        public IEnumerable<Actividad> Actividades { get; set; }
        public IEnumerable<UsuarioCategoriaProyecto> UsuarioCategoriaProyectos { get; set; }
        public IEnumerable<SelectListItem> EWP { get; set; }
        public SelectList SubEWP { get; set; }
        public SelectList TipoDocumento { get; set; }
        public SelectList Revision { get; set; }
        public SelectList NumeroDocumento { get; set; }
        public IEnumerable<TimeSheetEWP> TimeSheetsEWP { get; set; }
        public IEnumerable<DiaEspecial> DiaEspecials { get; set; }
        private EWPSelectListProvider Eslp = new EWPSelectListProvider();
        public IEnumerable<SelectListItem> Usuarios { get; set; }
        private UsuarioSelectListProvider uslp = new UsuarioSelectListProvider();
        public TurnoHorarioHelper turnoHelper { get; set; }

        public CrearEditarTimeSheetEWPViewModel()
        {
            Form = new CrearEditarTimeSheetEWPFormModel();
            turnoHelper = new TurnoHorarioHelper();
            EWP = Eslp.Provide();
            SubEWP = new SelectList(db.SubEWPs, "IdSubEWP", "Codigo");
            TipoDocumento = new SelectList(db.TipoDocumentos, "IdTipoDocumento", "Nombre");
            Revision = new SelectList(db.Revisions, "IdRevision", "Nombre");
            NumeroDocumento = new SelectList(db.NumeroDocumentos.Where(x => x.Vigente), "IdNumeroDocumento", "Codigo");
            
            DiaEspecials = db.DiaEspecials;
            Usuarios = uslp.Provide();
        }

        public CrearEditarTimeSheetEWPViewModel(CrearEditarTimeSheetEWPFormModel f) : this()
        {
            Form = f;
           
        }
    }
}