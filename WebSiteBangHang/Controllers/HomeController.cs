using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBangHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebSiteBangHang.Controllers
{
    
    public class HomeController : Controller
    {
        
        // GET: Home
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //lần lượt tạo các viewbag dể lấy list sản phảm từ cơ sở dữ liệu
            //list dienj điện thoại mới 
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false).ToList();
            //gán vào ViewBag
            ViewBag.ListDTM = lstDTM;
            //list laptop mới 
            var lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi ==1 && n.DaXoa == false).ToList();
          //gán vào ViewBag
            ViewBag.ListLTM = lstLTM;
           
             var lstMTBM = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false).ToList();
              //gán vào ViewBag
              ViewBag.ListMTBM = lstMTBM;
            return View();
        }


     
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();

        }
        [HttpPost]
        public ActionResult Dangky(ThanhVien tv,FormCollection f)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());//Kiểm tra capcha chop le
            if (this.IsCaptchaValid("CapCha không đúng"))
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ThongBao = "thêm thành công";
                    //thêm khách hàng vào cơ sở du liệu
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ThongBao = "thêm Thất bại";
                }
                return View();
            }
            ViewBag.ThongBao = "Sai mã capcha";
            return View();
        }
        //load cau hỏi bí mạt
        public List<string>LoadCauHoi()
        { 
            List<string> lstCauhoi = new List<string>();
            lstCauhoi.Add("Lộc boy có đẹp trai không");
            lstCauhoi.Add("Lộc booy đẹp trai đúng chứ");
            lstCauhoi.Add("Có yêu lộc Boy không nè");
            return lstCauhoi;
        }
        //xây dựng action đăng nhập
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //string sTaiKhoan = f["txtTenDangNhap"].ToString();
            //string sMatKhau = f["txtMatKhau"].ToString();
            //ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            //if( tv != null)
            //{
            //    Session["TaiKhoan"] = tv;
            //    return Content("<script>window.location.reload();</script>");
            //}    

            //return Content("tài khoản hoặc mật khẩu không đúng"); /*Controller chính nên k cần thêm tên controoler*/
            string taikhoan = f["txtTenDangNhap"].ToString();
            string matkhau = f["txtMatKhau"].ToString();
            //truy vấn kiểm tra đăng nhập lấy thông tin thnahf viên
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            if( tv != null)
            {
                Session["TaiKhoan"] = tv;
                //lấy ra list quyền thành viên tuong ứng với loại thnahf viên
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //Duyệt list Quyên
                string Quyen = "";
                foreach( var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);// cắt dấu ","
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);

                return Content("<script>window.location.reload();</script>");
            }
            return Content("tài khoản hoặc mật khẩu không đúng");
        }



        ///==========Phân Quyền===========
        public void PhanQuyen(string TaiKhoan,string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                TaiKhoan,
                                DateTime.Now,//begin
                                DateTime.Now.AddHours(3), //thời gian
                                false, //có lưu hay k
                                Quyen, //quyền gì
                                FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

                    Response.Cookies.Add(cookie);
        }
        //Tạo trang Ngăn Chặn quền truy cạp
        public ActionResult LoiPhanQuyen()
        {
            return View();
            //trả về cái view"không  đủ quyền đăng nhập được"

        }

      
        
                                //=====Xóa seesion====
            public ActionResult DangXuat()
            {
                    Session["TaiKhoan"] = null;
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index");
            }

    }
}