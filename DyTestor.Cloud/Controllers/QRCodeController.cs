using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyTestor.DataObject;
using DyTestor.Infrastructure;
using DyTestor.SericeContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DyTestor.Cloud.Controllers
{
    [Produces("application/json")]
    [Route("api/QRCode/[action]")]
    public class QRCodeController : Controller
    {
        private IQRCodeService qrcodeService;

        public QRCodeController()
        {
            this.qrcodeService = ServiceLocator.GetService<IQRCodeService>();
        }
        public async Task<QRCodeDataObject> Add(QRCodeDataObject data)
        {
            return await this.qrcodeService.Add(data);
        }
    }
}