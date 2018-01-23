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
    public class CapitalRequirementService
    {
        PurchaseEntities db = new PurchaseEntities();

        public void CapitalrequirementSave(CapitalRequiremenDto dto, string userId)
        {
            if (dto.DepartmentCode == null)
            {
                dto.DepartmentCode = "";
            }
            if (dto.ExpendType == null)
            {
                dto.ExpendType = "";
            }
            if (dto.ExpendPattern == null)
            {
                dto.ExpendPattern = "";
            }
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@Id",dto.CapitalRequirementId),
                                                       new SqlParameter("@ProjectId",dto.ProjectId),
                                                       new SqlParameter("@DepartmentCode",dto.DepartmentCode),
                                                       new SqlParameter("@ExpendType",dto.ExpendType),
                                                       new SqlParameter("@ExpendPattern",dto.ExpendPattern),
                                                       new SqlParameter("@UserId",userId)};
            db.Database.ExecuteSqlCommand("EXEC up_CapitalRequirement_S @Id,@ProjectId,@DepartmentCode,@ExpendType,@ExpendPattern,@UserId", para);

        }
        public List<HeaderDto> GetHeadList(DateTime startdate, DateTime endDate)
        {
            DateTime startDate_Temp = GetWeekFirstDay(startdate);
            DateTime endDate_Temp = GetWeekLastDay(endDate);

            List<HeaderDto> list = new List<HeaderDto>();

            while (startDate_Temp < endDate_Temp)
            {
                HeaderDto dto = new HeaderDto();
                dto.StartDate = startDate_Temp;
                dto.EndDate = startDate_Temp.AddDays(6);
                dto.Display = dto.StartDate.Month + "." + dto.StartDate.Day + "-" + dto.EndDate.Month + "." + dto.EndDate.Day;
                list.Add(dto);
                startDate_Temp = dto.EndDate.AddDays(1);
            }

            return list;
        }
        public List<LeftDto> GetLeftList(string serviceTrade,DateTime startdate, DateTime endDate,string projectName,string projectShortName)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade), new SqlParameter("@StartDate", startdate), 
                                                       new SqlParameter("@EndDate",endDate),
                                                       new SqlParameter("@ProjectName",projectName),
                                                       new SqlParameter("@ProjectShortName",projectShortName)
            };
            Type t = typeof(LeftDto);
            return db.Database.SqlQuery(t, "EXEC up_CapitalRequirement_LEFT_R @ServiceTrade,@StartDate,@EndDate,@ProjectName,@ProjectShortName", para).Cast<LeftDto>().ToList();
        }
        public List<DataDto> GetDataList(string serviceTrade,DateTime startDate, DateTime endDate,string projectName,string projectShortName, List<HeaderDto> list_Header, List<LeftDto> list_Left)
        {
            List<DataDto> listData = new List<DataDto>();
            List<FlowOrderDto> List_flowOrder = FlowOrderSearchByDate(serviceTrade,startDate, endDate, projectName, projectShortName);

            foreach (HeaderDto header in list_Header)
            {
                foreach (LeftDto left in list_Left)
                {
                    DataDto data = new DataDto();
                    data.ProjectId = left.ProjectId;
                    decimal? sumAmt = 0;
                    foreach (FlowOrderDto flowOrder in List_flowOrder)
                    {
                        if (flowOrder.ProjectId == left.ProjectId
                            &&flowOrder.PayTime <= header.EndDate
                            && flowOrder.PayTime >= header.StartDate)
                        {
                            sumAmt += flowOrder.PayAmt;
                            if (flowOrder.PaymentType == "预付款" || flowOrder.PaymentType == "押金")
                            {
                                data.AdvancePaymentChk = true;
                            }
                            if (flowOrder.FactChk == "1")
                            {
                                data.FactPayChk = true;
                            }
                        }                        
                    }
                    data.SumAmt = sumAmt;
                    listData.Add(data);
                }                
            }

            return listData;
        }
        List<FlowOrderDto> FlowOrderSearchByDate(string serviceTrade,DateTime startDate, DateTime endDate,string projectName,string projectShortName)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@ServiceTrade", serviceTrade),new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate),
                                                         new SqlParameter("@ProjectName",projectName),
                                                       new SqlParameter("@ProjectShortName",projectShortName)};
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_CapitalRequirement_R @ServiceTrade,@StartDate,@EndDate,@ProjectName,@ProjectShortName", para).Cast<FlowOrderDto>().ToList();
        }
        public List<FlowOrderDto> FlowOrderProjectSearch(DateTime startDate, DateTime endDate)
        {
            SqlParameter[] para = new SqlParameter[] { new SqlParameter("@StartDate", startDate), 
                                                       new SqlParameter("@EndDate",endDate)};
            Type t = typeof(FlowOrderDto);
            return db.Database.SqlQuery(t, "EXEC up_CapitalRequirement_Project_R @StartDate,@EndDate", para).Cast<FlowOrderDto>().ToList();
        }
        public DateTime GetWeekFirstDay(DateTime date)
        {
            return date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d")));  //本周周一
        }
        public DateTime GetWeekLastDay(DateTime date)
        {
            return GetWeekFirstDay(date).AddDays(6);  //本周周日
        }

    }
}
