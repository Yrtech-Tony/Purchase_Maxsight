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
    
    public partial class SupplierRequirementEmail
    {
        public int RequirementId { get; set; }
        public int SeqNo { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public string Recipients { get; set; }
        public string CCPerson { get; set; }
        public string Title { get; set; }
        public string EmailContent { get; set; }
        public string Remark { get; set; }
        public string InUserId { get; set; }
        public Nullable<System.DateTime> InDateTime { get; set; }
    }
}
