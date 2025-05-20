using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VideoRent.Business;
using VideoRent.Domain;

// Yan, Web API development pag. 115

namespace VideoRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController: ControllerBase
    {
        private readonly IConfiguration configuration;
        public GeneroController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<Genero>> Get()
        {
            var connectionString = configuration["ConnectionStrings:VideoRentDB"];
            GeneroBusiness generoBusiness = new GeneroBusiness(connectionString);
            IEnumerable<Genero> generos = await generoBusiness.GetGeneros();
            return generos;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
