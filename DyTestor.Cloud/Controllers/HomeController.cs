using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DyTestor.Cloud.Models;
using DyTestor.Infrastructure;
using DyTestor.SericeContracts;

namespace DyTestor.Cloud.Controllers
{
    public class HomeController : Controller
    {
        private IQRCodeService qrcodeService;

        public HomeController()
        {
            this.qrcodeService = ServiceLocator.GetService<IQRCodeService>();
        }
        public IActionResult Index(DateTime date)
        {
            List<DateTime> dateList = this.qrcodeService.GetDateList();
            if (date == DateTime.MinValue)
                date = dateList.FirstOrDefault();
            if (date == DateTime.MinValue)
                date = DateTime.Now;
            ViewBag.Date = date;
            ViewBag.DateList = dateList;          
            ViewBag.CodeList = this.qrcodeService.GetList(date);
            return View();
        }

    }
}
