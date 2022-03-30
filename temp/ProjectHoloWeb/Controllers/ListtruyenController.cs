using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;

namespace ProjectHoloWeb.Controllers
{
    public class ListtruyenController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: Listtruyen
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View(data.Comics.ToList());
        }

        //Thêm mới 1 truyện
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
            {
                ViewBag.listCAT = (from s in data.Catergories select s).ToList();
                return View();
            }
                
        }

        [HttpPost]
        public ActionResult Create(string comic, string description, string status, string author, string imageLink, FormCollection form)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
            {
                Author author0 = data.Authors.FirstOrDefault(p => p.author1 == author);
                if (author0 == null)
                {
                    author0 = new Author();
                    String lastCode = (from s in data.Authors select s).ToList().OrderByDescending(p => p.IDauthor).FirstOrDefault().IDauthor;
                    AutoUp q = new AutoUp(lastCode, "AUT", 3);
                    // them tac gia // author la ten tac gia
                    author0.IDauthor = q.CreateCodeAuto();
                    author0.author1 = author;
                    data.Authors.InsertOnSubmit(author0);
                    data.SubmitChanges();
                }
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
                comicX.IDauthor = author0.IDauthor;
                comicX.status = status;
                comicX.description = description;
                comicX.ImageLink = imageLink;

                data.Comics.InsertOnSubmit(comicX);
                data.SubmitChanges();

                // add the loai
                List<Catergory> listCat = (from s in data.Catergories select s).ToList();
                int n = listCat.Count;
                
                for (int i = 0;i < n; i++)
                {
                    if(form.Get(listCat[i].IDcatergory) == "true")
                    {
                        //them the loai
                        //autocode
                        String lastcode = data.ComicCatergories.ToList().OrderBy(p => p.id).LastOrDefault().id;
                        ComicCatergory comicCatergory = new ComicCatergory();
                        AutoUp autoUp = new AutoUp(lastcode, "COMCAT", 3);
                        comicCatergory.id = autoUp.CreateCodeAuto();
                        comicCatergory.IDcomic = comicX.IDcomic;
                        comicCatergory.IDcatergory = listCat[i].IDcatergory;

                        data.ComicCatergories.InsertOnSubmit(comicCatergory);
                        data.SubmitChanges();
                    }
                }

                return RedirectToAction("Index", "Listtruyen"); 
            }

        }

        //Xoá 1 truyện
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var comic = (from cm in data.Comics where cm.IDcomic == id select cm);
                return View(comic.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(string id)
        {
            // xoa the loai moi duoc
            List<ComicCatergory> listCOMCAT = (from s in data.ComicCatergories where s.IDcomic == id select s).ToList();
            if (listCOMCAT.Count != 0)
            {
                data.ComicCatergories.DeleteAllOnSubmit(listCOMCAT);
                data.SubmitChanges();
            }
            
            Comic comic = data.Comics.Where(n => n.IDcomic == id).SingleOrDefault();
            data.Comics.DeleteOnSubmit(comic); // xoá
            data.SubmitChanges();
            return RedirectToAction("Index", "Listtruyen");
        }

        //Sửa 1 truyện
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
            {
                ViewBag.listCAT = (from s in data.Catergories select s).ToList();
                ViewBag.listCOMCATselected = (from s in data.ComicCatergories where s.IDcomic == id select s).ToList();
                
                var comic = (from cm in data.Comics where cm.IDcomic == id select cm);
                return View(comic.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult Sua(FormCollection form, string IDcomic, string comic, string status, string author, string description, string ImageLink)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
            {
                Comic comicX = data.Comics.Where(n => n.IDcomic == IDcomic).SingleOrDefault();
                comicX.comic1 = comic;
                comicX.status = status;
                comicX.Author.author1 = author;
                comicX.description = description;
                comicX.ImageLink = ImageLink;
                
                data.SubmitChanges();

                List<ComicCatergory> listCOMCATselected = (from s in data.ComicCatergories where s.IDcomic == IDcomic select s).ToList();
                data.ComicCatergories.DeleteAllOnSubmit(listCOMCATselected);
                data.SubmitChanges();
                string[] listSelected = form.GetValues("list");
                int sum = listSelected.Length;
                for(int i = 0; i < sum; i++)
                {
                    ComicCatergory temp = new ComicCatergory();
                    string lastcode = (from z in data.ComicCatergories select z).ToList().LastOrDefault().id;
                    AutoUp autoUp = new AutoUp(lastcode, "COMCAT", 3);
                    temp.id = autoUp.CreateCodeAuto();
                    temp.IDcomic = comicX.IDcomic;
                    temp.IDcatergory = listSelected.ElementAt(i).ToString();

                    data.ComicCatergories.InsertOnSubmit(temp);
                    data.SubmitChanges();
                }

                return RedirectToAction("Index", "Listtruyen");
            }
        }
    }


}