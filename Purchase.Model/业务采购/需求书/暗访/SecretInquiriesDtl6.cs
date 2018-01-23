using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 资料提交
    /// </summary>
    public class SecretInquiriesDtl6
    {
        public SecretInquiriesDtl6()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 

        public string FirstCommit_Time { get; set; }//时间要求
        public string FirstCommit_CommitFile { get; set; }//提交资料
        public string FirstCommit_Type { get; set; }//提交方式

        public string LastCommit_Time { get; set; }//时间要求
        public string LastCommit_CommitFile { get; set; }//提交资料
        public string LastCommit_Type { get; set; }//提交方式
        public string RemarkDtl6 { get; set; }//其他说明

        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}