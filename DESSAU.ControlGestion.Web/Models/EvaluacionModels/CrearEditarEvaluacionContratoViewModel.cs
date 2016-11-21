using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class CrearEditarEvaluacionContratoViewModel
    {
        DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
            .WithConnectionStringFromConfiguration();
        public CrearEditarEvaluacionContratoFormModel Form { get; set; }
        public IEnumerable<EvaluacionContratoFormModel> EvaluacionContratoFORMs { get; set; }
        public IEnumerable<Pregunta> Preguntas { get; set; }
        public PlantillaEvaluacionContrato PlantillaEvaluacionContrato { get; set; }
        public string Periodo { get; set; }
        public IEnumerable<Contrato> Contratos { get; set; }

        public CrearEditarEvaluacionContratoViewModel()
        {
            Form = new CrearEditarEvaluacionContratoFormModel();
            EvaluacionContratoFORMs = new List<EvaluacionContratoFormModel>();
        }

        public CrearEditarEvaluacionContratoViewModel(CrearEditarEvaluacionContratoFormModel F) : this()
        {
            Form = F;
            //  esto sé que es la uno por ahora...
            PlantillaEvaluacionContrato = db.PlantillaEvaluacionContratos.SingleOrDefault(x => x.IdContrato == 1);
            Preguntas = PlantillaEvaluacionContrato.PlantillaEvaluacionContratoPreguntas.Select(x => x.Pregunta);
            //  sigo estirando este elástico...
            Contratos = db.Contratos.Where(x => x.IdContrato == 1);
        }
    }
}