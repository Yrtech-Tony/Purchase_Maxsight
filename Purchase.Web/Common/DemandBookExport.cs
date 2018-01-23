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
    public class DemandBookExport
    {
        private PurchaseEntities db = new PurchaseEntities();
        private RequirementService service = new RequirementService();

        public void ExportAnfang(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }

            List<Requirement_Anfang_RequirementDtl1> dtl1s = db.Requirement_Anfang_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Anfang_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.AgeGroup);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.EducationGroup);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.DrivingLicense);

                    if (!string.IsNullOrEmpty(Dtl1.RemarkDtl1))
                    {
                        string[] subRemarks = Dtl1.RemarkDtl1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 5, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Anfang_RequirementDtl2> dtl2s = db.Requirement_Anfang_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Anfang_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.AccessTime);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.SoundRecordChk);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.Videotape);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.TrainingMode);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.Appointment);
                    //msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.Avoid);
                    //msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.IntoShopAgain);
                    //msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.IntoShopAgainRate);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.GPSLocationOrSMS);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.MobileAnswer);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.TimeMothod);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl2.TelReturnVisit);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl2.TelReturnVisitNumberRequirement);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl2.OwnerreturenVisit);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl2.FocusAnswer);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl2.CustomerServiceLine);

                    if (!string.IsNullOrEmpty(Dtl2.RemarkDtl2))
                    {
                        string[] subRemarks = Dtl2.RemarkDtl2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 15, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }
            List<Requirement_Anfang_RequirementDtl3> dtl3s = db.Requirement_Anfang_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Anfang_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.ExecuteType);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.CustomerType);
                   // msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.ListSource);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.Approaches);

                    if (!string.IsNullOrEmpty(Dtl3.RemarkDtl3))
                    {
                        string[] subRemarks = Dtl3.RemarkDtl3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 5, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }


            List<Requirement_Anfang_RequirementDtl4> dtl4s = db.Requirement_Anfang_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Anfang_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.ExecuteType);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.CustomerType);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.BrandType);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.IntoShopType);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl4.CarChk);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl4.IntoShopTypeRate);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl4.CarType);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl4.CarPriceRange);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl4.CarLevel);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl4.SampleCount);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl4.Avoid);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl4.IntoShopAgain);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl4.IntoShopAgainRate);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl4.Compensation);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl4.ListSource);

                    if (!string.IsNullOrEmpty(Dtl4.RemarkDtl4))
                    {
                        string[] subRemarks = Dtl4.RemarkDtl4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 17, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Anfang_RequirementDtl5> dtl5s = db.Requirement_Anfang_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Anfang_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.CandidDevice);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.AssistantDevice);

                    if (!string.IsNullOrEmpty(Dtl5.RemarkDtl5))
                    {
                        string[] subRemarks = Dtl5.RemarkDtl5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }

            List<Requirement_Anfang_RequirementDtl6> dtl6s = db.Requirement_Anfang_RequirementDtl6.Where(x => x.RequirementId == intId).ToList();
            if (dtl6s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Anfang_RequirementDtl6 Dtl6 in dtl6s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl6.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl6.FirstCommit_Time);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl6.FirstCommit_CommitFile);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl6.FirstCommit_Type);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl6.LastCommit_Time);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl6.LastCommit_CommitFile);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl6.LastCommit_Type);

                    if (!string.IsNullOrEmpty(Dtl6.RemarkDtl6))
                    {
                        string[] subRemarks = Dtl6.RemarkDtl6.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 8, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }

            List<Requirement_Anfang_RequirementDtl7> dtl7s = db.Requirement_Anfang_RequirementDtl7.Where(x => x.RequirementId == intId).ToList();
            if (dtl7s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Anfang_RequirementDtl7 Dtl7 in dtl7s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl7.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl7.Evaluator_Age);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl7.Evaluator_Education);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl7.Evaluator_Experience);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl7.Evaluator_DriveChk);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl7.Evaluator_FactDriveAge);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl7.Evaluator_StaffRate);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl7.Tech_Age);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl7.Tech_Education);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl7.Tech_CertificateRequirements);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl7.Tech_Experience);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl7.Tech_DriveChk);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl7.Tech_StaffRate);

                    if (!string.IsNullOrEmpty(Dtl7.RemarkDtl7))
                    {
                        string[] subRemarks = Dtl7.RemarkDtl7.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 14, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportBianchengjibianji(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();
            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Bianchengjibianji_RequirementDtl1> dtl1s = db.Requirement_Bianchengjibianji_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianchengjibianji_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.gongzuofenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.gongzuoneirong);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.renyuangangwei);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.gongzuodidian);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.guigeyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.zhiliangbiaozhun);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl1.shuliang);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl1.shijiananpai);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 11, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_Bianchengjibianji_RequirementDtl2> dtl2s = db.Requirement_Bianchengjibianji_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianchengjibianji_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    //msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.renyuangangwei);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.gongzuoneirong);
                   // msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.gongzuodidian);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.guigeyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.zhiliangbiaozhun);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.shuliang);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.shijiananpai);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportBianmajifuhe(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Bianmajifuhe_RequirementDtl1> dtl1s = db.Requirement_Bianmajifuhe_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.fuheyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.fuheyaoqiuyangbenlianghuobili);
                    //msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.abjuan);
                    //msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.abjuanyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.jishuwenjuanfuhe);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.jishuwenjuanfuheyangbenlianghuobili);
                    //msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.wenjuanzuodaluyinfuhe);
                    //msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl1.wenjuanzuodaluyinfuheyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.fuheyaoqiu1);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.fuheyaoqiuyangbenlianghuobili1);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.fuheneirong);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl1.fuheneirongyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl1.fuheshijian);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl1.guangpankelu);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl1.guangpankeluyangbenlianghuobili);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 13, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_Bianmajifuhe_RequirementDtl2> dtl2s = db.Requirement_Bianmajifuhe_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.fuheyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.dianhuazixun);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.dianhuhuifang);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.yanbenbianji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.fuzhuyongpin);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.bianmaleixing);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.wenjuanbiancheng);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.timushuliang);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl2.zishushuliang);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl2.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl2.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl2.abjuan);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl2.abjuanyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl2.wenjuanzuodaluyinfuhe);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl2.wenjuanzuodaluyinfuheyangbenlianghuobili);
                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 17, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_Bianmajifuhe_RequirementDtl3> dtl3s = db.Requirement_Bianmajifuhe_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.shensuchuli);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.shenshuchuliyangbenhuobili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.shensuzhengjujiequ);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.shensuchuliyangbenlianghuobili1);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Bianmajifuhe_RequirementDtl4> dtl4s = db.Requirement_Bianmajifuhe_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.ziliaozhengli);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.ziliaozhengliyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.tijiaoxianxiatongjibiao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.tijiaoxianxiatongjibiaoyangbenlianghuobili);

                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Bianmajifuhe_RequirementDtl5> dtl5s = db.Requirement_Bianmajifuhe_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.dianhuzixunyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.dianhuazixunyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl5.zixunfuheyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl5.zixunfuheyaoqiuyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl5.dianhuyuyueyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl5.dianhuayuyueyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl5.yuyuetixingfuhe);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl5.yuyuetingxingfuheyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl5.dianhuajiuyuanyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl5.dianhuajiuyuanyangbenlianghuobili);

                    if (!string.IsNullOrEmpty(Dtl5.qitashuoming5))
                    {
                        string[] subRemarks = Dtl5.qitashuoming5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 12, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Bianmajifuhe_RequirementDtl6> dtl6s = db.Requirement_Bianmajifuhe_RequirementDtl6.Where(x => x.RequirementId == intId).ToList();
            if (dtl6s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl6 Dtl6 in dtl6s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl6.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl6.dianhuahuifangyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl6.shouhouhuifangyangbenlianghuobili);

                    if (!string.IsNullOrEmpty(Dtl6.qitashuoming6))
                    {
                        string[] subRemarks = Dtl6.qitashuoming6.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Bianmajifuhe_RequirementDtl7> dtl7s = db.Requirement_Bianmajifuhe_RequirementDtl7.Where(x => x.RequirementId == intId).ToList();
            if (dtl7s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl7 Dtl7 in dtl7s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl7.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl7.yangbenbianjieyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl7.yangbenbianjieyangbenlianghuobili);

                    if (!string.IsNullOrEmpty(Dtl7.qitashuoming7))
                    {
                        string[] subRemarks = Dtl7.qitashuoming7.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Bianmajifuhe_RequirementDtl71> dtl71s = db.Requirement_Bianmajifuhe_RequirementDtl71.Where(x => x.RequirementId == intId).ToList();
            if (dtl71s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl71 Dtl71 in dtl71s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl71.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl71.fuzuyongpinyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl71.fuzhuyongpinyangbenlianghuobili);

                    if (!string.IsNullOrEmpty(Dtl71.qitashuoming71))
                    {
                        string[] subRemarks = Dtl71.qitashuoming71.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Bianmajifuhe_RequirementDtl8> dtl8s = db.Requirement_Bianmajifuhe_RequirementDtl8.Where(x => x.RequirementId == intId).ToList();
            if (dtl8s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl8 Dtl8 in dtl8s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl8.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl8.wenjuanbiancheng);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl8.wenjuangenggai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl8.wenjuanneirongxiugaibili);

                    if (!string.IsNullOrEmpty(Dtl8.qitashuoming8))
                    {
                        string[] subRemarks = Dtl8.qitashuoming8.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 5, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Bianmajifuhe_RequirementDtl9> dtl9s = db.Requirement_Bianmajifuhe_RequirementDtl9.Where(x => x.RequirementId == intId).ToList();
            if (dtl9s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Bianmajifuhe_RequirementDtl9 Dtl9 in dtl9s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl9.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl9.yangbenluru);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl9.yangbenluruyangbenlianghuobili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl9.luruguanli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl9.luruguanlyangbenlianghuobili);

                    if (!string.IsNullOrEmpty(Dtl9.qitashuoming9))
                    {
                        string[] subRemarks = Dtl9.qitashuoming9.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportChangdibuzhan(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();
            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Changdibuzhan_RequirementDtl1> dtl1s = db.Requirement_Changdibuzhan_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Changdibuzhan_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Changdibuzhan_RequirementDtl2> dtl2s = db.Requirement_Changdibuzhan_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Changdibuzhan_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportCheliangzhanpinghui(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Cheliangzhanpinghui_RequirementDtl1> dtl1s = db.Requirement_Cheliangzhanpinghui_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.xingbie);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.xingbiebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.xueli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.nianling);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.nianlingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.jiatingnianshouru);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.jiatingyueshouru);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl2> dtl2s = db.Requirement_Cheliangzhanpinghui_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.peixunfangshi);
                   // msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.benpingmingdan);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 5, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl3> dtl3s = db.Requirement_Cheliangzhanpinghui_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.yonghufenlei);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.xianyouhuoqianzai);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.mingdanlaiyuan);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl3.goucheyugoushijianduan);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl3.jutigoucheshijian);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl3.shouhouweixiugabaoyang);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl3.cheliangleibie);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl3.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl3.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl3.peiefenbu);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl3.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl3.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl3.gongzuozhize);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl3.zhixingdidian);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, Dtl3.baochoufangshi);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, Dtl3.baochoujine);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 20, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl31> dtl31s = db.Requirement_Cheliangzhanpinghui_RequirementDtl31.Where(x => x.RequirementId == intId).ToList();
            if (dtl31s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl31 Dtl3 in dtl31s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.luyinleixing);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.luyinhegebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.wenjuanleixing);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.zhengzhaoleixing);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.hegezhengzhaobili);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming41))
                    {
                        string[] subRemarks = Dtl3.qitashuoming41.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl4> dtl4s = db.Requirement_Cheliangzhanpinghui_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.xingbie);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.xingbiebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.xueli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.nianling);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl4.nianlingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl4.jiatingnianshouru);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl4.jiatingyueshouru);

                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl5> dtl5s = db.Requirement_Cheliangzhanpinghui_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.fangwenshichang);
                    //msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.benpinmingdan);

                    if (!string.IsNullOrEmpty(Dtl5.qitashuoming5))
                    {
                        string[] subRemarks = Dtl5.qitashuoming5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 3, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl6> dtl6s = db.Requirement_Cheliangzhanpinghui_RequirementDtl6.Where(x => x.RequirementId == intId).ToList();
            if (dtl6s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl6 Dtl6 in dtl6s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl6.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl6.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl6.yonghufenlei);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl6.xianyouhuoqianzai);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl6.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl6.mingdanlaiyuan);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl6.goucheyugoushijianduan);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl6.jutigoucheshijian);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl6.shouhouweixiubaoyangshijian);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl6.cheliangleibie);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl6.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl6.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl6.canhuirenshu);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl6.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl6.danduyaoyue);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl6.peiefenbu);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl6.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, Dtl6.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, Dtl6.gongzuozhize);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, Dtl6.baochoufangshi);
                    msExcelUtil.SetCellValue(sheet, 21, startIndex, Dtl6.baochoujine);

                    if (!string.IsNullOrEmpty(Dtl6.qitashuomong6))
                    {
                        string[] subRemarks = Dtl6.qitashuomong6.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 22, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl7> dtl7s = db.Requirement_Cheliangzhanpinghui_RequirementDtl7.Where(x => x.RequirementId == intId).ToList();
            if (dtl7s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl7 Dtl7 in dtl7s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl7.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl7.kehurenshu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl7.kehushipin);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl7.kehucanyin);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl7.huichangshipin);

                    if (!string.IsNullOrEmpty(Dtl7.qitashuoming7))
                    {
                        string[] subRemarks = Dtl7.qitashuoming7.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl71> dtl71s = db.Requirement_Cheliangzhanpinghui_RequirementDtl71.Where(x => x.RequirementId == intId).ToList();
            if (dtl71s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl71 Dtl7 in dtl71s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl7.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl7.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl7.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl7.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl7.shuliangmingji);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl7.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl7.qitashuoming71))
                    {
                        string[] subRemarks = Dtl7.qitashuoming71.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl8> dtl8s = db.Requirement_Cheliangzhanpinghui_RequirementDtl8.Where(x => x.RequirementId == intId).ToList();
            if (dtl8s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl8 Dtl8 in dtl8s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl8.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl8.bilu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl8.bilutijiaoshijian);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl8.qitashuoming8);

                    if (!string.IsNullOrEmpty(Dtl8.qitashuoming8))
                    {
                        string[] subRemarks = Dtl8.qitashuoming8.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 5, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl9> dtl9s = db.Requirement_Cheliangzhanpinghui_RequirementDtl9.Where(x => x.RequirementId == intId).ToList();
            if (dtl9s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl9 Dtl9 in dtl9s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl9.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl9.xingbie);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl9.xingbiebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl9.xueli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl9.nianling);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl9.nianlingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl9.jiatingnianshouru);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl9.jiatingyueshouru);

                    if (!string.IsNullOrEmpty(Dtl9.qitashuoming9))
                    {
                        string[] subRemarks = Dtl9.qitashuoming9.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl10> dtl10s = db.Requirement_Cheliangzhanpinghui_RequirementDtl10.Where(x => x.RequirementId == intId).ToList();
            if (dtl10s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl10 Dtl10 in dtl10s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl10.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl10.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl10.zhixingchangdi);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl10.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl10.peixunfangshi);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl10.genfangxueqiu);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl10.genfangrenshu);
                    //msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl10.benpinmingdan);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl10.rijixingshi);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl10.rijizhouqi);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl10.liuzhixingshi);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl10.liuzhizhouqi);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl10.liuzhineirong);

                    if (!string.IsNullOrEmpty(Dtl10.qitashuoming10))
                    {
                        string[] subRemarks = Dtl10.qitashuoming10.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 13, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl11> dtl11s = db.Requirement_Cheliangzhanpinghui_RequirementDtl11.Where(x => x.RequirementId == intId).ToList();
            if (dtl11s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl11 Dtl11 in dtl11s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl11.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl11.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl11.yonghufenlei);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl11.xianyouhuoqianzai);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl11.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl11.mingdanlaiyuan);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl11.jingyingshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl11.gongzuoshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl11.goucheyugoushijianduan);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl11.jutigoucheshijian);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl11.shouhouweixiubaoyang);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl11.cheliangleibie);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl11.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl11.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl11.canhuirenshu);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl11.danduyaoyue);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl11.jingxiaosanfang);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, Dtl11.peiefenbu);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, Dtl11.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, Dtl11.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 21, startIndex, Dtl11.gongzuozhize);
                    msExcelUtil.SetCellValue(sheet, 22, startIndex, Dtl11.baochoufangshi);
                    msExcelUtil.SetCellValue(sheet, 23, startIndex, Dtl11.baochoujine);

                    if (!string.IsNullOrEmpty(Dtl11.qitashuoming11))
                    {
                        string[] subRemarks = Dtl11.qitashuoming11.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 24, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl12> dtl12s = db.Requirement_Cheliangzhanpinghui_RequirementDtl12.Where(x => x.RequirementId == intId).ToList();
            if (dtl12s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl12 Dtl12 in dtl12s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl12.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl12.peifangrenyuan);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl12.shuliang);

                    if (!string.IsNullOrEmpty(Dtl12.qitashuoming12))
                    {
                        string[] subRemarks = Dtl12.qitashuoming12.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Cheliangzhanpinghui_RequirementDtl13> dtl13s = db.Requirement_Cheliangzhanpinghui_RequirementDtl13.Where(x => x.RequirementId == intId).ToList();
            if (dtl13s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Cheliangzhanpinghui_RequirementDtl13 Dtl13 in dtl13s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl13.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl13.luxingleixing);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl13.hegeluyinbili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl13.wenjuanleixing);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl13.zhengzhaoleixing);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl13.hegezhengzhaoleixingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl13.xianchangdaochelv);

                    if (!string.IsNullOrEmpty(Dtl13.qitashuoming13))
                    {
                        string[] subRemarks = Dtl13.qitashuoming13.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 8, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportGongyingshangchailv(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();
            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Gongyingshangchailv_RequirementDtl1> dtl1s = db.Requirement_Gongyingshangchailv_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Gongyingshangchailv_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.feiyongbiaozhun);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.daodamudidi);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportGuankong(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();
            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Guangkong_RequirementDtl1> dtl1s = db.Requirement_Guangkong_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Guangkong_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.yaoyuechexingjiebie);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.yaoyuechezhushenhe);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.yaoyuechezhuzhenbie);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.jindukongzhi);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.chuchaijiankong);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.xiangmupeixun);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl1.luyinjianting);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl1.ziliaohecha);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl1.yangbenliang);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 12, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Guangkong_RequirementDtl2> dtl2s = db.Requirement_Guangkong_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Guangkong_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.jinchudianbaoshi);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.duanxindingweigpsdingwei);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.jizhongjieting);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.jindianqianyuyue);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.shouhouchezhuhuifang);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.kefuzhuanxinjieting);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.huishouziliaojinduhecha);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.huishouziliaozhengli);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl2.yaoyueluyinjianji);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 11, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Guangkong_RequirementDtl3> dtl3s = db.Requirement_Guangkong_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Guangkong_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.qudaoshebei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.xingzhengfentan);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportMianfangjidianfang(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Mianfangjidianfang_RequirementDtl1> dtl1s = db.Requirement_Mianfangjidianfang_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mianfangjidianfang_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.xingbie);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.xingbiebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.xueli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.nianling);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.nianlingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.jiatingshouru);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.jiatingyueshouru);
                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_Mianfangjidianfang_RequirementDtl2> dtl2s = db.Requirement_Mianfangjidianfang_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mianfangjidianfang_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.zhixingchangdiyaoqiu);
                    //msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.peixunfangshi);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.zhoumozhixing);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.zhidingwaixianhaoma);
                    //msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.benpinmingdan);
                    //msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.chenggonglv);
                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Mianfangjidianfang_RequirementDtl3> dtl3s = db.Requirement_Mianfangjidianfang_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mianfangjidianfang_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.yonghufenlei);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.xianyouhuoqianzai);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl3.goucheyugoushijianduan);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl3.jutigoucheshijian);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl3.shouhouweixiubaoyangshijian);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl3.cheliangleibie);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl3.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl3.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl3.peiefenbu);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl3.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl3.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl3.gongzuozhize);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl3.baochoufangshi);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, Dtl3.baochoujine);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, Dtl3.chenggonglv);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, Dtl3.mingdanlaiyuan);
                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 21, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Mianfangjidianfang_RequirementDtl4> dtl4s = db.Requirement_Mianfangjidianfang_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mianfangjidianfang_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl4.shiyongshijian);
                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Mianfangjidianfang_RequirementDtl5> dtl5s = db.Requirement_Mianfangjidianfang_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mianfangjidianfang_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.luyinleixing);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.hegeluyingbili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl5.wenjuanleixing);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl5.zhengzhaoleixing);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl5.hegezhenghaobili);
                    if (!string.IsNullOrEmpty(Dtl5.qitashuoming5))
                    {
                        string[] subRemarks = Dtl5.qitashuoming5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);
                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportMingjian(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Mingjian_RequirementDtl1> dtl1s = db.Requirement_Mingjian_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mingjian_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.luyin);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.luxiang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.paizhao);
                  //  msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.jindianguibiyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.peixunfangshi);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Mingjian_RequirementDtl2> dtl2s = db.Requirement_Mingjian_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mingjian_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.zhixingnandu);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.jindianfangshi);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.jindianfangshibili);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.baochoujine);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl2.jindianguibiyaoqiu);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 11, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Mingjian_RequirementDtl3> dtl3s = db.Requirement_Mingjian_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Mingjian_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Mingjian_RequirementDtl4> dtl4s = db.Requirement_Mingjian_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Mingjian_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.pingguyuantiaojiannianling);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.pingguyuantiaojianxueli);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.pingguyuantiaojianrenyuanbeifenbili);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.pingguyuantiaojianzhixingjingyan);

                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }

            List<Requirement_Mingjian_RequirementDtl5> dtl5s = db.Requirement_Mingjian_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Mingjian_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.shoucitijiaoshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.shoucitijiaotijiaoziliao);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl5.shoucitijiaotijiaofangshi);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl5.zuizhongtijiaoshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl5.zuizhongtijiaotijiaoziliao);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl5.zuizhongtijiaotijiaofangshi);

                    if (!string.IsNullOrEmpty(Dtl5.qitashuoming5))
                    {
                        string[] subRemarks = Dtl5.qitashuoming5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 8, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportPankujijiagediaocha(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Pankujijiagediaocha_RequirementDtl1> dtl1s = db.Requirement_Pankujijiagediaocha_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Pankujijiagediaocha_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.zhixingleixing);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.luyin);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.luxiang);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.paizhao);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.jindianguibiyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.peixunfangshi);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Pankujijiagediaocha_RequirementDtl2> dtl2s = db.Requirement_Pankujijiagediaocha_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Pankujijiagediaocha_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.pingpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.jindianfangshi);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.kaichejindian);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.jindianfangshibili);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.cheliangleixing);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl2.chexingjibie);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl2.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl2.baochoujine);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 13, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Pankujijiagediaocha_RequirementDtl3> dtl3s = db.Requirement_Pankujijiagediaocha_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Pankujijiagediaocha_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Pankujijiagediaocha_RequirementDtl4> dtl4s = db.Requirement_Pankujijiagediaocha_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Pankujijiagediaocha_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.pingguyuantiaojiannianling);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.pingguyuantiaojianxueli);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.pingguyuantiaojianrenyuanbeifenbili);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.pingguyuantiaojianzhixingjingyan);

                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }

            List<Requirement_Pankujijiagediaocha_RequirementDtl5> dtl5s = db.Requirement_Pankujijiagediaocha_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 4;
                foreach (Requirement_Pankujijiagediaocha_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.shoucitijiaoshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.shoucitijiaotijiaoziliao);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl5.shoucitijiaotijiaofangshi);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl5.zuizhongtijiaoshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl5.zuizhongtijiaotijiaoziliao);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl5.zuizhongtijiaotijiaofangshi);

                    if (!string.IsNullOrEmpty(Dtl5.qitashuoming5))
                    {
                        string[] subRemarks = Dtl5.qitashuoming5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 8, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 4);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportShenfangjirijiliuzhi(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl1> dtl1s = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Shenfangjirijiliuzhi_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.xingbie);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.xingbiebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.xueli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.nianling);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.nianlingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.jiatingnianshouru);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.jiatingyueshouru);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl2> dtl2s = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Shenfangjirijiliuzhi_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.zhixingchangdiyaoqiu);
                   // msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.peixunfangshi);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.genfangxuqiu);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.genfangrenshu);
                    //msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.benpinmingdan);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.rijixingshi);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.rijizhouqi);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.rijineirong);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl2.liuzhixingshi);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl2.liuzhizhouqi);
                    //msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl2.jingyingshijian);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl2.liuzhineirong);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 13, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl3> dtl3s = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Shenfangjirijiliuzhi_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.yonghufenlei);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.xianyouhuoqianzai);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl3.mingdanlaiyuan);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl3.jingyingshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl3.gongzuoshijianyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl3.goucheyugoushijianduan);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl3.jutigoucheshijian);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl3.shouhouweixiubaoyang);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl3.cheliangleixing);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl3.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl3.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl3.canhuirenshu);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl3.danduyaoyue);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, Dtl3.jingxiaosanfang);
                    //msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl3.gongzuoshijian);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, Dtl3.peiefenbu);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, Dtl3.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 21, startIndex, Dtl3.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 22, startIndex, Dtl3.gongzouzhize);
                    msExcelUtil.SetCellValue(sheet, 23, startIndex, Dtl3.baochoufangshi);
                    msExcelUtil.SetCellValue(sheet, 24, startIndex, Dtl3.baochoujine);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 25, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl4> dtl4s = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Shenfangjirijiliuzhi_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.guigeyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl4.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl5> dtl5s = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Shenfangjirijiliuzhi_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.peifangrenyuan);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.shuliang);

                    if (!string.IsNullOrEmpty(Dtl5.qitashuoming5))
                    {
                        string[] subRemarks = Dtl5.qitashuoming5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Shenfangjirijiliuzhi_RequirementDtl6> dtl6s = db.Requirement_Shenfangjirijiliuzhi_RequirementDtl6.Where(x => x.RequirementId == intId).ToList();
            if (dtl6s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Shenfangjirijiliuzhi_RequirementDtl6 Dtl6 in dtl6s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl6.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl6.luyin);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl6.hegeluyinbili);

                    if (!string.IsNullOrEmpty(Dtl6.zhengzhaoleixing))
                    {
                        string[] subRemarks = Dtl6.zhengzhaoleixing.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportWangluodiaoyan(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Wangluodiaoyan_RequirementDtl1> dtl1s = db.Requirement_Wangluodiaoyan_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Wangluodiaoyan_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.xingbie);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.xingbiebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.xueli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.nianling);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.nianlingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.jiatingnianshouru);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.jiatingyueshouru);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_Wangluodiaoyan_RequirementDtl2> dtl2s = db.Requirement_Wangluodiaoyan_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Wangluodiaoyan_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                   // msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.wenjuantaoshu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.timushuliang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.tuisongfangshi);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.tuisongcishu);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.peixunfangshi);
                    //msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.benpinmingdan);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Wangluodiaoyan_RequirementDtl3> dtl3s = db.Requirement_Wangluodiaoyan_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Wangluodiaoyan_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.yonghufenlei);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.xianyouhuoqianzai);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl3.goucheyugoushijianduan);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl3.jutigoucheshijian);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl3.shouhouweixiubaoyangshijian);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl3.cheliangleibie);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl3.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl3.chejiafanwen);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl3.peiefenbu);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl3.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl3.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl3.gongzuozhize);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl3.baochoufangshi);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, Dtl3.baochoujine);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, Dtl3.mingdanlaiyuan);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 20, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Wangluodiaoyan_RequirementDtl4> dtl4s = db.Requirement_Wangluodiaoyan_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Wangluodiaoyan_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.wenjuanleixing);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.hegeyonghuxinxibili);

                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 4, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportYanjiujishujufenxi(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Yanjiujishujufenxi_RequirementDtl1> dtl1s = db.Requirement_Yanjiujishujufenxi_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Yanjiujishujufenxi_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.gongzuofenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.gongzuoneirong);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.renyuangangwei);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.guigeyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.zhiliangbiaozhun);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.shuliang);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl1.shijiananpai);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 10, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Yanjiujishujufenxi_RequirementDtl2> dtl2s = db.Requirement_Yanjiujishujufenxi_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Yanjiujishujufenxi_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                   // msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.renyuangangwei);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.gongzuoneirong);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.gongzuodidian);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.guigeyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.zhiliangbiaozhun);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.shuliang);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.shijiananpai);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 8, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportYunshuzuche(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Yunshuzuche_RequirementDtl1> dtl1s = db.Requirement_Yunshuzuche_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Yunshuzuche_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.feilei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.pailiang);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.peizhi);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.changshangzhidaojia);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.yanse);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl1.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl1.cheliangleibie);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl1.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl1.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 13, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Yunshuzuche_RequirementDtl2> dtl2s = db.Requirement_Yunshuzuche_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Yunshuzuche_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.jiecheriqihuancheriqi);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl2.zhuangxieyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl2.zhuangxiecishu);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl2.yunshuluxian);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl2.shuliangmianji);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl2.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 11, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportZuotanhui(string id, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int intId = int.Parse(id);

            Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();

            List<Requirement_Anfang_RequirementDtl0> baseDtos = db.Requirement_Anfang_RequirementDtl0.Where(x => x.RequirementId == intId).ToList();
            int pId = int.Parse(mstDto.ProjectId);
            int seqNo = int.Parse(mstDto.SeqNO);
            ProjectCity projectCity = db.ProjectCity.Find(pId, seqNo);
            int startIndex = 3;
            foreach (Requirement_Anfang_RequirementDtl0 baseDto in baseDtos)
            {
                msExcelUtil.SetCellValue(sheet, 1, startIndex, 1);
                msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectShortName);
                msExcelUtil.SetCellValue(sheet, 3, startIndex, mstDto.Province);
                msExcelUtil.SetCellValue(sheet, 4, startIndex, mstDto.City);
                msExcelUtil.SetCellValue(sheet, 5, startIndex, baseDto.ExecuteCycle);
                msExcelUtil.SetCellValue(sheet, 6, startIndex, projectCity.ProjectType);
                msExcelUtil.SetCellValue(sheet, 7, startIndex, projectCity.ExcuteMode);
                msExcelUtil.SetCellValue(sheet, 8, startIndex, projectCity.Count);

                if (!string.IsNullOrEmpty(baseDto.Remark))
                {
                    string[] subRemarks = baseDto.Remark.Split('\n');
                    foreach (string subRemark in subRemarks)
                    {
                        if (string.IsNullOrEmpty(subRemark)) continue;
                        msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
                else
                {
                    startIndex++;
                    msExcelUtil.AddRow(sheet, startIndex);
                }
            }
            List<Requirement_Zuotanhui_RequirementDtl1> dtl1s = db.Requirement_Zuotanhui_RequirementDtl1.Where(x => x.RequirementId == intId).ToList();
            if (dtl1s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Zuotanhui_RequirementDtl1 Dtl1 in dtl1s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl1.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl1.xingbie);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl1.xingbiebili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl1.xueli);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl1.nianling);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl1.nianlingbili);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl1.jiatingnianshouru);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl1.jiatingyueshouru);

                    if (!string.IsNullOrEmpty(Dtl1.qitashuoming1))
                    {
                        string[] subRemarks = Dtl1.qitashuoming1.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            List<Requirement_Zuotanhui_RequirementDtl2> dtl2s = db.Requirement_Zuotanhui_RequirementDtl2.Where(x => x.RequirementId == intId).ToList();
            if (dtl2s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Zuotanhui_RequirementDtl2 Dtl2 in dtl2s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl2.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl2.zhixingchangdiyaoqiu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl2.fangwenshichang);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl2.peixunfangshi);
                   // msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl2.benpingmingdan);

                    if (!string.IsNullOrEmpty(Dtl2.qitashuoming2))
                    {
                        string[] subRemarks = Dtl2.qitashuoming2.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 5, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Zuotanhui_RequirementDtl3> dtl3s = db.Requirement_Zuotanhui_RequirementDtl3.Where(x => x.RequirementId == intId).ToList();
            if (dtl3s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Zuotanhui_RequirementDtl3 Dtl3 in dtl3s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl3.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl3.zhixingfenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl3.yonghufenlei);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl3.xianyouhuoqianxian);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl3.kehufenlei);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl3.goucheyugoushijianduan);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl3.jutigoucheshijian);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl3.shouhouweixiubaoyangshijian);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, Dtl3.cheliangleibie);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, Dtl3.pinpaifenlei);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, Dtl3.chejiafanwei);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, Dtl3.canhuirenshu);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, Dtl3.danduyaoyue);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, Dtl3.peiefenbu);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, Dtl3.peieshuoming);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, Dtl3.yangbenliang);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, Dtl3.gongzuozhize);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, Dtl3.baochoufangshi);
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, Dtl3.baochoujine);
                    msExcelUtil.SetCellValue(sheet, 20, startIndex, Dtl3.mingdanlaiyuan);

                    if (!string.IsNullOrEmpty(Dtl3.qitashuoming3))
                    {
                        string[] subRemarks = Dtl3.qitashuoming3.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 21, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Zuotanhui_RequirementDtl4> dtl4s = db.Requirement_Zuotanhui_RequirementDtl4.Where(x => x.RequirementId == intId).ToList();
            if (dtl4s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Zuotanhui_RequirementDtl4 Dtl4 in dtl4s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl4.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl4.kehurenshu);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl4.kehushipin);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl4.kehucanyin);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl4.huichangshipin);

                    if (!string.IsNullOrEmpty(Dtl4.qitashuoming4))
                    {
                        string[] subRemarks = Dtl4.qitashuoming4.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 6, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Zuotanhui_RequirementDtl5> dtl5s = db.Requirement_Zuotanhui_RequirementDtl5.Where(x => x.RequirementId == intId).ToList();
            if (dtl5s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Zuotanhui_RequirementDtl5 Dtl5 in dtl5s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl5.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl5.fenlei);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl5.leixingpinpai);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl5.guigexinghao);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl5.shuliangmingji);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl5.shiyongshijian);

                    if (!string.IsNullOrEmpty(Dtl5.qitashuoming5))
                    {
                        string[] subRemarks = Dtl5.qitashuoming5.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 7, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_Zuotanhui_RequirementDtl6> dtl6s = db.Requirement_Zuotanhui_RequirementDtl6.Where(x => x.RequirementId == intId).ToList();
            if (dtl6s.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_Zuotanhui_RequirementDtl6 Dtl6 in dtl6s)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtl6.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, Dtl6.luyinleixing);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, Dtl6.hegeluyinbili);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, Dtl6.zhengzhaoleixing);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, Dtl6.hegezhengzhaobili);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, Dtl6.xianchangdaochelv);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, Dtl6.bilu);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, Dtl6.bilutijiaoshijian);

                    if (!string.IsNullOrEmpty(Dtl6.qitashuoming))
                    {
                        string[] subRemarks = Dtl6.qitashuoming.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 9, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            List<Requirement_RequirementDtlLast> dtllasts = db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList();
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }
            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportYouxingshangpincaigou(string ids, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int startIndex = 3;
            int index = 1;
            string[] idArray = ids.Split(',');

            foreach (string id in idArray)
            {
                Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();
                List<RequirementInternalTangibleDto> dtos = service.RequiermentIntelTangibleSearch(id);
                foreach (RequirementInternalTangibleDto dto in dtos)
                {
                    //msExcelUtil.SetCellValue(sheet, 1, startIndex, dto.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectCode);
                    //msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.ServiceRegion);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.PurchaseType);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.PurchaseMode);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.SupplyService);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.ItemName);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.Brand);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.Specification);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.Model);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.Configuration);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, dto.Color);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.Material);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.Count);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.LogisticsRequirements);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.PackagingRequirements);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, dto.InspectionCycle);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, dto.Warranty);
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, dto.AfterSalePeriod);

                    if (!string.IsNullOrEmpty(dto.Remark))
                    {
                        string[] subRemarks = dto.Remark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 19, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }

                    index++;
                }
            }
            index = 1;

            List<Requirement_RequirementDtlLast> dtllasts = new List<Requirement_RequirementDtlLast>();
            foreach (string id in idArray)
            {
                int intId = int.Parse(id);
                dtllasts.AddRange(db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList());
            }
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    //msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, index);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                    index++;
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }

        public void ExportWuxingshangpincaigou(string ids, string path)
        {
            MSExcelUtil msExcelUtil = new MSExcelUtil();
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(path);
            Worksheet sheet = workbook.ActiveSheet;

            int index = 1;
            int startIndex = 3;
            string[] idArray = ids.Split(',');
            foreach (string id in idArray)
            {
                Service.DTO.RequiremetMstDto mstDto = service.RequirementMstSearchById(id).FirstOrDefault();
                List<RequirementInternalIntangibleDto> dtos = service.RequiermentIntelIntangibleSearch(id);
                foreach (RequirementInternalIntangibleDto dto in dtos)
                {
                    //msExcelUtil.SetCellValue(sheet, 1, startIndex, dto.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, index);
                    msExcelUtil.SetCellValue(sheet, 2, startIndex, mstDto.ProjectCode);
                    //msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.ServiceRegion);
                    msExcelUtil.SetCellValue(sheet, 3, startIndex, dto.PurchaseType);
                    msExcelUtil.SetCellValue(sheet, 4, startIndex, dto.PurchaseMode);
                    msExcelUtil.SetCellValue(sheet, 5, startIndex, dto.SupplyService);
                    msExcelUtil.SetCellValue(sheet, 6, startIndex, dto.CostStruct);
                    msExcelUtil.SetCellValue(sheet, 7, startIndex, dto.ItemName);
                    msExcelUtil.SetCellValue(sheet, 8, startIndex, dto.Brand);
                    msExcelUtil.SetCellValue(sheet, 9, startIndex, dto.Specification);
                    msExcelUtil.SetCellValue(sheet, 10, startIndex, dto.Model);
                    msExcelUtil.SetCellValue(sheet, 11, startIndex, dto.Configuration);
                    msExcelUtil.SetCellValue(sheet, 12, startIndex, dto.Count);
                    msExcelUtil.SetCellValue(sheet, 13, startIndex, dto.AfterSaleContent);
                    msExcelUtil.SetCellValue(sheet, 14, startIndex, dto.AfterSalePeriod);
                    msExcelUtil.SetCellValue(sheet, 15, startIndex, dto.Warranty);
                    msExcelUtil.SetCellValue(sheet, 16, startIndex, dto.StaffLevel);
                    msExcelUtil.SetCellValue(sheet, 17, startIndex, dto.OrderDate.HasValue ? dto.OrderDate.Value.ToString("yyyy-MM-dd") : "");
                    msExcelUtil.SetCellValue(sheet, 18, startIndex, dto.DeliveryDate.HasValue ? dto.DeliveryDate.Value.ToString("yyyy-MM-dd") : "");
                    msExcelUtil.SetCellValue(sheet, 19, startIndex, dto.AcceptanceDate.HasValue ? dto.AcceptanceDate.Value.ToString("yyyy-MM-dd") : "");

                    if (!string.IsNullOrEmpty(dto.Remark))
                    {
                        string[] subRemarks = dto.Remark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 20, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }

                    index++;
                }
            }
            index = 1;

            List<Requirement_RequirementDtlLast> dtllasts = new List<Requirement_RequirementDtlLast>();
            foreach (string id in idArray)
            {
                int intId = int.Parse(id);
                dtllasts.AddRange(db.Requirement_RequirementDtlLast.Where(x => x.RequirementId == intId).ToList());
            }
            if (dtllasts.Count > 0)
            {
                startIndex += 3;
                foreach (Requirement_RequirementDtlLast Dtllast in dtllasts)
                {
                    //msExcelUtil.SetCellValue(sheet, 1, startIndex, Dtllast.SeqNO);
                    msExcelUtil.SetCellValue(sheet, 1, startIndex, index);

                    if (!string.IsNullOrEmpty(Dtllast.OtherRemark))
                    {
                        string[] subRemarks = Dtllast.OtherRemark.Split('\n');
                        foreach (string subRemark in subRemarks)
                        {
                            if (string.IsNullOrEmpty(subRemark)) continue;
                            msExcelUtil.SetCellValue(sheet, 2, startIndex, subRemark);
                            startIndex++;
                            msExcelUtil.AddRow(sheet, startIndex);
                        }
                    }
                    else
                    {
                        startIndex++;
                        msExcelUtil.AddRow(sheet, startIndex);
                    }
                    index++;
                }
            }
            else
            {
                msExcelUtil.DeleteRowRange(sheet, startIndex + 1, 3);
            }

            msExcelUtil.SetWrapText(sheet, 1, 1000, false);

            workbook.Save();
             msExcelUtil.dispose();
        }
    }
}