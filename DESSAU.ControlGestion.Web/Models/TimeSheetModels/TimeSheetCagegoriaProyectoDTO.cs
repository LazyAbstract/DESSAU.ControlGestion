using DESSAU.ControlGestion.Web.Models.TimeSheetEWPModels;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    [Validator(typeof(TimeSheetCagegoriaProyectoDTOValidation))]
    public class TimeSheetCategoriaProyectoDTO
    {
        public int? IdUsuarioCategoriaProyecto { get; set; }
        public IEnumerable<TimeSheetDTO> TimeSheetDTOs { get; set; }

        public TimeSheetCategoriaProyectoDTO()
        {
            TimeSheetDTOs = new List<TimeSheetDTO>();
        }
    }
    public class TimeSheetCagegoriaProyectoDTOValidation : AbstractValidator<TimeSheetCategoriaProyectoDTO>
    {
        public TimeSheetCagegoriaProyectoDTOValidation()
        {
            Custom(x => {
                List<DateTime> diasErroneos = new List<DateTime>();
                foreach (var item in x.TimeSheetDTOs.Where(y=> y.Fecha.HasValue).GroupBy(y => y.Fecha))
                {
                    var bufferSumHoras = item.Sum(y => y.Horas.GetValueOrDefault(0));
                    if(bufferSumHoras!=0 && bufferSumHoras != (item.Key.Value.DayOfWeek == DayOfWeek.Friday ? 5: 10))
                    {
                        diasErroneos.Add(item.Key.Value);
                    }
                }
                if (diasErroneos.Any())
                {
                    return new ValidationFailure("TimeSheetDTOs", 
                        String.Format("De ingresar valores correctos en {0}, estos deben sumar 10 horas por día de lunes a jueves y 5 horas los viernes.", 
                            String.Join(", ",diasErroneos.Select(y=> y.ToString("ddd dd/MM"))))
                        );
                }
                return null;
            });
        }
    }

    //[Validator(typeof(TimeSheetEWPValidation))]

    //public class TimeSheetEWPValidation : AbstractValidator<CrearEditarTimeSheetEWPFormModel>
    //{
    //    public TimeSheetEWPValidation()
    //    {
    //        Custom(x => 
    //        {
    //            if(db)
    //            return null;
    //        });
    //    }
    //}


}