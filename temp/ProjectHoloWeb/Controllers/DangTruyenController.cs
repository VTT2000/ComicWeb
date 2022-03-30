using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;


namespace ProjectHoloWeb.Controllers
{
    public class DangTruyenController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: DangTruyen
        [HttpGet]
        public ActionResult Index()
        {
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(string comic, string description, string status, string author, string imageLink)
        {
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            // Tao cac keyword cung ko co thoi gian
            // Tim tac gia co exist chua
            Author author1 = data.Authors.FirstOrDefault(p => p.author1 == author);
            if (author1 == null)
            {
                author1 = new Author();
                String lastCode = (from s in data.Authors select s).ToList().OrderByDescending(p => p.IDauthor).FirstOrDefault().IDauthor;
                AutoUp q = new AutoUp(lastCode, "AUT", 3);
                // them tac gia // author la ten tac gia
                author1.IDauthor = q.CreateCodeAuto();
                author1.author1 = author;
                data.Authors.InsertOnSubmit(author1);
                data.SubmitChanges();
            }
            //tim ma tac gia cuoi cung
            
            // tao comic
            Comic comicX = new Comic();
            String lastCodeX = (from s in data.Comics select s).ToList().OrderByDescending(p => p.IDcomic).FirstOrDefault().IDcomic;
            AutoUp w = new AutoUp(lastCodeX, "COM", 3);
            comicX.IDcomic = w.CreateCodeAuto();
            comicX.comic1 = comic;
            comicX.created = DateTime.Now.Date;
            comicX.updated = DateTime.Now.Date;
            comicX.ratting = 0;
            comicX.views = 0;
            comicX.share = 0;
            comicX.IDauthor = author1.IDauthor;
            comicX.status = status;
            comicX.description = description;
            comicX.ImageLink = imageLink;
            // tao comic trans
            // Tim nhom truyen thong qua id tai khoan de tao ban ghi cho comic trans
            ComicTran comicTran = new ComicTran();
            String lastCodeCT = (from s in data.ComicTrans select s).ToList().OrderByDescending(p => p.id).FirstOrDefault().id;
            AutoUp ct = new AutoUp(lastCodeCT, "COMTRA", 3);
            comicTran.id = ct.CreateCodeAuto();
            comicTran.IDcomic = comicX.IDcomic;
            comicTran.IDteam = user.IDteam;

            
            data.Comics.InsertOnSubmit(comicX);
            data.ComicTrans.InsertOnSubmit(comicTran);
            data.SubmitChanges();

            //tao key word cho comic and author Chua co ham xu ly
            ViewBag.Loi = "Tạo bộ truyện tranh thành công!=>Đến đăng truyện để xem";

            return View();
        }
    }
}