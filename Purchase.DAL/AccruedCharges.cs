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
    
    public partial class AccruedCharges
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public string CostType { get; set; }
        public string DepartmentCode { get; set; }
        public string InUserId { get; set; }
        public Nullable<System.DateTime> InDateTime { get; set; }
    }
}
