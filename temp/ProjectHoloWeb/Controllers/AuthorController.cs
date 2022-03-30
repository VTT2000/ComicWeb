using ProjectHoloWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectHoloWeb.Controllers
{
    public class AuthorController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: Author
        public ActionResult Index(string id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var list = data.Comics.ToList().Where(p => p.IDauthor == id);
            ViewBag.tentacgia = data.Authors.ToList().FirstOrDefault(p=>p.IDauthor == id).author1;
            
            var list6 = from tt in data.Chapters select tt;
            ViewBag.listCHA = list6.ToList();
            return View(list);
        }
    }
}