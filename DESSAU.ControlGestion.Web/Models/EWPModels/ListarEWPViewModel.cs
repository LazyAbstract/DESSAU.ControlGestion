using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.EWPModels
{
    public class ListarEWPViewModel
    {
        public IPagedList<EWP> EWPs { get; set; }
        public string filtro { get; set; }
    }
}