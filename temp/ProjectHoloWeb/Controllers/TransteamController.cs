using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;

namespace ProjectHoloWeb.Controllers
{
    public class TransteamController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: Transteam
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else
                return View(data.TransTeams.ToList());
        }

        

        //Xoá 1 thể loại
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admins");
            else

            {
                var trans = (from tr in data.TransTeams where tr.IDteam == id select tr);
                return View(trans.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xoa(string id)
        {
            TransTeam trans = data.TransTeams.Where(n => n.IDteam == id).SingleOrDefault();
            data.TransTeams.DeleteOnSubmit(trans); // xoá
            data.SubmitChanges();
            return RedirectToAction("Index", "TransTeam");
        }
    }
}