using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SystemsOfLinearEquations.Controllers;

namespace SystemsOfLinearEquations.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null,
                "{controller}/{action}/",
                new { controller = "SLE", action = "Index"}
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Errors");

            Exception exception = Server.GetLastError();
            int code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;

            switch (code)
            {
                case 404:
                    routeData.Values.Add("action", "HttpError404");
                    break;
                default:
                    routeData.Values.Add("action", "GeneralError");
                    break;
            }

            Server.ClearError();
            Response.Clear();

            Response.TrySkipIisCustomErrors = true;

            IController errorsController = new ErrorsController();
            errorsController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));

        }
    }
}