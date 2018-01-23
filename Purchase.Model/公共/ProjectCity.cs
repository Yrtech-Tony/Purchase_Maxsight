using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    public class ProjectCity
    {
        public int Id { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Responsibilities { get; set; }
        public string ProjectType { get; set; }
        public string ExectMode { get; set; }
        public string SupplyService { get; set; }
        public string ItemName { get; set; }
        public int Count { get; set; }
        public string DemandBookStatus { get; set; }
        public string Status { get; set; }
        public bool EmailSendChk { get; set; }
        public string InUserId { get; set; }
        public DateTime InDateTime { get;set;}

        /*
         对应的项目信息
         */
        public Project Project { get; set; }
        public int ProjectId { get; set; } // 项目代码

        /*对应需求书的信息*/

        public string RequirmentType { get; set; }
        public int RequirmentId { get; set; }

  

    }
}
