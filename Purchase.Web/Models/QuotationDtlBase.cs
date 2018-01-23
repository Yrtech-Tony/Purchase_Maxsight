using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Purchase.Web.Models
{
    public class QuotationDtlBase
    {
        public string QuotationId { get; set; }
        public string QuotationType { get; set; }
        public int SeqNO { get; set; }
        public IEnumerable<int> SeqNOs { get; set; }
    }
}