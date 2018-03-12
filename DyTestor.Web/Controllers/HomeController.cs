using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DyTestor.Web.Models;
using DyTestor.Configuration;
using System.Data.SqlClient;

namespace DyTestor.Web.Controllers
{
    public class HomeController : Controller
    {
        private Communicator communicator;

        public HomeController()
        {
            this.communicator = new Communicator();
        }
        public IActionResult Index()
        {
            try
            {
                DyConfig config = this.communicator.GetConfig();
                ViewBag.Configuration = config;
            }
            catch
            {
                ViewBag.Configuration = null;
            }
            return View();
        }

        public void SaveConfig(DyConfig dyconfig)
        {
            this.communicator.SaveConfig(dyconfig);
        }
    }
}
