using Purchase.DAL;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service
{
    public class FlowOrderService
    {
        PurchaseEntities db = new PurchaseEntities();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="supplierId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<FlowOrderDto> FlowOrderSearch(string projectId, string supplierId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                       new SqlParameter("@SupplierId",supplierId),
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_FlowOrder_R @ProjectId,@SupplierId,@UserId", para).Cast<FlowOrderDto>().ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectShortName"></param>
        /// <param name="supplierName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<FlowOrderDto> FlowOrderSearch1(string projectName, string projectShortName, string supplierName,string serviceTrade,string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectName", projectName), 
                                                       new SqlParameter("@ProjectShortName",projectShortName),
                                                       new SqlParameter("@SupplierName",supplierName),
                                                       new SqlParameter("@UserId", userId),
                                                      new SqlParameter("@ServiceTrade", serviceTrade)};
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_FlowOrder_R1 @ProjectName,@ProjectShortName,@SupplierName,@UserId,@ServiceTrade", para).Cast<FlowOrderDto>().ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowOrderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<FlowOrderDto> FlowOrderSearchById(string flowOrderId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", flowOrderId), 
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_FlowOrderById_R @Id,@UserId", para).Cast<FlowOrderDto>().ToList();
        }
        public void FlowOrderSave(FlowOrderDto dto, string userId)
        {
            string EstimatedPaymentTime1 = "";
            string FactPaymentTime1 = "";
            if (dto.Remark == null)
                dto.Remark = "";
            if (dto.InvoiceAmt == null)
                dto.InvoiceAmt = "";
            if (dto.EstimatePaymentAmt == null)
                dto.EstimatePaymentAmt = "";
            if (dto.FactPaymentAmt == null)
                dto.FactPaymentAmt = "";
            if (dto.EstimatedPaymentTime != null)
                EstimatedPaymentTime1 = dto.EstimatedPaymentTime.ToString();
            if (dto.FactPaymentTime != null)
                FactPaymentTime1 = dto.FactPaymentTime.ToString();
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", dto.FlowOrderId.ToString()),
                                                       new SqlParameter("@ProjectId", dto.ProjectId.ToString()), 
                                                       new SqlParameter("@SupplierId", dto.SupplierId.ToString()),
                                                       new SqlParameter("@ExecuteCycleStartDate",dto.ExecuteCycleStartDate),
                                                       new SqlParameter("@ExecuteCycleEndDate",dto.ExecuteCycleEndDate),
                                                       new SqlParameter("@ExpendPattern",dto.ExpendPattern),
                                                       new SqlParameter("@ExpendType",dto.ExpendType),
                                                       new SqlParameter("@PaymentType",dto.PaymentType),
                                                       new SqlParameter("@PaymentCompany",dto.PaymentCompany),
                                                       new SqlParameter("@EstimatedPaymentTime",EstimatedPaymentTime1),
                                                       new SqlParameter("@EstimatedPaymentAmt",dto.EstimatePaymentAmt.ToString()),
                                                       new SqlParameter("@FactPaymentTime",FactPaymentTime1),
                                                       new SqlParameter("@FactPaymentAmt",dto.FactPaymentAmt.ToString()),
                                                       new SqlParameter("@InvoceAmt",dto.InvoiceAmt.ToString()),
                                                       new SqlParameter("@Payee",dto.Payee),
                                                       new SqlParameter("@Remark",dto.Remark),
                                                       new SqlParameter("@UserId",userId)};

            db.Database.ExecuteSqlCommand("EXEC up_FlowOrder_S @Id,@ProjectId,@SupplierId,@ExecuteCycleStartDate,@ExecuteCycleEndDate,@ExpendPattern,@ExpendType,@PaymentType,@PaymentCompany,@EstimatedPaymentTime,@EstimatedPaymentAmt,@FactPaymentTime,@FactPaymentAmt,@InvoceAmt,@Payee,@Remark,@UserId", para);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="projectShortName"></param>
        /// <param name="supplierName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startAmt"></param>
        /// <param name="endAmt"></param>
        /// <returns></returns>
        public List<FlowOrderDto> PaymentListSearch(string serviceTrade,string projectName, string projectShortName, string supplierName,DateTime? startDate,DateTime? endDate,decimal? startAmt,decimal? endAmt,string paymentStatus)
        {
            if (paymentStatus == null)
            {
                paymentStatus = "";
            }
            if (startAmt == null)
            {
                startAmt = 0;
            }
            if (endAmt == null)
            {
                endAmt = 999999999;
            }
            if (startDate == null)
            {
                startDate = new DateTime(1900,1,1);
            }
            if (endDate == null)
            {
                endDate = new DateTime(2080, 12, 1);
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@ProjectName", projectName), 
                                                       new SqlParameter("@ProjectShortName",projectShortName),
                                                       new SqlParameter("@SupplierName",supplierName),
                                                       new SqlParameter("@StartDate",startDate),
                                                        new SqlParameter("@EndDate",endDate),
                                                        new SqlParameter("@StartAmt",startAmt),
                                                        new SqlParameter("@EndAmt",endAmt),
                                                          new SqlParameter("@PaymentStatus",paymentStatus)
            };
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_FlowOrder_Payment_R @ServiceTrade,@ProjectName,@ProjectShortName,@SupplierName,@StartDate,@EndDate,@StartAmt,@EndAmt,@PaymentStatus", para).Cast<FlowOrderDto>().ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        public void PaymentListUpdate(FlowOrderDto dto, string userId)
        {

            if (dto.PreTaxAmt == null)
            {
                dto.PreTaxAmt = "";
            }
            if (dto.InvoceAmtThis == null)
            {
                dto.InvoceAmtThis = "";
            }
            if (dto.Finance_PaymentAmt == null)
            {
                dto.Finance_PaymentAmt = "";
            }
            if (dto.Finance_InvoceAmt == null)
            {
                dto.Finance_InvoceAmt = "";
            }
            if (dto.Finance_InvoceAmtThis == null)
            {
                dto.Finance_InvoceAmtThis = "";
            }
            if (dto.ProjectType == null)
            {
                dto.ProjectType = "";
            }
            if (dto.ConstractChk == null)
            {
                dto.ConstractChk = "";
            }
            if (dto.SettlementChk == null)
            {
                dto.SettlementChk = "";
            }
            if (dto.InvoceChk == null)
            {
                dto.InvoceChk = "";
            }
            if (dto.PaperChk == null)
            {
                dto.PaperChk = "";
            }
            if (dto.PaymentRemark == null)
            {
                dto.PaymentRemark = "";
            }
            if (dto.Finance_PaymentStatus == null)
            {
                dto.Finance_PaymentStatus = "";
            }
            if (dto.Finance_NotPayReason == null)
            {
                dto.Finance_NotPayReason = "";
            }
            if (dto.Finance_Constract == null)
            {
                dto.Finance_Constract = "";
            }
            if (dto.Finance_SettlementChk == null)
            {
                dto.Finance_SettlementChk = "";
            }
            if (dto.InvoiceNO == null)
            {
                dto.InvoiceNO = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", dto.FlowOrderId),
                                                        new SqlParameter("@PreTaxAmt", dto.PreTaxAmt), 
                                                       new SqlParameter("@InvoceAmtThis", dto.InvoceAmtThis),
                                                       new SqlParameter("@ProjectType",dto.ProjectType),
                                                       new SqlParameter("@ConstractChk",dto.ConstractChk),
                                                       new SqlParameter("@SettlementChk",dto.SettlementChk),
                                                       new SqlParameter("@InvoceChk",dto.InvoceChk),
                                                       new SqlParameter("@PaperChk",dto.PaperChk),
                                                       new SqlParameter("@InvoiceNO",dto.InvoiceNO),
                                                       new SqlParameter("@PaymentRemark",dto.PaymentRemark),
                                                       new SqlParameter("@Finance_PaymentStatus",dto.Finance_PaymentStatus),
                                                       new SqlParameter("@Finance_PaymentAmt",dto.Finance_PaymentAmt),
                                                       new SqlParameter("@Finance_NotPayReason",dto.Finance_NotPayReason),
                                                       new SqlParameter("@Finance_Constract",dto.Finance_Constract),
                                                       new SqlParameter("@Finance_SettlementChk",dto.Finance_SettlementChk),
                                                       new SqlParameter("@Finance_InvoceAmt",dto.Finance_InvoceAmt),
                                                       new SqlParameter("@Finance_InvoceAmtThis",dto.Finance_InvoceAmtThis),
                                                       new SqlParameter("@UserId",userId)};

            db.Database.ExecuteSqlCommand("EXEC up_FlowOrder_U @Id,@PreTaxAmt,@InvoceAmtThis,@ProjectType,@ConstractChk,@SettlementChk,@InvoceChk,@PaperChk,@InvoiceNO,@PaymentRemark,@Finance_PaymentStatus,@Finance_PaymentAmt,@Finance_NotPayReason,@Finance_Constract,@Finance_SettlementChk,@Finance_InvoceAmt,@Finance_InvoceAmtThis,@UserId", para);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        public void ApplyPayUpdate(string Id, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", Id),
                                                       new SqlParameter("@UserId",userId)};

            db.Database.ExecuteSqlCommand("EXEC up_FlowOrder_U1 @Id,@UserId", para);
        }
        public void InvoiceAmtUpdate(string Id, string invoiceAmt)
        {
            if (invoiceAmt == null)
            {
                invoiceAmt = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", Id),
                                                       new SqlParameter("@InvoiceAmt",invoiceAmt)};

            db.Database.ExecuteSqlCommand("EXEC up_FlowOrder_U2 @Id,@InvoiceAmt", para);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<FlowOrderBugetDto> FlowOrderBugetSearch(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId)};
            Type t = typeof(FlowOrderBugetDto);
            return db.Database.SqlQuery(t, "EXEC up_FlowOrder_ContingencyAndBugetSumAndFactSum_R @ProjectId", para).Cast<FlowOrderBugetDto>().ToList();
        }
    }
}
