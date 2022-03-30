using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;
namespace ProjectHoloWeb.Controllers
{
    public class UseradminController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: Useradmin
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View(data.Users.ToList());
        }

        //Xoá 1 user
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var user = (from u in data.Users where u.IDuser == id select u);
                return View(user.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(string id)
        {
            User user = data.Users.Where(n => n.IDuser == id).SingleOrDefault();
            data.Users.DeleteOnSubmit(user); // xoá
            data.SubmitChanges();
            return RedirectToAction("Index", "Useradmin");
        }

        //Xem chi tiết 1 user
        public ActionResult Details(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var user = (from u in data.Users where u.IDuser == id select u);
                return View(user.SingleOrDefault());
            }
        }
        //Sửa 1 user
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var user = (from u in data.Users where u.IDuser == id select u);
                return View(user.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Sua(string id)
        {
            {
                User user = data.Users.Where(n => n.IDuser == id).SingleOrDefault();
                UpdateModel(user); // sửa
                data.SubmitChanges();
                return RedirectToAction("Index", "Useradmin");
            }
        }
    }
}