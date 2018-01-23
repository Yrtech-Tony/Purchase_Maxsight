using Purchase.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Purchase.Service.DTO;

namespace Purchase.Service
{
    public class MasterService
    {
        PurchaseEntities db = new PurchaseEntities();
        #region 供应商查询(Popup)
        /// <summary>
        /// 供应商弹出框查询
        /// </summary>
        /// <param name="supplierCode"></param>
        /// <param name="supplierName"></param>
        /// <param name="supplierShortName"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public List<SupplierDto> SupplierPopupSearch(string supplierCode, string supplierName, string supplierShortName, string province, string city)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierCode", supplierCode), 
                                                       new SqlParameter("@SupplierName",supplierName),
                                                       new SqlParameter("@SupplierShortName", supplierShortName),
                                                       new SqlParameter("@Province",province),
                                                       new SqlParameter("@City", city)};
            Type t = typeof(SupplierDto);
            return db.Database.SqlQuery(t, "EXEC up_Supplier_R @SupplierCode,@SupplierName,@SupplierShortName,@Province,@City", para).Cast<SupplierDto>().ToList();
        }
        /// <summary>
        /// 根据供应商名称查询供应商的信息
        /// 也用于验证供应商是否存在
        /// </summary>
        /// <param name="supplierName"></param>
        /// <returns></returns>
        public List<SupplierDto> SupplierSearchBySupplierName(string supplierName, string serviceTrade)
        {
            if (supplierName == null)
            {
                supplierName = "";
            }
            if (serviceTrade == null)
            {
                serviceTrade = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierName", supplierName),
                                                                      new SqlParameter("@ServiceTrade", serviceTrade) };
            Type t = typeof(SupplierDto);
            return db.Database.SqlQuery(t, "EXEC up_Master_SupplierBySupplierName_R @SupplierName,@ServiceTrade", para).Cast<SupplierDto>().ToList();
        }
        #endregion
        #region 供应商查询
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierCode"></param>
        /// <param name="supplierName"></param>
        /// <param name="supplierShortName"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public List<SupplierDto> SupplierSearch(SupplierMng supplier, string userId, string applyStatuscode)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierCode", supplier.SupplierCode==null?"":supplier.SupplierCode), 
                                                       new SqlParameter("@SupplierName",supplier.SupplierName==null?"":supplier.SupplierName),
                                                       new SqlParameter("@SupplierShortName", supplier.SupplierShortName==null?"":supplier.SupplierShortName),
                                                       new SqlParameter("@SupplierType", supplier.SupplierType==null?"":supplier.SupplierType),
                                                       new SqlParameter("@ServiceTrade", supplier.ServiceTrade==null?"":supplier.ServiceTrade),
                                                       new SqlParameter("@PurchaseType", supplier.PurchaseType==null?"":supplier.PurchaseType),
                                                       new SqlParameter("@Province",supplier.Province==null?"":supplier.Province),
                                                       new SqlParameter("@City", supplier.City==null?"":supplier.City),
                                                       new SqlParameter("@UserId", userId),
                                                       new SqlParameter("@CooperationState", supplier.CooperationState==null?"":supplier.CooperationState),
                                                       new SqlParameter("@RecommendDepartment", supplier.RecommendDepartment==null?"":supplier.RecommendDepartment),
                                                       new SqlParameter("@ApplyStatusCode", applyStatuscode),
                                                        new SqlParameter("@InUserId", supplier.InUserId==null?"":supplier.InUserId)
            };
            Type t = typeof(SupplierDto);
            return db.Database.SqlQuery(t, "EXEC up_Supplier_R1 @SupplierCode,@SupplierName,@SupplierShortName,@SupplierType,@ServiceTrade,@PurchaseType,@Province,@City,@UserId,@CooperationState,@RecommendDepartment,@ApplyStatusCode,@InUserId", para).Cast<SupplierDto>().ToList();
        }
        public List<SupplierDto> SupplierSearchById(string supplierId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierId", supplierId==null?"0":supplierId), 
                                                       new SqlParameter("@UserId", userId)
            };
            Type t = typeof(SupplierDto);
            return db.Database.SqlQuery(t, "EXEC up_SupplierById_R @SupplierId,@UserId", para).Cast<SupplierDto>().ToList();
        }
        public void SupplierMngCopy(string supplierId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierId", supplierId.ToString()),
                                                      new SqlParameter("@UserId", userId.ToString())};

            db.Database.ExecuteSqlCommand("EXEC up_Supplier_Copy @SupplierId,@UserId", para);
        }
        /// <summary>
        /// 供应商新增或者，名称，银行信息变更时调用，且按照财务人数进行处理
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="inUserId"></param>
        /// <param name="modifyColumn"></param>
        public void SupplierModifySave(string supplierId, string inUserId, string modifyColumn, bool newChk)
        {

            List<UserInfoDto> list = UserInfoSearch("", "财务部", "");// 查询财务部人员的信息
            foreach (UserInfoDto userInfo in list)
            {

                SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierId", supplierId.ToString()),
                                                            new SqlParameter("@AlterUserId", userInfo.UserId.ToString()),
                                                            new SqlParameter("@InUserId", inUserId.ToString()),
                                                            new SqlParameter("@ModifyColumn", modifyColumn.ToString()),
                                                             new SqlParameter("@NewChk",newChk)};

                db.Database.ExecuteSqlCommand("EXEC up_SupplierAlter_S @SupplierId,@AlterUserId,@InUserId,@ModifyColumn,@NewChk", para);
            }
        }
        /// <summary>
        /// 点击不再提醒时调用
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="inUserId"></param>
        /// <param name="modifyColumn"></param>
        public void SupplierModifyAlterDelete(string supplierId, string alterUserId)
        {

            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierId", supplierId.ToString()),
                                                            new SqlParameter("@AlterUserId", alterUserId),
                                                            };

            db.Database.ExecuteSqlCommand("EXEC up_SupplierAlter_D @SupplierId,@AlterUserId", para);

        }
        #endregion
        #region 银行信息查询
        public List<SupplierDto> SupplierBankInfoSearch(SupplierMng supplier, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SupplierCode", supplier.SupplierCode==null?"":supplier.SupplierCode), 
                                                       new SqlParameter("@SupplierName",supplier.SupplierName==null?"":supplier.SupplierName),
                                                       new SqlParameter("@SupplierShortName", supplier.SupplierShortName==null?"":supplier.SupplierShortName),
                                                       new SqlParameter("@SupplierType", supplier.SupplierType==null?"":supplier.SupplierType),
                                                       new SqlParameter("@ServiceTrade", supplier.ServiceTrade==null?"":supplier.ServiceTrade),
                                                       new SqlParameter("@PurchaseType", supplier.PurchaseType==null?"":supplier.PurchaseType),
                                                       new SqlParameter("@Province",supplier.Province==null?"":supplier.Province),
                                                       new SqlParameter("@City", supplier.City==null?"":supplier.City),
            };
            Type t = typeof(SupplierDto);
            return db.Database.SqlQuery(t, "EXEC up_SupplierBankInfo_R @SupplierCode,@SupplierName,@SupplierShortName,@SupplierType,@ServiceTrade,@PurchaseType,@Province,@City", para).Cast<SupplierDto>().ToList();
        }
        #endregion
        #region 立项信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ProjectCityDto> ProjectCitySearchByProjectId(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId) };
            Type t = typeof(ProjectCityDto);
            return db.Database.SqlQuery(t, "EXEC up_Projects_ProjectCityByProjectId_R @ProjectId", para).Cast<ProjectCityDto>().ToList();
        }
        public List<ProjectCityDto> ProjectCitySearchByProjectIdAndGroupId(string projectId, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                        new SqlParameter("@GroupId", groupId)};
            Type t = typeof(ProjectCityDto);
            return db.Database.SqlQuery(t, "EXEC up_Projects_ProjectCityByProjectIdAndGroupId_R @ProjectId,@GroupId", para).Cast<ProjectCityDto>().ToList();
        }
        /// <summary>
        /// 根据项目名称查询项目信息
        /// 也用于验证项目是否存在
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public List<Projects> ProjectSearchByProjectName(string projectName)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectName", projectName) };
            Type t = typeof(Projects);
            return db.Database.SqlQuery(t, "EXEC up_Master_ProjectsByProjectName_R @ProjectName", para).Cast<Projects>().ToList();
        }
        public List<ProjectDto> ProjectStartDateSearch(string projectName, string projectShortName, string modelType, string serviceTrade)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectName", projectName),
                                                        new SqlParameter("@ProjectShortName", projectName),
                                                        new SqlParameter("@ModelType", projectName),
                                                        new SqlParameter("@ServiceTrade", projectName)};
            Type t = typeof(ProjectDto);
            return db.Database.SqlQuery(t, "EXEC up_Projects_StartDate_R @ProjectName,@ProjectShortName,@ModelType,@ServiceTrade", para).Cast<ProjectDto>().ToList();
        }
        /// <summary>
        /// 通过项目编号，省份，城市，查询样本量信息
        /// 也用于验证同一城市的样本量信息是否已经存在
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public List<ProjectCity> ProjectCitySearchByCityName(string projectId, string province, string city)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                        new SqlParameter("@Province", province),
                                                        new SqlParameter("@City", city)};
            Type t = typeof(ProjectCity);
            return db.Database.SqlQuery(t, "EXEC up_Project_ProjectCityExistChk_R @ProjectId,@Province,@City", para).Cast<ProjectCity>().ToList();
        }
        public List<ProjectDto> ProjectSearchMaster(string modelType, string projectCode, string projectName, string projectShortName, string serviceTrade, string userId)
        {
            if (serviceTrade == null)
                serviceTrade = "";
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectCode",projectCode),
                                                        new SqlParameter("@ProjectName",projectName),
                                                        new SqlParameter("@ProjectShortName",projectShortName),
                                                         new SqlParameter("@ModelType",modelType),
                                                         new SqlParameter("@ServiceTrade",serviceTrade),
                                                         new SqlParameter("@UserId",userId)
            };
            Type t = typeof(ProjectDto);
            return db.Database.SqlQuery(t, "EXEC up_Projects_R @ProjectCode,@ProjectName,@ProjectShortName,@ModelType,@ServiceTrade,@UserId", para).Cast<ProjectDto>().ToList();
        }
        public List<ProjectDto> ProjectSearch(string serviceTrade, string modelType, string departmentName, DateTime? startDate, DateTime? endDate, string projectCode, string projectName, string projectShortName, string step, string userId, string year, string userName)
        {
            if (startDate == null)
            {
                startDate = new DateTime(1970, 1, 1);
            }
            if (endDate == null)
            {
                endDate = new DateTime(9999, 12, 31);
            }
            if (serviceTrade == null)
            {
                serviceTrade = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade",serviceTrade),
                                                        new SqlParameter("@ModelType",modelType),
                                                        new SqlParameter("@DepartmentName",departmentName==null?"":departmentName),
                                                        new SqlParameter("@StartDate",startDate),
                                                        new SqlParameter("@EndDate",endDate),
                                                        new SqlParameter("@ProjectCode",projectCode==null?"":projectCode),
                                                        new SqlParameter("@ProjectName",projectName==null?"":projectName),
                                                        new SqlParameter("@ProjectShortName",projectShortName==null?"":projectShortName),
                                                        new SqlParameter("@Step",step==null?"":step),
                                                        new SqlParameter("@UserId",userId),
                                                        new SqlParameter("@Year",year==null?"":year),
                                                        new SqlParameter("@UserName",userName==null?"":userName),
            };
            Type t = typeof(ProjectDto);
            return db.Database.SqlQuery(t, "EXEC up_Projects_R1 @ServiceTrade,@ModelType,@DepartmentName,@StartDate,@EndDate,@ProjectCode,@ProjectName,@ProjectShortName,@Step,@UserId,@Year,@UserName", para).Cast<ProjectDto>().ToList();
        }
        public List<ProjectDto> ProjectSearchById(string projectId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId",projectId),
                                                         new SqlParameter("@UserId",userId)
            };
            Type t = typeof(ProjectDto);

            return db.Database.SqlQuery(t, "EXEC [up_ProjectsById_R] @ProjectId,@UserId", para).Cast<ProjectDto>().ToList();
        }
        public void ProjectDelete(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId.ToString()) };

            db.Database.ExecuteSqlCommand("EXEC up_Projects_D @ProjectId", para);
        }
        public void ProjectCityDelete(string projectId, string seqNO)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId.ToString()),
                                                        new SqlParameter("@SeqNO", seqNO.ToString())};

            db.Database.ExecuteSqlCommand("EXEC up_ProjectCity_D @ProjectId,@SeqNO", para);
        }
        public void ProjectPersonDelete(string projectId, string seqNO)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId.ToString()),
                                                        new SqlParameter("@SeqNO", seqNO.ToString())};

            db.Database.ExecuteSqlCommand("EXEC up_ProjectPerson_D @ProjectId,@SeqNO", para);
        }
        public void ProjectCopy(string oldProjectId, string newProjectId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@OldProjectId", oldProjectId.ToString()),
                                                        new SqlParameter("@NewProjectId", newProjectId.ToString()),
                                                        new SqlParameter("@UserId", userId.ToString())};

            db.Database.ExecuteSqlCommand("EXEC up_Projects_Copy @OldProjectId,@NewProjectId,@UserId", para);
        }
        #endregion
        #region 合同管理
        /// <summary>
        /// 合同查询
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="projectName"></param>
        /// <param name="projectShortName"></param>
        /// <param name="constractCode"></param>
        /// <param name="constractName"></param>
        /// <param name="supplierCode"></param>
        /// <param name="supplierName"></param>
        /// <param name="supplierShortName"></param>
        /// <returns></returns>
        public List<ConstractDto> ConstractSearch(string serviceTrade, string projectCode, string projectName, string projectShortName, string constractCode, string constractName, string supplierCode, string supplierName, string supplierShortName, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectCode", projectCode), 
                                                       new SqlParameter("@ProjectName",projectName),
                                                       new SqlParameter("@ProjectShortName", projectShortName),
                                                       new SqlParameter("@SupplierCode",supplierCode),
                                                       new SqlParameter("@SupplierName", supplierName),
                                                       new SqlParameter("@SupplierShortName", supplierShortName),
                                                       new SqlParameter("@ConstractCode", constractCode),
                                                       new SqlParameter("@ConstractName", constractName),
                                                       new SqlParameter("@UserId", userId),
                                                         new SqlParameter("@ServiceTrade", serviceTrade)
            };
            Type t = typeof(ConstractDto);
            return db.Database.SqlQuery(t, "EXEC up_Constract_R @ServiceTrade,@ProjectCode,@ProjectName,@ProjectShortName,@SupplierCode,@SupplierName,@SupplierShortName,@ConstractCode,@ConstractName,@UserId", para).Cast<ConstractDto>().ToList();

        }
        /// <summary>
        /// 根据Id查询合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ConstractDto> ConstractByIdSearch(string id, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", id),new SqlParameter("@UserId", userId)
            };
            Type t = typeof(ConstractDto);
            return db.Database.SqlQuery(t, "EXEC up_ConstractById_R @Id,@UserId", para).Cast<ConstractDto>().ToList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Constractid"></param>
        /// <param name="ConstractTemplateId"></param>
        /// <returns></returns>
        public List<ConstractCommonTextDto> ConstractCommonTextSearch(string Constractid, string ConstractTemplateId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractId", Constractid),
                                                        new SqlParameter("@ConstractTemplateId", ConstractTemplateId)
            };
            Type t = typeof(ConstractCommonTextDto);
            return db.Database.SqlQuery(t, "EXEC up_Constract_Content_R @ConstractId,@ConstractTemplateId", para).Cast<ConstractCommonTextDto>().ToList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Constractid"></param>
        /// <param name="ConstractTemplateId"></param>
        /// <param name="seqNO"></param>
        /// <param name="contentTxt"></param>
        /// <param name="contentTxtGen"></param>
        /// <param name="userId"></param>
        public void ConstractCommonTextSave(string Id, string Constractid, string ConstractTemplateId, string seqNO, string contentTxt, string contentTxtGen, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", Id),
                                                        new SqlParameter("@ConstractId", Constractid),
                                                        new SqlParameter("@ConstractTemplateId", ConstractTemplateId),
                                                        new SqlParameter("@ContentId", seqNO),
                                                        new SqlParameter("@ContentTxt", contentTxt),
                                                        new SqlParameter("@ContentTxtGen", contentTxtGen),
                                                        new SqlParameter("@UserId", userId)
            };

            db.Database.ExecuteSqlCommand("EXEC up_Constract_Content_S @Id,@ConstractId,@ConstractTemplateId,@ContentId,@ContentTxt,@ContentTxtGen,@UserId", para);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="constractId"></param>
        /// <returns></returns>
        public List<ConstractTemplateDtl> ConstractViewDtlSearch(string templateId, string constractId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractTemplateId", templateId), 
                                                       new SqlParameter("@ConstractId",constractId) };
            Type t = typeof(ConstractTemplateDtl);
            return db.Database.SqlQuery(t, "EXEC up_ConstractViewTemplateDtl_R @ConstractTemplateId,@ConstractId", para).Cast<ConstractTemplateDtl>().ToList();
        }
        /// <summary>
        /// 合同发送邮件时使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ConstractDto> ConstractByIdSearch1(string id)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", id)
            };
            Type t = typeof(ConstractDto);
            return db.Database.SqlQuery(t, "EXEC up_ConstractById_R1 @Id", para).Cast<ConstractDto>().ToList();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        public void ConstractEmailSendSave(string id, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",id), 
                                                        new SqlParameter("@UserId",userId)};
            db.Database.ExecuteSqlCommand("EXEC up_Constract_Email_U @Id,@UserId", para);
        }
        #region 合同复制
        public void ConstractCopy(string constractId, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",constractId), 
                                                        new SqlParameter("@UserId",userId)};
            db.Database.ExecuteSqlCommand("EXEC up_Constract_Copy @Id,@UserId", para);
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void ConstractDelete(string id)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractId", id) };
            db.Database.ExecuteSqlCommand("EXEC up_Constract_D @ConstractId", para);
        }
        /// <summary>
        /// 根据Id查询合同附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ConstractAttachment> ConstractAttachmentByIdSearch(string id, string seqNO)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractId", id),
                                              new SqlParameter("@SeqNO", seqNO)};
            Type t = typeof(ConstractAttachment);
            return db.Database.SqlQuery(t, "EXEC up_Constract_AttachmentFile_R @ConstractId,@SeqNO", para).Cast<ConstractAttachment>().ToList();

        }
        /// <summary>
        /// 合同保存
        /// </summary>
        /// <param name="constract"></param>
        /// <returns></returns>
        public int ConstractSave(Constract constract)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", constract.Id), 
                                                       new SqlParameter("@ConstractCode",constract.ConstractCode),
                                                       new SqlParameter("@ConstractName", constract.ConstractName),
                                                       new SqlParameter("@ProjectId", constract.ProjectId),
                                                       new SqlParameter("@SupplierId",constract.SupplierId),
                                                       new SqlParameter("@StartDate",constract.StartDate),
                                                       new SqlParameter("@EndDate",constract.EndDate),
                                                        new SqlParameter("@EmailSendDateTime",constract.EmailSendDateTime),
                                                        new SqlParameter("@UserId",constract.InUserId)
                                                        ,new SqlParameter("@PartAName",constract.InUserId)
                                                        ,new SqlParameter("@PartAAddress",constract.InUserId)
                                                        ,new SqlParameter("@PartAContacts",constract.InUserId)
                                                        ,new SqlParameter("@PartATel",constract.InUserId)
                                                        ,new SqlParameter("@PartAEmail",constract.InUserId)
                                                        ,new SqlParameter("@PartBName",constract.InUserId)
                                                        ,new SqlParameter("@PartBAddress",constract.InUserId)
                                                        ,new SqlParameter("@PartBContacts",constract.InUserId)
                                                        ,new SqlParameter("@PartBTel",constract.InUserId)
                                                        ,new SqlParameter("@PartBEmail",constract.InUserId)
                                                        ,new SqlParameter("@ExecuteRegion",constract.InUserId)
                                                        ,new SqlParameter("@RecheckRate",constract.InUserId)
                                                        ,new SqlParameter("@SampleCountAndQuota",constract.InUserId)
                                                        ,new SqlParameter("@CostDesc",constract.CostDesc)

            };
            Type t = typeof(int);
            return db.Database.SqlQuery(t, @"up_Constract_S @Id,@ConstractCode,@ConstractName,@ProjectId,@SupplierId,@StartDate,@EndDate,@EmailSendDateTime
                                                        ,@PartAName,@PartAAddress,@PartAContacts,@PartATel,@PartAEmail,@PartBName,@PartBAddress,@PartBContacts
                                        ,@PartBTel,@PartBEmail,@ExecuteRegion,@RecheckRate,@SampleCountAndQuota,@CostDesc", para).Cast<int>().First();
        }
        /// <summary>
        /// 合同附件保存
        /// </summary>
        /// <param name="file"></param>
        public void ConstractAttachmentSave(ConstractAttachment file)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", file.ConstractId.ToString()), 
                                                       new SqlParameter("@SeqNO", file.SeqNO.ToString()),
                                                       new SqlParameter("@FileName",file.FileName),
                                                        new SqlParameter("@UserId",file.InUserId)};
            db.Database.ExecuteSqlCommand("EXEC up_Constract_AttachmentFile_S @Id,@SeqNO,@FileName,@UserId", para);
        }
        #region 制作合同时，选择确认单类型列表
        public List<QuotationMstDto> Constract_QuotationTypeSearch(string projectId, string supplierId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), 
                                                       new SqlParameter("@SupplierId", supplierId)
            };
            Type t = typeof(QuotationMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Constract_ConstractQuotation_QuotationType_R @ProjectId,@SupplierId", para).Cast<QuotationMstDto>().ToList();
        }
        #endregion
        #region 生成合同时把确认单的信息放到合同内容
        #region 确认单列表_执行
        public List<Quotation_ZhiXingDto> QuotationConstractSearchZhixing(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(Quotation_ZhiXingDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Constract_Zhixing_Dtl_R @ProjectId,@SupplierId,@QuotationId", para).Cast<Quotation_ZhiXingDto>().ToList();
        }
        #endregion
        #region 确认单列表_编程
        public List<Quotation_BianChengDto> QuotationConstractSearchBiancheng(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(Quotation_BianChengDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Constract_Biancheng_Dtl_R @ProjectId,@SupplierId,@QuotationId", para).Cast<Quotation_BianChengDto>().ToList();
        }
        #endregion
        #region 确认单列表_复核
        public List<Quotation_FuHeDto> QuotationConstractSearchFuhe(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(Quotation_FuHeDto);
            return db.Database.SqlQuery(t, "EXEC up_Quotation_Constract_Fuhe_Dtl_R @ProjectId,@SupplierId,@QuotationId", para).Cast<Quotation_FuHeDto>().ToList();
        }
        #endregion
        #region 确认单列表_其他1
        public List<Quotation_QiTa1Dto> QuotationConstractSearchQitao1(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(Quotation_QiTa1Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Constract_Qita1_Dtl_R] @ProjectId,@SupplierId,@QuotationId", para).Cast<Quotation_QiTa1Dto>().ToList();
        }
        #endregion
        #region 确认单列表_其他2
        public List<Quotation_QiTa2Dto> QuotationConstractSearchQitao2(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(Quotation_QiTa2Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Constract_Qita2_Dtl_R] @ProjectId,@SupplierId,@QuotationId", para).Cast<Quotation_QiTa2Dto>().ToList();
        }
        #endregion
        #region 确认单列表_研究
        public List<Quotation_YanJiuDto> QuotationConstractSearchYanjiu(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(Quotation_YanJiuDto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Constract_Yanjiu_Dtl_R] @ProjectId,@SupplierId,@QuotationId", para).Cast<Quotation_YanJiuDto>().ToList();
        }
        #endregion
        #region 确认单列表_支持
        public List<ZhiChi01Dto> QuotationConstractSearchZhichi(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(ZhiChi01Dto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Constract_Zhichi_Dtl_R] @ProjectId,@SupplierId,@QuotationId", para).Cast<ZhiChi01Dto>().ToList();
        }
        #endregion
        #region 确认单列表_车展
        public List<Quotation_CheZhanDto> QuotationConstractSearchChezhan(string quotationId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", ""),
                                                     new SqlParameter("@SupplierId", ""),
                                                     new SqlParameter("@QuotationId",quotationId)
                                                      };
            Type t = typeof(Quotation_CheZhanDto);
            return db.Database.SqlQuery(t, "EXEC [dbo].[up_Quotation_Constract_Chezhan_Dtl_R] @ProjectId,@SupplierId,@QuotationId", para).Cast<Quotation_CheZhanDto>().ToList();
        }
        #endregion
        #endregion
        #endregion
        #region 合同模板登记及查询
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="templateType"></param>
        /// <returns></returns>
        public List<ConstractTemplate> ConstractTemplateSearch(string templateName, string templateType)
        {
            if (templateName == null)
            {
                templateName = "";
            }
            if (templateType == null)
            {
                templateType = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@TemplateName", templateName), 
                                                       new SqlParameter("@TemplateType",templateType) };
            Type t = typeof(ConstractTemplate);
            return db.Database.SqlQuery(t, "EXEC up_ConstractTemplate_R @TemplateName,@TemplateType", para).Cast<ConstractTemplate>().ToList();
        }
        public List<ConstractTemplateDtl> ConstractTemplateDtlSearch(string templateId, string seqNO)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractTemplateId", templateId), 
                                                       new SqlParameter("@SeqNO",seqNO) };
            Type t = typeof(ConstractTemplateDtl);
            return db.Database.SqlQuery(t, "EXEC up_ConstractTemplateDtl_R @ConstractTemplateId,@SeqNO", para).Cast<ConstractTemplateDtl>().ToList();
        }
        public List<ConstractTemplateDtl> ConstractTemplateDtlTitleSearch(string templateId, string seqNO)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractTemplateId", templateId), 
                                                       new SqlParameter("@SeqNO",seqNO) };
            Type t = typeof(ConstractTemplateDtl);
            return db.Database.SqlQuery(t, "EXEC up_ConstractTemplateDtl_Title_R @ConstractTemplateId,@SeqNO", para).Cast<ConstractTemplateDtl>().ToList();
        }
        public int ConstractTemplateSave(ConstractTemplate constract)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", constract.Id.ToString()), 
                                                       new SqlParameter("@TemplateName",constract.TemplateName),
                                                       new SqlParameter("@TemplateType", constract.TemplateType),
                                                       new SqlParameter("@UseChk", constract.UseChk),
                                                       new SqlParameter("@FileName",constract.FileName),
                                                       new SqlParameter("@UserId",constract.InUserId)
            };
            Type t = typeof(int);
            return db.Database.SqlQuery(t, "up_ConstractTemplate_S @Id,@TemplateName,@TemplateType,@UseChk,@FileName,@UserId", para).Cast<int>().First();
        }
        public void ConstractTemplateDtlDelete(ConstractTemplateDtl constract)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractTemplateId", constract.ConstractTemplateId.ToString()), 
                                                       new SqlParameter("@SeqNO", constract.SeqNO.ToString())};
            db.Database.ExecuteSqlCommand("EXEC up_ConstractTemplateDtl_D @ConstractTemplateId,@SeqNO", para);
        }
        public void ConstractTemplateDtlSave(ConstractTemplateDtl constract)
        {
            constract.QuotationChk = false;
            constract.ModifyChk = false;
            if (constract.ContentType2 == null)
            {
                constract.ContentType2 = "";
            }
            if (constract.TemplateContent == null)
            {
                constract.TemplateContent = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractTemplateId", constract.ConstractTemplateId.ToString()), 
                                                       new SqlParameter("@SeqNO", constract.SeqNO.ToString()),
                                                        new SqlParameter("@OrderNO", constract.OrderNO.ToString()),
                                                       new SqlParameter("@ContentType",constract.ContentType),
                                                       new SqlParameter("@ContentType2",constract.ContentType2),
                                                       new SqlParameter("@TemplateContent",constract.TemplateContent),
                                                       new SqlParameter("@QuotationChk",constract.QuotationChk),
                                                       new SqlParameter("@ModifyChk",constract.ModifyChk),
                                                        new SqlParameter("@UserId",constract.InUserId)};
            db.Database.ExecuteSqlCommand("EXEC up_ConstractTemplateDtl_S @ConstractTemplateId,@SeqNO,@OrderNO,@ContentType,@ContentType2,@TemplateContent,@QuotationChk,@ModifyChk,@UserId", para);
        }
        public void ConstractTemplateDtlInsert(ConstractTemplateDtl constract)
        {
            constract.QuotationChk = false;
            constract.ModifyChk = false;
            if (constract.ContentType2 == null)
            {
                constract.ContentType2 = "";
            }
            if (constract.TemplateContent == null)
            {
                constract.TemplateContent = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ConstractTemplateId", constract.ConstractTemplateId.ToString()), 
                                                       new SqlParameter("@SeqNO", constract.SeqNO.ToString()),
                                                        new SqlParameter("@OrderNO", constract.OrderNO.ToString()),
                                                       new SqlParameter("@ContentType",constract.ContentType),
                                                       new SqlParameter("@ContentType2",constract.ContentType2),
                                                       new SqlParameter("@TemplateContent",constract.TemplateContent),
                                                       new SqlParameter("@QuotationChk",constract.QuotationChk),
                                                       new SqlParameter("@ModifyChk",constract.ModifyChk),
                                                        new SqlParameter("@UserId",constract.InUserId)};
            db.Database.ExecuteSqlCommand("EXEC up_ConstractTemplateDtl_I @ConstractTemplateId,@SeqNO,@OrderNO,@ContentType,@ContentType2,@TemplateContent,@QuotationChk,@ModifyChk,@UserId", para);
        }
        #endregion
        #region 系统提醒
        public List<RemindDto> RemindSearch(string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@UserId", userId) };

            Type t = typeof(RemindDto);
            return db.Database.SqlQuery(t, "EXEC up_Remind_R @UserId", para).Cast<RemindDto>().ToList();
        }
        /// <summary>
        /// 首页提醒保存数据
        /// </summary>
        /// <param name="remindType"></param>
        /// <param name="remindId"></param>
        /// <param name="alterUserId"></param>
        public void RemindCancelSave(string remindType, string remindId, string alterUserId, string alterStartDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RemindType", remindType), 
                                                       new SqlParameter("@RemindId", remindId),
                                                        new SqlParameter("@AlterUserId",alterUserId),
                                                        new SqlParameter("@AlterStartDate",alterStartDate),
            };
            db.Database.ExecuteSqlCommand("EXEC up_RemindCancel_S_Type @RemindType,@RemindId,@AlterUserId,@AlterStartDate", para);
        }
        /// <summary>
        /// 首页不再提醒
        /// </summary>
        /// <param name="remindType"></param>
        /// <param name="remindId"></param>
        /// <param name="alterUserId"></param>
        public void RemindCancel(string remindType, string remindId, string alterUserId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RemindType", remindType), 
                                                       new SqlParameter("@RemindId", remindId),
                                                        new SqlParameter("@AlterUserId",alterUserId),
            };
            db.Database.ExecuteSqlCommand("EXEC up_RemindCancel_D @RemindType,@RemindId,@AlterUserId", para);
        }
        #endregion
        #region HiddenCode 查询
        public List<HiddenCode> HiddenCodeSearch(string hiddenCodeType)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@HiddenCodeType", hiddenCodeType) };

            Type t = typeof(HiddenCode);
            return db.Database.SqlQuery(t, "EXEC up_HiddenCode_R @HiddenCodeType", para).Cast<HiddenCode>().ToList();

        }
        #endregion
        #region 用户信息查询
        public List<UserInfoDto> UserInfoSearchByUserId(string userId)
        {
            if (userId == null)
                userId = "";
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@UserId", userId) };
            Type t = typeof(UserInfoDto);
            return db.Database.SqlQuery(t, "EXEC up_UserInfo_SearchByUserId @UserId", para).Cast<UserInfoDto>().ToList();
        }
        // 根据yoghurtId和部门查询账户信息
        public List<UserInfoDto> UserInfoSearch(string userId, string departmentCode, string roleTypeCode)
        {
            if (userId == null)
                userId = "";
            if (departmentCode == null)
                departmentCode = "";
            if (roleTypeCode == null)
                roleTypeCode = "";
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@UserId", userId),
                                                    new SqlParameter("@DepartmentCode", departmentCode),
                                                     new SqlParameter("@RoleTypeCode", roleTypeCode)};
            Type t = typeof(UserInfoDto);
            return db.Database.SqlQuery(t, "EXEC up_UserInfo_R @UserId,@DepartmentCode,@RoleTypeCode", para).Cast<UserInfoDto>().ToList();
        }


        #endregion
        #region 项目授权管理
        // P:项目 S:供应商
        public List<UserInfoDto> SetInfoByUserId(string userId, string userId_set, string type)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@UserId", userId)
                                                     ,new SqlParameter("@UserId_Set", userId_set)
                                                     , new SqlParameter("@Type", type) };
            Type t = typeof(UserInfoDto);
            return db.Database.SqlQuery(t, "EXEC up_UserInfo_SetInfoByUserId_R @UserId,@UserId_Set,@Type", para).Cast<UserInfoDto>().ToList();
        }
        public List<UserInfoDto> SetInfoLogSearch(string setId, string type)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SetId", setId), new SqlParameter("@Type", type), };
            Type t = typeof(UserInfoDto);
            return db.Database.SqlQuery(t, "EXEC up_UserInfo_SetInfoLogBySetId_R @SetId,@Type", para).Cast<UserInfoDto>().ToList();
        }
        public void SetInfoSave(string setId, string userId, string inUserId, string type)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@SetId", setId), 
                                                       new SqlParameter("@UserId", userId),
                                                       new SqlParameter("@InUserId",inUserId),
                                                       new SqlParameter("@Type",type)};
            db.Database.ExecuteSqlCommand("EXEC up_UserInfo_SetInfo_S @SetId,@UserId,@InUserId,@Type", para);
        }
        public void SetInfoSave_New(List<string> setId_All, List<string> setId_selected, string userId_set, string inUserId, string type)
        {
            string sql_del = "";
            sql_del += "DELETE Mst_UserInfoSetLog WHERE SetId IN (";
            foreach (string str in setId_All)
            {
                if (setId_All.IndexOf(str) == setId_All.Count - 1)
                {
                    sql_del += str + ")";
                }
                else
                { sql_del += str + ","; }
            }
            sql_del += " AND [Type] = '" + type + "'";
            sql_del += " AND UserId =  '" + userId_set + "'";
            db.Database.ExecuteSqlCommand(sql_del);

            if (setId_selected != null && setId_selected.Count > 0)
            {
                string sql_insert = "";
                sql_insert += "INSERT INTO Mst_UserInfoSetLog";
                foreach (string str in setId_selected)
                {
                    if (setId_selected.IndexOf(str) == setId_selected.Count - 1)
                    {
                        sql_insert += " SELECT '" + userId_set + "'," + str + ",'" + type + "','" + inUserId + "','" + DateTime.Now.ToString() + "'";
                    }
                    else
                    {
                        sql_insert += " SELECT '" + userId_set + "'," + str + ",'" + type + "','" + inUserId + "','" + DateTime.Now.ToString() + "' UNION";
                    }
                }
                db.Database.ExecuteSqlCommand(sql_insert);
            }
        }
        #endregion
        #region 不可预见费
        public List<ContingencyDto> ContingencySearch(string projectId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId) };
            Type t = typeof(ContingencyDto);
            return db.Database.SqlQuery(t, "EXEC up_Contingency_R @ProjectId", para).Cast<ContingencyDto>().ToList();
        }
        public void ContingencySave(ContingencyDto file)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", file.ProjectId.ToString()), 
                                                       new SqlParameter("@ContingencyFee", file.ContingencyFee.ToString()),
                                                        new SqlParameter("@UserId",file.InUserId)};
            db.Database.ExecuteSqlCommand("EXEC up_Constract_AttachmentFile_S @ProjectId,@ContingencyFee,@UserId", para);
        }
        #endregion
        #region 用户自定义查询
        #region  针对统计方式列表，Count SUM
        /// <summary>
        /// Type:(List:列表,Count:计数,Sum:求和)
        /// 
        /// List<string> subType:确认单，结算单,List<string> subTypeBase:城市样本量
        /// 针对统计方式列表，Count SUM
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="result"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CustomerColumnDto> CustomerSearch(Dictionary<string, string> condition, List<string> result, string type, List<string> subType, List<string> subTypeBase)
        {
            string startdate = "1900-01-01";// 
            string enddate = "2030-12-31";
            // 未选择开始时间或者结束时间的时候
            if (condition.ContainsKey("StartDate"))
            {
                startdate = condition["StartDate"].ToString();
            }
            if (condition.ContainsKey("EndDate"))
            {
                enddate = condition["EndDate"].ToString();
            }
            List<string> bianchengList = new List<string>();
            List<string> chezhanList = new List<string>();
            List<string> fuheList = new List<string>();
            List<string> qita1List = new List<string>();
            List<string> qita2List = new List<string>();
            List<string> yanjiuList = new List<string>();
            List<string> zhichiList = new List<string>();
            List<string> zhixingList = new List<string>();
            List<string> baseList = new List<string>();
            // 区分查询结果所属的汇总表类型，相同类型添加到同一列表
            List<CustomerColumn> customerColumnList = db.CustomerColumn.Where(x => x.ColumnName != "").ToList();
            foreach (CustomerColumn col in customerColumnList)
            {
                foreach (string str in result)
                {
                    if (str == col.ColumnName)
                    {
                        if (col.QuotationType == "Base") baseList.Add(str);
                        if (col.QuotationType == "Biancheng") bianchengList.Add(str);
                        if (col.QuotationType == "Chanzhan") chezhanList.Add(str);
                        if (col.QuotationType == "Fuhe") fuheList.Add(str);
                        if (col.QuotationType == "Qita1") qita1List.Add(str);
                        if (col.QuotationType == "Qita2") qita2List.Add(str);
                        if (col.QuotationType == "Yanjiu") yanjiuList.Add(str);
                        if (col.QuotationType == "Zhichi") zhichiList.Add(str);
                        if (col.QuotationType == "Zhixing") zhixingList.Add(str);
                    }
                }
            }
            // 区分查询条件所属的汇总表类型，相同的类型添加到同一列表
            List<string> bianchengList2 = new List<string>();
            List<string> chezhanList2 = new List<string>();
            List<string> fuheList2 = new List<string>();
            List<string> qita1List2 = new List<string>();
            List<string> qita2List2 = new List<string>();
            List<string> yanjiuList2 = new List<string>();
            List<string> zhichiList2 = new List<string>();
            List<string> zhixingList2 = new List<string>();
            List<string> baseList2 = new List<string>();
            foreach (CustomerColumn col in customerColumnList)
            {
                foreach (KeyValuePair<string, string> kv in condition)
                {
                    if (kv.Key == col.ColumnName && !string.IsNullOrEmpty(kv.Value))
                    {
                        if (col.QuotationType == "Base") baseList2.Add(kv.Key);
                        if (col.QuotationType == "Biancheng") bianchengList2.Add(kv.Key);
                        if (col.QuotationType == "Chanzhan") chezhanList2.Add(kv.Key);
                        if (col.QuotationType == "Fuhe") fuheList2.Add(kv.Key);
                        if (col.QuotationType == "Qita1") qita1List2.Add(kv.Key);
                        if (col.QuotationType == "Qita2") qita2List2.Add(kv.Key);
                        if (col.QuotationType == "Yanjiu") yanjiuList2.Add(kv.Key);
                        if (col.QuotationType == "Zhichi") zhichiList2.Add(kv.Key);
                        if (col.QuotationType == "Zhixing") zhixingList2.Add(kv.Key);
                    }
                }
            }

            if (!((bianchengList.Count + baseList.Count == result.Count
                    && bianchengList2.Count + baseList2.Count == condition.Keys.Count)
                || (chezhanList.Count + baseList.Count == result.Count
                   && chezhanList2.Count + baseList2.Count == condition.Keys.Count)
                || (fuheList.Count + baseList.Count == result.Count
                   && fuheList2.Count + baseList2.Count == condition.Keys.Count)
                || (qita1List.Count + baseList.Count == result.Count
                   && qita1List2.Count + baseList2.Count == condition.Keys.Count)
                || (qita2List.Count + baseList.Count == result.Count
                   && qita2List2.Count + baseList2.Count == condition.Keys.Count)
                || (yanjiuList.Count + baseList.Count == result.Count
                   && yanjiuList2.Count + baseList2.Count == condition.Keys.Count)
                || (zhichiList.Count + baseList.Count == result.Count
                  && zhichiList2.Count + baseList2.Count == condition.Keys.Count)
                || (zhixingList.Count + baseList.Count == result.Count
                   && zhixingList2.Count + baseList2.Count == condition.Keys.Count)
                ))
            {
                throw new Exception("所选条件不属于统一汇总表类型");
            }

            List<CustomerColumnDto> returnList = new List<CustomerColumnDto>();
            string sql = "";
            if (type == "List")
            {
                sql += "SELECT * FROM (";
                if (bianchengList.Count + baseList.Count == result.Count
                    && bianchengList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Biancheng_R");
                    sql += " UNION ";
                }
                if (chezhanList.Count + baseList.Count == result.Count
                   && chezhanList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Chezhan_R");
                    sql += " UNION ";
                }
                if (fuheList.Count + baseList.Count == result.Count
                   && fuheList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Fuhe_R");
                    sql += " UNION ";
                }
                if (qita1List.Count + baseList.Count == result.Count
                   && qita1List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Qita1_R");
                    sql += " UNION ";
                }
                if (qita2List.Count + baseList.Count == result.Count
                   && qita2List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Qita2_R");
                    sql += " UNION ";
                }
                if (yanjiuList.Count + baseList.Count == result.Count
                   && yanjiuList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Yanjiu_R");
                    sql += " UNION ";
                }
                if (zhichiList.Count + baseList.Count == result.Count
                  && zhichiList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Zhichi_R");
                    sql += " UNION ";
                }
                if (zhixingList.Count + baseList.Count == result.Count
                   && zhixingList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Zhixing_R");
                }
                if (sql.Substring(sql.Length - 6, 6) == "UNION ")
                {
                    sql = sql.Substring(0, sql.Length - 6);
                }
                sql += ") A";
                returnList = db.Database.SqlQuery(typeof(CustomerColumnDto)
                       , sql).Cast<CustomerColumnDto>().ToList();
            }
            else if (type == "Count")
            {
                sql += "SELECT ";
                foreach (string str in result)
                {
                    sql += str + ",";
                }
                sql += " CAST(Count(*) AS NVARCHAR) AS shuliang FROM (";
                // sql += "SELECT * FROM (";
                if (bianchengList.Count + baseList.Count == result.Count
                    && bianchengList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Biancheng_R");
                    sql += " UNION ALL ";
                }
                if (chezhanList.Count + baseList.Count == result.Count
                  && chezhanList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Chezhan_R");
                    sql += " UNION ALL ";
                }
                if (fuheList.Count + baseList.Count == result.Count
                   && fuheList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Fuhe_R");
                    sql += " UNION ALL ";
                }
                if (qita1List.Count + baseList.Count == result.Count
                   && qita1List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Qita1_R");
                    sql += " UNION ALL ";
                }
                if (qita2List.Count + baseList.Count == result.Count
                    && qita2List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Qita2_R");
                    sql += " UNION ALL ";
                }
                if (yanjiuList.Count + baseList.Count == result.Count
                   && yanjiuList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Yanjiu_R");
                    sql += " UNION ALL ";
                }
                if (zhichiList.Count + baseList.Count == result.Count
                   && zhichiList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Zhichi_R");
                    sql += " UNION ALL ";
                }
                if (zhixingList.Count + baseList.Count == result.Count
                   && zhixingList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Zhixing_R");
                }
                if (sql.Substring(sql.Length - 10, 10) == "UNION ALL ")
                {
                    sql = sql.Substring(0, sql.Length - 10);
                }
                sql += ") A";
                sql += " GROUP BY ";
                foreach (string str in result)
                {
                    if (result.IndexOf(str) == result.Count - 1)
                    {
                        sql += str;
                    }
                    else
                    {
                        sql += str + ",";
                    }
                }

                returnList = db.Database.SqlQuery(typeof(CustomerColumnDto)
                       , sql).Cast<CustomerColumnDto>().ToList();
            }
            else if (type == "Sum")
            {
                #region  查询汇总表对应的城市样本量
                List<string> yangbenliangtiaojian = new List<string>();
                List<string> resultLast = new List<string>();
                yangbenliangtiaojian.Add("ProjectName");
                yangbenliangtiaojian.Add("ProjectShortName");
                yangbenliangtiaojian.Add("ModelType");
                yangbenliangtiaojian.Add("ServiceRegion");
                yangbenliangtiaojian.Add("DepartmentCode");
                yangbenliangtiaojian.Add("CostDepartment");
                yangbenliangtiaojian.Add("ProjectType");
                yangbenliangtiaojian.Add("ExcuteMode");
                yangbenliangtiaojian.Add("PurchaseType");
                yangbenliangtiaojian.Add("PurchaseMode");
                yangbenliangtiaojian.Add("SupplyService");
                yangbenliangtiaojian.Add("ItemName");

                foreach (string str in result)
                {
                    if (!yangbenliangtiaojian.Contains(str))
                    {
                        resultLast.Add(str);
                    }
                }
                // 先查询汇总表的信息，主要是为了查询城市样本量
                sql = "SELECT DISTINCT * INTO #huizongbiao FROM ( ";
                if (bianchengList.Count + baseList.Count == result.Count
                   && bianchengList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Biancheng_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                    sql += " UNION ALL ";
                } if (chezhanList.Count + baseList.Count == result.Count
                  && chezhanList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName  ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Chezhan_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1 ";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                    sql += " UNION ALL ";
                }
                if (fuheList.Count + baseList.Count == result.Count
                 && fuheList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Fuhe_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1 ";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                    sql += " UNION ALL ";
                }
                if (qita1List.Count + baseList.Count == result.Count
                 && qita1List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Qita1_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1 ";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                    sql += " UNION ALL ";
                }
                if (qita2List.Count + baseList.Count == result.Count
                   && qita2List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Qita2_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1 ";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                    sql += " UNION ALL ";
                }
                if (yanjiuList.Count + baseList.Count == result.Count
                   && yanjiuList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Yanjiu_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1 ";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                    sql += " UNION ALL ";
                }
                if (zhichiList.Count + baseList.Count == result.Count
                  && zhichiList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Zhichi_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1 ";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                    sql += " UNION ALL ";
                }
                if (zhixingList.Count + baseList.Count == result.Count
                  && zhixingList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sql += "SELECT ISNULL(ProjectName,'') AS ProjectName,ISNULL(ProjectShortName,'') AS ProjectShortName,ISNULL(ModelType,'') AS ModelType,ISNULL(ServiceRegion,'') AS ServiceRegion,ISNULL(DepartmentCode,'') AS DepartmentCode,ISNULL(CostDepartment,'') AS CostDepartment,ISNULL(Province,'') AS Province,ISNULL(City,'') AS City,ISNULL(ProjectType,'') AS ProjectType,ISNULL(ExcuteMode1,'') AS ExcuteMode1,ISNULL(ExcuteMode,'') AS ExcuteMode,ISNULL(PurchaseType,'') AS PurchaseType,ISNULL(PurcheaseMode,'') AS PurcheaseMode,ISNULL(SupplyService,'') AS SupplyService,ISNULL(ItemName,'') AS ItemName ";
                    if (resultLast.Count > 0)
                    {
                        sql += ",";
                        foreach (string str in resultLast)
                        {
                            if (resultLast.IndexOf(str) == resultLast.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                    sql += " FROM fn_Stat_Huizongbiao_Zhixing_R" + "('" + startdate + "','" + enddate + "')";
                    sql += " A WHERE 1=1 AND Selected = 1 ";
                    foreach (KeyValuePair<string, string> kv in condition)
                    {
                        if (kv.Key != "StartDate" && kv.Key != "EndDate")
                        {
                            sql += " AND " + kv.Key + " IN(";
                            string[] valueList = kv.Value.Split(';');
                            for (int i = 0; i < valueList.Length; i++)
                            {
                                if (i == valueList.Length - 1)
                                {
                                    sql += "'" + valueList[i] + "')";
                                }
                                else
                                {
                                    sql += "'" + valueList[i] + "',";
                                }
                            }
                        }
                    }
                }
                sql += ") X ";

                if (sql.Substring(sql.Length - 10, 10) == "UNION ALL ")
                {
                    sql = sql.Substring(0, sql.Length - 10);
                }
                #endregion
                #region 查询确认单或者结算单的合计

                // 先查询每个类型的合计
                string sqlSum = "";
                //  sqlSum += "SELECT * INTO #T FROM ( ";
                if (bianchengList.Count + baseList.Count == result.Count
                  && bianchengList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Biancheng_R");
                    if (!string.IsNullOrEmpty(sqlSum))
                    {
                        sqlSum += " UNION ALL ";
                    }
                }
                if (chezhanList.Count + baseList.Count == result.Count
                  && chezhanList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Chezhan_R");
                    if (!string.IsNullOrEmpty(sqlSum))
                    {
                        sqlSum += " UNION ALL ";
                    }
                }
                if (fuheList.Count + baseList.Count == result.Count
                   && fuheList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Fuhe_R");
                    if (!string.IsNullOrEmpty(sqlSum))
                    {
                        sqlSum += " UNION ALL ";
                    }
                }
                if (qita1List.Count + baseList.Count == result.Count
                   && qita1List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Qita1_R");
                    if (!string.IsNullOrEmpty(sqlSum))
                    {
                        sqlSum += " UNION ALL ";
                    }
                }
                if (qita2List.Count + baseList.Count == result.Count
                   && qita2List2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Qita2_R");
                    if (!string.IsNullOrEmpty(sqlSum))
                    {
                        sqlSum += " UNION ALL ";
                    }
                }
                if (yanjiuList.Count + baseList.Count == result.Count
                   && yanjiuList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Yanjiu_R");
                    if (!string.IsNullOrEmpty(sqlSum))
                    {
                        sqlSum += " UNION ALL ";
                    }
                }
                if (zhichiList.Count + baseList.Count == result.Count
                   && zhichiList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Zhichi_R");
                    if (!string.IsNullOrEmpty(sqlSum))
                    {
                        sqlSum += " UNION ALL ";
                    }
                }
                if (zhixingList.Count + baseList.Count == result.Count
                   && zhixingList2.Count + baseList2.Count == condition.Keys.Count)
                {
                    sqlSum += GetSqlStr(condition, result, type, subType, "fn_Stat_Huizongbiao_Zhixing_R");
                }
                // 如果有数据的话生成#T
                if (!string.IsNullOrEmpty(sqlSum))
                {
                    sql += "SELECT * INTO #T FROM (";
                    sql += sqlSum;
                }
                if (sql.Substring(sql.Length - 10, 10) == "UNION ALL ")
                {
                    sqlSum = sql.Substring(0, sql.Length - 10);
                }
                if (!string.IsNullOrEmpty(sqlSum))
                {
                    sql += ") Y ";
                }
                // 当#T 有数据的时候生成#Sum,计算多个确认单类型的确认单和结算单合计
                if (!string.IsNullOrEmpty(sqlSum))
                {
                    // 计算各类型总合计
                    sql += " SELECT ";
                    foreach (string str in result)
                    {
                        // 如果勾选了确认单或者结算单，最后一个必须是逗号
                        if (subType.Count > 0)
                        {
                            sql += str + ",";
                        }
                        else // 没有勾选的话最后一个不是逗号
                        {
                            if (result.IndexOf(str) == result.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }

                        }
                    }
                    foreach (string sub in subType)
                    {
                        sql += GetfenhaoStr(sub, subType);
                    }
                    if (sql.Substring(sql.Length - 1, 1) == ",")
                    {
                        sql = sql.Substring(0, sql.Length - 1);
                    }
                    sql += " INTO #Sum FROM #T A";

                    if (result.Count > 0 && subType.Count > 0)
                    {
                        sql += " GROUP BY ";
                        foreach (string str in result)
                        {
                            if (result.IndexOf(str) == result.Count - 1)
                            {
                                sql += str;
                            }
                            else
                            {
                                sql += str + ",";
                            }
                        }
                    }
                }
                #endregion
                #region 查询城市样本量和总合计

                // 查询城市样本量和总合计

                // 如果有#Sum的时候

                if (!string.IsNullOrEmpty(sqlSum))
                {
                    sql += " SELECT DISTINCT B.*";
                }
                else
                {
                    sql += " SELECT DISTINCT ";
                }
                // 如果勾选了城市样本量
                // 只勾选了城市样本量的时候，不需要添加逗号
                if ((subType.Count == 0 && result.Count == 0) && subTypeBase.Count == 1)
                {
                    sql += " (SELECT CAST(chengshiyangbenliang AS NVARCHAR) FROM fn_Stat_ProjectCity_R(A.ProjectName,A.ProjectShortName,A.ModelType,A.ServiceRegion,A.DepartmentCode,A.CostDepartment,A.Province,A.City,A.ProjectType,A.ExcuteMode1,A.PurchaseType,A.PurcheaseMode,A.SupplyService,A.ItemName,"
                    + "'" + startdate + "'," + "'" + enddate + "')) AS chengshiyangbenliang ";
                }
                else
                {
                    sql += ",(SELECT CAST(chengshiyangbenliang AS NVARCHAR) FROM fn_Stat_ProjectCity_R(A.ProjectName,A.ProjectShortName,A.ModelType,A.ServiceRegion,A.DepartmentCode,A.CostDepartment,A.Province,A.City,A.ProjectType,A.ExcuteMode1,A.PurchaseType,A.PurcheaseMode,A.SupplyService,A.ItemName,"
                   + "'" + startdate + "'," + "'" + enddate + "')) AS chengshiyangbenliang ";
                }
                sql += " INTO #Last FROM #huizongbiao A ";
                // 当#T 有数据的时候生成#Sum
                if (!string.IsNullOrEmpty(sqlSum))
                {
                    sql += "INNER JOIN #Sum B ON 1=1";

                    if (result.Count > 0)
                    {
                        sql += " AND  ";
                        foreach (string str in result)
                        {
                            if (result.IndexOf(str) == result.Count - 1)
                            {
                                sql += "A." + str + "=B." + str;
                            }
                            else
                            {
                                sql += "A." + str + "=B." + str + " AND ";
                            }
                        }
                    }
                }
                #endregion
                #region 根据result计算城市样本量的合计，因为同一个result可能对应多条城市样本量

                sql += " SELECT ";
                if (result.Count > 0)
                {
                    foreach (string str in result)
                    {
                        if (result.IndexOf(str) == result.Count - 1)
                        {
                            sql += str;
                        }
                        else
                        {
                            sql += str + ",";
                        }
                    }
                }
                // 如果勾选了城市样本量
                // 计算城市样本量的合计，只和result有关，和subType无关
                if (result.Count == 0)
                {
                    sql += "sum(CAST(chengshiyangbenliang AS DECIMAL(19,2))) chengshiyangbenliang";
                }
                else
                {
                    sql += " ,sum(CAST(chengshiyangbenliang AS DECIMAL(19,2))) chengshiyangbenliang";
                }
                sql += " INTO #chengshiyangbenliangSum FROM #Last";
                if (result.Count > 0)
                {
                    sql += " GROUP BY ";
                    foreach (string str in result)
                    {
                        if (result.IndexOf(str) == result.Count - 1)
                        {
                            sql += str;
                        }
                        else
                        {
                            sql += str + ",";
                        }
                    }
                }

                sql += " SELECT DISTINCT ";
                if (!string.IsNullOrEmpty(sqlSum))
                {
                    sql += "B.*";
                }
                if ((subType.Count == 0 && result.Count == 0) && subTypeBase.Count == 1)
                {
                    sql += "CAST(chengshiyangbenliang AS NVARCHAR) AS chengshiyangbenliang";
                }
                else
                {
                    sql += ",CAST(chengshiyangbenliang AS NVARCHAR) AS chengshiyangbenliang";
                }
                sql += " FROM #chengshiyangbenliangSum A ";
                if (!string.IsNullOrEmpty(sqlSum))
                {
                    sql += "INNER JOIN #Sum B ON 1=1";

                    if (result.Count > 0)
                    {
                        sql += " AND  ";
                        foreach (string str in result)
                        {
                            if (result.IndexOf(str) == result.Count - 1)
                            {
                                sql += "A." + str + "=B." + str;
                            }
                            else
                            {
                                sql += "A." + str + "=B." + str + " AND ";
                            }
                        }
                    }
                }
                #endregion
                sql += " DROP TABLE #huizongbiao ";
                if (!string.IsNullOrEmpty(sqlSum))
                {
                    sql += " DROP TABLE #Sum DROP TABLE #T  ";
                }
                returnList = db.Database.SqlQuery(typeof(CustomerColumnDto)
                     , sql).Cast<CustomerColumnDto>().ToList();
            }
            return returnList;
        }
        public string GetSqlStr(Dictionary<string, string> condition, List<string> result, string type, List<string> subType, string huizongbiaoType)
        {
            string startdate = "1900-01-01";// 
            string enddate = "2030-12-31";
            // 未选择开始时间或者结束时间的时候
            if (condition.ContainsKey("StartDate"))
            {
                startdate = condition["StartDate"].ToString();
            }
            if (condition.ContainsKey("EndDate"))
            {
                enddate = condition["EndDate"].ToString();
            }
            string sql = "";
            // 如果选择了Sum,且没有勾选任何查询结果和也没有勾选结算单和确认单，直接返回空
            if (type == "Sum" && result.Count == 0 && subType.Count == 0)
            {
                return "";
            }
            if (type == "List")
            {
                sql += "SELECT DISTINCT "; // 如果是List 去掉重复项
                foreach (string str in result)
                {
                    if ((result.IndexOf(str) == result.Count - 1))
                    {
                        sql += str;
                    }
                    else
                    {
                        sql += str + ",";
                    }
                }
            }
            else if (type == "Count")
            {
                sql += "SELECT  "; // 如果是List 去掉重复项
                foreach (string str in result)
                {
                    if ((result.IndexOf(str) == result.Count - 1))
                    {
                        sql += str;
                    }
                    else
                    {
                        sql += str + ",";
                    }
                }
            }
            else if (type == "Sum")
            {
                sql += " SELECT DISTINCT ";
                foreach (string str in result)
                {
                    if ((result.IndexOf(str) == result.Count - 1) && subType.Count == 0)
                    {
                        sql += str;
                    }
                    else
                    {
                        sql += str + ",";
                    }
                }
            }
            //if (type == "Count")
            //{
            //    sql += " CAST(Count(*) AS NVARCHAR) AS shuliang";
            //}
            if (type == "Sum")
            {
                foreach (string sub in subType)
                {
                    sql += GetfenhaoStr(sub, subType);
                }
            }
            sql += " FROM " + huizongbiaoType + "('" + startdate + "','" + enddate + "')";
            sql += " A WHERE 1=1 AND Selected = 1 ";
            foreach (KeyValuePair<string, string> kv in condition)
            {
                if (kv.Key != "StartDate" && kv.Key != "EndDate")
                {
                    sql += " AND " + kv.Key + " IN(";
                    string[] valueList = kv.Value.Split(';');
                    for (int i = 0; i < valueList.Length; i++)
                    {
                        if (i == valueList.Length - 1)
                        {
                            sql += "'" + valueList[i] + "')";
                        }
                        else
                        {
                            sql += "'" + valueList[i] + "',";
                        }
                    }
                }
            }
            if (result.Count > 0)
            {
                sql += "AND (";
                // 查询结果全都为空的数据排除掉
                foreach (string str in result)
                {
                    if (result.IndexOf(str) == result.Count - 1)
                    { sql += "ISNULL(" + str + "," + "'')<>'' )"; }
                    else
                    {
                        sql += "ISNULL(" + str + "," + "'')<>'' OR ";
                    }
                }
            }
            if ((
                //type == "Count" || 
                type == "Sum") && result.Count > 0)
            {
                sql += " GROUP BY ";
                foreach (string str in result)
                {
                    if (result.IndexOf(str) == result.Count - 1)
                    {
                        sql += str;
                    }
                    else
                    {
                        sql += str + ",";
                    }
                }
            }
            return sql;
        }
        #endregion
        #region 针对统计方式Count(去重)


        /// <summary>
        /// 针对统计方式Count(去重)
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="result"></param>
        /// <param name="type"></param>
        /// <param name="countColumn"></param>
        /// <returns></returns>
        public List<CustomerColumnDto> CustomerSearchDistinctCount(Dictionary<string, string> condition, List<string> result, string type, string countColumn)
        {
            List<string> bianchengList = new List<string>();
            List<string> chezhanList = new List<string>();
            List<string> fuheList = new List<string>();
            List<string> qita1List = new List<string>();
            List<string> qita2List = new List<string>();
            List<string> yanjiuList = new List<string>();
            List<string> zhichiList = new List<string>();
            List<string> zhixingList = new List<string>();
            List<string> baseList = new List<string>();
            // 区分查询结果所属的汇总表类型，相同类型添加到同一列表
            List<CustomerColumn> customerColumnList = db.CustomerColumn.Where(x => x.ColumnName != "").ToList();
            foreach (CustomerColumn col in customerColumnList)
            {
                foreach (string str in result)
                {
                    if (str == col.ColumnName)
                    {
                        if (col.QuotationType == "Base") baseList.Add(str);
                        if (col.QuotationType == "Biancheng") bianchengList.Add(str);
                        if (col.QuotationType == "Chanzhan") chezhanList.Add(str);
                        if (col.QuotationType == "Fuhe") fuheList.Add(str);
                        if (col.QuotationType == "Qita1") qita1List.Add(str);
                        if (col.QuotationType == "Qita2") qita2List.Add(str);
                        if (col.QuotationType == "Yanjiu") yanjiuList.Add(str);
                        if (col.QuotationType == "Zhichi") zhichiList.Add(str);
                        if (col.QuotationType == "Zhixing") zhixingList.Add(str);
                    }
                }
            }
            // 区分查询条件所属的汇总表类型，相同的类型添加到同一列表
            List<string> bianchengList2 = new List<string>();
            List<string> chezhanList2 = new List<string>();
            List<string> fuheList2 = new List<string>();
            List<string> qita1List2 = new List<string>();
            List<string> qita2List2 = new List<string>();
            List<string> yanjiuList2 = new List<string>();
            List<string> zhichiList2 = new List<string>();
            List<string> zhixingList2 = new List<string>();
            List<string> baseList2 = new List<string>();
            foreach (CustomerColumn col in customerColumnList)
            {
                foreach (KeyValuePair<string, string> kv in condition)
                {
                    if (kv.Key == col.ColumnName && !string.IsNullOrEmpty(kv.Value))
                    {
                        if (col.QuotationType == "Base") baseList2.Add(kv.Key);
                        if (col.QuotationType == "Biancheng") bianchengList2.Add(kv.Key);
                        if (col.QuotationType == "Chanzhan") chezhanList2.Add(kv.Key);
                        if (col.QuotationType == "Fuhe") fuheList2.Add(kv.Key);
                        if (col.QuotationType == "Qita1") qita1List2.Add(kv.Key);
                        if (col.QuotationType == "Qita2") qita2List2.Add(kv.Key);
                        if (col.QuotationType == "Yanjiu") yanjiuList2.Add(kv.Key);
                        if (col.QuotationType == "Zhichi") zhichiList2.Add(kv.Key);
                        if (col.QuotationType == "Zhixing") zhixingList2.Add(kv.Key);
                    }
                }
            }
            if (!((bianchengList.Count + baseList.Count == result.Count
                  && bianchengList2.Count + baseList2.Count == condition.Keys.Count)
              || (chezhanList.Count + baseList.Count == result.Count
                 && chezhanList2.Count + baseList2.Count == condition.Keys.Count)
              || (fuheList.Count + baseList.Count == result.Count
                 && fuheList2.Count + baseList2.Count == condition.Keys.Count)
              || (qita1List.Count + baseList.Count == result.Count
                 && qita1List2.Count + baseList2.Count == condition.Keys.Count)
              || (qita2List.Count + baseList.Count == result.Count
                 && qita2List2.Count + baseList2.Count == condition.Keys.Count)
              || (yanjiuList.Count + baseList.Count == result.Count
                 && yanjiuList2.Count + baseList2.Count == condition.Keys.Count)
              || (zhichiList.Count + baseList.Count == result.Count
                && zhichiList2.Count + baseList2.Count == condition.Keys.Count)
              || (zhixingList.Count + baseList.Count == result.Count
                 && zhixingList2.Count + baseList2.Count == condition.Keys.Count)
              ))
            {
                throw new Exception("所选条件不属于统一汇总表类型");
            }
            List<CustomerColumnDto> returnList = new List<CustomerColumnDto>();
            string sql = "";
            sql += "SELECT ";
            foreach (string str in result)
            {
                sql += str + ",";
            }
            sql += "CAST(Count(" + countColumn + ") AS NVARCHAR) AS shuliang FROM (";
            if (bianchengList.Count + baseList.Count == result.Count
                   && bianchengList2.Count + baseList2.Count == condition.Keys.Count)
            {
                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Biancheng_R");
                sql += " UNION ";
            }
            if (chezhanList.Count + baseList.Count == result.Count
                  && chezhanList2.Count + baseList2.Count == condition.Keys.Count)
            {

                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Chezhan_R");
                sql += " UNION ";
            }
            if (fuheList.Count + baseList.Count == result.Count
                   && fuheList2.Count + baseList2.Count == condition.Keys.Count)
            {
                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Fuhe_R");
                sql += " UNION ";
            }
            if (qita1List.Count + baseList.Count == result.Count
                   && qita1List2.Count + baseList2.Count == condition.Keys.Count)
            {
                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Qita1_R");
                sql += " UNION ";
            }
            if (qita2List.Count + baseList.Count == result.Count
                    && qita2List2.Count + baseList2.Count == condition.Keys.Count)
            {
                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Qita2_R");
                sql += " UNION ";
            }
            if (yanjiuList.Count + baseList.Count == result.Count
                   && yanjiuList2.Count + baseList2.Count == condition.Keys.Count)
            {
                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Yanjiu_R");
                sql += " UNION ";
            }
            if (zhichiList.Count + baseList.Count == result.Count
                   && zhichiList2.Count + baseList2.Count == condition.Keys.Count)
            {
                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Zhichi_R");
                sql += " UNION ";
            }
            if (zhixingList.Count + baseList.Count == result.Count
                   && zhixingList2.Count + baseList2.Count == condition.Keys.Count)
            {
                sql += GetSqlStrDistinctCount(condition, result, type, countColumn, "fn_Stat_Huizongbiao_Zhixing_R");
            }
            // 如果最后以UNION 结尾，去掉UNION
            if (sql.Substring(sql.Length - 6, 6) == "UNION ")
            {
                sql = sql.Substring(0, sql.Length - 6);
            }
            sql += ") A";
            if (result.Count > 0)
            {
                sql += " GROUP BY ";
            }
            foreach (string str in result)
            {
                if (result.IndexOf(str) == result.Count - 1)
                {
                    sql += str;
                }
                else
                {
                    sql += str + ",";
                }
            }

            return db.Database.SqlQuery(typeof(CustomerColumnDto)
                         , sql).Cast<CustomerColumnDto>().ToList();

        }
        public string GetSqlStrDistinctCount(Dictionary<string, string> condition, List<string> result, string type, string countColumn, string huizongbiaoType)
        {
            string startdate = "1900-01-01";// 
            string enddate = "2030-12-31";
            // 未选择开始时间或者结束时间的时候
            if (condition.ContainsKey("StartDate"))
            {
                startdate = condition["StartDate"].ToString();
            }
            if (condition.ContainsKey("EndDate"))
            {
                enddate = condition["EndDate"].ToString();
            }
            string sql = "";
            sql += "SELECT ";
            foreach (string str in result)
            {
                if (result.IndexOf(str) == result.Count - 1)
                {
                    sql += str;
                }
                else
                {
                    sql += str + ",";
                }
            }
            if (!result.Contains(countColumn))
            {
                if (result.Count > 0)
                {
                    sql += "," + countColumn;
                }
                else
                {
                    sql += countColumn;
                }
            }
            sql += " FROM " + huizongbiaoType + "('" + startdate + "','" + enddate + "')";
            sql += " WHERE 1=1 AND Selected = 1 ";
            foreach (KeyValuePair<string, string> kv in condition)
            {
                if (kv.Key != "StartDate" && kv.Key != "EndDate")
                {
                    sql += " AND " + kv.Key + " IN(";
                    string[] valueList = kv.Value.Split(';');
                    for (int i = 0; i < valueList.Length; i++)
                    {
                        if (i == valueList.Length - 1)
                        {
                            sql += "'" + valueList[i] + "')";
                        }
                        else
                        {
                            sql += "'" + valueList[i] + "',";
                        }
                    }
                }
            }
            //sql += "/r/n";
            return sql;
        }
        #endregion
        public string GetfenhaoStr(string sub, List<string> subType)
        {
            string sql = "";
            if (subType.IndexOf(sub) == subType.Count - 1)
            {
                sql += "CAST(ISNULL(SUM(CAST(" + sub + " AS DECIMAL(19,2))),0) AS NVARCHAR)" + sub;
            }
            else
            {
                sql += "CAST(ISNULL(SUM(CAST(" + sub + " AS DECIMAL(19,2))),0) AS NVARCHAR)" + sub + ",";
            }
            return sql;
        }

        #endregion
    }
}
