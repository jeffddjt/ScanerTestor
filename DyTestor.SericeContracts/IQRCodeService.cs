using DyTestor.DataObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.SericeContracts
{
    public interface IQRCodeService
    {
        void Add(QRCodeDataObject code);
    }
}
