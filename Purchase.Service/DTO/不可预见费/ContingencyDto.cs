using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
   public class ContingencyDto
    {
       public string ProjectId { get; set; }
       public string ContingencyFee { get; set; }
       public string InUserId { get; set; }
       public DateTime? InDateTime { get; set; }
    }
}
