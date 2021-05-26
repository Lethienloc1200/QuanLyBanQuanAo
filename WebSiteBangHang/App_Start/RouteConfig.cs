using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebSiteBangHang
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //Cấu hình đường dẫn(khong tham số) trang index của khách hàng thnhf khach-hang

            routes.MapRoute(
                name: "Khachhang",
                url: "khach-hang",
                defaults: new { controller = "KhachHang", action = "Index", id = UrlParameter.Optional }
            );
            //cấu hình đường dẫn của trang xem chi tiết controller san pham (có tham sso)

            routes.MapRoute(
               name: "XemChiTiet",
               url: "{tensp}-{id}",
               defaults: new { controller = "SanPham", action = "XemChiTiet", id = UrlParameter.Optional }
           );



            //routes chính phải đặt dưới cùng
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
