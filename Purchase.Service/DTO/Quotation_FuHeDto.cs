﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class Quotation_FuHeDto : QuotationDto
    {
        public string QuotationId { get; set; } //确认单Id
        public string SeqNO { get; set; }//
        public string SupplierName { get; set; }//供应商名称
        public string SupplierId { get; set; }
        public string ServiceTrade { get; set; }//服务行业
        public string Province { get; set; }//执行省份
        public string City { get; set; }
        public string zhixingshengfen { get; set; }
        public string zhixingchengshi { get; set; }
        public string fuheyaoqiu { get; set; }
        public string yanbenbianji { get; set; }
        public string dianhuazixun { get; set; }
        public string dianhuahuifang { get; set; }
        public string bianmaleixing { get; set; }
        public string wenjuanbiancheng { get;set;}
        public string fuzhuyongpin { get; set; }
        public string hesuandanwei { get; set; }
        public string cankaojiage { get; set; }
        public Nullable<decimal> danjia { get; set; }
        private string _shuliang;
        public string shuliang
        {
            get
            {
                decimal? d = string.IsNullOrEmpty(_shuliang) ? new Nullable<decimal>() : decimal.Parse(_shuliang);
                return d.HasValue ? d.Value.ToString("#0.######") : "";
            }
            set
            {
                _shuliang = value;
            }
        }
        public string beizhu { get; set; }
        public string InUserId { get; set; }//
        public DateTime? InDateTime { get; set; }//
        public Nullable<decimal> yusuandanjia { get; set; }
        public Nullable<decimal> yusuandanjia_new { get; set; }
        public Nullable<decimal> yusuanshuliang { get; set; }
        public bool SelectedChk { get; set; }
        public string QuotationType { get; set; }
        public string RequirementId { get; set; }
        public string RequirementSeqNO { get; set; }
        public string RequirementMstType { get; set; }
        public bool? ConstractChk { get; set; }
        public string RequirementRemark { get; set; } // 需求书备注
    }
}
