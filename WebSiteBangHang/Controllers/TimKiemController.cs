using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBangHang.Models;
using PagedList;

namespace WebSiteBangHang.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa,int? page)
        {
            //timfkiesm theo ten sản phẩm
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //thực hiện chứ năng phân trang
            //tạo biến số sp trên trang
            int PageSize = 3;
            //tạo biến thư 2 : số trang hiện tại
            int PageNumber = (page ?? 1);
            //tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(PageNumber,PageSize));
        }

        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
           
            return RedirectToAction("KQTimKiem",new { sTuKhoa =sTuKhoa});
        }

        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            //tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));

            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP.OrderBy(n=>n.DonGia));
        }
    }
}