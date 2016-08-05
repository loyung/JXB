using JXB.PublicMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JXB.Areas.API.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /API/Home/
        public ActionResult Index(string Name)
        {
            return View((object)Name);
        }

        public ActionResult Error(string Msg)
        {
            return ReturnJsonApi(Msg, null);
        }
	}
}