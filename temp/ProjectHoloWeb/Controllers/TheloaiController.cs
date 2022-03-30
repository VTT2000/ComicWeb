using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;

namespace ProjectHoloWeb.Controllers
{
    public class TheloaiController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();

        // GET: Theloai
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View(data.Catergories.ToList());
        }

        //Thêm mới 1 thể loại
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Create(Catergory catergory)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
            {
                ProjectHoloWeb.Models.Catergory cate = new Catergory();
                string lastCode = data.Catergories.ToList().Last().IDcatergory.ToString();
                // hàm tạo tự động
                AutoUp x = new AutoUp(lastCode, "CAT", 3);
                //lastcode tim ban ghi cuoi cung lấy IDcategory, "CAT" mã code CAT001 , 3 là 3 số phía sau.
                cate.IDcatergory = x.CreateCodeAuto();
                cate.catergory1 = catergory.catergory1;
                cate.decriptions = catergory.decriptions;
                
                data.Catergories.InsertOnSubmit(cate);
                data.SubmitChanges();
                return RedirectToAction("Index", "Theloai");
            }

        }



        //Xoá 1 thể loại
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
            
            {
                var category = (from cate in data.Catergories where cate.IDcatergory == id select cate);
                return View(category.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(string id)
        {
            Catergory catergory = data.Catergories.Where(n => n.IDcatergory == id).SingleOrDefault();
            data.Catergories.DeleteOnSubmit(catergory); // xoá
            data.SubmitChanges();
            return RedirectToAction("Index", "Theloai");
        }

        //Sửa 1 thể loại
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var category = (from cate in data.Catergories where cate.IDcatergory == id select cate);
                return View(category.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Sua(string id)
        {
            {
                Catergory catergory = data.Catergories.Where(n => n.IDcatergory == id).SingleOrDefault();
                UpdateModel(catergory); //cập nhật 
                data.SubmitChanges();
                return RedirectToAction("Index", "Theloai");
            }
        }
    }
}