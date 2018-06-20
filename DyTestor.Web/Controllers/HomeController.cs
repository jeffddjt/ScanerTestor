using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DyTestor.Web.Models;
using DyTestor.Configuration;
using System.Data.SqlClient;
using DyTestor.SericeContracts;
using DyTestor.Infrastructure;
using DyTestor.DataObject;

namespace DyTestor.Web.Controllers
{
    public class HomeController : Controller
    {
        private Communicator communicator;
        private IQRCodeService qRCodeService;

        public HomeController()
        {
            this.communicator = new Communicator();
            this.qRCodeService = ServiceLocator.GetService<IQRCodeService>();
            
        }
        public IActionResult Index(int? pageNo,int? pageSize)
        {
            int pageCount = 0;
            int total = 0;
            if (pageNo == null) pageNo = 1;
            if (pageSize == null) pageSize = 20;
            IList<QRCodeDataObject> result = this.qRCodeService.GetList(pageNo.Value,pageSize.Value,out pageCount,out total);
            ViewBag.QRList = result;
            ViewBag.PageNo = pageNo.Value;
            ViewBag.PageSize = pageSize.Value;
            ViewBag.PageCount = pageCount;
            ViewBag.Total = total;
            return View();
        }
        public IActionResult Config()
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
