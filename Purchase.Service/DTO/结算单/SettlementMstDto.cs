using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class SettlementMstDto
    {
        public string ApplyId { get; set; }
        public string SeqNO { get; set; }// 申请序号
        public string ApplyStatusCode { get; set; }
        public bool UserChk { get; set; }
        public string SettlementId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public string SeqNO_Supplier { get; set; } // 供应商结算单序号
        public string QuotationGroupId { get; set; }
        public string QuotationGroupName { get; set; }
        public bool SettleChk { get; set; }
        public bool DelChk { get; set; }
        public string ExecuteCycle { get; set; }
        public string zhixingshengfen { get; set; }
        public string zhixingchengshi { get; set; }
        public string FlowOrderSum { get; set; }
        public string SupplyService { get; set; }// 提供服务
        public string ServiceRegion { get; set; }// 服务区域
        public string SupplierAmt { get; set; } //供应商的结算金额合计
        public string SupplierBugetAmt { get; set; }// 供应商的预算金额 
        public bool SettlementDtlFinishCheck { get; set; } // 结算单详细是否填写完毕
        public bool SettlementDtlDebitCheck { get; set; }// 是否填写扣款
        public bool SettlementDtlAdditionalCheck { get; set; } // 是否填写追加
        public bool RecheckFinishCheck { get; set;}// 结算单所在组是否审核完毕
        public bool CommitGroupChk { get; set; } // 是否已经添加到审核组中
        public string CommitGroupApplyStatusCode { get; set; }// 结算单所在组的审核状态
        public DateTime? InDateTime { get; set; }
                

    }
}
