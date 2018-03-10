using DyTestor.DataObject;
using DyTestor.Domain.Model;
using DyTestor.Repositories.Repository;
using DyTestor.SericeContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Application.Impl
{
    public class QRCodeService:IQRCodeService
    {
        private QRCodeRepository repository;
        public QRCodeService(QRCodeRepository repository)
        {
            this.repository = repository;
        }

        public void Add(QRCodeDataObject qrcode)
        {
            QRCode code = DyMapper.Map<QRCodeDataObject, QRCode>(qrcode);
            this.repository.Add(code);
            this.repository.Commit();
        }
    }
}
