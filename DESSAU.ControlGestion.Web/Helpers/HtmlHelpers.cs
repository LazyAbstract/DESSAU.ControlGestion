﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DESSAU.ControlGestion.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString ButtonIconActionLink(this HtmlHelper htmlHelper, string icon, string buttonTooltip, string action, string controllerName, object htmlAttributes, object routeValues)
        {
            TagBuilder builder;
            UrlHelper urlHelper;
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            builder = new TagBuilder("a");
            builder.InnerHtml = String.Format(@"<button type=""button"" class=""btn btn-default btn-xs"" >
  <span class=""glyphicon glyphicon-{0}"" rel=""tooltip"" title=""{1}""></span>
</button>", !String.IsNullOrEmpty(icon) ? icon : "start", buttonTooltip);
            builder.Attributes["href"] = urlHelper.Action(action, controllerName, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(builder.ToString());
        }

        //public static string ImprimeFormatoHoraAufen(this long ticks)
        //{
        //    return String.Format("{0}:{1}", Math.Floor(TimeSpan.FromTicks(ticks).TotalHours), (Math.Floor(TimeSpan.FromTicks(ticks).TotalMinutes % 60).ToString("00")));
        //}

        public static string ApreciacionGlobal(this HtmlHelper htmlHelper, double Promedio)
        {
            string Apreciacion = "Excepcional";
            if (Promedio < 2) Apreciacion = "Deficiente";
            else if(Promedio < 2.5) Apreciacion = "Mejorable";
            else if (Promedio < 3.5) Apreciacion = "Satisfactorio";
            else if (Promedio < 4.5) Apreciacion = "Notable";
            return Apreciacion;
        }
    }
}