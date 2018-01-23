using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class QuotationExport_Data_Dto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string zhixingshengfen { get; set; }
        public string zhixingchengshi { get; set; }
        public string QuotationMode { get; set; }
        public decimal? danjia { get; set; }
        public decimal? shuliang { get; set; }
        public string beizhu { get; set; }
    }
}
