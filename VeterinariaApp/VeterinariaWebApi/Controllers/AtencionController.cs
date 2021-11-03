using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaBackend.Dominio;
using VeterinariaBackend.Negocio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VeterinariaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtencionController : ControllerBase
    {

        private IGestorVeterinaria servicio;

        public AtencionController()
        {
            servicio = new GestorVeterinaria();
        }



        [HttpGet("GetAtencion/{id}")]
        public ActionResult GetAtencion(int id)
        {
            if (servicio.ObtenerAtencion(id).Count == 0)
            {
                return BadRequest("Problemas al consultar Mascota");
            }
            else
            {
                return Ok(servicio.ObtenerAtencion(id));
            }
        }

        [HttpGet("GetDetalleAtencion/{id}")]
        public ActionResult GetDetalleAtencion(int id)
        {

            if (servicio.GetIdAtencion(id).Count == 0)
            {
                return BadRequest("Problemas al consultar Mascota");
            }
            else
            {
                return Ok(servicio.GetIdAtencion(id));
            }
        }





        // GET: api/<AtencionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AtencionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AtencionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AtencionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AtencionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
