using DyTestor.DataObject;
using DyTestor.Domain.Model;
using DyTestor.Repositories.Repository;
using DyTestor.SericeContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyTestor.Application.Impl
{
    public class QRCodeService:IQRCodeService
    {
        private QRCodeRepository repository;
        public QRCodeService(QRCodeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<QRCodeDataObject> Add(QRCodeDataObject qrcode)
        {
            QRCode code = DyMapper.Map<QRCodeDataObject, QRCode>(qrcode);
            await this.repository.Add(code);
            await this.repository.Commit();
            return DyMapper.Map<QRCode, QRCodeDataObject>(code);
        }

        public List<DateTime> GetDateList()
        {
            return this.repository.GetAll().Select(p => p.CreateTime.Date).OrderBy(p=>p).Distinct().ToList();
        }

        public async Task<List<QRCodeDataObject>> GetList()
        {
            List<QRCode> list = await this.repository.GetAll().OrderBy(p => p.CreateTime).ToAsyncEnumerable().ToList();
            return DyMapper.Map<List<QRCode>, List<QRCodeDataObject>>(list);
        }
        public List<QRCodeDataObject> GetList(int pageNo,int pageSize,out int pageCount,out int total)
        {
            var query = this.repository.GetAll();
            total = query.Count();
            pageCount = (total+pageSize-1) / pageSize;
            IQueryable<QRCode> list = this.repository.GetAll().OrderBy(p => p.CreateTime).Skip((pageNo - 1) * pageSize).Take(pageSize);

            return DyMapper.Map<List<QRCode>, List<QRCodeDataObject>>(list.ToList());
        }

        public List<QRCodeDataObject> GetList(DateTime date)
        {
            List<QRCode> codeList = this.repository.GetAll().Where(p => p.CreateTime.Date == date).OrderBy(p => p.CreateTime).ToList();
            return DyMapper.Map<List<QRCode>, List<QRCodeDataObject>>(codeList);
        }
    }
}
