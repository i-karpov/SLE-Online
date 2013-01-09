using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace SystemsOfLinearEquations.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult HttpError404()
        {
            Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
            return View();
        }

        public ViewResult GeneralError()
        {
            Response.StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
            return View();
        }
    }
}
