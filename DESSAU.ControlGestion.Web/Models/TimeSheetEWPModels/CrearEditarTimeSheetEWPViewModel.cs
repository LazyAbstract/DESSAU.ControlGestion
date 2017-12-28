using DESSAU.ControlGestion.Core;
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
        private EWPSelectListProvider Eslp = new EWPSelectListProvider();

        public CrearEditarTimeSheetEWPViewModel()
        {
            Form = new CrearEditarTimeSheetEWPFormModel();

            EWP = Eslp.Provide();
            SubEWP = new SelectList(db.SubEWPs, "IdSubEWP", "Codigo");
            TipoDocumento = new SelectList(db.TipoDocumentos, "IdTipoDocumento", "Nombre");
            Revision = new SelectList(db.Revisions, "IdRevision", "Nombre");
            NumeroDocumento = new SelectList(db.NumeroDocumentos, "IdNumeroDocumento", "Codigo");
        }

        public CrearEditarTimeSheetEWPViewModel(CrearEditarTimeSheetEWPFormModel f) : this()
        {
            Form = f;
           
        }
    }
}