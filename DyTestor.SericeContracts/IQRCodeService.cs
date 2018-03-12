using DyTestor.DataObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.SericeContracts
{
    public interface IQRCodeService
    {
        QRCodeDataObject Add(QRCodeDataObject code);
        List<QRCodeDataObject> GetList();
        List<DateTime> GetDateList();
        List<QRCodeDataObject> GetList(DateTime date);
    }
}
