using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class RecheckStatusDtlDto
    {
        public string ApplyId { get;set;}
        public string SeqNO { get; set; }
        public string RecheckUserId { get; set; }
        public string UserName { get; set; }
        public string RecheckStatusCode { get; set; }
        public string RecheckReason { get; set; }
        public DateTime InDateTime { get; set; }
    }
}
