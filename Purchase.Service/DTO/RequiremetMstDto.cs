using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class RequiremetMstDto
    {
        public string ModelType { get; set; }
        public string ProjectId { get; set; } 
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string SeqNO { get; set; }
        public string RequirementGroupId { get; set; }
        public string RequirementGroupName { get; set; }
        public string HistoryChk { get; set; } //是否为最新需求书
        public string ApplyId { get; set; }
        public string RequirementId { get; set; }
        public string RequirementType { get; set; }
        public string RequirementName { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public bool Selected { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }

        public string Responsibilites { get; set; }
        public string ProjectType { get; set; }
        public string ExcuteMode { get; set; }
        public string PurchaseType { get; set; }
        public string PurchaseMode { get; set; }
        public string SupplyService { get; set; }
        public string ItemName { get; set; }
        public string Count { get; set; }
        public string Remark { get; set; }// 城市样本量的备注
    }
}
