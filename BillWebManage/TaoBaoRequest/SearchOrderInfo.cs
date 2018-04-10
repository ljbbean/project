using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaoBaoRequest
{
    public class SearchOrderInfo
    {
        public IList<object> MainOrders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalNumber { get; set; }
        public int TotalPage { get; set; }

        public string ErrorMsg { get; set; }
    }
}
