using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using WebSiteBangHang.Models;
using System.Net;

namespace WebSiteBangHang.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.DaXoa== false).OrderByDescending(n =>n.MaSP));
        }


        [HttpGet]
        public ActionResult TaoMoi()
        {
            //Load drop dowlisst nhà cung cấp và loại sản phẩm và mã nhà SX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)] /*kiểm tra nguy hiểm của microsoft*/
        //==============================Tạo mới=======================================
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            //Load drop dowlisst nhà cung cấp và loại sản phẩm và mã nhà SX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
    
            //Kiểm tra Hình đã tồn tại chưa
            if (HinhAnh.ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                //lấy hình ảnh chuyển vào thư mục hình ảnh 
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                //nếu như thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                if(System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh đã tồn tại";
                     return View();
                }    
                else
                {
                    //Lấy hình ảnh đưa vào thư mục HinhAnhSP
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = fileName;
                   
                }   
            }
            db.SanPhams.Add(sp); 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //==============================Chỉnh sửa==================================
        [HttpGet]

        public ActionResult ChinhSua(int? id)
        {
            //lấy sản phẩm cần sửa dựa vào id
            if(id ==null )
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp ==null)
            {
                return HttpNotFound();
            }    

            //Load drop dowlisst nhà cung cấp và loại sản phẩm và mã nhà SX
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX",sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)] /*kiểm tra nguy hiểm của microsoft*/
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {
            //nếu dữ liệu dầu vào chắc chắc đúng
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
          

            db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //lấy sane phẩm cần chỉnh sửa
            if(id ==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp == null)
            {
                return HttpNotFound();

            }
            //load dropdoawList nhà cung cáp và dropdoawlist loại sản phẩm ,mẫ nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int id) // xóa dấu chấm hỏi đi nếu sai
        {
            //lấy sản phẩm cần sửa dựa vào id
            if ( id == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}