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
    
    public partial class Quotation_Yanjiu_Dtl
    {
        public int QuotationId { get; set; }
        public int SeqNO { get; set; }
        public string zhixingshengfen { get; set; }
        public string zhixingchengshi { get; set; }
        public string zhixingfangshi { get; set; }
        public string gongzuofenlei { get; set; }
        public string gongzuoneirong { get; set; }
        public string leixingpinpai { get; set; }
        public string guigeyaoqiu { get; set; }
        public string zhiliangbiaozhun { get; set; }
        public string hesuandanwei { get; set; }
        public string cankaojiage { get; set; }
        public Nullable<decimal> danjia { get; set; }
        public Nullable<decimal> yusuandanjia { get; set; }
        public Nullable<decimal> shuliang { get; set; }
        public string beizhu { get; set; }
        public string InUserId { get; set; }
        public Nullable<System.DateTime> InDateTime { get; set; }
        public Nullable<bool> SelectedChk { get; set; }
        public Nullable<int> RequirementId { get; set; }
        public Nullable<int> RequirementSeqNO { get; set; }
        public string RequirementMstType { get; set; }
        public string RequirementRemark { get; set; }
    }
}
