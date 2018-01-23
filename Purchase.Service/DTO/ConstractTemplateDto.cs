using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class ConstractTemplateDto
    {
        public string Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateType { get; set; }
        public string UseChk { get; set; }
        public string FileName { get; set; }
        public string InUserId { get; set; }
        public DateTime? InDateTime { get; set; }
       
    }
}
