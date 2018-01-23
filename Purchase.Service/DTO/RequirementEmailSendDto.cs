using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class RequirementEmailSendDto
    {
        public string RequirementId { get; set; }
        public string SeqNO { get; set; }
        public string SupplierId { get; set; }
        public DateTime InDateTime { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierShortName { get; set; }
        public string UserName { get; set; }
    }
}
