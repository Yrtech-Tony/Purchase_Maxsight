using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 人员要求
    /// </summary>
    public class SecretInquiriesDtl7
    {
        public SecretInquiriesDtl7()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 

        /* 评估员 */
        public string Evaluator_Age { get; set; }//年龄
        public string Evaluator_Education { get; set; }//学历
        public string Evaluator_Experience { get; set; }//执行经验
        public string Evaluator_DriveChk { get; set; }//驾照
        public string Evaluator_FactDriveAge { get; set; }//实际驾龄
        public string Evaluator_StaffRate { get; set; }//人员备份比例

        /* 技师 */
        public string Tech_Age { get; set; }//年龄
        public string Tech_Education { get; set; }//学历
        public string Tech_Experience { get; set; }//执行经验
        public string Tech_DriveChk { get; set; }//驾照
        public string Tech_FactDriveAge { get; set; }//实际驾龄
        public string Tech_StaffRate { get; set; }//人员备份比例
        public string RemarkDtl7 { get; set; }//其他说明

        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}