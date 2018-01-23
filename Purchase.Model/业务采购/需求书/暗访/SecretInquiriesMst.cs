using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 需求书-暗访 主表
    /// </summary>
    public class SecretInquiriesMst
    {
        public SecretInquiriesMst()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; }
        public string ExecuteCycle { get; set; }// 执行周期
        public string Remark { get; set; }// 其他说明
    }
}