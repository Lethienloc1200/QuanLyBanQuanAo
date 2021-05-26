using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBangHang.Models;

namespace WebSiteBangHang.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult ChuaThanhToan()
        {
            //Lấy Danh sách các đơn hàng chưa duyệt
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            //lấy danh sách đơn hàng chưa giao
            var lstDSDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan==true).OrderBy(n => n.NgayGiao);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var lstDSDHDG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan== true);
            return View(lstDSDHDG);
        }

        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //kiểm tra hệ thống có hợp lệ không
            if(id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //kiểm tra đon đạt hàng có tồn tại không
            if( model == null)
            {
                return HttpNotFound();
            }
            //lấy danh sách chi tiết đơn hàng để hiển thị cho nguoiwd f=dùng thấy
            var lstChiTietDH = db.ChiTietDonHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            //truy vấ lấy ra dữ liệu của đơn hàng đó
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();

         
            //lấy danh sách chi tiết đơn hàng để hiển thị cho nguoiwd dùng thấy
            var lstChiTietDH = db.ChiTietDonHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(ddhUpdate);
        }

    }
}