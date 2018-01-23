using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class AccruedChargesDto
    {
        public string AccruedChargeId { get; set; }
        public string CostType { get; set; }
        public string ExecuteCycle { get; set; }
        public string DepartmentCode { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectCode { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string AccruedChargesAmt { get; set; }
        public string PayMonth { get; set; }
        public string FlowInvoceAmt { get; set; }
        public string SourceChk { get; set; }
        
    }
}
