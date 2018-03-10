using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DyTestor.Web.Models;
using DyTestor.Configuration;

namespace DyTestor.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Configuration = new DyConfig();
            return View();
        }

        public string SaveConfig(DyConfig dyconfig)
        {
            return "ok";
        }
    }
}
