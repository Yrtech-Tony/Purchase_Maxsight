using Pechkin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Web.Common
{
    public class HtmlToPDFTool
    {
        public static void StringToPDF(string html,string fileName)
        {
            using (IPechkin pechkin = Factory.Create(new GlobalConfig()))            
            {
                ObjectConfig oConfig = new ObjectConfig();
                oConfig.SetMinFontSize(14);
                byte[] pdf = pechkin.Convert(oConfig, html);
                File.WriteAllBytes(fileName, pdf);
            }
        }
    }
}
