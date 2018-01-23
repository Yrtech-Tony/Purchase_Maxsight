using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    public class ProgramAndEditMst
    {
        public ProgramAndEditMst() { }
        public int Id { get; set; }//项目编号
        public string ExecuteCycle { get; set; }// 执行周期
        public string Remark { get; set; }// 其他说明 
    }
}
