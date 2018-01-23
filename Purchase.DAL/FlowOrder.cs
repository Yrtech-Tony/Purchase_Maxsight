//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Purchase.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class FlowOrder
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<System.DateTime> ExecuteCycleStartDate { get; set; }
        public Nullable<System.DateTime> ExecuteCycleEndDate { get; set; }
        public string ExpendPattern { get; set; }
        public string ExpendType { get; set; }
        public string PaymentType { get; set; }
        public string PaymentCompany { get; set; }
        public Nullable<System.DateTime> EstimatedPaymentTime { get; set; }
        public Nullable<decimal> EstimatePaymentAmt { get; set; }
        public Nullable<System.DateTime> FactPaymentTime { get; set; }
        public Nullable<decimal> FactPaymentAmt { get; set; }
        public Nullable<decimal> InvoiceAmt { get; set; }
        public string Payee { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> ApplyPayChk { get; set; }
        public string ModifyUserId { get; set; }
        public Nullable<System.DateTime> ModifyDateTime { get; set; }
        public string InUserId { get; set; }
        public Nullable<System.DateTime> InDateTime { get; set; }
        public Nullable<decimal> PreTaxAmt { get; set; }
        public Nullable<decimal> InvoceAmtThis { get; set; }
        public string ProjectType { get; set; }
        public string ConstractChk { get; set; }
        public string SettlementChk { get; set; }
        public string InvoceChk { get; set; }
        public string InvoiceNO { get; set; }
        public string PaperChk { get; set; }
        public string PaymentRemark { get; set; }
        public string Finance_PaymentStatus { get; set; }
        public Nullable<decimal> Finance_PaymentAmt { get; set; }
        public string Finance_NotPayReason { get; set; }
        public string Finance_Constract { get; set; }
        public string Finance_SettlementChk { get; set; }
        public Nullable<decimal> Finance_InvoceAmt { get; set; }
        public Nullable<decimal> Finance_InvoceAmtThis { get; set; }
        public Nullable<int> SettlementId { get; set; }
    }
}