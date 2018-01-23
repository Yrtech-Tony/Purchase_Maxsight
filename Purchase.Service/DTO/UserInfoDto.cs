using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class UserInfoDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DepartmentCode { get; set; }
        public string PositionCode { get; set; }
        public string RoleTypecode { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string EmailFooter { get; set; }

        // 授权的项目信息
        public string SetCode { get; set; }
        public string SetName { get; set; }
        public string SetShortName { get; set; }
        public string SetId { get; set; }
        public string SetUserName { get; set; }

        public DateTime? SetInDateTime { get; set; }
        public string SetInUserName { get; set; }

        public bool Selected { get; set; }

    }
}
