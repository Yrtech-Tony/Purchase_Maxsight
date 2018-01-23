using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    public class Project
    {
        public int Id { get; set; } //项目Id，系统自动生成
        public string ProjectCode { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public string ProjectShortName { get; set; }//项目简称
        public string ModelType { get; set; }//模块类型（采购归属)
        public string Step { get; set; }//阶段
        public string DepartmentCode { get; set; }//所属部门
        public string Year { get; set; }//项目年份
        public DateTime StartDate { get; set; }//项目开始时间
        public DateTime EndDate { get; set; }//项目结束时间
        public string ExcuteType { get; set; }//执行方式（按全国，按省份，按城市）
        public string InUserId { get; set; }
        public DateTime InDateTime { get; set; }

        public List<ProjectPerson> ProjectPersons { get; set; }
        public List<ProjectCity> ProjectCitys { get; set; }
    }
}
