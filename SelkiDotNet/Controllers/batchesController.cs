using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SelkiDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace SelkiDotNet.Controllers
{
    public class batchesController : ApiController
    {
        LetMeHackEntities db = new LetMeHackEntities();
        // GET: api/batches
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/batches/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/batches
        public HttpResponseMessage Post([FromBody]DtoBatch batch)
        {
            var check = db.batches.FirstOrDefault(b => b.batch_id == batch.batch_id);

            if(check != null)
            {
                dto_ConflictMessage message = new dto_ConflictMessage();
                message.message = "Batch with the same name: " + batch.batch_name + " already exists.";
                message.status = "409";
                message.developerMessage = "Batch creation failed because the batchName: " + batch.batch_name + " already exists.";
                return Request.CreateResponse(HttpStatusCode.Conflict, message);
            }
            else
            {
                batch newbatch = new batch();
                newbatch.batch_name = batch.batch_name;
                db.batches.Add(newbatch);
                db.SaveChanges();

                var baseUrl = Url.Link("DefaultApi", new { controller = "batches", newbatch.batch_id });/*Url.Content("~/");*/ /*Request.RequestUri.GetLeftPart(UriPartial.Authority);*/

                
                Dto_batchResponse response = new Dto_batchResponse();
                response.self = baseUrl.ToString();
                response.batchName = newbatch.batch_name;

                var rmsg = Request.CreateResponse(HttpStatusCode.Created, response);
               
                rmsg.Headers.Location = new Uri(Request.RequestUri + "/" + newbatch.batch_id.ToString());
                return rmsg;
            }
        }

        // PUT: api/batches/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/batches/5
        public void Delete(int id)
        {
        }
    }
}
