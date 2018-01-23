using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    //供应商附件
    public class SupplierMngAttchmentFile
    {
        public SupplierMngAttchmentFile() { }
        public int Id { get; set; }
      public string FileName{get;set;}
      public bool UploadChk{get;set;}
      public string InUserId{get;set;}
      public DateTime InDateTime{get;set;}

      public SupplierMng SupplierMng { get; set; }
      public int SupplierMngId { get; set; }
    }
}
