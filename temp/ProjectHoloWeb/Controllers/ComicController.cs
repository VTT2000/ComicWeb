using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;

namespace ProjectHoloWeb.Controllers
{
    public class ComicController : Controller
    {
        // GET: Comic
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        public ActionResult DetailComic(string id)
        {
            if (Session["Taikhoan"].ToString().CompareTo("") != 0)
            {
                
                // dong file mat' cookies có thể do chạy express mô phỏng nên đóng trình duyet mát lun cookie
                // ko co cookies
                if (Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi") == null)
                {
                    ViewBag.TD = "Theo dõi truyện này";
                    HttpCookie cookie = new HttpCookie(Session["Taikhoan"].ToString() + "Theodoi");
                    cookie.Values.Add(id, "false");
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                }
                // co cookie
                // 3 truong hop
                if(Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi").Values.Get(id) == null)
                {
                    ViewBag.TD = "Theo dõi truyện này";
                    HttpCookie cookie = Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi");
                    cookie.Values.Add(id, "false");
                    Response.Cookies.Set(cookie);
                }
                if(Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi").Values.Get(id) == "false")
                {
                    ViewBag.TD = "Theo dõi truyện này";
                }
                if(Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi").Values.Get(id) == "true")
                {
                    ViewBag.TD = "Bỏ theo dõi";
                }
            }

            var list2 = from tt in data.TransTeams select tt;
            ViewBag.listTEA = list2.ToList();
            var list3 = from tt in data.ComicTrans select tt;
            ViewBag.listCOMTRA = list3.ToList();

            var list4 = from tt in data.Catergories select tt;
            ViewBag.listCAT = list4.ToList();
            var list5 = from tt in data.ComicCatergories select tt;
            ViewBag.listCOMCAT = list5.ToList();
            
            Comic com = (from s in data.Comics where s.IDcomic == id select s).ToList().First();
            
            var list1 = from tt in data.Chapters where tt.IDcomic == id select tt;
            ViewBag.listCHA = list1.ToList();
            
            return View(com);
        }

        
        public ActionResult Theodoi(string id)
        {
            if(Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi").Values.Get(id) == "false")
            {
                HttpCookie cookie = Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi");
                cookie.Values.Set(id, "true");
                Response.Cookies.Set(cookie);
            }
            else
            {
                HttpCookie cookie = Request.Cookies.Get(Session["Taikhoan"].ToString() + "Theodoi");
                cookie.Values.Set(id, "false");
                Response.Cookies.Set(cookie);
            }

            return RedirectToAction("DetailComic","Comic",new { id = id});
        }



        public ActionResult Chapter(string id,string idc)// idc la id comic , id la chapter1 hay ten chap
        {
            ViewBag.chapter = id;
            ViewBag.IDcomic = idc;
            if(String.IsNullOrEmpty(id) || String.IsNullOrEmpty(idc))
            {
                return HttpNotFound();
            }

            var list = from tt in data.Chapters where tt.IDcomic == idc select tt;
            ViewBag.listCHA = list.ToList();
            
            var chapterID = from tt in data.Chapters where tt.chapter1 == id where tt.IDcomic == idc select tt;
            String idchap = chapterID.ToList().First().IDchapter.ToString();
            var listChap = from tt in data.ImageChapters where tt.IDchapter == idchap orderby tt.IDimagechapter ascending select tt;
            return View(listChap);
        }
        public ActionResult BeforeChap(string id,string idc)// idc la id comic , id la chapter1 hay ten chap
        {
            // Lay list tat ca chap cua id comic
            var listChap = from tt in data.Chapters where tt.IDcomic == idc select tt;
            // xac dinh vi tri chap hien tai
            int vt = listChap.ToList().FindIndex(p => p.chapter1 == id);
            // giam vi tri, -1 do chap truoc
            vt = vt - 1;
            // xet vi tri sau khi giam
            if(vt < 0)
            {
                vt = 0;
            }
            // lay ten chapter sau khi giam
            string chap1 = listChap.ToList()[vt].chapter1;
            
            return RedirectToAction("Chapter","Comic", new { id = chap1, idc = idc});
        }
        public ActionResult AfterChap(string id,string idc)// idc la id comic , id la chapter1 hay ten chap
        {
            // Lay list tat ca chap cua id comic
            var listChap = from tt in data.Chapters where tt.IDcomic == idc select tt;
            // xac dinh vi tri chap hien tai
            int vt = listChap.ToList().FindIndex(p => p.chapter1 == id);
            // tang vi tri, +1 do chap truoc
            vt = vt + 1;
            // xet vi tri sau khi tang
            if (vt == listChap.ToList().Count)
            {
                vt = vt - 1;
            }
            // lay ten chapter sau khi tang
            string chap1 = listChap.ToList()[vt].chapter1;

            return RedirectToAction("Chapter", "Comic", new { id = chap1, idc = idc });
        }

        [HttpGet]
        public ActionResult Search(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return View();
            }
            var list = data.Comics.ToList().Where(p => p.comic1.ToLower().Contains(key));

            var list6 = from tt in data.Chapters select tt;
            ViewBag.listCHA = list6.ToList();


            return View(list);
        }
        public ActionResult TypePartial()
        {
            var list = from s in data.Catergories select s;
            return View(list);
        }
        public ActionResult Type(string id,int ? sort)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            ViewBag.ID = id;
            ViewBag.CAT = data.Catergories.Single(p => p.IDcatergory == id).catergory1.ToString();

            int xep = (sort ?? 0);
            var listIDcomic = (from s in data.ComicCatergories where s.IDcatergory == id select s).ToList();

            
            var list3 = from tt in data.ComicTrans select tt;
            ViewBag.listCOMTRA = list3.ToList();
            var list4 = from tt in data.Chapters select tt;
            ViewBag.listCHA = list4.ToList();
            var list5 = from tt in data.Catergories select tt;
            ViewBag.listCAT = list5.ToList();


            if (xep == 1)
            {
                return View(listIDcomic.OrderBy(p => p.Comic.comic1));
            }
            if (xep == 2)
            {
                return View(listIDcomic.OrderByDescending(p=>p.Comic.views));
            }
            if (xep == 3)
            {
                return View(listIDcomic.OrderByDescending(p => p.Comic.updated));
            }
            if (xep == 4)
            {
                return View(listIDcomic.OrderByDescending(p => p.Comic.created));
            }

            return View(listIDcomic);
        }
    }
}