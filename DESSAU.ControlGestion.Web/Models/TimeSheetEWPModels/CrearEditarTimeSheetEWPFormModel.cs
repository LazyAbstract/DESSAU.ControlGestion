using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetEWPModels
{
    public class CrearEditarTimeSheetEWPFormModel : IDataErrorInfo
    {
        public int? IdActividadEWP { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdUsuarioCategoriaProyecto { get; set; }
        [DisplayName("EWP")]
        public int? IdEWP { get; set; }
        [DisplayName("SubEWP")]
        public int? IdSubEWP { get; set; }
        [DisplayName("Tipo Documento")]
        public int? IdTipoDocumento { get; set; }
        [DisplayName("Revisión")]
        public int? IdRevision { get; set; }
        [DisplayName("Número Documento DESSAU")]
        public int? IdNumeroDocumento { get; set; }   
        public DateTime? Fecha { get; set; }      
        public int HorasReportadasEWP { get; set; }
        public IEnumerable<TimeSheetFormModelEWPDTO> DTO { get; set; }
        public List<int> DTOvalues { get; set; }
        public bool validar { get; set; }
        public CrearEditarTimeSheetEWPFormModel()
        {
            validar = false;
            DTOvalues = new List<int>();
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                if (validar)
                {
                    DESSAUControlGestionDataContext db = new DESSAUControlGestionDataContext()
                        .WithConnectionStringFromConfiguration();

                    if (Fecha.Value.DayOfWeek == DayOfWeek.Saturday || Fecha.Value.DayOfWeek == DayOfWeek.Sunday)
                        return "No se pueden reportar horas trabajadas durante el fin de semana";

                    if (HorasReportadasEWP == 0 && DTO.Sum(x => x.HorasReportadas) == 0) return "Las Horas a reportar no pueden ser igual a 0.";
                    if (HorasReportadasEWP > 0)
                    {
                        string errorEWP = "Para reportar EWP: ";
                        bool hayError = false;
                        if (!IdSubEWP.HasValue) { errorEWP += "La SubEWP es requerida. "; hayError = true; }
                        if (!IdTipoDocumento.HasValue) { errorEWP += "El Tipo de Documento es requerido. "; hayError = true; }
                        if (!IdRevision.HasValue) { errorEWP += "La Revisión es requerida. "; hayError = true; }
                        if (!IdNumeroDocumento.HasValue) { errorEWP += "El Número de Documento es requerido."; hayError = true; }
                        if (hayError) return errorEWP;
                    }

                    var timeSheetsEWP = db.TimeSheetEWPs
                        .Where(x => x.IdUsuarioCategoriaProyecto == IdUsuarioCategoriaProyecto && x.Fecha == Fecha.Value);
                    //var timeSheets = db.TimeSheets
                    //    .Where(x => x.IdUsuarioCategoriaProyecto == IdUsuarioCategoriaProyecto && x.Fecha == Fecha.Value);

                    int sumaEWP = 0;
                    if (timeSheetsEWP.Any())
                    {
                        sumaEWP = timeSheetsEWP.Sum(x => x.HorasReportadas.GetValueOrDefault(0));
                    }

                    //int suma = 0;
                    //if (timeSheets.Any())
                    //{
                    //    suma = timeSheets.Sum(x => x.HorasReportadas.GetValueOrDefault(0));
                    //}

                    int HorasReportadasDTO = 0;
                    if(DTO.Any())
                    {
                        HorasReportadasDTO = DTO.Sum(x => x.HorasReportadas);
                    }

                    //if(HorasReportadasDTO > 0)

                    if ((Fecha.Value.DayOfWeek != DayOfWeek.Friday) && (sumaEWP + HorasReportadasEWP + HorasReportadasDTO) > 10)
                    {
                        return "Está reportando " + (sumaEWP + HorasReportadasDTO + HorasReportadasEWP).ToString() + " horas trabajadas para el día "
                            + Fecha.Value.ToShortDateString() + ". El máximo es de 10 horas para los días de lunes a jueves.";
                    }
                    else if (Fecha.Value.DayOfWeek == DayOfWeek.Friday && sumaEWP + HorasReportadasEWP + HorasReportadasDTO > 5)
                    {
                        return "Está reportando " + (sumaEWP + HorasReportadasDTO + HorasReportadasEWP).ToString() + " horas trabajadas para el día "
                            + Fecha.Value.ToShortDateString() + ". El máximo es de 5 horas para el día viernes.";
                    }
                }
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get { return string.Empty; }
        }

        #endregion
    }
}