using DyTestor.DataObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DyTestor.SericeContracts
{
    public interface IQRCodeService
    {
        Task<QRCodeDataObject> Add(QRCodeDataObject code);
        Task<List<QRCodeDataObject>> GetList();
        List<QRCodeDataObject> GetList(int pageNo, int pageSize, out int pageCount,out int total);
        List<DateTime> GetDateList();
        List<QRCodeDataObject> GetList(DateTime date);
    }
}
