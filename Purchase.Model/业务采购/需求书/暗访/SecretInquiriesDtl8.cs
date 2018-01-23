using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 其他要求
    /// </summary>
    public class SecretInquiriesDtl9
    {
        public SecretInquiriesDtl9()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 
        public string OtherRemark { get; set; }//其他要求


        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}