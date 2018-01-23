using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class Huizongbiao_ShoudongDto
    {
        public string SupplierId { get; set; }
        public string ProjectId { get; set; }
        public string SettlementId { get; set; }
        public string SettlementSeqNO { get; set; }         
        public string SupplierName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectCode { get; set; }
        public string zhixingshengfen { get; set; }
        public string zhixingchengshi { get; set; }

        public decimal? shuliang { get; set; }
        public decimal? danjia { get; set; }
        public string heji { get; set; }
       
        public string Remark { get; set; }

        public string Year { get; set; }
        public string FeeContent { get; set; }
    }
}
