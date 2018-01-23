using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    public class ProjectPerson
    {
        public ProjectPerson() { }

        public int Id { get; set; }
        public string PersonName { get; set; }// 参与项目人名
        public string InUserId { get; set; }//
        public DateTime InDateTime { get; set; }//

        public Project Project { get; set; }
        public int ProjectId { get; set; } // 项目代码
    }
}
