﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ChartTreeVM
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }

        public string AccountName { get; set; }
    }
}