using DESSAU.ControlGestion.Core;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DESSAU.ControlGestion.Web.Models.NumeroDocumentoModels
{
    public class ListarNumeroDocumentoViewModel
    {
        public IPagedList<NumeroDocumento> NumeroDocumentos { get; set; }
        public string filtro { get; set; }
    }
}