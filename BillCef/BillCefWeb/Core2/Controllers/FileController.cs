using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core2.Model;
using Core2.DAL;

namespace Core2.Controllers
{
    [Produces("application/json")]
    [Route("api/File")]
    public class FileController : Controller
    {
        // GET: api/File
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { };
        }
        // GET: api/File/5
        [HttpGet("{id}", Name = "Get")]
        public PlugDownFileModel Get(string id)
        {
            MySqlContext mysql = new MySqlContext();
            List<Plugs> list = mysql.plugs.ToList();
            Plugs model = list.Find(m =>
            {
                return m.PID.ToString() == id && m.PStatus == 0;
            });
            if (model == null)
            {
                return null;
            }
            PlugDownFileModel pmodel = new PlugDownFileModel();
            pmodel.ID = model.PID;
            pmodel.Kind = model.PKind;
            pmodel.DownFile = string.Format("{0}{1}", model.PDownpathWeb, model.PDownPath);
            pmodel.Ext = model.PExt;
            pmodel.MainFileName = model.PWindowName;
            pmodel.Name = model.PName;
            pmodel.OpenWay = model.PShowWay;
            pmodel.Total = model.PTotal;
            pmodel.UpdateDate = model.PUpdateDate;
            return pmodel;
        }

        // POST: api/File
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/File/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
