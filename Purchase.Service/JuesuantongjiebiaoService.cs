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
    public class JuesuantongjiebiaoService
    {
        PurchaseEntities db = new PurchaseEntities();
        #region 业务采购
        public List<Juesuantongjibiao1_LeftDto> Juesuantongjiebiao1_yewu_Left_R(string serviceTrade,string QuotationType, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@QuotationType", QuotationType), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao1_LeftDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_yewu_LEFT_R @ServiceTrade,@QuotationType,@StartDate,@EndDate", para).Cast<Juesuantongjibiao1_LeftDto>().ToList();
        }
        public List<Juesuantongjibiao1_HeaderDto> Juesuantongjiebiao1_Head_Search(string serviceTrade,string QuotationType, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),
                                                        new SqlParameter("@QuotationType", QuotationType),
                                                        new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao1_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_yewu_Head_R @ServiceTrade,@QuotationType,@StartDate,@EndDate", para).Cast<Juesuantongjibiao1_HeaderDto>().ToList();
        }
        public List<Juesuantongjibiao1_DataDto> Juesuantongjiebiao1_Data_Search(string serviceTrade,string QuotationType, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@QuotationType", QuotationType), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao1_DataDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_yewu_Data_R @ServiceTrade,@QuotationType,@StartDate,@EndDate", para).Cast<Juesuantongjibiao1_DataDto>().ToList();
        }

        public List<Juesuantongjibiao2Dto> Juesuantongjiebiao2_yewu_Search(string serviceTrade,string QuotationType, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@QuotationType", QuotationType),
                                                        new SqlParameter("@StartDate", startDate),
                                                        new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao2Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao2_yewu_R @ServiceTrade,@QuotationType,@StartDate,@EndDate", para).Cast<Juesuantongjibiao2Dto>().ToList();
        }
        #endregion
        #region 内部采购
        public List<Juesuantongjibiao1_inter_LeftDto> Juesuantongjiebiao1_inter_Left_R(string modelType, string serviceTrade,string purchase, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ModelType",modelType),
                                                        new SqlParameter("@ServiceTrade",serviceTrade),
                                                        new SqlParameter("@PurchaseType",purchase),
                                                        new SqlParameter("@StartDate", startDate), 
                                                        new SqlParameter("@EndDate",endDate)
            };
            Type t = typeof(Juesuantongjibiao1_inter_LeftDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_inter_LEFT_R @ModelType,@ServiceTrade,@PurchaseType,@StartDate,@EndDate", para).Cast<Juesuantongjibiao1_inter_LeftDto>().ToList();
        }
        public List<Juesuantongjibiao1_inter_HeaderDto> Juesuantongjiebiao1_inter_Head_Search(string modelType, string serviceTrade,string purchase, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ModelType",modelType),
                                                        new SqlParameter("@ServiceTrade",serviceTrade),
                                                        new SqlParameter("@PurchaseType",purchase),
                                                        new SqlParameter("@StartDate", startDate), 
                                                        new SqlParameter("@EndDate",endDate)
            };
            Type t = typeof(Juesuantongjibiao1_inter_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_inter_Head_R @ModelType,@ServiceTrade,@PurchaseType,@StartDate,@EndDate", para).Cast<Juesuantongjibiao1_inter_HeaderDto>().ToList();
        }
        public List<Juesuantongjibiao1_inter_DataDto> Juesuantongjiebiao1_inter_Data_Search(string modelType,string serviceTrade, string purchase, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ModelType",modelType),
                                                        new SqlParameter("@ServiceTrade",serviceTrade),
                                                        new SqlParameter("@PurchaseType",purchase),
                                                        new SqlParameter("@StartDate", startDate), 
                                                        new SqlParameter("@EndDate",endDate)
            };
            Type t = typeof(Juesuantongjibiao1_inter_DataDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_inter_Data_R @ModelType,@ServiceTrade,@PurchaseType,@StartDate,@EndDate", para).Cast<Juesuantongjibiao1_inter_DataDto>().ToList();
        }

        public List<Juesuantongjibiao2_interDto> Juesuantongjiebiao2_inter_Search(string modelType,string serviceTrade, string purchase, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ModelType",modelType),
                                                         new SqlParameter("@ServiceTrade",serviceTrade),
                                                        new SqlParameter("@PurchaseType",purchase),
                                                        new SqlParameter("@StartDate", startDate), 
                                                        new SqlParameter("@EndDate",endDate)
            };
            Type t = typeof(Juesuantongjibiao2_interDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao2_inter_R @ModelType,@ServiceTrade,@PurchaseType, @StartDate,@EndDate", para).Cast<Juesuantongjibiao2_interDto>().ToList();
        }
        #endregion
        #region 手动追加
          public List<Juesuantongjibiao1_Shoudong_LeftDto> Juesuantongjiebiao1_Shoudong_Left_R(string serviceTrade,DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao1_Shoudong_LeftDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_Shoudong_LEFT_R @StartDate,@EndDate,@ServiceTrade", para).Cast<Juesuantongjibiao1_Shoudong_LeftDto>().ToList();
        }
        public List<Juesuantongjibiao1_Shoudong_HeaderDto> Juesuantongjiebiao1_Shoudong_Head_Search(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),
                                                        new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao1_Shoudong_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_Shoudong_Head_R @StartDate,@EndDate,@ServiceTrade", para).Cast<Juesuantongjibiao1_Shoudong_HeaderDto>().ToList();
        }
        public List<Juesuantongjibiao1_Shoudong_DataDto> Juesuantongjiebiao1_Shoudong_Data_Search(string serviceTrade,DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao1_Shoudong_DataDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao1_Shoudong_Data_R @StartDate,@EndDate,@ServiceTrade", para).Cast<Juesuantongjibiao1_Shoudong_DataDto>().ToList();
        }
        public List<Juesuantongjibiao2_ShoudongDto> Juesuantongjiebiao2_Shoudong_Search(string serviceTrade,DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),
                                                        new SqlParameter("@StartDate", startDate),
                                                        new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Juesuantongjibiao2_ShoudongDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jusuantongjibiao2_Shoudong_R @StartDate,@EndDate, @ServiceTrade", para).Cast<Juesuantongjibiao2_ShoudongDto>().ToList();
        }
        #endregion

    }
}
