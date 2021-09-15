using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private CosmosClient cosmosClient;

        PricesController(CosmosClient client)
        {
            this.cosmosClient = client;
        }

        // GET: api/<PricesController>
        [HttpGet]
        public string Get()
        {
            var PricingService = new PricingService();

            return "Price: " + PricingService.GetPrice("whocares", this.cosmosClient);
        }

        // GET api/<PricesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PricesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PricesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PricesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
