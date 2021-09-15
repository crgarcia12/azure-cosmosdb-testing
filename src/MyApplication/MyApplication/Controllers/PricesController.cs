using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using MyApplication.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private CosmosClient cosmosClient;

        public PricesController(CosmosClient client)
        {
            this.cosmosClient = client;
        }

        // GET: api/<PricesController>
        [HttpGet]
        public async Task<List<Product>> Get()
        {
            var pricingService = new PricingService();
            return await pricingService.GetPricesAsync(this.cosmosClient);
        }

        // GET api/<PricesController>/5
        [HttpGet("{name}")]
        public async Task<Product> Get(string name)
        {
            var pricingService = new PricingService();
            return await pricingService.GetPriceAsync(name, this.cosmosClient); ;
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
