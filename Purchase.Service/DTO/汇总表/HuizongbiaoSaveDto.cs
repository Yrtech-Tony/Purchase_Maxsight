using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO.汇总表
{
    public class HuizongbiaoSaveDto
    {
        public string HuizongbiaoId { get; set; }
        public string ProjectId { get; set; }
        public string SupplierId { get; set; }
        public string SettlementId { get; set; }
        public string SettlementSeqNO { get; set; }
        public string Remark { get; set; }
    }
}
