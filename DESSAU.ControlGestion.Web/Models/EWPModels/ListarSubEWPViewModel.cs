using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EWPModels
{
    public class ListarSubEWPViewModel
    {
        public IPagedList<SubEWP> SubEWPs { get; set; }
        public string filtro { get; set; }
    }
}