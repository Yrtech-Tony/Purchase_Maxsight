using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 设备要求
    /// </summary>
    public class SecretInquiriesDtl5
    {
        public SecretInquiriesDtl5()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 

        public string CandidDevice { get; set; }//偷拍设备
        public string AssistantDevice { get; set; }//辅助设备
        public string RemarkDtl5 { get; set; }//其他说明


        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}