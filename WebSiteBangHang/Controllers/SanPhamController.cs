using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBangHang.Models;
using PagedList;
using PagedList.Mvc;

namespace WebSiteBangHang.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
   
 //taj 2 partial view san phaamr 1 laf 2 deer hieern thij sanr phaamr theo 2 style khachs nhau       
        [ ChildActionOnly]
        public ActionResult SanPhamStylePartial()
        {
            return PartialView();
        }
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
        //xây dựng trang xem chi tiết
        public ActionResult XemChiTiet(int? id,string tensp) //coi lại
        {
            //kiểm tra tham số truyền vào rỗng hay không
            if(id== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//kiểm tra tính hợp lệ

            }
            //Nếu khôn thì truy xuất csdl
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa==false);
            if( sp == null)
            {
                //thông báo nếu như không có sp đó
                return HttpNotFound();

            }    
            return View(sp);
        }
        //xây dựng 1 action sản phẩm theo mã lioij sản phẩm và nahf sản xuất
        public ActionResult SanPham(int? MaLoaiSP,int? MaNSX,int? page)
        {
            if(MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }    
            //load sản phẩm dựa theo 2 tiêu chí loại sản phẩm và mã sản ơhaamr và mã nhà sản xuất
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if(lstSP.Count()==0)
            {
                //thông báo nếu như không có sp đó
                return HttpNotFound();
            }
            if(Request.HttpMethod != "GET")
            {
                page = 1;
            }    
            //thực hiện chứ năng phân trang
            //tạo biến số sp trên trang
            int PageSize = 3;
            //tạo biến thư 2 : số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber,PageSize));
        }
        
    }
}