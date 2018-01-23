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
    public class ApplyService
    {
        PurchaseEntities db = new PurchaseEntities();
        #region 提交申请

        /// <summary>
        /// 进行申请时调用，申请时同时往状态表插入申请状态
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        public int ApplyCommit(Apply apply, string nextRecheckUserId)
        {
            // 申请信息
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", apply.Id.ToString()), 
                                                       new SqlParameter("@ApplyReason",apply.ApplyReason==null?"":apply.ApplyReason),
                                                       new SqlParameter("@ApplyTypeId", apply.ApplyTypeId==null?"":apply.ApplyTypeId),
                                                        new SqlParameter("@ProjectId",apply.ProjectId),
                                                       new SqlParameter("@ApplyUserId",apply.ApplyUserId==null?"":apply.ApplyUserId)};
            Type t = typeof(int);
            int applyId = db.Database.SqlQuery(t, "EXEC up_Apply_Apply_S @ApplyId,@ApplyReason,@ApplyTypeId,@ProjectId,@ApplyUserId", para).Cast<int>().First();

            // 申请状态信息
            ApplyRecheckStatus status = new ApplyRecheckStatus();
            if (apply.ApplyReason == null)
                apply.ApplyReason = "";
            status.ApplyId = applyId;
            status.RecheckUserId = apply.ApplyUserId;
            status.RecheckStatusCode = "申请";
            status.RecheckReason = apply.ApplyReason;
            status.InDateTime = DateTime.Now;
            ApplyRecheckStatusUpdate(status, nextRecheckUserId);

            return applyId;
        }
        /// <summary>
        /// 申请时保存详细的信息时调用，主要针对供应商，其他的申请参考ProjectId即可
        /// </summary>
        /// <param name="status"></param>
        public void ApplyDtlSave(ApplyDtl applyDtl)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyDtl.ApplyId.ToString()), 
                                                       new SqlParameter("@ApplyTypeId", applyDtl.ApplyTypeId),
                                                       new SqlParameter("@ApplyContentId",applyDtl.ApplyContentId.ToString()),
                                                        new SqlParameter("@UserId",applyDtl.InUserId)};

            db.Database.ExecuteSqlCommand("EXEC up_Apply_ApplyDtl_S @ApplyId,@ApplyTypeId,@ApplyContentId,@UserId", para);
        }
        public void ApplyDtlSave_Settlement(ApplyDtl applyDtl)
        {

            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyDtl.ApplyId.ToString()), 
                                                       new SqlParameter("@ApplyTypeId", applyDtl.ApplyTypeId),
                                                       new SqlParameter("@ApplyContentId",applyDtl.ApplyContentId.ToString()),
                                                       new SqlParameter("@UserId",applyDtl.InUserId)};

            db.Database.ExecuteSqlCommand("EXEC up_Apply_ApplyDtl_Settlement_S @ApplyId,@ApplyTypeId,@ApplyContentId,@UserId", para);
        }
        public void ApplyDtlSave_SupplierMng(ApplyDtl applyDtl)
        {

            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyDtl.ApplyId.ToString()), 
                                                       new SqlParameter("@ApplyTypeId", applyDtl.ApplyTypeId),
                                                       new SqlParameter("@ApplyContentId",applyDtl.ApplyContentId.ToString()),
                                                       new SqlParameter("@UserId",applyDtl.InUserId)};

            db.Database.ExecuteSqlCommand("EXEC up_Apply_ApplyDtl_SupplierMng_S @ApplyId,@ApplyTypeId,@ApplyContentId,@UserId", para);
        }
        public void ApplyDtlSave_FlowOrder(ApplyDtl applyDtl)
        {

            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyDtl.ApplyId.ToString()), 
                                                       new SqlParameter("@ApplyTypeId", applyDtl.ApplyTypeId),
                                                       new SqlParameter("@ApplyContentId",applyDtl.ApplyContentId.ToString()),
                                                       new SqlParameter("@UserId",applyDtl.InUserId)};

            db.Database.ExecuteSqlCommand("EXEC up_Apply_ApplyDtl_FlowOrder_S @ApplyId,@ApplyTypeId,@ApplyContentId,@UserId", para);
        }
        public void ApplyDtlDelete(ApplyDtl applyDtl)
        {
            if (applyDtl.ApplyTypeId == null)
            {
                applyDtl.ApplyTypeId = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyDtl.ApplyId.ToString()),
                                                        new SqlParameter("@ApplyTypeId", applyDtl.ApplyTypeId.ToString())};

            db.Database.ExecuteSqlCommand("EXEC up_Apply_ApplyDtl_D @ApplyId,@ApplyTypeId", para);
        }
        #endregion
        #region 待我审核查询
        /// <summary>
        /// 我审核的信息查询
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="applyUserId"></param>
        /// <param name="applyTypeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="recheckStatusCode"></param>
        /// <param name="recheckUserId"></param>
        /// <returns></returns>
        public List<MyApplyDto> MyRecheckSearch(string applyId, string applyUserId, string applyTypeId, DateTime startDate, DateTime endDate, string recheckStatusCode, string recheckUserId
            , string projectName, string projectShortName, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId), 
                                                       new SqlParameter("@ApplyUserId",applyUserId),
                                                       new SqlParameter("@ApplyTypeId", applyTypeId),
                                                       new SqlParameter("@StartDate",startDate),
                                                       new SqlParameter("@EndDate", endDate),
                                                        new SqlParameter("@RecheckStatusCode", recheckStatusCode),
                                                        new SqlParameter("@RecheckUserId", recheckUserId),
                                                        new SqlParameter("@ProjectName", projectName),
                                                        new SqlParameter("@ProjectShortName", projectShortName),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(MyApplyDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_MyRecheck_R @ApplyId,@ApplyUserId,@ApplyTypeId,@StartDate,@EndDate,@RecheckStatusCode,@RecheckUserId,@ProjectName,@ProjectShortName,@UserId", para).Cast<MyApplyDto>().ToList();
        }
        /// <summary>
        /// 待我审核查询
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="applyUserId"></param>
        /// <param name="applyTypeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<MyApplyDto> MyNeedRecheckSearch(string applyId, string applyUserId, string applyTypeId, DateTime startDate, DateTime endDate, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId), 
                                                       new SqlParameter("@ApplyUserId",applyUserId),
                                                       new SqlParameter("@ApplyTypeId", applyTypeId),
                                                       new SqlParameter("@StartDate",startDate),
                                                       new SqlParameter("@EndDate", endDate),
            new SqlParameter("@UserId", userId)};
            Type t = typeof(MyApplyDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_MyRecheck_Need_R @ApplyId,@ApplyUserId,@ApplyTypeId,@StartDate,@EndDate,@UserId", para).Cast<MyApplyDto>().ToList();
        }
        /// <summary>
        /// 在审核的过程中，当进行批准或者拒绝的时候调用
        /// 若是批准nextRecheckUserId为选择的下一个批准人
        /// 若是拒绝nextRecheckUserId 为上一个批准人，或者申请人
        /// 若是终止,或者是最后一个审核人时 nextRecheckUserId=""
        /// </summary>
        /// <param name="status"></param>
        public void ApplyRecheckStatusUpdate(ApplyRecheckStatus status, string nextRecheckUserId)
        {
            if (nextRecheckUserId == null)
                nextRecheckUserId = "";
            if (status.RecheckReason == null)
            {
                status.RecheckReason = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", status.ApplyId), 
                                                       new SqlParameter("@SeqNO", status.SeqNO),
                                                       new SqlParameter("@RecheckUserId", status.RecheckUserId),
                                                       new SqlParameter("@RecheckStatusCode",status.RecheckStatusCode),
                                                       new SqlParameter("@RecheckReason",status.RecheckReason),
                                                       new SqlParameter("@NextRecheckUserId",nextRecheckUserId)};

            db.Database.ExecuteSqlCommand("EXEC up_Apply_Apply_RecheckStatus_S @ApplyId,@SeqNO,@RecheckUserId,@RecheckStatusCode,@RecheckReason,@NextRecheckUserId", para);
        }
        /// <summary>
        /// 当审核状态为拒绝时,查询上一个批准人或者申请人
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="recheckUserId"></param>
        /// <returns></returns>
        public string PreRecheckUserSearch(string applyId, string recheckUserId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId",applyId), 
                                                       new SqlParameter("@RecheckUserId", recheckUserId)};
            Type t = typeof(ApplyRecheckStatus);
            return db.Database.SqlQuery(t, "EXEC up_Apply_Apply_PreRecheckUser_R @ApplyId,@RecheckUserId", para).Cast<ApplyRecheckStatus>().ToList()[0].RecheckUserId;
        }

        #endregion
        #region 我的申请查询

        /// <summary>
        /// 我的申请查询
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="applyUserId"></param>
        /// <param name="applyTypeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<MyApplyDto> MyApplySearch(string applyId, string applyUserId, string applyTypeId, DateTime startDate, DateTime endDate, string statusCode,string projectName,string projectShortName,string supplierName,string supplierShortName,string recheckStatusCode)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId), 
                                                       new SqlParameter("@ApplyUserId",applyUserId),
                                                       new SqlParameter("@ApplyTypeId", applyTypeId),
                                                       new SqlParameter("@StartDate",startDate),
                                                       new SqlParameter("@EndDate", endDate),
                                                       new SqlParameter("@StatusCode", statusCode),
                                                       new SqlParameter("@ProjectName", projectName),
                                                       new SqlParameter("@ProjectShortName", projectShortName),
                                                       new SqlParameter("@SupplierName", supplierName),
                                                       new SqlParameter("@SupplierShortName", supplierShortName),
                                                       new SqlParameter("@RecheckStatusCode", recheckStatusCode),
            };
            Type t = typeof(MyApplyDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_MyApply_R @ApplyId,@ApplyUserId,@ApplyTypeId,@StartDate,@EndDate,@StatusCode,@ProjectName,@ProjectShortName,@SupplierName,@SupplierShortName,@RecheckStatusCode", para).Cast<MyApplyDto>().ToList();
        }
        /// <summary>
        /// 通过申请Id查询审核的详细
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public List<RecheckStatusDtlDto> RecheckDtlSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(RecheckStatusDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_Apply_RecheckStatus_R @ApplyId", para).Cast<RecheckStatusDtlDto>().ToList();
        }
        #endregion
        #region 申请详细查询
        /// <summary>
        /// 申请的项目详细查询
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public List<ProjectDto> ApplyDtl_ProjectSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(ProjectDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Projects_R @ApplyId", para).Cast<ProjectDto>().ToList();
        }
        /// <summary>
        /// 申请的供应商的查询
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public List<SupplierDto> ApplyDtl_SupplierSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(SupplierDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Supplier_R @ApplyId", para).Cast<SupplierDto>().ToList();
        }
        public List<SupplierDto> ApplyDtl_SupplierByApplyIdAndSupplierId(string applyId, string supplierId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId), 
                                                         new SqlParameter("@SupplierId", supplierId) };
            Type t = typeof(SupplierDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_SupplierDtlByApplyIdAndSupplierId_R @ApplyId,@SupplierId", para).Cast<SupplierDto>().ToList();
        }
        public List<FlowOrderDto> ApplyDtl_FlowOrderByApplyIdAndFlowOrderId(string applyId, string flowOrderId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId), 
                                                         new SqlParameter("@FlowOrderId", flowOrderId) };
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_FlowOrderByApplyIdAndFlowOrderId_R @ApplyId,@FlowOrderId", para).Cast<FlowOrderDto>().ToList();
        }
        /// <summary>
        /// 申请详细的合同查询
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public List<ConstractDto> ApplyDtl_ConstractSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(ConstractDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Constract_R @ApplyId", para).Cast<ConstractDto>().ToList();
        }
        /// <summary>
        /// 查询当前申请下确认单类型，根据不同的类型分别来显示数据
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public List<QuotationMstDto> ApplyDtl_QuotationTypeSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(QuotationMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_QuotationType_R @ApplyId", para).Cast<QuotationMstDto>().ToList();
        }
        #region 申请根据不同的确认单类型显示不同的数据
        #region 确认单列表_执行
        public List<Quotation_ZhiXingDto> QuotationApplySearchZhixing(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };
            Type t = typeof(Quotation_ZhiXingDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Quotation_Zhixing_R @ApplyId", para).Cast<Quotation_ZhiXingDto>().ToList();
        }
        #endregion
        #region 确认单列表_编程
        public List<Quotation_BianChengDto> QuotationApplySearchBiancheng(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };
            Type t = typeof(Quotation_BianChengDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Quotation_Biancheng_R @ApplyId", para).Cast<Quotation_BianChengDto>().ToList();
        }
        #endregion
        #region 确认单列表_复核
        public List<Quotation_FuHeDto> QuotationApplySearchFuhe(string applyId)
        {

            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };
            Type t = typeof(Quotation_FuHeDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Quotation_Fuhe_R @ApplyId", para).Cast<Quotation_FuHeDto>().ToList();
        }
        #endregion
        #region 确认单列表_其他1
        public List<Quotation_QiTa1Dto> QuotationApplySearchQitao1(string applyId)
        {

            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };
            Type t = typeof(Quotation_QiTa1Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Apply_ApplyDtl_Quotation_Qita1_R] @ApplyId", para).Cast<Quotation_QiTa1Dto>().ToList();
        }
        #endregion
        #region 确认单列表_其他2
        public List<Quotation_QiTa2Dto> QuotationApplySearchQitao2(string applyId)
        {

            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };
            Type t = typeof(Quotation_QiTa2Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Apply_ApplyDtl_Quotation_Qita2_R] @ApplyId", para).Cast<Quotation_QiTa2Dto>().ToList();
        }
        #endregion
        #region 确认单列表_研究
        public List<Quotation_YanJiuDto> QuotationApplySearchYanjiu(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };

            Type t = typeof(Quotation_YanJiuDto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Apply_ApplyDtl_Quotation_Yanjiu_R] @ApplyId", para).Cast<Quotation_YanJiuDto>().ToList();
        }
        #endregion
        #region 确认单列表_支持
        public List<ZhiChi01Dto> QuotationApplySearchZhichi(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };
            Type t = typeof(ZhiChi01Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Apply_ApplyDtl_Quotation_Zhichi_R] @ApplyId", para).Cast<ZhiChi01Dto>().ToList();
        }
        #endregion
        #region 确认单列表_车展
        public List<Quotation_CheZhanDto> QuotationApplySearchChezhan(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId)
                                                      };
            Type t = typeof(Quotation_CheZhanDto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Apply_ApplyDtl_Quotation_Chezhan_R]  @ApplyId", para).Cast<Quotation_CheZhanDto>().ToList();
        }
        #endregion
        #region 更新选择的确认单
        public void QuotationSelectSave(string quotationId, string seqNO, string quotationType, bool selectChk)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@QuotationId", quotationId), 
                                                       new SqlParameter("@SeqNO", seqNO),
                                                       new SqlParameter("@QuotationType",quotationType),
                                                        new SqlParameter("@SelectChk",selectChk)};

            db.Database.ExecuteSqlCommand("EXEC up_Apply_ApplyDtl_Quotation_S @QuotationId,@SeqNO,@QuotationType,@SelectChk", para);
        }
        #endregion
        #endregion
        /// <summary>
        /// 申请详细的确认单的查询
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public List<QuotationMstDto> ApplyDtl_QuotationSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(QuotationMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Quotation_R @ApplyId", para).Cast<QuotationMstDto>().ToList();
        }
        /// <summary>
        /// 申请详细的需求书查询  
        /// </summary>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public List<RequiremetMstDto> ApplyDtl_RequirementSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(RequiremetMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Requirement_R @ApplyId", para).Cast<RequiremetMstDto>().ToList();
        }
        #region 结算单

        public List<SettlementBugetAmtDto> ApplyDtl_SettlementBugetAmtSearch(string projectId, string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(SettlementBugetAmtDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_SettlementAmtByProjects_R1 @ApplyId,@ProjectId", para).Cast<SettlementBugetAmtDto>().ToList();
        }
        public List<SettlementMstDto> ApplyDtl_SettlementSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(SettlementMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_Settlement_R @ApplyId", para).Cast<SettlementMstDto>().ToList();
        }
        public List<SettlementDtlDto> ApplyDtl_SettlementDtlByApplyIdAndSettlementIdSearch(string applyid,string settlementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyid),
                                                    new SqlParameter("@SettlementId", settlementId) };
            Type t = typeof(SettlementDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_SettlementDtlByApplyIdAndSettlementId_R @ApplyId,@SettlementId", para).Cast<SettlementDtlDto>().ToList();
        }
        #region 结算单手动填写追加
        public List<SettlementDtlDto> ApplyDtl_SettlementDtlByApplyIdAndSettlementIdSearch_shoudong(string applyid, string settlementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyid),
                                                    new SqlParameter("@SettlementId", settlementId) };
            Type t = typeof(SettlementDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_SettlementDtlByApplyIdAndSettlementId_shoudong_R @ApplyId,@SettlementId", para).Cast<SettlementDtlDto>().ToList();
        }
        #endregion
       
        public List<SettlementDtlDto> ApplyDtl_SettlementDtlZhuijiaByApplyIdSearch(string applyid)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyid), };
            Type t = typeof(SettlementDtlDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_SettlementDtl_zhuijia @ApplyId", para).Cast<SettlementDtlDto>().ToList();
        }
        #endregion
        public List<FlowOrderDto> ApplyDtl_FlowOrderSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_FlowOrder_R @ApplyId", para).Cast<FlowOrderDto>().ToList();
        }
        public List<FlowOrderBugetDto> ApplyDtl_FlowOrderBugetSearch(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(FlowOrderBugetDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyDtl_FlowOrder_ContingencyAndBugetSumAndFactSum_R @ApplyId", para).Cast<FlowOrderBugetDto>().ToList();
        }
        #endregion
        #region 审核人查询
        public List<RecheckUserSelectDto> RecheckUserSelect(string recheckType, string applyUserId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RecheckType", recheckType),
                                                         new SqlParameter("@ApplyUserId", applyUserId)};
            Type t = typeof(RecheckUserSelectDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_Apply_RecheckUserSelect_R @RecheckType,@ApplyUserId", para).Cast<RecheckUserSelectDto>().ToList();
        }
        #endregion
        #region 判断提交人是否在审核流程中
        public string RecheckProcessExistChk(string recheckType, string applyUserId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RecheckType", recheckType), 
                                                       new SqlParameter("@ApplyUserId",applyUserId)};
            Type t = typeof(string);
            return db.Database.SqlQuery(t, "EXEC up_RecheckExistChk @RecheckType,@ApplyUserId", para).Cast<string>().First();
        }
        #endregion
        #region 根据申请Id查询申请信息
        public List<MyApplyDto> ApplySearchById(string applyId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ApplyId", applyId) };
            Type t = typeof(MyApplyDto);
            return db.Database.SqlQuery(t, "EXEC up_Apply_ApplyInfoByApplyId_R @ApplyId", para).Cast<MyApplyDto>().ToList();
        }
        #endregion
    }
}
