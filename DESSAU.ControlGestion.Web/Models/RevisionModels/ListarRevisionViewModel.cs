using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.RevisionModels
{
    public class ListarRevisionViewModel
    {
        public IPagedList<Revision> Revisiones { get; set; }
        public string filtro { get; set; }
    }
}