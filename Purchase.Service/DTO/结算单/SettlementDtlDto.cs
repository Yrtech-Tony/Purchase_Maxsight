using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class SettlementDtlDto
    {
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public string Quotationid { get; set; }
        public string SeqNO { get; set; }
        public string zhixingshengfen { get; set; }
        public string zhixingchengshi { get; set; }
        public string FeeContent { get; set; }
        public string danjia { get; set; }
        public decimal? SettleCount { get; set; }
        public string SettleAmt { get; set; }
        public string Remark { get; set; }
        public string SettlementType { get; set; }
        public string SettlementId { get; set; }
        public string settlementSeqNO { get; set; }

    }
}
