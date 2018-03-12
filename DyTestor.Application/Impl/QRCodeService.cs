using DyTestor.DataObject;
using DyTestor.Domain.Model;
using DyTestor.Repositories.Repository;
using DyTestor.SericeContracts;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public QRCodeDataObject Add(QRCodeDataObject qrcode)
        {
            QRCode code = DyMapper.Map<QRCodeDataObject, QRCode>(qrcode);
            this.repository.Add(code);
            this.repository.Commit();
            return DyMapper.Map<QRCode, QRCodeDataObject>(code);
        }

        public List<DateTime> GetDateList()
        {
            return this.repository.GetAll().Select(p => p.CreateTime.Date).OrderBy(p=>p).Distinct().ToList();
        }

        public List<QRCodeDataObject> GetList()
        {
            return DyMapper.Map<List<QRCode>, List<QRCodeDataObject>>(this.repository.GetAll().OrderBy(p => p.CreateTime).ToList());
        }

        public List<QRCodeDataObject> GetList(DateTime date)
        {
            List<QRCode> codeList = this.repository.GetAll().Where(p => p.CreateTime.Date == date).OrderBy(p => p.CreateTime).ToList();
            return DyMapper.Map<List<QRCode>, List<QRCodeDataObject>>(codeList);
        }
    }
}
