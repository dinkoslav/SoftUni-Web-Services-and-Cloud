using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Problem3.DistanceCalculatorRESTService.Controllers
{
    [Authorize]
    public class PointsController : ApiController
    {
        // GET api/points
        [HttpGet]
        public decimal Get([FromUri] int startX, int startY, int endX, int endY)
        {
            return (decimal)Math.Sqrt(Math.Pow((endX - startX), 2) + Math.Pow((endY - startY), 2));
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
