using AutoMapper;
using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class TimeSheetDTO
    {
        public int? IdTimeSheet { get; set; }
        public DateTime Fecha { get; set; }
        public int? IdActividad { get; set; }
        public int? HorasPlanificadas { get; set; }
        public int? HorasReportadas { get; set; }
        public int? IdUsuarioCategoriaProyecto { get; set; }
        public int? Horas { get; set; }
    }

}