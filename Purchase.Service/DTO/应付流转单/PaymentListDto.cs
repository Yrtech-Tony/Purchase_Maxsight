using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class PaymentListDto
    {
        public string PaymentListId { get; set; }
        public string Year { get; set; }
        public DateTime? FactPaymentTime { get; set; }
        public string DepartmentCode { get; set; }
        public string ModelType { get; set; }
        public string ServiceTrade { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string SupplierName { get; set; }
        public string Payee { get; set; }
        public string PretaxAmt { get; set; }
        public string TaxAmt { get; set; }
        public string FactPaymentAmt { get; set; }
        public string InvoiceAmt { get; set; }
        public string InvoceAmtThis { get; set; }

        public string ProjectType { get; set; }
        public string ExecuteCycle { get; set; }
        public string ConstractChk { get; set; }
        public string SettlementChk { get; set; }
        public string InvoceChk { get; set; }
        public string PaperChk { get; set; }
        public string InvoiceNO { get; set; }
        public string Remark { get; set; }
        public string Finance_PaymentStatus { get; set; }
        public string Finance_PaymentAmt { get; set; }
        public string Finance_NotPayReason { get; set; }
        public string Finance_Constract { get; set; }
        public string Finance_SettlementChk { get; set; }
        public string Finance_InvoceAmt { get; set; }
        public string Finance_InvoceAmtThis { get; set; }
        public string InUserId { get; set; }
        public string InDateTime { get; set; }
        public string ModifyUserId { get; set; }
        public string ModifyDateTime { get; set; }
    }
}
