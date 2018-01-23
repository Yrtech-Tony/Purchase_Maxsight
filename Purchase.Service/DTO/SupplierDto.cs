using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class SupplierDto
    {
        public string ApplyId { get; set; }
        public string SeqNO { get; set; }
        public string ApplyStatusCode { get; set; }
        public string PayCycle { get; set; }
        public string SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public string SupplierType { get; set; }
        public string SupplierType1 { get; set; }
        public string NotServiceType { get; set; }
        public string ServiceTrade { get; set; }
        public string CooperationState { get; set; }
        public string PurchaseType { get; set; }
        public string CooperationMode { get; set; }
        public string RecommendDepartment { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string CompanyTelCode { get; set; }
        public string CompanyFaxCode { get; set; }
        public string BranchCompanyAddress { get; set; }
        public string ProvideService { get; set; }
        public string CorporateName { get; set; }
        public string CorporatePosition { get; set; }
        public string CorporateTel { get; set; }
        public string CorporateFixTel { get; set; }
        public string CorporateEmail { get; set; }
        public string BussinessMainName { get; set; }
        public string BussinessMainPosition { get; set; }
        public string BussinessMainTel { get; set; }
        public string BussinessMainFixTel { get; set; }
        public string BussinessMainEmail { get; set; }
        public string BussinessSecondName { get; set; }
        public string BussinessSecondPosition { get; set; }
        public string BussinessSecondTel { get; set; }
        public string BussinessSecondFixTel { get; set; }
        public string BussinessSecondEmail { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string RegisterAmt { get; set; }
        public DateTime? OperationStartDate { get; set; }
        public DateTime? OperationEndDate { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string VATRate { get; set; }
        public string AccountName { get; set; }
        public string AccountBankFullName { get; set; }
        public string AccountBankNo { get; set; }
        public string AccountBankAddress { get; set; }
        public string Remark { get; set; }
        public string BankModifyColumn { get; set; }
        public DateTime? BankModifyDateTime { get; set; }
        public string ExecuteType { get; set; }
        public bool? UseChk { get; set; }
        public string InUserId { get; set; }
        public string UserName { get; set; }
        public DateTime? InDateTime { get; set; }
        public string ModifyUserId { get; set; }
        public DateTime? ModifyDateTime { get; set; }
        public bool UserChk { get; set; }
        public string TempChk { get; set; }// Y:是临时保存，N:是正式数据。""：正式数据
        public bool BussinessPngChk { get; set; }
        public bool TaxRegistPngChk { get; set; }
        public bool OraCodePngChk { get; set; }

    }
}
