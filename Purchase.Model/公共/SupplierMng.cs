using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    // 供应商信息
    public class SupplierMng
    {
        public SupplierMng() { }
        public int Id { get; set; }
        public string SupplierCode { get; set; } // 供应商代码
        public string SupplierName { get; set; }//供应名称
        public string SupplierShortName { get; set; }//供应商简称
        public string SupplierType { get; set; }//供应商类型
        public string ServiceTrade { get; set; }//服务行业
        public string CooperationState { get; set; }//合作状态
        public string PurchaseType { get; set; }//采购方式
        public string CooperationMode { get; set; }//合作方式
        public string RecommendDepartment { get; set; }//推荐部门
        public string Province { get; set; }//省份
        public string City { get; set; }//城市
        public string Address { get; set; }//公司地址
        public string PostCode { get; set; }//邮编
        public string CompanyTelCode { get; set; }//公司电话
        public string CompanyFaxCode { get; set; }//公司传真
        public string BranchCompanyAddress { get; set; }//分公司地址
        public string ProvideService { get; set; }//提供服务
        public string CorporateName { get; set; }//法人姓名
        public string CorporatePosition { get; set; }//法人职位
        public string CorporateTel { get; set; }//法人手机
        public string CorporateFixTel { get; set; }//法人固定电话
        public string CorporateEmail { get; set; }//法人邮箱
        public string BussinessMainName { get; set; }//主要联系人姓名
        public string BussinessMainPosition { get; set; }//主要联系人职位
        public string BussinessMainTel { get; set; }//主要联系人手机
        public string BussinessMainFixTel { get; set; }//主要联系人座机
        public string BussinessMainEmail { get; set; }//主要联系人电话
        public string BussinessSecondName { get; set; }//次要联系人姓名
        public string BussinessSecondPosition { get; set; }//次要联系人职位
        public string BussinessSecondTel { get; set; }//次要联系人手机
        public string BussinessSecondFixTel { get; set; }//次要联系人固话
        public string BussinessSecondEmail { get; set; }//次要联系人 Email
        public string IDType { get; set; }// 证件类型
        public string IDNumber { get; set; }//证件号码
        public string RegisterAmt { get; set; }//注册自检
        public string OperationStartDate { get; set; }//营业开始时间
        public string OperationEndDate { get; set; }//营业结束时间
        public string TaxRegistrationNo { get; set; }//税务登记证号
        public string VATRate { get; set; }//增值税税率
        public string AccountName { get; set; }//开户户名
        public string AccountBankFullName { get; set; }//开户银行全称
        public string AccountBankNo { get; set; }//开户账号
        public string AccountBankAddress { get; set; }//开户行地址
        public string Remark { get; set; }//备注
        public string InUserId { get; set; }//登记人
        public DateTime InDateTime { get; set; }//登记时间

        public List<SupplierMngAttchmentFile> SupplierMngAttchmentFiles { get; set; }
    }
}
