using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class Group_YusuanDto
    {
        public int Id { get; set; }
        public string QuotationYusuanGroupName { get; set; }
        public int ProjectId { get; set; }
        public bool LastChk { get; set; }
        public string InUserId { get; set; }
        public DateTime IndateTime { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectCode { get; set; }
    }
}
