using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ExportAndUnExportVM
    {
        public IEnumerable<Header> Header { get; set; }
        public IEnumerable<St_Header> St_Header { get; set; }
    }
}