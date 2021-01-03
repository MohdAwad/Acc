using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Acc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

            );
            routes.MapRoute(
                name: "tow",
                url: "{controller}/{action}/{id}/{id2}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional }
                );
            routes.MapRoute(
                    name: "three",
                    url: "{controller}/{action}/{id}/{id2}/{id3}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional, id3 = UrlParameter.Optional }
                    );
            routes.MapRoute(
                    name: "four",
                    url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional, id3 = UrlParameter.Optional, id4 = UrlParameter.Optional }
                    );
            routes.MapRoute(
                   name: "five",
                   url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional, id3 = UrlParameter.Optional, id4 = UrlParameter.Optional, id5 = UrlParameter.Optional }
                   );
            routes.MapRoute(
        name: "Siven",
        url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7} ",
        defaults: new
        {
            controller = "Home",
            action = "Index",
            id = UrlParameter.Optional,
            id2 = UrlParameter.Optional,
            id3 = UrlParameter.Optional,
            id4 = UrlParameter.Optional,
            id5 = UrlParameter.Optional,
            id6 = UrlParameter.Optional,
            id7 = UrlParameter.Optional 
            
        }
        );
            routes.MapRoute(
                    name: "Nine",
                    url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional, id3 = UrlParameter.Optional, id4 = UrlParameter.Optional, id5 = UrlParameter.Optional, id6 = UrlParameter.Optional,
                    id7 = UrlParameter.Optional,id8 = UrlParameter.Optional,id9 = UrlParameter.Optional}
                    );

            routes.MapRoute(
                 name: "ten",
                 url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}",
                 defaults: new
                 {
                     controller = "Home",
                     action = "Index",
                     id = UrlParameter.Optional,
                     id2 = UrlParameter.Optional,
                     id3 = UrlParameter.Optional,
                     id4 = UrlParameter.Optional,
                     id5 = UrlParameter.Optional,
                     id6 = UrlParameter.Optional,
                     id7 = UrlParameter.Optional,
                     id8 = UrlParameter.Optional,
                     id9 = UrlParameter.Optional,
                     id10 = UrlParameter.Optional
                 }
                 );

  routes.MapRoute(
     name: "eleven",
     url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}/{id11}",
     defaults: new
     {
         controller = "Home",
         action = "Index",
         id = UrlParameter.Optional,
         id2 = UrlParameter.Optional,
         id3 = UrlParameter.Optional,
         id4 = UrlParameter.Optional,
         id5 = UrlParameter.Optional,
         id6 = UrlParameter.Optional,
         id7 = UrlParameter.Optional,
         id8 = UrlParameter.Optional,
         id9 = UrlParameter.Optional,
         id10 = UrlParameter.Optional,
         id11 = UrlParameter.Optional
     }
     );

 routes.MapRoute(
name: "Twelve",
url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}/{id11}/{id12}",
defaults: new
{
controller = "Home",
action = "Index",
id = UrlParameter.Optional,
id2 = UrlParameter.Optional,
id3 = UrlParameter.Optional,
id4 = UrlParameter.Optional,
id5 = UrlParameter.Optional,
id6 = UrlParameter.Optional,
id7 = UrlParameter.Optional,
id8 = UrlParameter.Optional,
id9 = UrlParameter.Optional,
id10 = UrlParameter.Optional,
id11 = UrlParameter.Optional,
id12 = UrlParameter.Optional
}
);


            routes.MapRoute(
            name: "Sixteen",
            url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}/{id11}/{id12}/{id13}/{id14}/{id15}/{id16}",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,
                id2 = UrlParameter.Optional,
                id3 = UrlParameter.Optional,
                id4 = UrlParameter.Optional,
                id5 = UrlParameter.Optional,
                id6 = UrlParameter.Optional,
                id7 = UrlParameter.Optional,
                id8 = UrlParameter.Optional,
                id9 = UrlParameter.Optional,
                id10 = UrlParameter.Optional,
                id11 = UrlParameter.Optional,
                id12 = UrlParameter.Optional,
                id13 = UrlParameter.Optional,
                id14 = UrlParameter.Optional,
                id15 = UrlParameter.Optional,
                id16 = UrlParameter.Optional
            }
            );
   routes.MapRoute(
name: "SevenTeen",
url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}/{id11}/{id12}/{id13}/{id14}/{id15}/{id16}/{id17}",
defaults: new
{
    controller = "Home",
    action = "Index",
    id = UrlParameter.Optional,
    id2 = UrlParameter.Optional,
    id3 = UrlParameter.Optional,
    id4 = UrlParameter.Optional,
    id5 = UrlParameter.Optional,
    id6 = UrlParameter.Optional,
    id7 = UrlParameter.Optional,
    id8 = UrlParameter.Optional,
    id9 = UrlParameter.Optional,
    id10 = UrlParameter.Optional,
    id11 = UrlParameter.Optional,
    id12 = UrlParameter.Optional,
    id13 = UrlParameter.Optional,
    id14 = UrlParameter.Optional,
    id15 = UrlParameter.Optional,
    id16 = UrlParameter.Optional,
    id17 = UrlParameter.Optional
}
);


            routes.MapRoute(
name: "EightTeen",
url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}/{id11}/{id12}/{id13}/{id14}/{id15}/{id16}/{id17}/{id18}",
defaults: new
{
 controller = "Home",
 action = "Index",
 id = UrlParameter.Optional,
 id2 = UrlParameter.Optional,
 id3 = UrlParameter.Optional,
 id4 = UrlParameter.Optional,
 id5 = UrlParameter.Optional,
 id6 = UrlParameter.Optional,
 id7 = UrlParameter.Optional,
 id8 = UrlParameter.Optional,
 id9 = UrlParameter.Optional,
 id10 = UrlParameter.Optional,
 id11 = UrlParameter.Optional,
 id12 = UrlParameter.Optional,
 id13 = UrlParameter.Optional,
 id14 = UrlParameter.Optional,
 id15 = UrlParameter.Optional,
 id16 = UrlParameter.Optional,
 id17 = UrlParameter.Optional,
 id18 = UrlParameter.Optional
}
);

            routes.MapRoute(
name: "nineTeen",
url: "{controller}/{action}/{id}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}/{id11}/{id12}/{id13}/{id14}/{id15}/{id16}/{id17}/{id18}/{id19}",
defaults: new
{
controller = "Home",
action = "Index",
id = UrlParameter.Optional,
id2 = UrlParameter.Optional,
id3 = UrlParameter.Optional,
id4 = UrlParameter.Optional,
id5 = UrlParameter.Optional,
id6 = UrlParameter.Optional,
id7 = UrlParameter.Optional,
id8 = UrlParameter.Optional,
id9 = UrlParameter.Optional,
id10 = UrlParameter.Optional,
id11 = UrlParameter.Optional,
id12 = UrlParameter.Optional,
id13 = UrlParameter.Optional,
id14 = UrlParameter.Optional,
id15 = UrlParameter.Optional,
id16 = UrlParameter.Optional,
id17 = UrlParameter.Optional,
id18 = UrlParameter.Optional,
    id19 = UrlParameter.Optional
}
);

        }
    }
}
