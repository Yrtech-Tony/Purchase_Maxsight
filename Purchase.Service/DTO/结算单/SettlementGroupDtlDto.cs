using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class SettlementGroupDtlDto
    {
        public string ProjectId { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public string SeqNO_Supplier { get; set; }
        public string SupplierAmt { get; set; }
        public string QuotationGroupName { get; set; }

        public bool SettlementChk { get; set; }
        
        public bool SelectedChk { get; set; }
        public string GroupId { get; set; }
        public string SeqNO { get; set; }
        public string SettlementId { get; set; }
        public string InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
    }
}
