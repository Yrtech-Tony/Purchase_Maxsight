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
    public class RequirementService
    {
        PurchaseEntities db = new PurchaseEntities();

        #region 需求书查询
        /// <summary>
        /// 查询需求书的基本信息 
        /// </summary>
        /// <param name="modelType">模块类型</param>  
        /// <param name="projectCode"></param>
        /// <param name="projectName"></param>
        /// <param name="projectShortName"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="historyChk">是:1,否：0，全部:""</param>
        /// <returns></returns>
        public List<RequiremetMstDto> RequirementMstSearch(string modelType, string projectCode, string projectName, string projectShortName, string province, string city,string historyChk,string userId,string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", modelType), 
                                                       new SqlParameter("@ProjectCode ",projectCode),
                                                       new SqlParameter("@ProjectName", projectName),
                                                       new SqlParameter("@ProjectShortName", projectShortName),
                                                       new SqlParameter("@Province",province),
                                                       new SqlParameter("@City", city),
                                                        new SqlParameter("@HistoryChk", historyChk),
                                                         new SqlParameter("@GroupId", groupId),
                                                     new SqlParameter("@UserId", userId)};
            Type t = typeof(RequiremetMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_RequirementMst_R @ModelType,@ProjectCode,@ProjectName,@ProjectShortName,@Province,@City,@HistoryChk,@GroupId,@UserId", para).Cast<RequiremetMstDto>().ToList();
        }
        public List<RequiremetMstDto> RequirementMstSearchById(string quirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", quirementId)};
            Type t = typeof(RequiremetMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_RequirementMstById_R @Id", para).Cast<RequiremetMstDto>().ToList();
        }
       
        public int RequirementMstSave(Requirement_RequirementMst requirementMst)
        {
            if (requirementMst.RequirementGroupId == null)
            {
                requirementMst.RequirementGroupId = 0;
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", requirementMst.Id), 
                                                       new SqlParameter("@ProjectId",requirementMst.ProjectId),
                                                       new SqlParameter("@CitySeqNO", requirementMst.CitySeqNo),
                                                       new SqlParameter("@RequirementType", requirementMst.RequirementType),
                                                       new SqlParameter("@UserId",requirementMst.InUserId),
                                                        new SqlParameter("@RequirementGroupId",requirementMst.RequirementGroupId)};
            Type t = typeof(int);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_RequirementMst_S @Id,@ProjectId,@CitySeqNO,@RequirementType,@UserId,@RequirementGroupId", para).Cast<int>().First();
        }
        public List<RequirementEmailSendDto> RequirementEmailSendSearch(string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementId) };
            Type t = typeof(RequirementEmailSendDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_RequirementEmailSend_R @RequirementId", para).Cast<RequirementEmailSendDto>().ToList();
        }
        public void RequirementDeleteById(string requirementid)
        {
            if (requirementid == null)
            {
                requirementid = "";
            
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementid) };

            db.Database.ExecuteSqlCommand("EXEC up_Requirement_DeleteById  @RequirementId", para);
        }
        #endregion
        #region 需求书弹出框


        /// <summary>
        /// 需求书弹出框
        /// 根据项目查询需求书
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<RequiremetMstDto> RequirementMstByProjectIdSearch(string projectId,string province,string city,string requirementGroupId)
        {
            if (province == null)
            {
                province = "";
            }
            if (city == null)
            {
                city = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                     new SqlParameter("@Province", province),
                                                     new SqlParameter("@City", city),
                                                     new SqlParameter("@GroupId", requirementGroupId)};
            Type t = typeof(RequiremetMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_RequirementMstByProjectId_R @ProjectId,@Province,@City,@GroupId", para).Cast<RequiremetMstDto>().ToList();
        }
        #endregion
        #region 需求书暗访
        /// <summary>
        /// 基本信息查询
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        public List<RequirementDtl0Dto> RequiermentDtl0Search(string projectId, string seqNo,string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                        new SqlParameter("@SeqNO", seqNo),
                                                     new SqlParameter("@GroupId", groupId)};
            Type t = typeof(RequirementDtl0Dto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_Anfang_Requirementdtl0_R @ProjectId,@SeqNO,@GroupId", para).Cast<RequirementDtl0Dto>().ToList();
        }
       
        #endregion
        #region 内部采购
        /// <summary>
        /// 无形商品查询
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        public List<RequirementInternalIntangibleDto> RequiermentIntelIntangibleSearch(string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementId) };
            Type t = typeof(RequirementInternalIntangibleDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_Internal_Intangible_R @RequirementId", para).Cast<RequirementInternalIntangibleDto>().ToList();
        }
        public List<RequirementInternalIntangibleDto> RequiermentIntelIntangibleSearch1(string projectId,string citySeqNo,string groupId,string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                     new SqlParameter("@CitySeqNO", citySeqNo),
                                                      new SqlParameter("@GroupId", groupId),
                                                       new SqlParameter("@RequirementId", requirementId),
            };
            Type t = typeof(RequirementInternalIntangibleDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_Internal_Intangible_R1 @ProjectId,@CitySeqNO,@GroupId,@RequirementId", para).Cast<RequirementInternalIntangibleDto>().ToList();
        }

        /// <summary>
        /// 有形商品查询
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        public List<RequirementInternalTangibleDto> RequiermentIntelTangibleSearch(string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", requirementId) };

            Type t = typeof(RequirementInternalTangibleDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_Internal_Tangible_R @RequirementId", para).Cast<RequirementInternalTangibleDto>().ToList();
        }
        public List<RequirementInternalTangibleDto> RequiermentIntelTangibleSearch1(string projectId,string citySeqNo,string groupId,string requirementId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                     new SqlParameter("@CitySeqNO", citySeqNo),
                                                      new SqlParameter("@GroupId", groupId),
                                                       new SqlParameter("@RequirementId", requirementId),
            };

            Type t = typeof(RequirementInternalTangibleDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_Internal_Tangible_R1 @ProjectId,@CitySeqNO,@GroupId,@RequirementId", para).Cast<RequirementInternalTangibleDto>().ToList();
        }
        #endregion
        #region 需求书复制
        /// <summary>
        /// 需求书复制先查询样本量对应的信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ProjectCityDto> RequirementGroupCopyProjectCitySearch(string projectId, string oldGroupId,string newGroupId,string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                        new SqlParameter("@OldGroupId", oldGroupId),
                                                        new SqlParameter("@NewGroupId", newGroupId),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(ProjectCityDto);
            return db.Database.SqlQuery(t, "EXEC up_RequirementCopy_ProjectCity_R @ProjectId,@OldGroupId,@NewGroupId,@UserId", para).Cast<ProjectCityDto>().ToList();
        }
        public void RequirementCopy(string fromSeqNo, string fromGroupId,string toSeqNO, string toGroupId, string UserId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@FromSeqNO", fromSeqNo), 
                                                       new SqlParameter("@FromGroupId", fromGroupId),
                                                       new SqlParameter("@ToSeqNO",toSeqNO),
                                                       new SqlParameter("@ToGroupId",toGroupId),
                                                        new SqlParameter("@UserId",UserId)};

            db.Database.ExecuteSqlCommand("EXEC up_RequirementCopy_S @FromSeqNO,@FromGroupId,@ToSeqNO,@ToGroupId,@UserId", para);
        }
        public void RequirementCopyInterGroup(string RequirementId, string ToSeqNO, string GroupId, string UserId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementId", RequirementId), 
                                                       new SqlParameter("@ToSeqNO", ToSeqNO),
                                                       new SqlParameter("@GroupId",GroupId),
                                                        new SqlParameter("@UserId",UserId)};

            db.Database.ExecuteSqlCommand("EXEC up_RequirementCopy_InterGroup_S @RequirementId,@ToSeqNO,@GroupId,@UserId", para);
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
        #region 需求书组
        public List<RequirementGroupDto> RequirementGroupSearch(string projectId,string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(RequirementGroupDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_Group_R @ProjectId,@UserId", para).Cast<RequirementGroupDto>().ToList();
        }
        public List<RequirementGroupDto> RequirementGroupSearchById(string projectId, string groupId,string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId),
                                                        new SqlParameter("@GroupId", groupId),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(RequirementGroupDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_GroupById_R @ProjectId,@GroupId,@UserId", para).Cast<RequirementGroupDto>().ToList();
        }
        public List<RequirementGroupDto> RequirementGroupSearchByProjectName(string ModelType, string projectName, string projectShortName, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ModelType", ModelType),
                new SqlParameter("@ProjectName", projectName),
                                                        new SqlParameter("@ProjectShortName", projectShortName),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(RequirementGroupDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_GroupByProjectName_R @ModelType,@ProjectName,@ProjectShortName,@UserId", para).Cast<RequirementGroupDto>().ToList();
        }
        /// <summary>
        /// 通过组Id查询 需求书
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<RequiremetMstDto> RequirementMstSearchByGroupId(string projectId, string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ProjectId", projectId), new SqlParameter("@GroupId", groupId) };
            Type t = typeof(RequiremetMstDto);
            return db.Database.SqlQuery(t, "EXEC up_Requirement_RequirementMstByGroupId_R @ProjectId,@GroupId", para).Cast<RequiremetMstDto>().ToList();
        }
        public int RequirementGroupSave(string groupId, string projectId, string requirementName, string userId)
        {
             SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", groupId),
                                                         new SqlParameter("@ProjectId", projectId),
                                                         new SqlParameter("@RequirementGroupName", requirementName),
                                                        new SqlParameter("@UserId", userId)};
            Type t = typeof(int);
            int applyId = db.Database.SqlQuery(t, "EXEC up_Requirement_Group_S @Id,@ProjectId,@RequirementGroupName,@UserId", para).Cast<int>().First();
            return applyId;
        }
        #endregion
        #region 查询需求书样本培训总量
        public int RequirementSampleCount(string requirementType,string requirementId,string seqNO)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@RequirementType", requirementType),
                                                        new SqlParameter("@RequirementId", requirementId),
                                                         new SqlParameter("@SeqNO", seqNO)
                                                        };
            Type t = typeof(int);
            int applyId = db.Database.SqlQuery(t, "EXEC up_Requirement_SampleCountSum @RequirementType,@RequirementId,@SeqNO", para).Cast<int>().First();
            return applyId;
        
        }
        #endregion
        #region 需求书组删除
         public void RequirementGroupDelete(string groupId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@GroupId", groupId.ToString())};

            db.Database.ExecuteSqlCommand("EXEC up_Requirement_Group_D @GroupId", para);
        }
        #endregion
    }
}
