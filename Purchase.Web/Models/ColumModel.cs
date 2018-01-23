using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Purchase.Web.Models
{
    public class ColumModel
    {
        public bool Hidden { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Align { get; set; }
        public int Width { get; set; }
    }
}