using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class QuotationGroupDto
    {
        public string GroupId { get; set; }
        public string QuotationGroupName { get; set; }
        public string ProjectId { get; set; }
        public string InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectShortName { get; set; }
        public string ApplyStatusCode { get; set; }
        public string ApplyId { get; set; }
        public string SeqNO { get; set; }
        public bool UserChk { get; set; }
        public bool HasData { get; set; }
        public bool SettlementChk { get; set; }
        public string RequirementGroupName { get; set; }
        public string RequirementGroupId { get; set; }
      
    }
}
