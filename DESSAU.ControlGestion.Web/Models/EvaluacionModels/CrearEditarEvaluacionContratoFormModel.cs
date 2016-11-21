using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EvaluacionModels
{
    public class CrearEditarEvaluacionContratoFormModel
    {       
        [Required]
        public string Periodo { get; set; }
        //public int? IdEvaluacionContrato { get; set; } 
    }

    public class EvaluacionContratoFormModel
    {
        public int? IdEvaluacionContrato { get; set; }
        public IEnumerable<EvaluacionContratoPreguntaDTO> EvaluacionContratoPreguntaDTOs { get; set; }
        public DateTime? Fecha { get; set; }

        //  me estoy llendo un poco a lashit con la copia
        public int IdContrato { get; set; }
    }

    public class EvaluacionContratoPreguntaDTO
    {
        public int? IdEvaluacionContratoPregunta { get; set; }        
        public int? IdPregunta { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Los valores asignados deben ser entre 1 y 5")]
        public int? ValorObtenido { get; set; } 
    }
}