using DyTestor.DataObject;
using DyTestor.Domain.Model;
using DyTestor.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Application.Impl
{
    public class QRCodeService
    {
        private QRCodeRepository repository;
        public QRCodeService()
        {
            this.repository = new QRCodeRepository();
        }

        public void Add(QRCodeDataObject qrcode)
        {
            QRCode code = DyMapper.Map<QRCodeDataObject, QRCode>(qrcode);
            this.repository.Add(code);
            this.repository.Commit();
        }
    }
}
