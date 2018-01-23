using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class HeaderDto
    {
        public string ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Display { get; set; }
        public decimal? SumAmt { get; set; }
        public bool EstPayChk { get; set; }
        public bool AdvancePaymentChk { get; set; }
    }
}
