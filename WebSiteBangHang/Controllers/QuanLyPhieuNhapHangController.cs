using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBangHang.Models;





namespace WebSiteBangHang.Controllers
{
    public class QuanLyPhieuNhapHangController : Controller
    {
        // GET: QuanLyPhieuNhapHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, ChiTietPhieuNhap lstModel)
        {

            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            //sau khi kiểm tra tất cả các dữ liệu dầu vào
            ////gán đã xóa
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            // //save change để lấy được mã phiếu nhập g
            lstModel.MaSP = model.MaPN;
            //cập nhật hàng tồn
            // SanPham sp = db.SanPhams.Single(n => n.MaSP == lstModel.MaSP);
            //  sp.SoLuongTon += lstModel.SoLuongNhap;
            //foreach (var item in lstModel)
            //{

            ////   //gán mã phiếu nhập cho tất cả chi tiết phiếu nhập
            // item.MaPN = model.MaPN;
            //}

            db.ChiTietPhieuNhaps.Add(lstModel);
            db.SaveChanges();

            return View();

        }
        ///////////////////////////DSSP sắp hết hàng//////////////////////////
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            //ds sản phầm có sso luongj tông bé hơn hoặc n=bằng 5
            var lstSP = db.SanPhams.Where(n => n.DaXoa == false && n.SoLuongTon <= 5);
            return View(lstSP);
        }
        //tạo view nhập hàng đơn
        ///==========================NHAP HANG DON ===================
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp == null)
            {
                return HttpNotFound();
            }    

            return View(sp);
        }
        //=========================================
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);

            ////////    //sau khi kiểm tra tất cả các dữ liệu dầu vào
            ////////    //gán đã xóa
            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            // save change để lấy được mã phiếu nhập g
            ctpn.MaPN = model.MaPN;
            ////////    //cập nhật tônf
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();

            return View(sp);
           
        }


















        //==========giải phóng biến cho vùng nhớ============
                    protected override void Dispose(bool disposing)
                    {
                        if(disposing)
                        {
                            if (db != null)
                                db.Dispose();
                                db.Dispose();   
                        }    
                        base.Dispose(disposing);
                    }
    }
}
        