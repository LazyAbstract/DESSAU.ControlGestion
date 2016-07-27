using AutoMapper;
using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class CrearEditarTimeSheetFormModel
    {
        public int IdUsuario { get; set; }
        public IEnumerable<TimeSheetDTO> TimeSheetDTOs { get; set; }

        public CrearEditarTimeSheetFormModel()
        {
            TimeSheetDTOs = new List<TimeSheetDTO>();
        }

        public CrearEditarTimeSheetFormModel(IEnumerable<TimeSheet> timeSheets):this()
        {
            Mapper.Map<IEnumerable<TimeSheet>, IEnumerable<TimeSheetDTO>>(timeSheets, TimeSheetDTOs);
        }
}

    public class TimeSheetDTO
    {
        public int? IdTimeSheet { get; set; }
        public DateTime Fecha { get; set; }
        public int? IdActividad { get; set; }
        public int? HorasPlanificadas { get; set; }
        public int? HorasReportadas { get; set; }
        public int? IdUsuarioCategoriaProyecto { get; set; }
    }

}