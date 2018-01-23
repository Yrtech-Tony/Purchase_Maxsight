﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class CapitalRequiremenDto
    {
        public string CapitalRequirementId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectName { get; set; }
        public string DepartmentCode { get; set; }
        public string ExpendType { get;set; }
        public string ExpendPattern { get; set; }


    }
}
