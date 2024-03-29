﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace t_iv_mvc4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "edit",
             url: "PhoneBook/Edit/{recid}",
             defaults: new { controller = "PhoneBook", action = "EditAdd" }
            );
            routes.MapRoute(
            name: "add",
            url: "PhoneBook/Add",
            defaults: new { controller = "PhoneBook", action = "EditAdd" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PhoneBook", action = "Index", id = UrlParameter.Optional }
            );
         
        }
    }
}