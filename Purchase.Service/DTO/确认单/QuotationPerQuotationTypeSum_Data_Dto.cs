using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class QuotationPerQuotationTypeSum_Data_Dto
    {

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public string SupplierCode { get; set; }
        public string QuotationType { get; set; }

        public decimal? SumAmt { get; set; }
    }
}
