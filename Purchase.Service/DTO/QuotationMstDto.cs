using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class QuotationMstDto
    {
        public string QuotationId { get; set; }
        public string QuotationType { get; set; }
        public string QuotationTypeTxt { get; set; }
        public string ProjectCode { get; set; }
        public string QuotationGroupName { get; set; }
        public string ProjectName { get; set; }

        public string ProjectId { get; set; }
        public string ProjectShortName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public string ModelType { get; set; }

        public bool SelectChk { get; set; }
        public string SelectChkName { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string ApplyId { get; set; }
        public string GroupId { get; set; }

    }
}
