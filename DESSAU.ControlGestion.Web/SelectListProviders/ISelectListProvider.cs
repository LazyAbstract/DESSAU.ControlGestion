using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DESSAU.ControlGestion.Web.SelectListProviders
{
    interface ISelectListProvider
    {
        SelectList Provide(object selected);
    }
}
