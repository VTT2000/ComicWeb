using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectHoloWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Admins",
            url: "Admins",
            defaults: new { controller = "Admins", action = "Index" }
               );

            routes.MapRoute(
            name: "Tìm kiếm",
            url: "tim-kiem",
            defaults: new { controller = "Comic", action = "Search" }
                );

            routes.MapRoute(
              name: "Chương",
              url: "chapter",
              defaults: new { controller = "Comic", action = "Chapter" }
              );         
            routes.MapRoute(
             name: "Thể loại",
             url: "the-loai",
             defaults: new { controller = "Comic", action = "Type", id = UrlParameter.Optional }
             );
            routes.MapRoute(
               name: "Chi tiết",
               url: "truyen",
               defaults: new { controller = "Comic", action = "DetailComic" , id = UrlParameter.Optional }
               );
            
            routes.MapRoute(
             name: "User",
             url: "user",
             defaults: new { controller = "User", action = "Users", id = UrlParameter.Optional }
             );
            routes.MapRoute(
              name: "Đăng Ký",
              url: "dang-ky",
              defaults: new { controller = "User", action = "Register" }
              );
            routes.MapRoute(
              name: "Đăng Nhập",
              url: "dang-nhap",
              defaults: new { controller = "User", action = "Login" }
              );

            routes.MapRoute(
               name: "Home",
               url: "trang-chu",
               defaults: new { controller = "Home", action = "home" }
               );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "home", id = UrlParameter.Optional }
            );
      
        }
    }
}
