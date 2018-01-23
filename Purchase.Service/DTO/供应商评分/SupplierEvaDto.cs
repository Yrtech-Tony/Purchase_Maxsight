using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class SupplierEvaDto
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string SupplierName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public decimal? YunzuobuScore { get; set; }
        public decimal? QudaoPMScore { get; set; }
        public decimal? QCScore { get; set; }
        public decimal? YanjiuScore { get; set; }
        public decimal? SupplierScore { get; set; }
       
        public string SubjectType { get; set; }
        public string SubjectId { get; set; }
        public string ProjectId { get; set; }
        public string SupplierId { get; set; }
        public string AnswerId { get; set; }
        public string ModifyUserId { get; set; }
        public DateTime? ModifyDateTime { get; set; }
        public string InUserId { get; set; }
        public string Remark { get; set; }
    }
}
