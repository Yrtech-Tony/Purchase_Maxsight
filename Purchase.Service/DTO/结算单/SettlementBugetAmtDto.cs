using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class SettlementBugetAmtDto
    {
        public string YusuanAmt_Projects { get; set; }// 项目的预算金额
        public string SettlementFinishAmt { get; set; } // 已经结算的金额
        public string ThisGroupAmt { get; set; }// 本次提交的总金额
        public string LeftAmt { get; set; }// 剩余金额:预算-已经结算-本次提交
        public string CommitAmt { get; set; }// 已经提交的金额:已经结算金额+本次提交金额
    }
}
