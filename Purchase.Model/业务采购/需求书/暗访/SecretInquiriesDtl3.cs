using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 邀约要求
    /// </summary>
    public class SecretInquiriesDtl3
    {
        public SecretInquiriesDtl3()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 
        public string ExecuteType { get; set; }//执行分类
        public string CustomerType { get; set; }//客户分类
        public string ListSource { get; set; }//名单来源
        public string Approaches { get; set; }//邀约方式
        public string RemarkDtl3 { get; set; }//其他说明

        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}