using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class QuotationTypeDto
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string QuotationGroupId { get; set; }
        public string QuotationType { get; set; }
        public string QuotationTypeName { get; set; }
    }
}
