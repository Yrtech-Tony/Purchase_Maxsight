using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class ProjectDto
    {
        public string ApplyId { get; set; }
        public string SeqNO { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ModelType { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ExcuteType { get; set; }
        public string Status { get;set; }
        public string StatusDate{get;set;}
        public string Step { get; set; }
        public string ApplyStatusCode { get; set; }
        public string CostDepartment { get; set; }
        public string ServiceRegion { get; set; }
        public bool UserChk { get; set; }
        public string Remark { get; set; }
        public int CityCount { get; set; }
        public int PersonCount { get; set; }
        public string InUserId { get; set; }
        public string UserName { get; set; }
        public string ServiceTrade { get; set; }
        public DateTime StartDate_Min { get; set; }
        public DateTime StartDate_Max { get; set; }
    }
}
