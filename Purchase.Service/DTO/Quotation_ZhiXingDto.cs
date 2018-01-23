using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class Quotation_ZhiXingDto : QuotationDto
    {
        public string QuotationId { get; set; } //确认单Id
        public string SeqNO { get; set; }//
        public string SupplierName { get; set; }//供应商名称
        public string SupplierId { get; set; }
        public string ServiceTrade { get; set; }//服务行业
        public string Province { get; set; }//执行省份
        public string City { get; set; }//执行城市
        public string ExecuteMode { get; set; }//执行方式
        public string IntoShopType { get; set; }
        public string UserType { get; set; }//用户类型
        public string ExistingOrPotential { get; set; }//xian
        public string CustomerType { get; set; }//
        public string ExcuteType { get; set; }//
        public string AccountUnit { get; set; }//
        public string ProposedPrice { get; set; }//
        public decimal? UnitPrice { get; set; }//
        private string _Count;
        public string Count
        {
            get
            {
                decimal? d = string.IsNullOrEmpty(_Count) ? new Nullable<decimal>() : decimal.Parse(_Count);
                return d.HasValue ? d.Value.ToString("#0.######") : "";
            }
            set
            {
                _Count = value;
            }
        }
        public string Remark { get; set; }//
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
