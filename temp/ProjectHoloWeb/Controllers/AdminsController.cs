using ProjectHoloWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.Data.Sql;

namespace ProjectHoloWeb.Controllers
{
    public class AdminsController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();

        // GET: Admins
        public ActionResult Index()
        //Tài khoản admin null quay về login
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["Password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.useradmin == tendn && n.passadmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admins");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }


    }
}