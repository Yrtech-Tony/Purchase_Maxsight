using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class SupplierEvaAnaDto
    {
        
        public string SupplierName { get; set; }
        public string SubjectType { get;set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectId { get; set; }
        public string SupplierId { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string DepartmentName { get; set; }
        public string MainSubjectName { get; set; }
        public decimal? YunzuobuScore_Avg { get; set; }
        public decimal? QCbuScore_Avg { get; set; }
        public decimal? YanjiubuScore_Avg { get; set; }
        public decimal? Score_Avg { get; set; }
        public string ModifyUserId { get; set; }
        public DateTime? ModifyDateTime { get; set; }
       

    }
}
