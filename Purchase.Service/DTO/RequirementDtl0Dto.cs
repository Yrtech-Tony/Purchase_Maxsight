using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class RequirementDtl0Dto
    {
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string PurchaseType { get; set; }//采购分类
        public string PurchaseMode { get; set; }//采购方式
        public string Responsibilites { get; set; }//工作职责
        public string ProjectType { get; set; }//项目类型
        public string ExcuteMode { get; set; }//执行方式
        public string SupplyService { get; set; }//提供服务
        public string ItemName { get; set; }//品名
        public string Count { get; set; }//样本量
        public string Status { get; set; }//状态
        public string HistoryChk { get; set; }
        public string RequirementId { get; set; }//需求书Id
        public string RequirementType { get; set; }// 需求书类型


        public string ExecuteCycle { get; set; }
        public string Remark { get; set; }
    }
}
