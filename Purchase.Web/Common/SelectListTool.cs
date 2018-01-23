using Purchase.DAL;
using Purchase.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Purchase.Web.Common
{
    public class SelectListTool
    {
        private static IQueryable<HiddenCode> hiddenCodeListSource;
        public static IQueryable<HiddenCode> HiddenCodeListSource
        {
            get
            {
                //if (hiddenCodeListSource == null)
                {
                    using (PurchaseEntities db = new PurchaseEntities())
                    {
                        hiddenCodeListSource = db.HiddenCode.Where(x => x.UseChk.HasValue && x.UseChk.Value);
                    }
                }
                return hiddenCodeListSource;
            }
        }
        public static List<SelectListItem> RoleTypeList()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "", Value = "" });
            using (PurchaseEntities db = new PurchaseEntities())
            {
                foreach (Mst_RoleType roleType in db.Mst_RoleType)
                {
                    lst.Add(new SelectListItem()
                    {
                        Text = roleType.RoleTypeName,
                        Value = roleType.RoleTypeCode
                    });
                }
            }
            return lst;
        }
        public static List<SelectListItem> DeparmentList()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "", Value = "" });
            using (PurchaseEntities db = new PurchaseEntities())
            {
                foreach (Mst_Department department in db.Mst_Department)
                {
                    lst.Add(new SelectListItem()
                    {
                        Text = department.DepartmentName,
                        Value = department.DepartmentCode
                    });
                }
            }
            return lst;
        }
        public static List<SelectListItem> PositionList()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "", Value = "" });
            using (PurchaseEntities db = new PurchaseEntities())
            {
                foreach (Mst_Position position in db.Mst_Position)
                {
                    lst.Add(new SelectListItem()
                    {
                        Text = position.PositionName,
                        Value = position.PositionCode
                    });
                }
            }
            return lst;
        }
        public static List<SelectListItem> MstUserList()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "", Value = "" });
            using (PurchaseEntities db = new PurchaseEntities())
            {
                foreach (Mst_UserInfo userinfo in db.Mst_UserInfo)
                {
                    lst.Add(new SelectListItem()
                    {
                        Text = userinfo.UserName,
                        Value = userinfo.UserId
                    });
                }
            }
            return lst;
        }
        public static List<SelectListItem> HiddenCodeList(string type)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "", Value = "" });
            MasterService mstservice = new MasterService();
            //using (PurchaseEntities db = new PurchaseEntities())
            //{
            // var source = db.HiddenCode.Where(x => x.UseChk.HasValue && x.UseChk.Value).OrderBy(x=>x.InDateTime);
            var source = mstservice.HiddenCodeSearch(type);
            foreach (HiddenCode hCode in source)
            {
                if (hCode.UseChk == true)
                {
                    lst.Add(new SelectListItem()
                    {
                        Text = hCode.Name,
                        Value = hCode.Code
                    });
                }
            }
            //} 

            return lst;
        }

        private static List<SelectListItem> _HiddenCodeType;
        public static List<SelectListItem> HiddenCodeType
        {
            get
            {
                if (_HiddenCodeType == null)
                {
                    using (PurchaseEntities db = new PurchaseEntities())
                    {
                        _HiddenCodeType = new List<SelectListItem>();
                        foreach (HiddenCodeType type in db.HiddenCodeType.OrderBy(x => x.TypName).ToList())
                        {
                            _HiddenCodeType.Add(new SelectListItem()
                            {
                                Text = type.TypName,
                                Value = type.TypName
                            });
                        }
                    }
                }
                return _HiddenCodeType;
            }
        }

        private static List<SelectListItem> _UserInfoList;
        public static List<SelectListItem> UserInfoList
        {
            get
            {
                if (_UserInfoList == null)
                {
                    using (PurchaseEntities db = new PurchaseEntities())
                    {
                        _UserInfoList = new List<SelectListItem>();
                        _UserInfoList.Add(new SelectListItem() { Text = "", Value = "" });
                        foreach (Mst_UserInfo user in db.Mst_UserInfo.OrderBy(x => x.InDateTime).ToList())
                        {
                            _UserInfoList.Add(new SelectListItem()
                            {
                                Text = user.UserName,
                                Value = user.UserId
                            });
                        }
                    }
                }
                return _UserInfoList;
            }
            set { _UserInfoList = value; }
        }


        public static List<SelectListItem> UnionListAndDistinct(params List<SelectListItem>[] selListArr)
        {
            IEnumerable<SelectListItem> unionList = new List<SelectListItem>();
            if (selListArr != null && selListArr.Length > 0)
            {
                foreach (List<SelectListItem> lst in selListArr)
                {
                    unionList = unionList.Union(lst).Distinct(new SelectListItemComparer());
                }
            }
            return unionList.ToList();
        }
    }

    public class SelectListItemComparer : IEqualityComparer<SelectListItem>
    {
        public bool Equals(SelectListItem x, SelectListItem y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(SelectListItem obj)
        {
            return 1;
        }
    }
}