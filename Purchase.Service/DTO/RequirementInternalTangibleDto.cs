using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
   public class RequirementInternalTangibleDto
    {
       public string RequirementId{get;set;}
      public string SeqNO{get;set;}
      public string Brand{get;set;}
      public string Specification{get;set;}
      public string Model{get;set;}
      public string Configuration{get;set;}
      public string Color{get;set;}
      public string Material{get;set;}
      public string LogisticsRequirements{get;set;}
      public string PackagingRequirements{get;set;}
      public string Warranty{get;set;}
      public string AfterSalePeriod{get;set;}
      public string InspectionCycle { get; set; }
      public string Remark{get;set;}
      public string InUserId{get;set;}
      public DateTime? InDateTime{get;set;}


      public string ProjectId { get; set; }
      public string ProjectCode { get; set; }
      public string ServiceRegion { get; set; }
      public string PurchaseType { get; set; }
      public string PurchaseMode { get; set; }
      public string SupplyService { get; set; }
      public string ItemName { get; set; }
      public string Count { get; set; }
      public string RequirementGroupId { get; set; }
    }
}
