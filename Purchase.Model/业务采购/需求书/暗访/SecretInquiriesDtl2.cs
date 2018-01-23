using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Purchase.Model
{
    /// <summary>
    /// 执行要求
    /// </summary>
    public class SecretInquiriesDtl2
    {
        public SecretInquiriesDtl2()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Id { get; set; } 
        public string AccessTime { get; set; }//访问时长
        public string SoundRecordChk { get; set; }//录音与否
        public string Videotape { get; set; }//录像与否
        public string TrainingMode { get; set; }// 培训方式
        public string Appointment { get; set; }//进店前预约
        public string avoid { get; set; }//进店规避要求
        public string IntoShopAgain { get; set; }//二次进店
        public string IntoShopAgainRate { get; set; }//二次进店比例
        public string GPSLocationOrSMS { get; set; }//GPS定位/短信定位
        public string MobileAnswer { get; set; }//手机答卷
        public string TimeMothod { get; set; }//报时方式
        public string TelReturnVisit { get; set; }//电话回访
        public string TelReturnVisitNumberRequirement { get; set; }//电话回访号码要求
        public string OwnerreturenVisit { get; set; }//车主回访
        public string FocusAnswer { get; set; }//集中接听
        public string CustomerServiceLine { get; set; }//客服专线接听
        public string RemarkDtl2 { get; set; }//其他说明

        public SecretInquiriesMst SecretInquiriesMst { get; set; }
        public int SecretInquiriesMstId { get; set; }
    }
}