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
                    if(bufferSumHoras!=0 && bufferSumHoras != 10)
                    {
                        diasErroneos.Add(item.Key.Value);
                    }
                }
                if (diasErroneos.Any())
                {
                    return new ValidationFailure("TimeSheetDTOs", 
                        String.Format("De ingresar valores en {0}, estas deben sumar 10 horas por día.", 
                            String.Join(", ",diasErroneos.Select(y=> y.ToString("ddd dd/MM"))))
                        );
                }
                return null;
            });
        }
    }
}