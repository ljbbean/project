using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Data;
using System.Text;
using TaoBaoRequest;

namespace Test001.DataHandler
{
    public class BillList : Page
    {
        public override void Initialize()
        {
            base.Initialize();
            DateTime time = DateTime.Now;
            Context["startDate"] = new DateTime(time.Year, time.Month, 1);
        }
    }
}
