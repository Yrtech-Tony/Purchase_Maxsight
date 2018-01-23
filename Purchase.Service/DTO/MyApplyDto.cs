using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class MyApplyDto
    {
        public int ApplyId { get; set; }
        public string SeqNO { get; set; }
        public string ApplyUserId { get; set; }
        public string UserName { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string ApplyTypeId { get; set; }
        public string ApplyStatusCode { get; set; }
        public string ApplyReason { get; set; }
        public DateTime InDateTime { get; set; }// 申请时间
        public string RecheckStatusCode { get; set; } //审核状态
        public DateTime RecheckDateTime { get; set; } // 审核时间
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string SupplierName { get; set; }
        
    }
}
