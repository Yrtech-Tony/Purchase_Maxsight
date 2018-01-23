using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Purchase.Web.Models
{
    public class SupplierMngModel
    {
        public int Id { get; set; }
        [Display(Name = "供应商代码")]
        public string SupplierCode { get; set; }
        [Display(Name = "供应名称")]
        public string SupplierName { get; set; }
        [Display(Name = "供应商简称")]
        public string SupplierShortName { get; set; }
        [Display(Name = "供应商类型")]
        public string SupplierType { get; set; }
        [Display(Name = "服务行业")]
        public string ServiceTrade { get; set; }
        [Display(Name = "合作状态")]
        public string CooperationState { get; set; }
        [Display(Name = "采购方式")]
        public string PurchaseType { get; set; }
        [Display(Name = "合作方式")]
        public string CooperationMode { get; set; }
        [Display(Name = "推荐部门")]
        public string RecommendDepartment { get; set; }
        [Display(Name = "省份")]
        public string Province { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        [Display(Name = "公司地址")]
        public string Address { get; set; }
        [Display(Name = "邮编")]
        public string PostCode { get; set; }
        [Display(Name = "公司电话")]
        public string CompanyTelCode { get; set; }
        [Display(Name = "公司传真")]
        public string CompanyFaxCode { get; set; }
         [Display(Name = "分公司地址")]
        public string BranchCompanyAddress { get; set; }
         [Display(Name = "提供服务")]
        public string ProvideService { get; set; }
         [Display(Name = "法人姓名")]
        public string CorporateName { get; set; }
         [Display(Name = "法人职位")]
        public string CorporatePosition { get; set; }
         [Display(Name = "法人手机")]
        public string CorporateTel { get; set; }
         [Display(Name = "法人固定电话")]
        public string CorporateFixTel { get; set; }
         [Display(Name = "法人邮箱")]
        public string CorporateEmail { get; set; }
         [Display(Name = "主要联系人姓名")]
        public string BussinessMainName { get; set; }
         [Display(Name = "主要联系人职位")]
        public string BussinessMainPosition { get; set; }
         [Display(Name = "主要联系人手机")]
        public string BussinessMainTel { get; set; }
        [Display(Name = "主要联系人传真")]
        public string BussinessMainFixTel { get; set; }
        [Display(Name = "主要联系人邮箱")]
        public string BussinessMainEmail { get; set; }
        [Display(Name = "次要联系人姓名")]
        public string BussinessSecondName { get; set; }
        [Display(Name = "次要联系人职位")]
        public string BussinessSecondPosition { get; set; }
       [Display(Name = "次要联系人手机")]
        public string BussinessSecondTel { get; set; }
        [Display(Name = "次要联系人固话")]
        public string BussinessSecondFixTel { get; set; }
        [Display(Name = "次要联系人 Email")]
        public string BussinessSecondEmail { get; set; }
        [Display(Name = "证件类型")]
        public string IDType { get; set; } 
        [Display(Name = "证件号码")]
        public string IDNumber { get; set; }
        [Display(Name = "注册自检")]
        public string RegisterAmt { get; set; }
        [Display(Name = "营业开始时间")]
        public string OperationStartDate { get; set; }
        [Display(Name = "营业结束时间")]
        public string OperationEndDate { get; set; }
        [Display(Name = "税务登记证号")]
        public string TaxRegistrationNo { get; set; }
        [Display(Name = "增值税税率")]
        public string VATRate { get; set; }
        [Display(Name = "开户户名")]
        public string AccountName { get; set; }
        [Display(Name = "开户银行全称")]
        public string AccountBankFullName { get; set; }
        [Display(Name = "开户账号")]
        public string AccountBankNo { get; set; }
        [Display(Name = "开户行地址")]
        public string AccountBankAddress { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "登记人")]
        public string InUserId { get; set; }
        [Display(Name = "登记时间")]
        public DateTime InDateTime { get; set; }
    }
}