using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class RemindDto
    {
        public string RemindContent { get; set; }
        public string AlterCancelChk { get; set; }

        public string RemindObject { get; set; }
        public string Remark { get; set; }
        public string RemindId { get; set; }
        public string AlterUserId { get; set; }
        public DateTime EndDate { get; set; }
    }
}
