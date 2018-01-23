using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class FlowOrderBugetDto
    {
        public string ProjectId { get; set; } // 
        public string Contingency { get; set; }//不可预见费 
        public string BugetSum { get; set; }// 预算金额
        public string FactSum { get; set; }//实际付款金额
        public bool OverFlowCheck { get; set; }// 是否超出预算
      
    }
}
