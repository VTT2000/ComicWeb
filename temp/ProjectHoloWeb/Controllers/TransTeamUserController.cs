using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectHoloWeb.Models;

namespace ProjectHoloWeb.Controllers
{
    public class TransTeamUserController : Controller
    {
        dbDataTruyenTranhDataContext data = new dbDataTruyenTranhDataContext();
        // GET: TransTeamUser
        public ActionResult Index()
        {
            if(Session["Taikhoan"].ToString().Length == 0)
            {
                return HttpNotFound();
            }
            // lay ban ghi id user hien tai
            ProjectHoloWeb.Models.User x = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            ViewBag.User = x;
            ViewBag.Trans = new SelectList(data.TransTeams.ToList(), "IDteam", "team");

            var listUserTrans = (from s in data.Users where s.IDteam == x.IDteam  select s).ToList();

            return View(listUserTrans);
        }
        
        public ActionResult Doivitrinhom(string id, int vt)
        {
            ProjectHoloWeb.Models.User x = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            ViewBag.User = x;
            ProjectHoloWeb.Models.User dbUpUser = data.Users.FirstOrDefault(p => p.IDuser == id);
            if(vt == 1)
            {
                // chuyern thanh chu nhom // mac dinh chu nhom old thanh quan ly // do chu nhom only one
                // tim chu nhom old chuyen thanh quan ly
                ProjectHoloWeb.Models.User dbUserOldMaster = (from s in data.Users where s.IDteam == dbUpUser.IDteam where s.IDtransper == "TRAPER001" select s).Single();
                dbUserOldMaster.IDtransper = "TRAPER002";

                dbUpUser.IDtransper = "TRAPER001";
                data.SubmitChanges();
            }
            if(vt == 2)
            {
                dbUpUser.IDtransper = "TRAPER002";
                data.SubmitChanges();
            }
            if(vt == 3)
            {
                dbUpUser.IDtransper = "TRAPER003";
                data.SubmitChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult RoiNhom(string id)
        {
            ProjectHoloWeb.Models.User x = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            ViewBag.User = x;
            ProjectHoloWeb.Models.User dbUpUser = data.Users.FirstOrDefault(p => p.IDuser == id);
            // chu nhom roi nhom tu dong chon quan ly dau tien len lam truong nhom
            if (dbUpUser.IDtransper.CompareTo("TRAPER001") == 0)
            {
                ProjectHoloWeb.Models.User dbUserselected = (from s in data.Users where s.IDteam == dbUpUser.IDteam where s.IDtransper == "TRAPER002" select s).First();
                dbUserselected.IDtransper = "TRAPER001";
                data.SubmitChanges();
            }
            dbUpUser.IDtransper = null;
            dbUpUser.IDteam = null;
            data.SubmitChanges();

            return RedirectToAction("Index");
        }
        
        public ActionResult Duyet(string id)
        {
            ProjectHoloWeb.Models.User x = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            ViewBag.User = x;
            ProjectHoloWeb.Models.User dbUpUser = data.Users.FirstOrDefault(p => p.IDuser == id);
            dbUpUser.IDtransper = "TRAPER003";
            data.SubmitChanges();

            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult DangKyTV(string idTea)
        {
            ProjectHoloWeb.Models.User x = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            ViewBag.User = x;
            // dang ky là update bảng ghi user với quyền TRAPER000 và IDteam chọn
            // SESSION id
            ProjectHoloWeb.Models.User dbUpUser = data.Users.FirstOrDefault(p => p.IDuser == Session["TaikhoanID"].ToString());
            dbUpUser.IDtransper = "TRAPER000";
            dbUpUser.IDteam = idTea;
            data.SubmitChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult HuyDangKyTV()
        {
            ProjectHoloWeb.Models.User x = (from s in data.Users where s.IDuser == Session["TaikhoanID"].ToString() select s).ToList().First();
            ViewBag.User = x;
            // huy dang ký update bảng ghi user với quyền TRAPER và IDteam null
            // SESSION id
            ProjectHoloWeb.Models.User dbUpUser = data.Users.FirstOrDefault(p => p.IDuser == Session["TaikhoanID"].ToString());
            dbUpUser.IDtransper = null;
            dbUpUser.IDteam = null;
            data.SubmitChanges();

            return RedirectToAction("Index");
        }

    }
}