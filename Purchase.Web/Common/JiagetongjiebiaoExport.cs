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
    public class JiagetongjiebiaoExport
    {
        public void ExportBiancheng(string path, List<Jiagetongjibiao_HeaderDto> lst_Head, List<Jiagetongjibiao_Left_Biancheng_Dto> lst_Left, List<Jiagetongjibiao_Data_Biancheng_Dto> lst_Data,DateTime startDate,DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));

            //动态列头
            var dStartColIndex = 9;
            foreach (Jiagetongjibiao_HeaderDto dto in lst_Head)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.zhixingchengshi);
            }

            var rowIndex = 3;
            foreach (Jiagetongjibiao_Left_Biancheng_Dto dto in lst_Left)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, rowIndex - 1);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.gongzuofenlei);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.gongzuoneirong);
                msExcelUtil.SetCellValue(sheet, 5, rowIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, 6, rowIndex, dto.guigexinghao);
                msExcelUtil.SetCellValue(sheet, 7, rowIndex, dto.zhiliangbiaozhun);
                msExcelUtil.SetCellValue(sheet, 8, rowIndex, dto.hesuandanwei);

                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Jiagetongjibiao_Data_Biancheng_Dto dto in lst_Data)
            {
                dStartColIndex = 9;
                var dataRowIndex = 3;
                int dcolIndex = lst_Head.FindIndex(x => x.zhixingchengshi == dto.zhixingchengshi);
                int drowIndex = lst_Left.FindIndex(x => x.ExcuteMode == dto.ExcuteMode
                                                        && x.gongzuofenlei == dto.gongzuofenlei
                                                        && x.gongzuoneirong == dto.gongzuoneirong
                                                        && x.leixingpinpai == dto.leixingpinpai
                                                        && x.guigexinghao == dto.guigexinghao
                                                        && x.zhiliangbiaozhun == dto.zhiliangbiaozhun
                                                        && x.hesuandanwei == dto.hesuandanwei);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.danjia_AVG);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        public void ExportZhixing(string path, List<Jiagetongjibiao_HeaderDto> lst_Head, List<Jiagetongjibiao_Left_Zhixing_Dto> lst_Left, List<Jiagetongjibiao_Data_Zhixing_Dto> lst_Data,DateTime startDate,DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            //动态列头
            var dStartColIndex = 21;
            foreach (Jiagetongjibiao_HeaderDto dto in lst_Head)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.zhixingchengshi);
            }

            var rowIndex = 3;
            foreach (Jiagetongjibiao_Left_Zhixing_Dto dto in lst_Left)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, rowIndex - 1);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.zhixingfenlei);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.gongzuozhize);
                msExcelUtil.SetCellValue(sheet, 5, rowIndex, dto.jindianfangshi);
                msExcelUtil.SetCellValue(sheet, 6, rowIndex, dto.yonghufenlei);
                msExcelUtil.SetCellValue(sheet, 7, rowIndex, dto.kehufenlei);
                msExcelUtil.SetCellValue(sheet, 8, rowIndex, dto.xianyouhuoqianzai);
                msExcelUtil.SetCellValue(sheet, 9, rowIndex, dto.fangwenshichang);
                msExcelUtil.SetCellValue(sheet, 10, rowIndex, dto.chenggonglv);
                msExcelUtil.SetCellValue(sheet, 11, rowIndex, dto.tuisongfangshi);
                msExcelUtil.SetCellValue(sheet, 12, rowIndex, dto.canhuirenshu);
                msExcelUtil.SetCellValue(sheet, 13, rowIndex, dto.genfangxuqiu);
                msExcelUtil.SetCellValue(sheet, 14, rowIndex, dto.cheliangleibei);
                msExcelUtil.SetCellValue(sheet, 15, rowIndex, dto.pinpaifenlei);
                msExcelUtil.SetCellValue(sheet, 16, rowIndex, dto.chejiafanwei);
                msExcelUtil.SetCellValue(sheet, 17, rowIndex, dto.goucheyugoushijianduan);
                msExcelUtil.SetCellValue(sheet, 18, rowIndex, dto.gongzuoshijian);
                msExcelUtil.SetCellValue(sheet, 19, rowIndex, dto.jingyingshijian);
                msExcelUtil.SetCellValue(sheet, 20, rowIndex, dto.hesuandanwei);
                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Jiagetongjibiao_Data_Zhixing_Dto dto in lst_Data)
            {
                dStartColIndex = 21;
                var dataRowIndex = 3;
                int dcolIndex = lst_Head.FindIndex(x => x.zhixingchengshi == dto.zhixingchengshi);
                int drowIndex = lst_Left.FindIndex(x => x.ExcuteMode == dto.ExcuteMode
                                                        && x.zhixingfenlei == dto.zhixingfenlei
                                                        && x.gongzuozhize == dto.gongzuozhize
                                                        && x.jindianfangshi == dto.jindianfangshi
                                                        && x.yonghufenlei == dto.yonghufenlei
                                                        && x.kehufenlei == dto.kehufenlei
                                                        && x.xianyouhuoqianzai == dto.xianyouhuoqianzai
                                                        && x.fangwenshichang == dto.fangwenshichang
                                                        && x.chenggonglv == dto.chenggonglv
                                                        && x.tuisongfangshi == dto.tuisongfangshi
                                                        && x.canhuirenshu == dto.canhuirenshu
                                                        && x.genfangxuqiu == dto.genfangxuqiu
                                                        && x.cheliangleibei == dto.cheliangleibei
                                                        && x.pinpaifenlei == dto.pinpaifenlei
                                                        && x.chejiafanwei == dto.chejiafanwei
                                                        && x.goucheyugoushijianduan == dto.goucheyugoushijianduan
                                                        && x.gongzuoshijian == dto.gongzuoshijian
                                                        && x.jingyingshijian == dto.jingyingshijian
                                                        && x.hesuandanwei == dto.hesuandanwei);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;

                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.danjia_AVG);
            }

            workbook.Save();
            msExcelUtil.dispose(); ;
        }
        public void ExportFuhe(string path, List<Jiagetongjibiao_HeaderDto> lst_Head, List<Jiagetongjibiao_Left_Fuhe_Dto> lst_Left, List<Jiagetongjibiao_Data_Fuhe_Dto> lst_Data, DateTime startDate, DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            //动态列头
            var dStartColIndex = 14;
            foreach (Jiagetongjibiao_HeaderDto dto in lst_Head)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.zhixingchengshi);
            }

            var rowIndex = 3;
            foreach (Jiagetongjibiao_Left_Fuhe_Dto dto in lst_Left)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, rowIndex - 1);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.fuheyaoqiu);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.fuheshijian);
                msExcelUtil.SetCellValue(sheet, 5, rowIndex, dto.dianhuazixun);
                msExcelUtil.SetCellValue(sheet, 6, rowIndex, dto.dianhuhuifang);
                msExcelUtil.SetCellValue(sheet, 7, rowIndex, dto.yanbenbianji);
                msExcelUtil.SetCellValue(sheet, 8, rowIndex, dto.bianmaleixing);
                msExcelUtil.SetCellValue(sheet, 9, rowIndex, dto.wenjuanbiancheng);
                msExcelUtil.SetCellValue(sheet, 10, rowIndex, dto.timushuliang);
                msExcelUtil.SetCellValue(sheet, 11, rowIndex, dto.zishushuliang);
                msExcelUtil.SetCellValue(sheet, 12, rowIndex, dto.yangbenluru);
                msExcelUtil.SetCellValue(sheet, 13, rowIndex, dto.hesuandanwei);
                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Jiagetongjibiao_Data_Fuhe_Dto dto in lst_Data)
            {
                dStartColIndex = 14;
                var dataRowIndex = 3;
                int dcolIndex = lst_Head.FindIndex(x => x.zhixingchengshi == dto.zhixingchengshi);
                int drowIndex = lst_Left.FindIndex(x => x.ExcuteMode == dto.ExcuteMode
                                                        && x.fuheyaoqiu == dto.fuheyaoqiu
                                                        && x.fuheshijian == dto.fuheshijian
                                                        && x.dianhuazixun == dto.dianhuazixun
                                                        && x.dianhuhuifang == dto.dianhuhuifang
                                                        && x.yanbenbianji == dto.yanbenbianji
                                                        && x.bianmaleixing == dto.bianmaleixing
                                                        && x.timushuliang == dto.timushuliang
                                                        && x.zishushuliang == dto.zishushuliang
                                                        && x.wenjuanbiancheng == dto.wenjuanbiancheng
                                                        && x.yangbenluru == dto.yangbenluru
                                                        && x.hesuandanwei == dto.hesuandanwei);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.danjia_AVG);
            }

            workbook.Save();
            msExcelUtil.dispose(); ;
        }
        public void ExportYanjiu(string path, List<Jiagetongjibiao_HeaderDto> lst_Head, List<Jiagetongjibiao_Left_Yanjiu_Dto> lst_Left, List<Jiagetongjibiao_Data_Yanjiu_Dto> lst_Data, DateTime startDate, DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            //动态列头
            var dStartColIndex = 9;
            foreach (Jiagetongjibiao_HeaderDto dto in lst_Head)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.zhixingchengshi);
            }

            var rowIndex = 3;
            foreach (Jiagetongjibiao_Left_Yanjiu_Dto dto in lst_Left)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, rowIndex - 1);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.gongzuofenlei);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.gongzuoneirong);
                msExcelUtil.SetCellValue(sheet, 5, rowIndex, dto.leixingpinpai);
                msExcelUtil.SetCellValue(sheet, 6, rowIndex, dto.guigeyaoqiu);
                msExcelUtil.SetCellValue(sheet, 7, rowIndex, dto.zhiliangbiaozhun);
                msExcelUtil.SetCellValue(sheet, 8, rowIndex, dto.hesuandanwei);

                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Jiagetongjibiao_Data_Yanjiu_Dto dto in lst_Data)
            {
                dStartColIndex = 9;
                var dataRowIndex = 3;
                int dcolIndex = lst_Head.FindIndex(x => x.zhixingchengshi == dto.zhixingchengshi);
                int drowIndex = lst_Left.FindIndex(x => x.ExcuteMode == dto.ExcuteMode
                                                        && x.gongzuofenlei == dto.gongzuofenlei
                                                        && x.gongzuoneirong == dto.gongzuoneirong
                                                        && x.leixingpinpai == dto.leixingpinpai
                                                        && x.guigeyaoqiu == dto.guigeyaoqiu
                                                        && x.zhiliangbiaozhun == dto.zhiliangbiaozhun
                                                        && x.hesuandanwei == dto.hesuandanwei);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.danjia_AVG);
            }

            workbook.Save();
            msExcelUtil.dispose();
        }
        public void ExportQita1(string path, List<Jiagetongjibiao_Qita1_Dto> lst,DateTime startDate,DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            int startIndex = 3;
            int index = 1;
            foreach (Jiagetongjibiao_Qita1_Dto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ModelType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplyService);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ItemName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia_AVG);
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

        public void ExportQita2(string path, List<Jiagetongjibiao_Qita2_Dto> lst,DateTime startDate,DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            int startIndex = 3;
            int index = 1;
            foreach (Jiagetongjibiao_Qita2_Dto dto in lst)
            {
                int colIndex = 1;
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ModelType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseType);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.PurchaseMode);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.SupplyService);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.CostStruct);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.ItemName);
                msExcelUtil.SetCellValue(sheet, colIndex++, startIndex, dto.danjia_AVG);
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

        //public void ExportYouxingshangpincaigou(string path, List<Object> lst)
        //{
        //    ExportQita1(path, lst);
        //}
        //public void ExportWuxingshangpincaigou(string path, List<Object> lst)
        //{
        //    ExportQita2(path, lst);
        //}

        public void ExportChezhan(string path, List<Jiagetongjibiao_HeaderDto> lst_Head, List<Jiagetongjibiao_Left_Chezhan_Dto> lst_Left, List<Jiagetongjibiao_Data_Chezhan_Dto> lst_Data, DateTime startDate, DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            //动态列头
            var dStartColIndex = 14;
            foreach (Jiagetongjibiao_HeaderDto dto in lst_Head)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.zhixingchengshi);
            }

            var rowIndex = 3;
            foreach (Jiagetongjibiao_Left_Chezhan_Dto dto in lst_Left)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, rowIndex - 1);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.zhixingfenlei);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.yonghufenlei);
                msExcelUtil.SetCellValue(sheet, 5, rowIndex, dto.kehufenlei);
                msExcelUtil.SetCellValue(sheet, 6, rowIndex, dto.xianyouhuoqianzai);
                msExcelUtil.SetCellValue(sheet, 7, rowIndex, dto.cheliangleibie);
                msExcelUtil.SetCellValue(sheet, 8, rowIndex, dto.pinpaifenlei);
                msExcelUtil.SetCellValue(sheet, 9, rowIndex, dto.chejiafanwei);
                msExcelUtil.SetCellValue(sheet, 10, rowIndex, dto.goucheyugoushijianduan);
                msExcelUtil.SetCellValue(sheet, 11, rowIndex, dto.gongzuozhize);
                msExcelUtil.SetCellValue(sheet, 12, rowIndex, dto.canhuirenshu);
                msExcelUtil.SetCellValue(sheet, 13, rowIndex, dto.hesuandanwei);

                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Jiagetongjibiao_Data_Chezhan_Dto dto in lst_Data)
            {
                dStartColIndex = 14;
                var dataRowIndex = 3;
                int dcolIndex = lst_Head.FindIndex(x => x.zhixingchengshi == dto.zhixingchengshi);
                int drowIndex = lst_Left.FindIndex(x => x.ExcuteMode == dto.ExcuteMode
                                                        && x.zhixingfenlei == dto.zhixingfenlei
                                                        && x.yonghufenlei == dto.yonghufenlei
                                                        && x.kehufenlei == dto.kehufenlei
                                                        && x.xianyouhuoqianzai == dto.xianyouhuoqianzai
                                                        && x.cheliangleibie == dto.cheliangleibie
                                                        && x.pinpaifenlei == dto.pinpaifenlei
                                                        && x.chejiafanwei == dto.chejiafanwei
                                                        && x.goucheyugoushijianduan == dto.goucheyugoushijianduan
                                                        && x.gongzuozhize == dto.gongzuozhize
                                                        && x.canhuirenshu == dto.canhuirenshu
                                                        && x.hesuandanwei == dto.hesuandanwei);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.danjia_AVG);
            }

            workbook.Save();
            msExcelUtil.dispose(); ;
        }

        public void ExportZhichi(string path, List<Jiagetongjibiao_HeaderDto> lst_Head, List<Jiagetongjibiao_Left_Zhichi_Dto> lst_Left, List<Jiagetongjibiao_Data_Zhichi_Dto> lst_Data, DateTime startDate, DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            //动态列头
            var dStartColIndex = 5;
            foreach (Jiagetongjibiao_HeaderDto dto in lst_Head)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.zhixingchengshi);
            }

            var rowIndex = 3;
            foreach (Jiagetongjibiao_Left_Zhichi_Dto dto in lst_Left)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, rowIndex - 1);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 3, rowIndex, dto.fenlei);
                msExcelUtil.SetCellValue(sheet, 4, rowIndex, dto.hesuandanwei);

                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Jiagetongjibiao_Data_Zhichi_Dto dto in lst_Data)
            {
                dStartColIndex = 5;
                var dataRowIndex = 3;
                int dcolIndex = lst_Head.FindIndex(x => x.zhixingchengshi == dto.zhixingchengshi);
                int drowIndex = lst_Left.FindIndex(x => x.ExcuteMode == dto.ExcuteMode
                                                        && x.fenlei == dto.fenlei
                                                        && x.hesuandanwei == dto.hesuandanwei);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.danjia_AVG);
            }

            workbook.Save();
            msExcelUtil.dispose(); ;
        }

        public void ExportZhuijia(string path, List<Jiagetongjibiao_HeaderDto> lst_Head, List<Jiagetongjibiao_Left_Shoudong_Dto> lst_Left, List<Jiagetongjibiao_Data_Shoudong_Dto> lst_Data, DateTime startDate, DateTime endDate)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;
            //统计周期
            msExcelUtil.SetCellValue(sheet, 2, 1, string.Format("{0} 至 {1}", startDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日")));
            //动态列头
            var dStartColIndex = 3;
            foreach (Jiagetongjibiao_HeaderDto dto in lst_Head)
            {
                msExcelUtil.CopyColumn(sheet, dStartColIndex);
                msExcelUtil.AddColumn(sheet, dStartColIndex + 1);
                msExcelUtil.SetCellValue(sheet, dStartColIndex++, 2, dto.zhixingchengshi);
            }

            var rowIndex = 3;
            foreach (Jiagetongjibiao_Left_Shoudong_Dto dto in lst_Left)
            {
                msExcelUtil.SetCellValue(sheet, 1, rowIndex, rowIndex - 1);
                msExcelUtil.SetCellValue(sheet, 2, rowIndex, dto.FeeContent);

                rowIndex++;
                msExcelUtil.AddRow(sheet, rowIndex);
            }
            msExcelUtil.DeleteRow(sheet, rowIndex);

            foreach (Jiagetongjibiao_Data_Shoudong_Dto dto in lst_Data)
            {
                dStartColIndex = 3;
                var dataRowIndex = 3;
                int dcolIndex = lst_Head.FindIndex(x => x.zhixingchengshi == dto.zhixingchengshi);
                int drowIndex = lst_Left.FindIndex(x => x.FeeContent == dto.FeeContent);
                dStartColIndex += dcolIndex;
                dataRowIndex += drowIndex;
                msExcelUtil.SetCellValue(sheet, dStartColIndex, dataRowIndex, dto.danjia_AVG);
            }

            workbook.Save();
            msExcelUtil.dispose(); ;
        }

    }
}