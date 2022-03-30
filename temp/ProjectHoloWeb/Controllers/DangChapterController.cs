using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;
using PagedList;
using PagedList.Mvc;

namespace ProjectHoloWeb.Controllers
{
    public class DangChapterController : Controller
    {
        // GET: DangChapter
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        public ActionResult Index(int ? page)
        {
            // Danh sach truyen cac thao tac can thiet
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            ViewBag.listCHA = (from s in data.Chapters select s).ToList();
            int pageSize = 3;
            int pageNum = (page ?? 1);
            var list0 = from s in data.ComicTrans where s.IDteam == user.IDteam orderby s.Comic.created descending select s;

            return View(list0.ToPagedList(pageNum, pageSize));
        }

        public ActionResult UpdateChap(string id, string idchap)
        {
            // id la id comic
            // list them xoa chap
            // tro ve
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            var a = (from s in data.ImageChapters where s.IDchapter == idchap select s).ToList();
            ViewBag.IDcomic = id;
            ViewBag.Chap = (from s in data.Chapters where s.IDcomic == id where s.IDchapter == idchap select s).ToList().FirstOrDefault();
            string imageLink = "";
            foreach(var item in a)
            {
                if(item.ImageLink == a.LastOrDefault().ImageLink)
                {
                    imageLink = imageLink + item.ImageLink;
                }
                else
                {
                    imageLink = imageLink + item.ImageLink + "\n";
                }
            }
            ViewBag.ImageLink = imageLink;
            var idComicX = data.Comics.ToList().FirstOrDefault(p => p.IDcomic == id);
            return View(idComicX);
        }
        [HttpPost]
        public ActionResult UpdateChap(string id, string idChap, string chapter, string imageLink)
        {
            // id la id comic
            // list them xoa chap
            // tro ve
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }

            List<ImageChapter> imageChapters = data.ImageChapters.ToList().FindAll(p => p.IDchapter == idChap);

            data.ImageChapters.DeleteAllOnSubmit(imageChapters);
            data.SubmitChanges();

            string[] listImageLink = imageLink.ToString().Split('\n');
            foreach (string item in listImageLink)
            {
                ImageChapter imgItem = new ImageChapter();
                imgItem.IDchapter = idChap;
                imgItem.ImageLink = item;
                data.ImageChapters.InsertOnSubmit(imgItem);
                data.SubmitChanges();
            }

            var a = (from s in data.ImageChapters where s.IDchapter == idChap select s).ToList();
            ViewBag.IDcomic = id;
            ViewBag.Chap = (from s in data.Chapters where s.IDcomic == id where s.IDchapter == idChap select s).ToList().FirstOrDefault();
            string imageLink0 = "";
            foreach (var item in a)
            {
                if (item.ImageLink == a.LastOrDefault().ImageLink)
                {
                    imageLink0 = imageLink0 + item.ImageLink;
                }
                else
                {
                    imageLink0 = imageLink0 + item.ImageLink + "\n";
                }
            }
            ViewBag.ImageLink = imageLink0;
            var idComicX = data.Comics.ToList().FirstOrDefault(p => p.IDcomic == id);
            ViewBag.Loi = "Cập nhật chap thành công";
            return View(idComicX);
        }


        public ActionResult DeleteChap(string id, string idchap, int ? page)
        {
            // id la id comic
            // list them xoa chap
            // tro ve
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }

            Chapter chapter = data.Chapters.ToList().FirstOrDefault(p => p.IDchapter == idchap);
            List<ImageChapter> imageChapters = (from s in data.ImageChapters where s.IDchapter == chapter.IDchapter select s).ToList();

            data.ImageChapters.DeleteAllOnSubmit(imageChapters);
            data.Chapters.DeleteOnSubmit(chapter);
            data.SubmitChanges();

            return RedirectToAction("CapNhat","DangChapter", new { id = id, page = page});
        }
        public ActionResult CapNhat(string id, int ? page)
        {
            // id la id comic
            // list them xoa chap
            // tro ve
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            ViewBag.Comic = (from s in data.Comics select s).ToList().FirstOrDefault(p=>p.IDcomic == id);
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var list0 = from s in data.Chapters where s.IDcomic == id select s;

            return View(list0.ToPagedList(pageNum, pageSize));
        }

        public ActionResult XoaComic(string id)
        {
            // chu nhom moi co quyen xoa
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault(p=>p.IDtransper == "TRAPER001");
            if (user == null)
            {
                return HttpNotFound();
            }
            // xoa category
            List<ComicCatergory> listCOMCAT = (from s in data.ComicCatergories where s.IDcomic == id select s).ToList();
            if (listCOMCAT.Count != 0)
            {
                data.ComicCatergories.DeleteAllOnSubmit(listCOMCAT);
                data.SubmitChanges();
            }
            // id la id comic
            // tim list chapter, image chapter cua truyen, comtrans
            Comic comic = new Comic();
            comic = data.Comics.ToList().Find(p => p.IDcomic == id);
            ComicTran comicTran = new ComicTran();
            comicTran = data.ComicTrans.ToList().Find(p => p.IDcomic == id);
            List<Chapter> chapters = new List<Chapter>();
            chapters = data.Chapters.ToList().FindAll(p => p.IDcomic == id);
            List<ImageChapter> imageChapters = new List<ImageChapter>();
            imageChapters = data.ImageChapters.ToList().FindAll(p => p.Chapter.IDcomic == id);


            if(imageChapters.Count != 0)
            {
                data.ImageChapters.DeleteAllOnSubmit(imageChapters);
                data.SubmitChanges();
            }
            if(chapters.Count != 0)
            {
                data.Chapters.DeleteAllOnSubmit(chapters);
                data.SubmitChanges();
            }
            if(comicTran.id != null)
            {
                data.ComicTrans.DeleteOnSubmit(comicTran);
                data.SubmitChanges();
            }
            if(comic.IDcomic != null)
            {
                data.Comics.DeleteOnSubmit(comic);
                data.SubmitChanges();
            }
            
            return RedirectToAction("Index","DangChapter");
        }

        public ActionResult ThemChap(string id)
        {
            // id la id comic
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().FirstOrDefault();
            if(user == null)
            {
                return HttpNotFound();
            }
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            ViewBag.listCHA = (from s in data.Chapters select s).ToList();
            var x = (from s in data.Comics where s.IDcomic == id select s).ToList().Single();
            ViewBag.IdCom = x.IDcomic;
            return View(x);
        }

        [HttpPost]
        public ActionResult ThemChap(string idComic, string chapter, string imageLink)
        {
            // id la id comic
            ProjectHoloWeb.Models.User user = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            ViewBag.User = user;
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            ViewBag.listCHA = (from s in data.Chapters select s).ToList();
            

            // thuc hien dk dung

            // add table chapter
            Chapter chapterX = new Chapter();
            string lastCodeX = data.Chapters.ToList().Last().IDchapter.ToString();
            AutoUp idCreateCHA = new AutoUp(lastCodeX, "CHA", 3);
            chapterX.IDchapter = idCreateCHA.CreateCodeAuto();
            chapterX.chapter1 = chapter;
            chapterX.IDcomic = idComic;
            chapterX.created = DateTime.Now.Date;
            chapterX.update = DateTime.Now.Date;
            data.Chapters.InsertOnSubmit(chapterX);
            data.SubmitChanges();

            //String[] lines = text.split("\\r?\\n"); cat chuoi theo xuong dong new line
            // add table imagechapter
            string[] listImageLink = imageLink.ToString().Split('\n');
            foreach(string item in listImageLink)
            {
                ImageChapter imgItem = new ImageChapter();
                imgItem.IDchapter = chapterX.IDchapter;
                imgItem.ImageLink = item;
                data.ImageChapters.InsertOnSubmit(imgItem);
                data.SubmitChanges();
            }

            ViewBag.Loi = "Đăng chapter thành công";

            var x = (from s in data.Comics where s.IDcomic == idComic select s).ToList().Single();
            return View(x);
        }

    }
}