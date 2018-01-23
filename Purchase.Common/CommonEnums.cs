using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Common
{
    public enum ApplyType
    {
        供应商,
        合同,
        结算单,
        确认单,
        需求书,
        立项,
        流转单,
    }

    public enum ApprovalProcessStatus
    {
        进行中,
        申请,
        审核完毕,

    }

    public enum ApprovalStatus
    {
        终止,
        拒绝,
        申请,
        同意,
    }
}
