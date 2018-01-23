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
    public class SupplierEvaService
    {
        PurchaseEntities db = new PurchaseEntities();

        public List<SupplierEvaDto> SupplierEvaAnwserSearch(string projectId, string subjectType, string supplierName)
        {
            if (supplierName == null) supplierName = "";
            if (subjectType == null) subjectType = "";
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ProjectId",projectId),
                                                        new SqlParameter("@SubjectType",subjectType),
                                                        new SqlParameter("@SupplierName", supplierName)
            };
            Type t = typeof(SupplierEvaDto);
            return db.Database.SqlQuery(t, "EXEC up_Supplier_SupplierEvaAnswer_R @ProjectId,@SubjectType,@SupplierName", para).Cast<SupplierEvaDto>().ToList();
        }
        public List<SupplierEvaDto> SupplierEvaAnwserAnaSearch(string subjectType, string projectName, string projectShortName, string supplierName)
        {
            subjectType = subjectType == null ? "" : subjectType;
            projectName = projectName == null ? "" : projectName;
            projectShortName = projectShortName == null ? "" : projectShortName;
            supplierName = supplierName == null ? "" : supplierName;
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@SubjectType",subjectType),
                new SqlParameter("@ProjectName",projectName),
                new SqlParameter("@ProjectShortName",projectShortName),
                new SqlParameter("@SupplierName",supplierName)
            };
            Type t = typeof(SupplierEvaDto);
            return db.Database.SqlQuery(t, "EXEC up_Supplier_SupplierEvaAnswer_R1 @SubjectType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<SupplierEvaDto>().ToList();
        }
        public void SupplierEvaAnswerSave(SupplierEvaAnswer answer, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id", answer.Id.ToString()), 
                                                      new SqlParameter("@ProjectId", answer.ProjectId.ToString()), 
                                                      new SqlParameter("@SupplierId", answer.SupplierId.ToString()), 
                                                      //new SqlParameter("@EvaSubjectId", answer.EvaSubjectId.ToString()), 
                                                      new SqlParameter("@Score", answer.SupplierScore.ToString()) ,
                                                      new SqlParameter("@UserId",userId) 
            };
            db.Database.ExecuteSqlCommand("EXEC up_Supplier_SupplierEvaAnswer_S @Id,@ProjectId,@SupplierId,@EvaSubjectId,@Score,@UserId", para);
        }

    }
}
