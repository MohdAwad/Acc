using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class FavScreen
    {
        public int Id { get; set; }


        public int CompanyID { get; set; }

        public string UserId { get; set; }

        public string ScreenName { get; set; }

        public string ScreenUrl { get; set; }

        public int ScreenType { get; set; }

        public int Order { get; set; }
    }
}