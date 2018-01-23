using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    public class ConstractDto
    {
        public string ApplyId { get; set; }
        public string SeqNO { get; set; }
        public string ApplyStatusCode { get; set; }
        public string ConstractId { get; set; }
        public string ConstractCode { get; set; }
        public string ConstractName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName_Fix { get; set; }
        public string ProjectShortName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }
        public string SupplierShortName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? EmailSendDateTime { get; set; }
        public string AttachmentFileName { get; set; }
        public string InUserId { get; set; }
        public DateTime? InDateTime { get; set; }

        public int? TemplateId { get; set; }
        public string TemplateName { get; set; }

        public string PartAName { get; set; }
        public string PartAAddress { get; set; }
        public string PartAContacts { get; set; }// 联系人
        public string PartATel { get; set; }
        public string PartAEmail { get; set; }
        public string PartAPostCode { get; set; }
        public string PartBName { get; set; }
        public string PartBAddress { get; set; }
        public string PartBContacts { get; set; }// 联系人
        public string PartBTel { get; set; }
        public string PartBEmail { get; set; }
        public string PartBPostCode { get; set; }
        public string ExecuteRegion { get; set; }
        public string RecheckRate { get; set; }
        public string SampleCountAndQuota { get; set; }
        public string CostDesc { get; set; }
        public string PlatformName { get; set; }
        public DateTime? LastFinishDate { get; set; }
        public string StrLastFinishDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string StrDeliveryDate { get; set; }
        public string buzhanshijian { get; set; }
        public string chezhanshijian { get; set; }
        public string yunshuluxian { get; set; }
        public string fuwuneirong { get; set; }
        public string xieyiyouxiaoshijian { get; set; }
        public string zhanshizhanlanweituoneirong { get; set; }
        public string yunshucheliangxinxi { get; set; }
        public string QuotationIdList { get; set; }
        public string QuotationRemark { get; set; }
        public string QuotationId2List { get; set; }
        public string Quotation2Remark { get; set; }
        public bool UserChk { get; set; }
        public string BussinessMainEmail { get; set; }

        public string BussinessSecondEmail { get; set; }
    }
}
