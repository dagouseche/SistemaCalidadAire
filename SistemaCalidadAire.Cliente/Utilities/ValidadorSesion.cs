using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaCalidadAire.Cliente.Utilities
{
    public class ValidadorSesion : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsNewSession || string.IsNullOrEmpty(Data.UsuarioLogeado))
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Index", "Sitio");

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Result = new JavaScriptResult() { Script = "window.location = '" + url + "'" };
                else
                    filterContext.Result = new RedirectResult(url);
            }
        }
    }
}