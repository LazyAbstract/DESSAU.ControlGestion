using AutoMapper;
using DESSAU.ControlGestion.Core;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    [Validator(typeof(TimeSheetDTOValidation))]
    public class TimeSheetDTO
    {
        public int? IdTimeSheet { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdActividad { get; set; }
        public int? HorasPlanificadas { get; set; }
        public int? HorasReportadas { get; set; }
        [Required]
        public int? Horas { get; set; }
    }

    public class TimeSheetDTOValidation : AbstractValidator<TimeSheetDTO>
    {
        public TimeSheetDTOValidation()
        {
            RuleFor(x => x.Horas).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(10);
        }
    }
}