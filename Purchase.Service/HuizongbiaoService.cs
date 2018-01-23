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
    public class HuizongbiaoService
    {
        PurchaseEntities db = new PurchaseEntities();
        public List<Huizongbiao_BianchengDto> HuizongbiaoSearch_Biancheng(string ModelType, string ProjectName, string ProjectShortName, string SupplierName)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType),
                new SqlParameter("@ProjectName", ProjectName), 
                new SqlParameter("@ProjectShortName", ProjectShortName),
                new SqlParameter("@SupplierName", SupplierName)};
            Type t = typeof(Huizongbiao_BianchengDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Biancheng_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<Huizongbiao_BianchengDto>().ToList();
        }
        public List<Huizongbiao_ChezhanDto> HuizongbiaoSearch_Chezhan(string ModelType, string ProjectName, string ProjectShortName, string SupplierName)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType), new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName)};
            Type t = typeof(Huizongbiao_ChezhanDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Chezhan_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<Huizongbiao_ChezhanDto>().ToList();
        }
        public List<Huizongbiao_FuheDto> HuizongbiaoSearch_Fuhe(string ModelType, string ProjectName, string ProjectShortName, string SupplierName)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType), new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName)};
            Type t = typeof(Huizongbiao_FuheDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Fuhe_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<Huizongbiao_FuheDto>().ToList();
        }
        public List<Huizongbiao_YanjiuDto> HuizongbiaoSearch_Yanjiu(string ModelType, string ProjectName, string ProjectShortName, string SupplierName)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType), new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName)};
            Type t = typeof(Huizongbiao_YanjiuDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Yanjiu_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<Huizongbiao_YanjiuDto>().ToList();
        }
        public List<Huizongbiao_Qita1Dto> HuizongbiaoSearch_Qita1(string ModelType, string ProjectName, string ProjectShortName, string SupplierName,string ServiceTrade)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType), new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName),
                                                        new SqlParameter("@ServiceTrade", ServiceTrade)};
            Type t = typeof(Huizongbiao_Qita1Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Qita1_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName,@ServiceTrade", para).Cast<Huizongbiao_Qita1Dto>().ToList();
        }
        public List<Huizongbiao_Qita2Dto> HuizongbiaoSearch_Qita2(string ModelType, string ProjectName, string ProjectShortName, string SupplierName, string ServiceTrade)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType), new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName),
                                                        new SqlParameter("@ServiceTrade", ServiceTrade)};
            Type t = typeof(Huizongbiao_Qita2Dto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Qita2_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName,@ServiceTrade", para).Cast<Huizongbiao_Qita2Dto>().ToList();
        }
        public List<Huizongbiao_ZhichiDto> HuizongbiaoSearch_Zhichi(string ModelType, string ProjectName, string ProjectShortName, string SupplierName)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType), new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName)};
            Type t = typeof(Huizongbiao_ZhichiDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Zhichi_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<Huizongbiao_ZhichiDto>().ToList();
        }
        public List<Huizongbiao_ZhixingDto> HuizongbiaoSearch_Zhixing(string ModelType, string ProjectName, string ProjectShortName, string SupplierName)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType), new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName)};
            Type t = typeof(Huizongbiao_ZhixingDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_Zhixing_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<Huizongbiao_ZhixingDto>().ToList();
        }
        public List<Huizongbiao_ShoudongDto> HuizongbiaoSearch_Shoudong(string ModelType, string ProjectName, string ProjectShortName, string SupplierName)
        {
            SqlParameter[] para = new SqlParameter[] {new SqlParameter("@ModelType", ModelType)
                                                       , new SqlParameter("@ProjectName", ProjectName),  
                                                        new SqlParameter("@ProjectShortName", ProjectShortName),
                                                        new SqlParameter("@SupplierName", SupplierName)};
            Type t = typeof(Huizongbiao_ShoudongDto);
            return db.Database.SqlQuery(t, "EXEC up_Anas_Huizongbiao_shoudong_R @ModelType,@ProjectName,@ProjectShortName,@SupplierName", para).Cast<Huizongbiao_ShoudongDto>().ToList();
        }
        public void HuizongbiaoSave(string Id, string projectId, string supplierId, string settlementId, string settlementSeqNO, string remark, string userId)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",Id), 
                                                new SqlParameter("@ProjectId",projectId), 
                                                new SqlParameter("@SupplierId",supplierId), 
                                                new SqlParameter("@SettlementId",settlementId), 
                                                new SqlParameter("@SettlementSeqNO",settlementSeqNO), 
                                                 new SqlParameter("@Remark",remark), 
                                                        new SqlParameter("@UserId",userId)};
            db.Database.ExecuteSqlCommand("EXEC up_Anas_Huizongbiao_S @Id,@ProjectId,@SupplierId,@SettlementId,@SettlementSeqNO,@Remark,@UserId", para);
        }
    }
}
