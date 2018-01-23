using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class Juesuantongjibiao2_interDto
    {
        public string SupplyService { get; set; }
        public string ExcuteType { get; set; }// 对应费用构成
        public decimal? SupplyServiceSumAmt { get; set; }
        public decimal? SettleCount { get; set; }
        public decimal? SupplyService_Avg { get; set; }

    }
}
