using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 配额要求
    /// </summary>
    public class SecretInquiriesDtl4
    {
        public SecretInquiriesDtl4()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 
        public string ExecuteType { get; set; }//执行分类
        public string CustomerType { get; set; }//客户分类
        public string BrandType { get; set; }// 品牌分类
        public string IntoShopType { get; set; }// 进店方式
        public string CarChk { get; set; }// 开车进店
        public string IntoShopTypeRate { get; set; }// 进店方式比例
        public string CarType { get; set; }// 车辆类别
        public string CarPriceRange { get; set; }// 车价范围
        public string CarLevel { get; set; }// 车型级别
        public string SampleCount { get; set; }// 样本量
        public string Compensation { get; set; }// 报酬金额
        public string RemarkDtl4 { get; set; }//其他说明

        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}