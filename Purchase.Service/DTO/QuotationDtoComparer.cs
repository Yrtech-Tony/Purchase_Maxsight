using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Service.DTO
{
    //定义一个类继承IEqualityComparer接口
    public class QuotationDtoComparer : IEqualityComparer<QuotationDto>
    {
        string[] ignoreProp = new string[] { "RequirementId", "RequirementSeqNO" };
        public bool Equals(QuotationDto x, QuotationDto y)
        {
            PropertyInfo[] props = x.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (ignoreProp.Contains(prop.Name))
                {
                    continue;
                }
                object tValue = prop.GetValue(x);
                object value = prop.GetValue(y);
                if (tValue != null && value != null)
                {
                    bool flag = tValue.ToString() == value.ToString();
                    if (!flag)
                    {
                        return flag;
                    }
                }
                else if (tValue == null && value == null)
                {
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        public int GetHashCode(QuotationDto obj)
        {
            return 1;
        }
    }
}
