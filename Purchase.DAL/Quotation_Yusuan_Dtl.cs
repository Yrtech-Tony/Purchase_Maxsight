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
    
    public partial class Quotation_Yusuan_Dtl
    {
        public int QuotationId { get; set; }
        public int QuotationSeqNO { get; set; }
        public int YusuanGroupId { get; set; }
        public Nullable<decimal> yusuandanjia_new { get; set; }
        public Nullable<decimal> yusuanshuliang { get; set; }
        public Nullable<System.DateTime> Indatetime { get; set; }
        public string InUserId { get; set; }
        public Nullable<System.DateTime> ModifyDateTime { get; set; }
        public string ModifyUserId { get; set; }
    }
}