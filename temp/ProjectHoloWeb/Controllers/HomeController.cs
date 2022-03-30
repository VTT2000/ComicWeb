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
    public class HomeController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        public ActionResult home(int ? page)
        {
            var list2 = from tt in data.TransTeams select tt;
            ViewBag.listTEA = list2.ToList();
            var list3 = from tt in data.ComicTrans select tt;
            ViewBag.listCOMTRA = list3.ToList();

            var list4 = from tt in data.Catergories select tt;
            ViewBag.listCAT = list4.ToList();
            var list5 = from tt in data.ComicCatergories select tt;
            ViewBag.listCOMCAT = list5.ToList();
            var list6 = from tt in data.Chapters select tt;
            ViewBag.listCHA = list6.ToList();

            int pageSize = 6;
            int pageNum = (page ?? 1);
            var list1 = from tt in data.Comics select tt;

            return View(list1.ToPagedList(pageNum,pageSize));
        }

    }
}