using DESSAU.ControlGestion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.TimeSheetModels
{
    public class VerTimeSheetEWPViewModel
    {
        public IEnumerable<TimeSheetEWP> TimeSheets { get; set; }
    }
}