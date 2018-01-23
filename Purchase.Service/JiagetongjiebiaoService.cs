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
    public class JiagetongjiebiaoService
    {
        PurchaseEntities db = new PurchaseEntities();
        #region 业务采购
        #region 编程

        public List<Jiagetongjibiao_Left_Biancheng_Dto> Jiagetongjiebiao_Biancheng_Left_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Left_Biancheng_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Biancheng_LEFT_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Left_Biancheng_Dto>().ToList();
        }
        public List<Jiagetongjibiao_HeaderDto> Jiagetongjiebiao_Biancheng_Head_R(string serviceTrade,DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Biancheng_Header_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_HeaderDto>().ToList();
        }
        public List<Jiagetongjibiao_Data_Biancheng_Dto> Jiagetongjiebiao_Biancheng_Data_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Data_Biancheng_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Biancheng_Data_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Data_Biancheng_Dto>().ToList();
        }
        #endregion
        #region 车展
        public List<Jiagetongjibiao_Left_Chezhan_Dto> Jiagetongjiebiao_Chezhan_Left_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Left_Chezhan_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Chezhan_LEFT_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Left_Chezhan_Dto>().ToList();
        }
        public List<Jiagetongjibiao_HeaderDto> Jiagetongjiebiao_Chezhan_Head_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Chezhan_Header_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_HeaderDto>().ToList();
        }
        public List<Jiagetongjibiao_Data_Chezhan_Dto> Jiagetongjiebiao_Chezhan_Data_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Data_Chezhan_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Chezhan_Data_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Data_Chezhan_Dto>().ToList();
        }
        #endregion
        #region 复核
        public List<Jiagetongjibiao_Left_Fuhe_Dto> Jiagetongjiebiao_Fuhe_Left_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Left_Fuhe_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Fuhe_LEFT_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Left_Fuhe_Dto>().ToList();
        }
        public List<Jiagetongjibiao_HeaderDto> Jiagetongjiebiao_Fuhe_Head_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Fuhe_Header_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_HeaderDto>().ToList();
        }
        public List<Jiagetongjibiao_Data_Fuhe_Dto> Jiagetongjiebiao_Fuhe_Data_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Data_Fuhe_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Fuhe_Data_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Data_Fuhe_Dto>().ToList();
        }
        #endregion
        #region 研究
        public List<Jiagetongjibiao_Left_Yanjiu_Dto> Jiagetongjiebiao_Yanjiu_Left_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Left_Yanjiu_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Yanjiu_Left_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Left_Yanjiu_Dto>().ToList();
        }
        public List<Jiagetongjibiao_HeaderDto> Jiagetongjiebiao_Yanjiu_Head_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Yanjiu_Header_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_HeaderDto>().ToList();
        }
        public List<Jiagetongjibiao_Data_Yanjiu_Dto> Jiagetongjiebiao_Yanjiu_Data_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Data_Yanjiu_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Yanjiu_Data_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Data_Yanjiu_Dto>().ToList();
        }
        #endregion
        #region 支持
        public List<Jiagetongjibiao_Left_Zhichi_Dto> Jiagetongjiebiao_Zhichi_Left_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Left_Zhichi_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Zhichi_LEFT_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Left_Zhichi_Dto>().ToList();
        }
        public List<Jiagetongjibiao_HeaderDto> Jiagetongjiebiao_Zhichi_Head_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Zhichi_Header_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_HeaderDto>().ToList();
        }
        public List<Jiagetongjibiao_Data_Zhichi_Dto> Jiagetongjiebiao_Zhichi_Data_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Data_Zhichi_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Zhichi_Data_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Data_Zhichi_Dto>().ToList();
        }
        #endregion
        #region 执行
        public List<Jiagetongjibiao_Left_Zhixing_Dto> Jiagetongjiebiao_Zhixing_Left_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Left_Zhixing_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Zhixing_LEFT_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Left_Zhixing_Dto>().ToList();
        }
        public List<Jiagetongjibiao_HeaderDto> Jiagetongjiebiao_Zhixing_Head_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Zhixing_Header_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_HeaderDto>().ToList();
        }
        public List<Jiagetongjibiao_Data_Zhixing_Dto> Jiagetongjiebiao_Zhixing_Data_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Data_Zhixing_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Zhixing_Data_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Data_Zhixing_Dto>().ToList();
        }
        #endregion
        #endregion
        #region 内部采购
        public List<Jiagetongjibiao_Qita1_Dto> Jiajiatongjibiao_Qita1_R(string modelType, string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ModelType",modelType),
                new SqlParameter("@ServiceTrade", serviceTrade),
                                                        new SqlParameter("@StartDate", startDate), 
                                                        new SqlParameter("@EndDate",endDate)
            };
            Type t = typeof(Jiagetongjibiao_Qita1_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Qita1 @ModelType,@ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Qita1_Dto>().ToList();
        }
        public List<Jiagetongjibiao_Qita2_Dto> Jiajiatongjibiao_Qita2_R(string modelType, string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] {  new SqlParameter("@ModelType",modelType),
                new SqlParameter("@ServiceTrade", serviceTrade),
                                                        new SqlParameter("@StartDate", startDate), 
                                                        new SqlParameter("@EndDate",endDate)
            };
            Type t = typeof(Jiagetongjibiao_Qita2_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Qita2 @ModelType,@ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Qita2_Dto>().ToList();
        }
       
        #endregion
        #region 手动追加
        public List<Jiagetongjibiao_Left_Shoudong_Dto> Jiagetongjiebiao_Shoudong_Left_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Left_Shoudong_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Shoudong_LEFT_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Left_Shoudong_Dto>().ToList();
        }
        public List<Jiagetongjibiao_HeaderDto> Jiagetongjiebiao_Shoudong_Head_R(string serviceTrade,DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_HeaderDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Shoudong_Header_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_HeaderDto>().ToList();
        }
        public List<Jiagetongjibiao_Data_Shoudong_Dto> Jiagetongjiebiao_Shoudong_Data_R(string serviceTrade, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(Jiagetongjibiao_Data_Shoudong_Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Jiagetongjibiao_Shoudong_Data_R @ServiceTrade,@StartDate,@EndDate", para).Cast<Jiagetongjibiao_Data_Shoudong_Dto>().ToList();
        }
        #endregion

    }
}
