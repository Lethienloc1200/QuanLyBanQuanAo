using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBangHang.Models;

namespace WebSiteBangHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            //gio hàng da ton tai
            List<ItemGioHang> lstGioHang =Session["GioHang"] as List<ItemGioHang>;
            if( lstGioHang == null)
            {
                // nếu  sesion giỏ hàng chưa tồn tại thì khởi tạo giở hàng bằng 
               lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
      
        }
       //////////// //Thêm giỏ hàng thông thuongef 
        public ActionResult ThemGioHang(int MaSP ,string strURL)
        {
            //Kiển tra Sản phẩm có tông tại trong CSDL   hay không
            SanPham SP = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(SP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //trường hợp thứ nhất sản phẩm dẫ tồn tại trong giỏ hàng

            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if( spCheck != null)
            {
                //kieerm tra trước khi cho khách hàng mua hàng
                if(SP.SoLuongTon < spCheck.SoLuong) //spCheck
                {
                    return View("ThongBao");
                }    
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
           
            ItemGioHang itemGH = new ItemGioHang(MaSP) ;
            if (SP.SoLuongTon < itemGH.SoLuong)//item
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        ///////////// phương tính tổng số lượng
        public double TinhTongSoLuong()
        {
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if( lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n =>n.SoLuong);

        }
        ///////////// phương thức tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }




        // GET: GioHang

        //trả về cái partial không cần tạo controoler
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong()==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
                    
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult XemGioHang()
        {
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            return View(lstGioHang);
        }
        //chỉnh sửa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            if( Session["GioHang"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            //SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            //Kiển tra Sản phẩm có tông tại trong CSDL   hay không
            SanPham SP = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (SP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy ra list gio hàng từ sesion
            List<ItemGioHang> lstGioHang = LayGioHang();
            //kieermtr a sản phẩm đó có tồn tại trong gio hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck ==null)
            {
                return RedirectToAction("Index", "Home");
            }
            //lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            //nếu tồn tại thì 
            return View(spCheck);
        
        }
        //xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {

            //kieerm tra Số Lượng tồn
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if(spCheck.SoLuongTon < itemGH.SoLuong )
            {
                return View("ThongBao");
            }
            //Cập nhật số lượng trong seseion gio hàng
            //bước 1 : lây list <gioHang> từ giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            //bước 2 lấy Sản phầm cần cập nhật từ giỏ hàng ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
           //tiến hành cập nhật lại số lượng cũng như thành tiền
            itemGHUpdate.SoLuong = itemGH.SoLuong;// update số lượng
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;// update thành tiền
            return RedirectToAction("XemGioHang");
           
        }
        //==========Xoa=========
        public ActionResult XoaGioHang(int? MaSP)
        {
            //kieermt ra sesion gio hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            //Kiển tra Sản phẩm có tông tại trong CSDL   hay không
            SanPham SP = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (SP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy ra list gio hàng từ sesion
            List<ItemGioHang> lstGioHang = LayGioHang();
            //kieermtr a sản phẩm đó có tồn tại trong gio hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xóa item trong giohang
            lstGioHang.Remove(spCheck); //xóa easy
            return RedirectToAction("XemGioHang");
        }
        //xây dựng chức năng đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            //kiểm tra sesion giỏ hàng tồn tại chưa
            if(Session["GioHang"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if(Session["TaiKhoan"]==null)
            {
                //thêm khách hàng vào bẩng khách hàng chưa có tài khoản
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else
            {   
                //đối với khách hnngf là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen; //chú ý "tv" là lấy từ đăng nhập
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }    
            //Thêm đon hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            // ddh.DaHuy = false; chưa tạo
            //ddh.DaXoa = false; chưa tạo
            db.DonDatHangs.Add(ddh);
            db.SaveChanges(); //savecahngse để sinh ra MaDDH

            //ThemChi tiết đơn đặt hàng
            List<ItemGioHang> lstGh = LayGioHang();
            foreach(var item in lstGh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonHangs.Add(ctdh);
            }
              db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }

        //thêm giỏ hàng ajax
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            //Kiển tra Sản phẩm có tông tại trong CSDL   hay không
            SanPham SP = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (SP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //trường hợp thứ nhất sản phẩm dẫ tồn tại trong giỏ hàng

            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //kieermt ra trước khi cho khách hàng mua hàng
                if (SP.SoLuongTon < spCheck.SoLuong) //spCheck
                {
                    return Content("<script> alert(\" Sản phẩm đã hết hàng\")</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (SP.SoLuongTon < itemGH.SoLuong)//item
            {
                return Content("<script> alert(\" Sản phẩm đã hết hàng\")</script>");
            }
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }

    }
}