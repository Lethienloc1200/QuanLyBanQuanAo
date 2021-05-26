using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSiteBangHang.Models.MetaData
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien  //phải có partial
    {
        internal sealed class ThanhVienMetadata
        {
           // đặt nội dung cập nhật tại đây để khi cập nhật model k bị mất đi
        }

    }
}