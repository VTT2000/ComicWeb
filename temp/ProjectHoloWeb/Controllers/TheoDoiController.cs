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
    public class TheoDoiController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: TheoDoi
        public ActionResult Index(int ? page)
        {
            ViewBag.User = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            if (Session["Taikhoan"].ToString().Trim().Length == 0)
            {
                return HttpNotFound();
            }
            String tk = Session["Taikhoan"].ToString();
            ViewBag.listCHA = (from s in data.Chapters select s).ToList();
            if (Request.Cookies.Get(tk + "Theodoi") == null)
            {
                return View();
            }
            else
            {
                var listTdtk = Request.Cookies.Get(tk + "Theodoi").Values;
                List<Comic> listComic = (from s in data.Comics select s).ToList();
                List<Comic> list = new List<Comic>();

                for (int i = 0; i < listTdtk.Count; i++)
                {

                    if (listTdtk.Get(i).CompareTo("true") == 0)
                    {
                        foreach (Comic item in listComic)
                        {
                            if (listTdtk.GetKey(i).CompareTo(item.IDcomic) == 0)
                            {
                                list.Add(item);
                                break;
                            }
                        }
                    }

                }
                int pageSize = 6;
                int pageNum = (page ?? 1);

                return View(list.ToPagedList(pageNum, pageSize));
            }
        }
    }
}