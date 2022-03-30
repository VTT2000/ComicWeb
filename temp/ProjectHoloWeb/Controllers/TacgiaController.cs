using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;
namespace ProjectHoloWeb.Controllers
{
    public class TacgiaController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: Tacgia
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View(data.Authors.ToList());
        }
        //Thêm mới 1 tác giả
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(Author author)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
            {
                ProjectHoloWeb.Models.Author au = new Author();
                string lastCode = data.Authors.ToList().Last().IDauthor.ToString();
                // hàm tạo tự động
                AutoUp x = new AutoUp(lastCode, "AUT", 3);
                //lastcode tim ban ghi cuoi cung lấy IDauthor, "AUT" mã code AUT001 , 3 là 3 số phía sau.
                au.IDauthor = x.CreateCodeAuto();
                au.author1 = author.author1;

                data.Authors.InsertOnSubmit(au);
                data.SubmitChanges();
                return RedirectToAction("Index", "Tacgia");
            }

        }

        //Xoá 1 tác giả
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var author = (from au in data.Authors where au.IDauthor == id select au);
                return View(author.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(string id)
        {
            Author author = data.Authors.Where(n => n.IDauthor == id).SingleOrDefault();
            data.Authors.DeleteOnSubmit(author); // xoá
            data.SubmitChanges();
            return RedirectToAction("Index", "Tacgia");
        }

        //Sửa 1 tác giả
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var author = (from au in data.Authors where au.IDauthor == id select au);
                return View(author.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Sua(string id)
        {
            {
                Author author = data.Authors.Where(n => n.IDauthor == id).SingleOrDefault();
                UpdateModel(author); // sửa
                data.SubmitChanges();
                return RedirectToAction("Index", "Tacgia");
            }
        }
    }
}