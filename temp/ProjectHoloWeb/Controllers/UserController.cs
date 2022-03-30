using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;

namespace ProjectHoloWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        //-- Khai báo data du lieu
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            ProjectHoloWeb.Models.User user = new Models.User();
            user = data.Users.FirstOrDefault(p => p.mail == email);
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi"] = "Email không để trống";
            }
            else
            if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi"] = "Password không để trống";
            }
            else
            if(user != null) // if ket qua select null, thi ko co tai khoan đối với data
            {
                if (password.CompareTo(user.pass) == 0) // sau do so sach mat khau
                {
                    ViewData["Loi"] = "OK";
                    Session["Taikhoan"] = user.mail;
                    Session["TaikhoanID"] = user.IDuser;
                    Session["Username"] = user.username;

                    //if(Request.Cookies["Theodoi"+user.mail] == null)
                    //{
                    //    HttpCookie cookie = new HttpCookie("Theodoi"+user.mail);

                    //    Response.Cookies.Add(cookie);
                    //}
                    //else
                    //{

                    //}
                    return RedirectToAction("home", "Home");
                }
                else
                {
                    ViewData["Loi"] = "Sai mật khẩu";
                }
            }
            else
            {
                ViewData["Loi"] = "Tài khoản không tồn tại";
            }
            ViewData["email"] = email;
            ViewData["password"] = password;
            return this.Login();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string email, string password, string password_confirmation)
        {
            
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi"] = "Email không để trống";
            }
            else
            if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi"] = "Password không để trống";
            }
            else
            if (String.IsNullOrEmpty(password_confirmation))
            {
                ViewData["Loi"] = "Xác nhận Password không để trống";

            }
            else
            if (data.Users.FirstOrDefault(p=>p.mail == email) != null) // if ket qua select null, thi ko co tai khoan đối với data
            {
                ViewData["Loi"] = "Tài khoản đã tồn tại vui lòng nhập Email khác";
            }
            else
            if(password != password_confirmation)
            {
                ViewData["Loi"] = "Password và xác nhận Password không đúng";
            }
            else
            {
                // thông tin ok tiến hành tạo tài khoản
                ProjectHoloWeb.Models.User userRes = new User();
                string lastCode = data.Users.ToList().Last().IDuser.ToString();
                // hàm tạo tự động
                AutoUp x = new AutoUp(lastCode,"USE",3);
                //lastcode tim ban ghi cuoi cung lấy IDuser, "USE" mã code USE001 , 3 là 3 số phía sau.
                // đoi voi "USEKEY" thì cái USE thành USEKEY
                userRes.IDuser = x.CreateCodeAuto();
                userRes.mail = email;
                userRes.pass = password;
                userRes.username = "chưa đặt";

                // luu csdl
                data.Users.InsertOnSubmit(userRes);
                data.SubmitChanges();
                // tro ve trang dang nhập
                

                return RedirectToAction("Login");
            }

            ViewData["email"] = email;
            ViewData["password"] = password;
            ViewData["password_confirmation"] = password_confirmation;
            return this.Register();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult LogOut()
        {
            Session["Taikhoan"] = "";
            Session["TaikhoanID"] = "";
            Session["Username"] = "";
            return RedirectToAction("Home", "home");
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Users(string id)
        {

            var user = (from tt in data.Users where tt.IDuser == id select tt).ToList().FirstOrDefault();
            if(user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        [HttpPost]
        public ActionResult Users(string username, string email, string password, string password_confirmation)
        {
            

            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi"] = "Email không để trống";
            }
            else
            if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi"] = "Password không để trống";
            }
            else
            if (String.IsNullOrEmpty(password_confirmation))
            {
                ViewData["Loi"] = "Xác nhận Password không để trống";

            }
            else
            if (data.Users.FirstOrDefault(p => p.mail == email) == null) // if ket qua select null, thi ko co tai khoan đối với data
            {
                ViewData["Loi"] = "Email đã tồn tại vui lòng nhập Email khác";
            }
            else
            if (password != password_confirmation)
            {
                ViewData["Loi"] = "Password và xác nhận Password không đúng";
            }
            else
            {
                Models.User userUP = data.Users.Single(p => p.mail == Session["Taikhoan"].ToString());
                userUP.username = username;
                userUP.mail = email;
                userUP.pass = password;
                data.SubmitChanges();
                
                ViewBag.thongBaoUserLuuThayDoi = "1";
                return View(userUP);
            }
            Models.User x = new Models.User();
            x.username = username;
            x.mail = email;
            x.pass = password;
            return View(x);
        }


    }
}