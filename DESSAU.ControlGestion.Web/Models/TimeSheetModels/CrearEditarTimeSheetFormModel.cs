using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class CrearEditarTimeSheetFormModel
    {
        public int idUsuario { get; set; }
        public List<DateTime> FechaDeseHasta { get; set; }
        public IEnumerable<TimeSheetDTO> TimeSheetDTOs { get; set; }
        public IEnumerable<DiaEspecial> DiaEspecials { get; set; }
}

    public class TimeSheetDTO
    {
        public int? idTimeSheet { get; set; }
        public DateTime Fecha { get; set; }
        public int? IdTipoActividad { get; set; }
        public int? HorasPlanificadas { get; set; }
        public int? HorasReportadas { get; set; }
    }

}