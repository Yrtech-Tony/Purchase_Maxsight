using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 样本定义
    /// </summary>
    public class SecretInquiriesDtl1
    {
        public SecretInquiriesDtl1()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 
        public string AgeGroup { get; set; }//年龄段
        public string EducationGroup { get; set; }//学历区间
        public string DrivingLicense { get; set; }//驾照与否
        public string RemarkDtl1 { get; set; }//其他说明


        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}