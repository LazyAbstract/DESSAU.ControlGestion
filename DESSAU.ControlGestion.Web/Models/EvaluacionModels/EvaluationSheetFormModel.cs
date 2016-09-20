using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class EvaluationSheetFormModel
    {
        public int? IdProyecto { get; set; }
        [DisplayName("Categoría")]
        public int? IdCategoria { get; set; }

        [Required]
        [DisplayName("Fecha")]
        public DateTime? Fecha { get; set; }
        public string ClaseBootstrap { get; set; }

    }

    public class EvaluacionFormModel
    {
        public int? IdEvaluacion { get; set; }

        public int? IdUsuario { get; set; }

        [Required]
        public DateTime? Fecha {get;set;}

        public IEnumerable<EvaluacionPreguntaDTO> EvaluacionPreguntaDTOs { get; set; }

    }

    public class EvaluacionPreguntaDTO
    {
        public int? IdEvaluacionPregunta { get; set; }

        public int? IdPregunta { get; set; }

        [Required]
        public int? ValorObtenido { get; set; }
    }
}