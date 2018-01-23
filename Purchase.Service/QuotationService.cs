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

    public class QuotationService
    {
        PurchaseEntities db = new PurchaseEntities();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="projectCode"></param>
        /// <param name="projectName"></param>
        /// <param name="projectShortName"></param>
        /// <param name="supplierCode"></param>
        /// <param name="supplierName"></param>
        /// <param name="supplierShortName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<QuotationTypeDto> QuotationTypeByProject(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId)
                                                      };
            Type t = typeof(QuotationTypeDto);
            return db.Database.SqlQuery(t, "EXEC up_QuotationTypeByProjectId_R @ProjectId", para).Cast<QuotationTypeDto>().ToList();
        }
        #region 确认单供应商查询
        /// <summary>
        /// 查询已经上传确认单的供应商的信息
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="projectCode"></param>
        /// <param name="projectName"></param>
        /// <param name="projectShortName"></param>
        /// <param name="supplierCode"></param>
        /// <param name="supplierName"></param>
        /// <param name="supplierShortName"></param>
        /// <returns></returns>
        public List<QuotationMstDto> QuotationSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType), 
                                                       new SqlParameter("@ProjectCode ",projectCode),
                                                       new SqlParameter("@ProjectName", projectName),
                                                       new SqlParameter("@ProjectShortName", projectShortName),
                                                       new SqlParameter("@SupplierCode",supplierCode),
                                                        new SqlParameter("@SupplierName",supplierName),
                                                         new SqlParameter("@SupplierShortName",supplierShortName),
                                                          new SqlParameter("@UserId",userId),
                                                      };
            Type t = typeof(QuotationMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_QuotationMst_R @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@UserId", para).Cast<QuotationMstDto>().ToList();
        }
        public int QuotationSave(QuotationMstDto quotationMst, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", quotationMst.QuotationId.ToString()), 
                                                       new SqlParameter("@ProjectId",quotationMst.ProjectId),
                                                       new SqlParameter("@SupplierId", quotationMst.SupplierId),
                                                        new SqlParameter("@SelectChk",quotationMst.SelectChk),
                                                        new SqlParameter("@Status",quotationMst.Status == null?"":quotationMst.Status),
                                                       new SqlParameter("@UserId",userId),
                                                       new SqlParameter("@QuotationType",quotationMst.QuotationType),
                                                        new SqlParameter("@Remark",quotationMst.Remark == null?"":quotationMst.Remark),
                                                         new SqlParameter("@GroupId",quotationMst.GroupId == null?"0":quotationMst.GroupId)
            };
            Type t = typeof(int);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_QuotationMst_S @Id,@ProjectId,@SupplierId,@SelectChk,@Status,@UserId,@QuotationType,@Remark,@GroupId", para).Cast<int>().First();
        }
        /// <summary>
        /// 根据确认单的Id查询对应的项目信息和供应商信息
        /// 主要用于从确认单供应商跳转到确认单列表页面的自动查询
        /// 
        /// 暂时不使用
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public List<QuotationMstDto> ProjectAndSupplierByQuotationIdSearch(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", quotationId)
                                                      };
            Type t = typeof(QuotationMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_ProjectAndSupplierByQuotationId_R @Id", para).Cast<QuotationMstDto>().ToList();
        }
        public List<Quotation_QuotationMst> QuotationByProjectAndSupplierAndType(string projectId, string supplierId, string quotationType)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                        new SqlParameter("@QuotationType", quotationType)
                                                      };
            Type t = typeof(Quotation_QuotationMst);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_QuotationExistChk_R @ProjectId,@SupplierId,@QuotationType", para).Cast<Quotation_QuotationMst>().ToList();
        }
        #endregion
        #region 确认单列表_执行
        public List<Quotation_ZhiXingDto> Quotation_ZhiXingSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                     new SqlParameter( "@SupplierShortName", supplierShortName),
                                                      new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(Quotation_ZhiXingDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Zhixing_Dtl_R @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<Quotation_ZhiXingDto>().ToList();
        }
        public List<Quotation_ZhiXingDto> Quotation_ZhiXingSearchByQuotationIdAndSeqNO(string QuotationId, string seqNO)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", QuotationId),
                                                     new SqlParameter("@SeqNO", seqNO),
                                                      };
            Type t = typeof(Quotation_ZhiXingDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Zhixing_DtlByQuotationIdAndSeqNo_R @Id,@SeqNO", para).Cast<Quotation_ZhiXingDto>().ToList();
        }

        #endregion
        #region 确认单列表_编程
        public List<Quotation_BianChengDto> Quotation_BianChengSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                      new SqlParameter( "@SupplierShortName", supplierShortName),
                                                       new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(Quotation_BianChengDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Biancheng_Dtl_R @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<Quotation_BianChengDto>().ToList();
        }
        #endregion
        #region 确认单列表_复核
        public List<Quotation_FuHeDto> Quotation_FuHeSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                      new SqlParameter( "@SupplierShortName", supplierShortName),
                                                       new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(Quotation_FuHeDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Fuhe_Dtl_R @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<Quotation_FuHeDto>().ToList();
        }
        #endregion
        #region 确认单列表_其他1
        public List<Quotation_QiTa1Dto> Quotation_QiTa1Search(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                     new SqlParameter( "@SupplierShortName", supplierShortName),
                                                      new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(Quotation_QiTa1Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Qita1_Dtl_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<Quotation_QiTa1Dto>().ToList();
        }
        #endregion
        #region 确认单列表_其他2
        public List<Quotation_QiTa2Dto> Quotation_QiTa2Search(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                     new SqlParameter( "@SupplierShortName", supplierShortName),
                                                      new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(Quotation_QiTa2Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Qita2_Dtl_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<Quotation_QiTa2Dto>().ToList();
        }
        #endregion
        #region 确认单列表_研究
        public List<Quotation_YanJiuDto> Quotation_YanJiuSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                     new SqlParameter( "@SupplierShortName", supplierShortName),
                                                      new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(Quotation_YanJiuDto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Yanjiu_Dtl_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<Quotation_YanJiuDto>().ToList();
        }
        #endregion
        #region 确认单列表_支持
        public List<ZhiChi01Dto> Quotation_ZhichiSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                      new SqlParameter( "@SupplierShortName", supplierShortName),
                                                       new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(ZhiChi01Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Zhichi_Dtl_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<ZhiChi01Dto>().ToList();
        }
        #endregion
        #region 确认单列表_车展
        public List<Quotation_CheZhanDto> Quotation_ChezhanSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                      new SqlParameter( "@SupplierShortName", supplierShortName),
                                                       new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk)
                                                      };
            Type t = typeof(Quotation_CheZhanDto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Chezhan_Dtl_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<Quotation_CheZhanDto>().ToList();
        }
        #endregion
        #region 每个供应商每个类型确认单合计查询
        public List<QuotationPerQuotationTypeSum_Left_Dto> Quotation_PerQuotationTypeSumSearch_Left(string projectId, string supplierCode, string supplierShortName, string supplierName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierCode", supplierCode),
                                                          new SqlParameter("@SupplierName", supplierName),
                                                          new SqlParameter("@SupplierShortName", supplierShortName),
                                                          new SqlParameter("@LastChk",lastChk),
                                                          new SqlParameter("@GroupId",groupId)
            };
            Type t = typeof(QuotationPerQuotationTypeSum_Left_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_PerQuotationTypeSum_LEFT_R @ProjectId,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<QuotationPerQuotationTypeSum_Left_Dto>().ToList();
        }
        public List<QuotationPerQuotationTypeSum_Head_Dto> Quotation_PerQuotationTypeSumSearch_Head(string projectId, string supplierCode, string supplierShortName, string supplierName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierCode", supplierCode),
                                                          new SqlParameter("@SupplierName", supplierName),
                                                          new SqlParameter("@SupplierShortName", supplierShortName),
                                                          new SqlParameter("@LastChk",lastChk),
                                                          new SqlParameter("@GroupId",groupId)
            };
            Type t = typeof(QuotationPerQuotationTypeSum_Head_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_PerQuotationTypeSum_Head_R @ProjectId,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<QuotationPerQuotationTypeSum_Head_Dto>().ToList();
        }
        public List<QuotationPerQuotationTypeSum_Data_Dto> Quotation_PerQuotationTypeSumSearch_Data(string projectId, string supplierCode, string supplierShortName, string supplierName, string lastChk, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierCode", supplierCode),
                                                          new SqlParameter("@SupplierName", supplierName),
                                                          new SqlParameter("@SupplierShortName", supplierShortName),
                                                          new SqlParameter("@LastChk",lastChk),
                                                          new SqlParameter("@GroupId",groupId)
            };
            Type t = typeof(QuotationPerQuotationTypeSum_Data_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_PerQuotationTypeSum_Data_R @ProjectId,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId", para).Cast<QuotationPerQuotationTypeSum_Data_Dto>().ToList();
        }
        #endregion
        #region 需求书提交确认
        public List<CommitCheckDto> CommitCheckSearch(string projectId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@UserId", userId) };
            Type t = typeof(CommitCheckDto);
            return db.Database.SqlQuery(t, "EXEC up_RequirementCommitCheck_R @ProjectId,@UserId", para).Cast<CommitCheckDto>().ToList();
        }
        #endregion
        #region 通过Id查询确认单Mst
        public List<QuotationMstDto> QuotationSearchById(string projectId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@UserId", userId) };
            Type t = typeof(QuotationMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_QuotationMstById_R @ProjectId,@UserId", para).Cast<QuotationMstDto>().ToList();
        }
        #endregion
        #region 导出确认单查询
        public List<Quotation_ZhiXingDto> QuotationGenSearch_Zhixing(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_ZhiXingDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Gen_Zhixing_R @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_ZhiXingDto>().ToList();
        }
        public List<Quotation_BianChengDto> QuotationGenSearch_Biancheng(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_BianChengDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Biancheng_R] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_BianChengDto>().ToList();
        }
        public List<Quotation_FuHeDto> QuotationGenSearch_Fuhe(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_FuHeDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Fuhe_R] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_FuHeDto>().ToList();
        }
        public List<Quotation_YanJiuDto> QuotationGenSearch_Yanjiu(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_YanJiuDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Yanjiu_R] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_YanJiuDto>().ToList();
        }
        public List<Quotation_QiTa1Dto> QuotationGenSearch_Qita1(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_QiTa1Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Qita1_R] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_QiTa1Dto>().ToList();
        }
        public List<Quotation_QiTa2Dto> QuotationGenSearch_Qita2(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_QiTa2Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Qita2_R] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_QiTa2Dto>().ToList();
        }
        #region 支持
        public List<ZhiChi01Dto> QuotationGenSearch_Zhichi01(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(ZhiChi01Dto);
            List<ZhiChi01Dto> lst = db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Zhichi_R01] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<ZhiChi01Dto>().ToList();
            lst.ForEach(x =>
            {
                x.DtoType = requirmentType + "_Zhichi01";
            });
            return lst;
        }
        public List<ZhiChi01Dto> QuotationGenSearch_Zhichi02(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(ZhiChi01Dto);
            List<ZhiChi01Dto> lst = db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Zhichi_R02] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<ZhiChi01Dto>().ToList();
            lst.ForEach(x =>
            {
                x.DtoType = requirmentType + "_Zhichi02";
            });
            return lst;
        }
        #endregion
        #region 车展
        public List<Quotation_CheZhanDto> QuotationGenSearch_Chenzhan01(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_CheZhanDto);
            List<Quotation_CheZhanDto> lst = db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Chezhan_R01] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_CheZhanDto>().ToList();
            lst.ForEach(x =>
            {
                x.DtoType = "Chenzhan01";
            });
            return lst;
        }
        public List<Quotation_CheZhanDto> QuotationGenSearch_Chenzhan02(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_CheZhanDto);
            List<Quotation_CheZhanDto> lst = db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Chezhan_R02] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_CheZhanDto>().ToList();
            lst.ForEach(x =>
            {
                x.DtoType = "Chenzhan02";
            });
            return lst;
        }
        public List<Quotation_CheZhanDto> QuotationGenSearch_Chenzhan03(string projectId, string citySeqNO, string requirmentType, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@CitySeqNO", citySeqNO),
                                                          new SqlParameter("@RequirementType", requirmentType),
                                                          new SqlParameter("@RequirementId", requirementId)};
            Type t = typeof(Quotation_CheZhanDto);
            List<Quotation_CheZhanDto> lst = db.Database.SqlQuery(t, "EXEC [up_Quotation_Gen_Chezhan_R03] @ProjectId,@CitySeqNO,@RequirementType,@RequirementId", para).Cast<Quotation_CheZhanDto>().ToList();
            lst.ForEach(x =>
            {
                x.DtoType = "Chenzhan02";
            });
            return lst;
        }
        #endregion
        #endregion
        #region 确认单导出
        public List<QuotationExport_Data_Dto> QuotationExport_DataSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId, string quotationType)
        {
            projectCode = projectCode == null ? "" : projectCode;
            projectName = projectName == null ? "" : projectName;
            projectShortName = projectShortName == null ? "" : projectShortName;
            supplierCode = supplierCode == null ? "" : supplierCode;
            supplierName = supplierName == null ? "" : supplierName;
            supplierShortName = supplierShortName == null ? "" : supplierShortName;
            groupId = groupId == null ? "" : groupId;
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                      new SqlParameter( "@SupplierShortName", supplierShortName),
                                                       new SqlParameter( "@GroupId", groupId),
                                                     new SqlParameter( "@LastChk", lastChk),
                                                     new SqlParameter( "@QuotationType", quotationType)
                                                      };
            Type t = typeof(QuotationExport_Data_Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_QuotationExport_Data_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId,@QuotationType", para).Cast<QuotationExport_Data_Dto>().ToList();
        }
        public List<QuotationExport_Head_Dto> QuotationExport_HeadSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId, string quotationType)
        {
            projectCode = projectCode == null ? "" : projectCode;
            projectName = projectName == null ? "" : projectName;
            projectShortName = projectShortName == null ? "" : projectShortName;
            supplierCode = supplierCode == null ? "" : supplierCode;
            supplierName = supplierName == null ? "" : supplierName;
            supplierShortName = supplierShortName == null ? "" : supplierShortName;
            groupId = groupId == null ? "" : groupId;
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                      new SqlParameter( "@SupplierShortName", supplierShortName),
                                                       new SqlParameter( "@GroupId", groupId),
                                                      new SqlParameter( "@LastChk", lastChk),
                                                     new SqlParameter( "@QuotationType", quotationType)
                                                      };
            Type t = typeof(QuotationExport_Head_Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_QuotationExport_Head_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId,@QuotationType", para).Cast<QuotationExport_Head_Dto>().ToList();
        }
        public List<QuotationExport_Left_Dto> QuotationExport_LeftSearch(string modelType, string projectCode, string projectName, string projectShortName, string supplierCode, string supplierName, string supplierShortName, string lastChk, string groupId, string quotationType)
        {
            projectCode = projectCode == null ? "" : projectCode;
            projectName = projectName == null ? "" : projectName;
            projectShortName = projectShortName == null ? "" : projectShortName;
            supplierCode = supplierCode == null ? "" : supplierCode;
            supplierName = supplierName == null ? "" : supplierName;
            supplierShortName = supplierShortName == null ? "" : supplierShortName;
            groupId = groupId == null ? "" : groupId;
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType),
                                                     new SqlParameter("@ProjectCode", projectCode),
                                                     new SqlParameter( "@ProjectName", projectName),
                                                     new SqlParameter( "@ProjectShortName", projectShortName),
                                                     new SqlParameter( "@SupplierCode", supplierCode),
                                                     new SqlParameter( "@SupplierName", supplierName),
                                                      new SqlParameter( "@SupplierShortName", supplierShortName),
                                                       new SqlParameter( "@GroupId", groupId),
                                                      new SqlParameter( "@LastChk", lastChk),
                                                     new SqlParameter( "@QuotationType", quotationType)
                                                      };
            Type t = typeof(QuotationExport_Left_Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_QuotationExport_Left_R] @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@LastChk,@GroupId,@QuotationType", para).Cast<QuotationExport_Left_Dto>().ToList();
        }
        #endregion
        #region 确认单组
        public List<QuotationGroupDto> QuotationGroupSearch(string projectId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(QuotationGroupDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Group_R @ProjectId,@UserId", para).Cast<QuotationGroupDto>().ToList();
        }
        public List<QuotationGroupDto> QuotationGroupByProjectNameSearch(string ModelType, string projectName, string projectShortName, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", ModelType),
                new SqlParameter("@ProjectName", projectName),
                                                        new SqlParameter("@ProjectShortName", projectShortName),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(QuotationGroupDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_GroupByProjectName_R @ModelType,@ProjectName,@ProjectShortName,@UserId", para).Cast<QuotationGroupDto>().ToList();
        }
        /// <summary>
        /// 通过组Id查询 确认单
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<QuotationMstDto> QuotationMstSearchByGroupId(string projectId, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), new SqlParameter("@GroupId", groupId) };
            Type t = typeof(QuotationMstDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationMstByGroupId_R] @ProjectId,@GroupId", para).Cast<QuotationMstDto>().ToList();
        }
        public int QuotationGroupSave(string groupId, string projectId, string quotationName, string userId, bool? settlementChk, string requirementGroupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", groupId),
                                                         new SqlParameter("@ProjectId", projectId),
                                                         new SqlParameter("@QuotationGroupName", quotationName),
                                                          new SqlParameter("@SettlementChk", settlementChk),
                                                        new SqlParameter("@UserId", userId),
                                                        new SqlParameter("@RequirementGroupId", requirementGroupId)
            };
            Type t = typeof(int);
            int applyId = db.Database.SqlQuery(t, "EXEC up_Quotation_Group_S @Id,@ProjectId,@QuotationGroupName,@SettlementChk,@UserId,@RequirementGroupId ", para).Cast<int>().First();
            return applyId;
        }
        public void QuotationGroupSettlementUpdate(string projectId, string groupId, bool settlementChk)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", groupId),
                                                         new SqlParameter("@ProjectId", projectId),
                                                          new SqlParameter("@SettlementChk", settlementChk)
            };
            db.Database.ExecuteSqlCommand("EXEC up_Quotation_Group_SettlementChk_U @Id,@ProjectId,@SettlementChk", para);
        }
        #endregion
        #region 确认单选中更新暂时不使用
        public void Quotation_QuotationMstUpdate(string id, bool selectChk, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", id),
                                                     new SqlParameter("@SelectChk", selectChk),
                                                     new SqlParameter("@UserId", userId),
                                                      };
            db.Database.ExecuteSqlCommand("EXEC up_Quotation_QuotationMst_U @Id,@SelectChk,@UserId", para);
        }
        #endregion
        #region 确认单登记
        public List<Quotation_BianChengDto> QuotationRegSearch_Biancheng(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                          new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                          new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(Quotation_BianChengDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Biancheng_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<Quotation_BianChengDto>().ToList();
        }
        public List<Quotation_CheZhanDto> QuotationRegSearch_Chezhan(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                         new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                           new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(Quotation_CheZhanDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Chezhan_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<Quotation_CheZhanDto>().ToList();
        }
        public List<Quotation_FuHeDto> QuotationRegSearch_Fuhe(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                          new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                           new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(Quotation_FuHeDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Fuhe_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<Quotation_FuHeDto>().ToList();
        }
        public List<Quotation_QiTa1Dto> QuotationRegSearch_Qita1(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                           new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                          new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(Quotation_QiTa1Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Qita1_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<Quotation_QiTa1Dto>().ToList();
        }
        public List<Quotation_QiTa2Dto> QuotationRegSearch_Qita2(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                          new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                            new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(Quotation_QiTa2Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Qita2_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<Quotation_QiTa2Dto>().ToList();
        }
        public List<Quotation_YanJiuDto> QuotationRegSearch_Yanjiu(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                          new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                           new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(Quotation_YanJiuDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Yanjiu_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<Quotation_YanJiuDto>().ToList();
        }
        public List<ZhiChi01Dto> QuotationRegSearch_Zhichi(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                         new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                           new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(ZhiChi01Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Zhichi_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<ZhiChi01Dto>().ToList();
        }
        public List<Quotation_ZhiXingDto> QuotationRegSearch_Zhixing(string projectId, string supplierId, string groupId, string requirementGroupId, string province, string city, string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                        new SqlParameter("@SupplierId", supplierId),
                                                          new SqlParameter("@GroupId", groupId),
                                                          new SqlParameter("@RequirementGroupId", requirementGroupId),
                                                          new SqlParameter("@Province", province),
                                                          new SqlParameter("@City", city),
                                                           new SqlParameter("@RequirementId", requirementId)
            };
            Type t = typeof(Quotation_ZhiXingDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationReg_Zhixing_R] @ProjectId,@SupplierId,@GroupId,@RequirementGroupId,@Province,@City,@RequirementId", para).Cast<Quotation_ZhiXingDto>().ToList();
        }
        #endregion
        #region 判断确认单预算是否保存过
        public int QuotationYusuanSaveCheck(string projectId, string quotationgroupId, string quotationType)
        {
            SqlParameter[] para = new SqlParameter[] {  
                                                       new SqlParameter("@ProjectId",projectId),
                                                       new SqlParameter("@QuotationGroupId", quotationgroupId),
                                                        new SqlParameter("@QuotationType",quotationType)

            };
            Type t = typeof(int);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_YusuanSaveCheck_R @ProjectId,@QuotationGroupId,@QuotationType", para).Cast<int>().First();
        }
        #endregion
        #region 查询项目和确认单组下确认单类型
        public List<QuotationTypeDto> QuotationTypeSearchByProjectIdAndGroupId(string projectId, string quotationGroupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                         new SqlParameter("@QuotationGroupId", quotationGroupId)
            };
            Type t = typeof(QuotationTypeDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_QuotationTypeByProjectIdAndGroupId_R] @ProjectId,@QuotationGroupId", para).Cast<QuotationTypeDto>().ToList();
        }
        #endregion
        #region 确认单组删除
        public void QuotationGroupDelete(string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@GroupId", groupId.ToString()) };

            db.Database.ExecuteSqlCommand("EXEC up_Quotation_Group_D @GroupId", para);
        }
        #endregion
        #region 确认单组复制
        #region 确认单列表_执行
        /// <summary>
        /// 通过确认单组查询对应的需求书
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<RequiremetMstDto> RequirementMstSearchByQuotationGroupId(string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@QuotationGroupId", groupId) };
            Type t = typeof(RequiremetMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_RequirementMstByQuotationGroupId_R @QuotationGroupId", para).Cast<RequiremetMstDto>().ToList();
        }
        public List<Quotation_ZhiXingDto> Quotation_ZhiXingCopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<Quotation_ZhiXingDto> quotationList = new List<Quotation_ZhiXingDto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Anfang"
                    || requirementMst.RequirementType == "Mingjian"
                    || requirementMst.RequirementType == "Pankujijiagediaocha"
                    || requirementMst.RequirementType == "Mianfangjidianfang"
                    || requirementMst.RequirementType == "Zuotanhui"
                    || requirementMst.RequirementType == "Shenfangjirijiliuzhi"
                    || requirementMst.RequirementType == "Wangluodiaoyan"
                    || requirementMst.RequirementType == "Guankong"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                                                                new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(Quotation_ZhiXingDto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Zhixing @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<Quotation_ZhiXingDto>().ToList());
                }
            }
            //return quotationList.Distinct<Quotation_ZhiXingDto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }


        #endregion
        #region 确认单列表_编程
        public List<Quotation_BianChengDto> Quotation_BianChengCopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<Quotation_BianChengDto> quotationList = new List<Quotation_BianChengDto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Bianchengjibianji"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                                                            new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(Quotation_BianChengDto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Biancheng @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<Quotation_BianChengDto>().ToList());
                }
            }
            //return quotationList.Distinct<Quotation_BianChengDto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }
        #endregion
        #region 确认单列表_复核
        public List<Quotation_FuHeDto> Quotation_FuHeCopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<Quotation_FuHeDto> quotationList = new List<Quotation_FuHeDto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Bianmajifuhe"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                         new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(Quotation_FuHeDto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Fuhe @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<Quotation_FuHeDto>().ToList());
                }
            }
            //return quotationList.Distinct<Quotation_FuHeDto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }
        #endregion
        #region 确认单列表_其他1
        public List<Quotation_QiTa1Dto> Quotation_QiTa1CopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<Quotation_QiTa1Dto> quotationList = new List<Quotation_QiTa1Dto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Youxingshangpincaigou"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                         new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(Quotation_QiTa1Dto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Qita1 @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<Quotation_QiTa1Dto>().ToList());
                }
            }
            //return quotationList.Distinct<Quotation_QiTa1Dto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }
        #endregion
        #region 确认单列表_其他2
        public List<Quotation_QiTa2Dto> Quotation_QiTa2CopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<Quotation_QiTa2Dto> quotationList = new List<Quotation_QiTa2Dto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Wuxingshangpincaigou"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                         new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(Quotation_QiTa2Dto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Qita2 @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<Quotation_QiTa2Dto>().ToList());
                }
            }
            //return quotationList.Distinct<Quotation_QiTa2Dto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }
        #endregion
        #region 确认单列表_研究
        public List<Quotation_YanJiuDto> Quotation_YanJiuCopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<Quotation_YanJiuDto> quotationList = new List<Quotation_YanJiuDto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Yanjiujishujufenxi"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                         new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(Quotation_YanJiuDto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Yanjiu @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<Quotation_YanJiuDto>().ToList());
                }
            }
            //return quotationList.Distinct<Quotation_YanJiuDto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }
        #endregion
        #region 确认单列表_支持
        public List<ZhiChi01Dto> Quotation_ZhichiCopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<ZhiChi01Dto> quotationList = new List<ZhiChi01Dto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Changdibuzhan"
                    || requirementMst.RequirementType == "Yunshuzuche"
                    || requirementMst.RequirementType == "Gongyingshangchailv"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                         new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(ZhiChi01Dto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Zhichi @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<ZhiChi01Dto>().ToList());
                }
            }
            //return quotationList.Distinct<ZhiChi01Dto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }
        #endregion
        #region 确认单列表_车展
        public List<Quotation_CheZhanDto> Quotation_ChezhanCopySearch(string newquotationGroupId, string oldquotationGroupId)
        {
            List<Quotation_CheZhanDto> quotationList = new List<Quotation_CheZhanDto>();
            List<RequiremetMstDto> list = RequirementMstSearchByQuotationGroupId(newquotationGroupId);
            foreach (RequiremetMstDto requirementMst in list)
            {
                if (requirementMst.RequirementType == "Cheliangzhanpinghui"
                    )
                {
                    SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementMst.RequirementId),
                         new SqlParameter("@NewQuotationGroupId", newquotationGroupId),
                                                                new SqlParameter("@QuotationGroupId",oldquotationGroupId),
                                                      };
                    Type t = typeof(Quotation_CheZhanDto);
                    quotationList.AddRange(db.Database.SqlQuery(t, "EXEC up_Quotation_Group_CopySearch_Chezhan @RequirementId,@NewQuotationGroupId,@QuotationGroupId", para).Cast<Quotation_CheZhanDto>().ToList());
                }
            }

            //return quotationList.Distinct<Quotation_CheZhanDto>(new QuotationDtoComparer()).ToList();
            return quotationList.OrderBy(x => x.SupplierName).ToList();
        }
        #endregion
        #endregion
        #region 预算确认单
        #region 预算确认单组查询
        public List<Group_YusuanDto> QuotationGroup_Yusuan(string projectId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Group_YusuanDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Group_Yusuan_R] @ProjectId,@UserId", para).Cast<Group_YusuanDto>().ToList();
        }
        #endregion
        #region 预算确认单组保存
        public void QuotationGrouupYusuanSave(string quotationGroupYusuanId, string quotationYusuanGroupName, string projectId, bool LastChk, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", quotationGroupYusuanId),
                                                       new SqlParameter("@QuotationYusuanGroupName", quotationYusuanGroupName),
                                                       new SqlParameter("@ProjectId", projectId),
                                                       new SqlParameter("@LastChk", LastChk),
                                                       new SqlParameter("@UserId", userId)
            };
            db.Database.ExecuteSqlCommand("EXEC up_Quotation_Group_Yusuan_S @Id,@QuotationYusuanGroupName,@ProjectId,@LastChk,@UserId", para);
        }
        #region 预算确认单组删除
        public void QuotationGrouupYusuanDelete(string quotationGroupYusuanId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", quotationGroupYusuanId)
            };
            db.Database.ExecuteSqlCommand("EXEC up_Quotation_Group_Yusuan_D @Id", para);
        }
        #endregion
        #endregion
        #region 预算确认单查询
        public List<Quotation_BianChengDto> Yusuan_BianchengSearch(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Quotation_BianChengDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Biancheng_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<Quotation_BianChengDto>().ToList();
        }
        public List<Quotation_CheZhanDto> Yusuan_ChezhanSearch(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Quotation_CheZhanDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Chezhan_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<Quotation_CheZhanDto>().ToList();
        }
        public List<Quotation_FuHeDto> Yusuan_FuheSearch(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Quotation_FuHeDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Fuhe_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<Quotation_FuHeDto>().ToList();
        }
        public List<Quotation_QiTa1Dto> Yusuan_Qita1Search(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Quotation_QiTa1Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Qita1_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<Quotation_QiTa1Dto>().ToList();
        }
        public List<Quotation_QiTa2Dto> Yusuan_Qita2Search(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Quotation_QiTa2Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Qita2_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<Quotation_QiTa2Dto>().ToList();
        }
        public List<Quotation_YanJiuDto> Yusuan_YanjiuSearch(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Quotation_YanJiuDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Yanjiu_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<Quotation_YanJiuDto>().ToList();
        }
        public List<ZhiChi01Dto> Yusuan_ZhichiSearch(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(ZhiChi01Dto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Zhichi_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<ZhiChi01Dto>().ToList();
        }
        public List<Quotation_ZhiXingDto> Yusuan_ZhixingSearch(string yusuanGroupId, string quotationGroupId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@YusuanGroupId", yusuanGroupId), 
                                                        new SqlParameter("@QuotationGroupId", quotationGroupId), 
                                                         new SqlParameter("@UserId", userId)
            };
            Type t = typeof(Quotation_ZhiXingDto);
            return db.Database.SqlQuery(t, "EXEC [up_Quotation_Yusuan_Dtl_Zhixing_R] @YusuanGroupId,@QuotationGroupId,@UserId", para).Cast<Quotation_ZhiXingDto>().ToList();
        }
        #endregion
        #region 预算确认单保存
        public void QuotationYusuanSave(string quotationId, string quotationSeqNO, string yusuanGroupId, string yusuandanjia_new, string yusuanshuliang, string userId)
        {
            yusuandanjia_new = yusuandanjia_new == null ? "" : yusuandanjia_new;
            yusuanshuliang = yusuanshuliang == null ? "" : yusuanshuliang;
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@QuotationId", quotationId),
                                                       new SqlParameter("@QuotationSeqNO", quotationSeqNO),
                                                       new SqlParameter("@YusuanGroupId", yusuanGroupId),
                                                       new SqlParameter("@Yusuandanjia_new", yusuandanjia_new),
                                                       new SqlParameter("@Yusuanshuliang", yusuanshuliang),
                                                       new SqlParameter("@UserId", userId)
            };
            db.Database.ExecuteSqlCommand("EXEC up_Quotation_Yusuan_S @QuotationId,@QuotationSeqNO,@YusuanGroupId,@Yusuandanjia_new,@Yusuanshuliang,@UserId", para);
        }
        #endregion
        #endregion

    }



}
