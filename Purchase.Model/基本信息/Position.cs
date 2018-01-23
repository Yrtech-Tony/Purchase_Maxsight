using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Model
{
    public class Position
    {
        public Position() { }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string InUserId { get; set; }
        public DateTime InDateTime { get; set; }
    }
}
