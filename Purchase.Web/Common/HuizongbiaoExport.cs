using Microsoft.Office.Interop.Excel;
using Purchase.DAL;
using Purchase.Service;
using Purchase.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XHX.Common;

namespace Purchase.Web.Common
{
    public class HuizongbiaoExport
    {
        public void ExportBiancheng(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 2;
            int index = 1;
            foreach (Huizongbiao_BianchengDto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, index);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xiangmujingli);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingchengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuofenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuoneirong);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.renyuangangwei);
                //msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuodidian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.guigexinghao);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhiliangbiaozhun);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.qitashuoming);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.hesuandanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        public void ExportZhixing(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 2;
            int index = 1;
            foreach (Huizongbiao_ZhixingDto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, index);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xiangmujingli);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingchengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingfenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.kehufenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.yonghufenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.pinpaifenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuozhize);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.cheliangleibei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.chejiafanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.jindianfangshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.jindianguibiyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.fangwenshichang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.chenggonglv);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingnandu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingjingyan);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.tuisongfangshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ercijindian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xianyouhuoqianzai);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.mingdanlaiyuan);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.canhuirenshu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.peifangyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.goucheyugoushijianduan);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.jutigoucheshijian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shouhouweixiubaoyangshijian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.jingyingshijianyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuoshijianyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.peiefenbu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.peieshuoming);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.baochoufangshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.hesuandanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        public void ExportFuhe(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 2;
            int index = 1;
            foreach (Huizongbiao_FuheDto dto in lst)
            {

                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, index);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xiangmujingli);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingchengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.fuheyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.abjuan);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.jishuwenjuanfuhe);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.wenjuanzuodaluyinfuhe);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.fuheyaoqiu1);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.fuheneirong);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.fuheshijian);

                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.dianhuazixun);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.dianhuhuifang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.yanbenbianji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.bianmaleixing);

                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.timushuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zishushuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.dianhuyuyueyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.wenjuanbiancheng);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.yangbenluru);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.luruguanli);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.fuzhuyongpin);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.hesuandanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        public void ExportYanjiu(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 2;
            int index = 1;
            foreach (Huizongbiao_YanjiuDto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, index);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xiangmujingli);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingchengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExcuteMode);

                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuofenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuoneirong);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.renyuangangwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.guigeyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhiliangbiaozhun);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.qitashuoming);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.hesuandanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        public void ExportQita1(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 2;
            int index = 1;
            foreach (Huizongbiao_Qita1Dto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Year);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ModelType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ServiceRegion);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.DepartmentCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.CostDepartment);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplyService);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ItemName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Brand);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Specification);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Model);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Configuration);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Color);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Material);

                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.LogisticsRequirements);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PackagingRequirements);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.InspectionCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.AfterSalePeriod);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Warranty);

                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportQita2(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 2;
            int index = 1;
            foreach (Huizongbiao_Qita2Dto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Year);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ModelType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ServiceRegion);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.DepartmentCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.CostDepartment);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplyService);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.CostStruct);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ItemName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Brand);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Specification);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Model);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Configuration);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.AfterSaleContent);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.AfterSalePeriod);

                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Warranty);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.OrderDate);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.DeliveryDate.HasValue ? dto.DeliveryDate .Value.ToString("yyyy-MM-dd"): "");
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.AcceptanceDate.HasValue ? dto.AcceptanceDate.Value.ToString("yyyy-MM-dd") : "");

                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportYouxingshangpincaigou(string path, List<Object> lst)
        {
            ExportQita1(path, lst);
        }
        public void ExportWuxingshangpincaigou(string path, List<Object> lst)
        {
            ExportQita2(path, lst);
        }

        public void ExportChezhan(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int index = 1;
            int startIndex = 2;
            foreach (Huizongbiao_ChezhanDto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, index);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xiangmujingli);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingchengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingfenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.kehufenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.yonghufenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.pinpaifenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuozhize);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.cheliangleibie);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.chejiafanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xianyouhuoqianzai);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingdidian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.mingdanlaiyuan);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.canhuirenshu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.genfangxuqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.peifangxuqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danduyaoyue);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.goucheyugoushijianduan);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.jutigoucheshijian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shouhouweixiubaoyangshijian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.jingyingshijianyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.gongzuoshijianyaoqiu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.peiefenbu);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.peieshuoming);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.baochoufangshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.hesuandanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportZhichi(string path, List<Object> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int index = 1;
            int startIndex = 2;
            foreach (Huizongbiao_ZhichiDto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, index);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.xiangmujingli);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingchengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.fenlei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.guigexinghao);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliangmianji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shiyongshijian);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.cheliangleibei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.chejiafanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.qitashuoming);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.hesuandanwei);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

        public void ExportZhuijia(string path, List<Huizongbiao_ShoudongDto> lst)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int index = 1;
            int startIndex = 2;
            foreach (Huizongbiao_ShoudongDto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, index);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Year);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ProjectCode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplierName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingshengfen);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.zhixingchengshi);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.FeeContent);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.shuliang);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.heji);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.Remark);

                if (index < lst.Count)
                {
                    msExcelUtil.CopyRow(sheet, startIndex);
                    startIndex++;
                    index++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            workbook.Save();
            msExcelUtil.dispose();
        }

    }
}