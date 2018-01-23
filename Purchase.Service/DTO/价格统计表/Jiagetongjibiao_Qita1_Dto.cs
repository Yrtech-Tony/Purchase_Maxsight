using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class Jiagetongjibiao_Qita1_Dto
    {
        public string ModelType { get; set; }
        public string PurchaseType { get; set; }
        public string PurchaseMode { get; set; }
        public string SupplyService { get; set; }
        public string ItemName { get; set; }
       
        public decimal? danjia_AVG { get; set; }

    }
}
