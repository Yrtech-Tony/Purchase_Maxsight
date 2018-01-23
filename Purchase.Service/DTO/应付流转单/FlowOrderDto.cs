using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class FlowOrderDto
    {
        public string ApplyId { get; set; }
        public string SeqNO { get; set; }
        public string ApplyStatusCode { get; set; }
        public string Year { get; set; }
        public string FlowOrderId { get; set; }
        public string ModelType { get; set; }
        public string DepartmentCode { get; set; }
        public string ServiceTrade { get; set; }
        public string CostDepartment { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectCode { get; set; }
        public DateTime? ExecuteCycleStartDate { get; set; }
        public DateTime? ExecuteCycleEndDate { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string BudgetAmt { get; set; }
        public string BudgetLeftAmt { get; set; }
        public string ExpendPattern { get; set; }
        public string ExpendType { get; set; }
        public string PaymentType { get; set; }
        public string PaymentCompany { get; set; }
        public DateTime? EstimatedPaymentTime { get; set; }
        public string EstimatePaymentAmt { get; set; }
        public DateTime? FactPaymentTime { get; set; }
        public string FactPaymentAmt { get; set; }
        public string InvoiceAmt { get; set; }
        public string Payee { get; set; }
        public string Remark { get; set; }
        public string InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
        public string ModifyUserId { get; set; }
        public DateTime? ModifyDateTime { get; set; }
        public string PreTaxAmt { get; set; }
        public string TaxAmt { get; set; }
        public string InvoceAmtThis { get; set; }
        public string ProjectType { get; set; }
        public string ConstractChk { get; set; }
        public string SettlementChk { get; set; }
        public string InvoceChk { get; set; }
        public string PaperChk { get; set; }
        public string InvoiceNO { get; set; }
        public string PaymentRemark { get; set; }
        public string Finance_PaymentStatus { get; set; }
        public string Finance_PaymentAmt { get; set; }
        public string Finance_NotPayReason { get; set; }
        public string Finance_Constract { get; set; }
        public string Finance_SettlementChk { get; set; }
        public string Finance_InvoceAmt { get; set; }
        public string Finance_InvoceAmtThis { get; set; }
        public bool UserChk { get; set; }
        public bool ApplyPayChk { get; set; }
        public DateTime? PayTime { get; set; }
        public Decimal? PayAmt { get; set; }
        public string FactChk { get; set; }
    }
}
