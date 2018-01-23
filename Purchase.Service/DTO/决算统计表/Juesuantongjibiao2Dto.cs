using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class Juesuantongjibiao2Dto
    {
        public string ExecuteMode { get; set; }
        public string ExcuteType { get; set; }
        public decimal? ExecuteModeSumAmt { get; set; }
        public decimal? SettleCount { get; set; }
        public decimal? Execute_Avg { get; set; }

    }
}
