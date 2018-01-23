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
    public class AccruedChargeService
    {
        PurchaseEntities db = new PurchaseEntities();
        /// <summary>
        /// 查询供应商计提的情况
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="supplierId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AccruedChargesDto> AccruedChargesSearch(string serviceTrade,string projectName, string projectShortName,string supplierName)
        {
            if (projectName == null)
            {
                projectName = "";
            }
            if (projectShortName == null)
            {
                projectShortName = "";
            }
            if (supplierName == null)
            {
                supplierName = "";
            }


            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@ProjectName", projectName), 
                                                       new SqlParameter("@ProjectShortName",projectShortName),
                                                       new SqlParameter("@SupplierName", supplierName)};
            Type t = typeof(AccruedChargesDto);
            return db.Database.SqlQuery(t, "EXEC up_AccruedCharges_R @ServiceTrade,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<AccruedChargesDto>().ToList();
        }

        public void AccruedChargesSave(AccruedChargesDto dto,string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",dto.AccruedChargeId),
                                                       new SqlParameter("@ProjectId",dto.ProjectId),
                                                       new SqlParameter("@SupplierId",dto.SupplierId),
                                                       new SqlParameter("@CostType",dto.CostType),
                                                       new SqlParameter("@DepartmentCode",dto.DepartmentCode),
                                                       new SqlParameter("@UserId",userId)};
            db.Database.ExecuteSqlCommand("EXEC up_AccruedCharges_S @Id,@ProjectId,@SupplierId,@CostType,@DepartmentCode,@UserId", para);
            
        }
     
      
      
    }
}
