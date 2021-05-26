using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBangHang.Models;//gọi using model
using System.Web.Security;

namespace WebSiteBangHang.Controllers
{
    [Authorize(Roles = "QuanTri")]  
    public class KhachHangController : Controller
    {
       
        // GET: KhachHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //[Authorize(Roles = "QuanLySanPham")]  //Phân QUYỀN

        public ActionResult Index()
        {
            //tao một lisst danh sach khach hang
            // truy vấn dữ liệu thông qua câu lệnh
            //đổi list sẽ lấy toàn bộ dữ liệu khách hàng
            ///////////cách 1:
            // var lstKH = from KH in db.KhachHangs select KH;
            //Cách 2 dùng phương thức hỗ trợ sẵn
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
           //ví dụ cách 1
                    public ActionResult View()
                    {
                        //taoj mootj lisst danh sach khach hang
                        var lstKH = from KH in db.KhachHangs select KH;
                        return View(lstKH);
                    }
        //public ActionResult TruyVanDoiTuong()
        //{
        //    //Truy vấn 1 doi ttuong bnagwf câu lệnh truy vấn
        //    //bước 1 lấy ra danh sách khách hàng
        //    var lstKH = from kh in db.KhachHangs where kh.MaKH==1 select kh;
        //    //bước 2 
        //    // KhachHang khang = lstKH.FirstOrDefault();//phần tử đầu tiên có thể trả về null ỏ đối tượng khách hàng
        //    //c2
        //    //lấy dối tượng khách hàng dựa trên phương thức hỗ trợ
        //    KhachHang khang = db.KhachHangs.SingleOrDefault(n=>n.MaKH==1);
        //    return View(khang);
        //}
        //public ActionResult SortDuLieu()
        //{
        //    //phương thức sắp xếp dữ liệu
        //    List<KhachHang> lstKH = db.KhachHangs.OrderBy(n => n.TenKH).ToList();
        //    return View(lstKH);
        //}
        //public ActionResult GroupDuLieu()
        //{
        //    //group dữ liệu
        //    List<ThanhVien> lstKH = db.ThanhViens.OrderBy(n => n.TaiKhoan).ToList();
        //    return View(lstKH);
        //}
    }
}