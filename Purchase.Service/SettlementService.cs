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
    public class SettlementService
    {
        PurchaseEntities db = new PurchaseEntities();
        #region 结算单管理
        /// <summary>
        /// 查询供应商结算单结算的概况
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="supplierId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SettlementMstDto> SettlementMstSearch(string projectId, string supplierId, string userId,string delChk)
        {
            List<SettlementMstDto> list = new List<SettlementMstDto>();
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                       new SqlParameter("@SupplierId",supplierId),
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(SettlementMstDto);
            
            list =  db.Database.SqlQuery(t, "EXEC up_SettlementMst_R @ProjectId,@SupplierId,@UserId", para).Cast<SettlementMstDto>().ToList();

            if (string.IsNullOrEmpty(delChk))
            {
                return list;
            }
            else if (delChk == "Y")
            {
                return list.Where(x => x.DelChk == true).ToList();
            }
            else if (delChk == "N")
            {
                return list.Where(x => x.DelChk == false).ToList();
            }
            else { return list; }
        }
        /// <summary>
        /// 供应商结算单Mst保存
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        public int SettlementMstSave(SettlementMstDto dto, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", dto.SettlementId.ToString()),
                                                       new SqlParameter("@ProjectId", dto.ProjectId.ToString()), 
                                                       new SqlParameter("@SupplierId", dto.SupplierId.ToString()),
                                                       new SqlParameter("@QuotationGroupId", dto.QuotationGroupId.ToString()),
                                                       new SqlParameter("@SettleChk",dto.SettleChk),
                                                       new SqlParameter("@UserId",userId)};
            Type t = typeof(int);
            //db.Database.ExecuteSqlCommand("EXEC up_SettlementMst_S @Id,@ProjectId,@SupplierId,@SettleChk,@UserId", para);
            int settlementId = db.Database.SqlQuery(t, "EXEC up_SettlementMst_S @Id,@ProjectId,@SupplierId,@QuotationGroupId,@SettleChk,@UserId", para).Cast<int>().First();

            return settlementId;

        }
        public void SettlementMstUpdate(string settlementId, bool settlementChk, bool delChk, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",settlementId),
                                                       new SqlParameter("@SettleChk",settlementChk),
                                                       new SqlParameter("@DelChk",delChk),
                                                       new SqlParameter("@UserId",userId)};
            db.Database.ExecuteSqlCommand("EXEC up_SettlementMst_U @Id,@SettleChk,@DelChk,@UserId", para);

        }
        public void SettlementMstDelete(string settlementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SettlementId", settlementId) };
            db.Database.ExecuteSqlCommand("EXEC up_SettlementDtl_D @SettlementId", para);

        }
        #endregion
        #region MyRegion
        public List<SettlementDtlDto> SettlementDtlSearch_Before(string projectId, string supplierId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                       new SqlParameter("@SupplierId",supplierId),
                                                       new SqlParameter("@QuotationGroupId",quotationGroupId),
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(SettlementDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_SettlementDtl_Before_R @ProjectId,@SupplierId,@QuotationGroupId,@UserId", para).Cast<SettlementDtlDto>().ToList();
        }
        public List<SettlementDtlDto> SettlementDtlSearch_After(string settlementId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SettlementId", settlementId), 
                                                       //new SqlParameter("@SupplierId",supplierId),
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(SettlementDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_SettlementDtl_After_R @SettlementId,@UserId", para).Cast<SettlementDtlDto>().ToList();
        }
        #region 结算单手动填写追加


        public List<SettlementDtlDto> SettlementDtlSearch_After_shoudong(string settlementId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SettlementId", settlementId), 
                                                       //new SqlParameter("@SupplierId",supplierId),
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(SettlementDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_SettlementDtl_shoudong_R @SettlementId,@UserId", para).Cast<SettlementDtlDto>().ToList();
        }
        public void SettlementDtlSave_shoudong(SettlementDtlDto dto, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SettlementId",dto.SettlementId),
                                                       new SqlParameter("@SettlementSeqNO",dto.settlementSeqNO),
                                                       new SqlParameter("@QuotationId",dto.Quotationid),
                                                       new SqlParameter("@QuotationSeqNO",dto.SeqNO),
                                                       new SqlParameter("@FeeContent",dto.FeeContent),
                                                       new SqlParameter("@SettleUnitPrice",dto.danjia),
                                                       new SqlParameter("@SettleCount",dto.SettleCount.ToString()),
                                                       new SqlParameter("@SettleAmt",dto.SettleAmt.ToString()),
                                                       new SqlParameter("@Remark",dto.Remark),
                                                       new SqlParameter("@SettlementType",dto.SettlementType),
                                                       new SqlParameter("@UserId",userId)
            };
            db.Database.ExecuteSqlCommand("EXEC up_SettlementDtl_shoudong_S @SettlementId,@SettlementSeqNO,@QuotationId,@QuotationSeqNO,@FeeContent,@SettleUnitPrice,@SettleCount,@SettleAmt,@Remark,@SettlementType,@UserId", para);
        }
        #endregion
        public List<SettlementDtlDto> SettlementDtlSearch_zhuijia(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId) };
            Type t = typeof(SettlementDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_SettlementDtl_zhuijia_R @ProjectId", para).Cast<SettlementDtlDto>().ToList();
        }
        public List<SettlementMstDto> SettleDtlMainSearch(string projectId, string supplierId, string settlementId,string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                       new SqlParameter("@SupplierId",supplierId),
                                                       new SqlParameter("@SettlementId",settlementId),
                                                       new SqlParameter("@QuotationGroupId",quotationGroupId),
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(SettlementMstDto);
            return db.Database.SqlQuery(t, "EXEC up_SettlementDtl_ProjectAndSupplier_R @ProjectId,@SupplierId,@SettlementId,@QuotationGroupId,@UserId", para).Cast<SettlementMstDto>().ToList();
        }
        public string ProjectSumAmtSearch(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId) };
            Type t = typeof(string);
            string yusuanSumAmt = db.Database.SqlQuery(t, "EXEC up_Quotation_YusuanSum_R @ProjectId", para).Cast<string>().First();

            return yusuanSumAmt;
        }
        public void SettlementDtlSave(SettlementDtlDto dto, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SettlementId",dto.SettlementId),
                                                       new SqlParameter("@SettlementSeqNO",dto.settlementSeqNO),
                                                       new SqlParameter("@QuotationId",dto.Quotationid),
                                                       new SqlParameter("@QuotationSeqNO",dto.SeqNO),
                                                       new SqlParameter("@SettleUnitPrice",dto.danjia),
                                                       new SqlParameter("@SettleCount",dto.SettleCount.ToString()),
                                                       new SqlParameter("@SettleAmt",dto.SettleAmt.ToString()),
                                                       new SqlParameter("@Remark",dto.Remark),
                                                       new SqlParameter("@SettlementType",dto.SettlementType),
                                                       new SqlParameter("@UserId",userId)
            };
            db.Database.ExecuteSqlCommand("EXEC up_SettlementDtl_S @SettlementId,@SettlementSeqNO,@QuotationId,@QuotationSeqNO,@SettleUnitPrice,@SettleCount,@SettleAmt,@Remark,@SettlementType,@UserId", para);
        }
        #endregion
        #region 结算单组
        public List<SettlementGroupDto> SettlementGroupSearch(string projectId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(SettlementGroupDto);
            return db.Database.SqlQuery(t, "EXEC up_Settlement_Group_R @ProjectId,@UserId", para).Cast<SettlementGroupDto>().ToList();
        }
        public void SettlementGroupSave(SettlementGroupDto dto, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",dto.GroupId),
                                                       new SqlParameter("@ProjectId",dto.ProjectId),
                                                       new SqlParameter("@GroupName",dto.GroupName),
                                                       new SqlParameter("@UserId",userId)};

            db.Database.ExecuteSqlCommand("EXEC up_Settlement_Group_S @Id,@ProjectId,@GroupName,@UserId", para);
        }
        public List<SettlementGroupDtlDto> SettlementMstSearchByGroupId(string projectId, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                       new SqlParameter("@GroupId",groupId)};
            Type t = typeof(SettlementGroupDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_Settlement_SettlementMstByGroupId_R @ProjectId,@GroupId", para).Cast<SettlementGroupDtlDto>().ToList();
        }
        public void SettlementGroupDtlSave(SettlementGroupDtlDto dto, string userId)
        {
            if (dto.SeqNO == null)
                dto.SeqNO = "";
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SettlementGroupId",dto.GroupId),
                                                       new SqlParameter("@SeqNO",dto.SeqNO),
                                                       new SqlParameter("@SettlementId",dto.SettlementId),
                                                       new SqlParameter("@UserId",userId)};

            db.Database.ExecuteSqlCommand("EXEC up_Settlement_SettlementGroupDtl_S @SettlementGroupId,@SeqNO,@SettlementId,@UserId", para);
        }
        public void SettlementGroupDtlDelete(string settlementGroupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SettlementGroupId", settlementGroupId) };

            db.Database.ExecuteSqlCommand("EXEC up_Settlement_SettlementGroupDtl_D @SettlementGroupId", para);
        }
        #region 结算单组删除
        public void SettlementGroupDelete(string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@GroupId", groupId.ToString()) };

            db.Database.ExecuteSqlCommand("EXEC up_Settlement_Group_D @GroupId", para);
        }
        #endregion
        #endregion
        public List<SettlementBugetAmtDto> SettlementBugetAmtSearch(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId)};
            Type t = typeof(SettlementBugetAmtDto);
            return db.Database.SqlQuery(t, "EXEC up_Settlement_SettlementAmtByProjects_R @ProjectId", para).Cast<SettlementBugetAmtDto>().ToList();
        }
        #region 结算单金额分配流转单
        public List<FlowOrderDto> SettlementFlowOrderSearch(string projectId,string supplierId,string settlementId,bool settleChk, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                       new SqlParameter("@SupplierId", supplierId),
                                                       new SqlParameter("@SettlementId", settlementId),
                                                       new SqlParameter("@SettleChk", settleChk),
                                                       new SqlParameter("@UserId", userId)};
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_FlowOrder_R_Settlement @ProjectId,@SupplierId,@SettlementId,@SettleChk,@UserId", para).Cast<FlowOrderDto>().ToList();
        }
        public void SettlementFlowOrderUpdate(string flowOrderId, string factPaymentDateTime, string facPaymentAmt, string settlementId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",flowOrderId),
                                                       new SqlParameter("@FactPaymentTime",factPaymentDateTime),
                                                       new SqlParameter("@FactPaymentAmt",facPaymentAmt),
                                                       new SqlParameter("@SettlementId",settlementId),
                                                       new SqlParameter("@UserId",userId)};

            db.Database.ExecuteSqlCommand("EXEC up_FlowOrder_U_Settlement @Id,@FactPaymentTime,@FactPaymentAmt,@SettlementId,@UserId", para);
        }
        #endregion
    }
}
