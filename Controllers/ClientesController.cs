using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSClientes.Models;

namespace MSClientes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private clientesContext dbContext;
        private readonly ILogger<ClientesController> log;

        public ClientesController(ILogger<ClientesController> logger) 
        {
            log = logger;
            dbContext = new clientesContext();
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<Cliente>> Search([FromQuery]string nombre = "", [FromQuery] int idCliente = -1)
        {
            List<Cliente> clientes = null;

            clientes = await dbContext.Clientes
                .Where(cliente => cliente.Nombre.Contains(nombre))
                .Where(cliente => (idCliente >= 0 && cliente.Id == idCliente) || 
                (idCliente < 0 && cliente.Id != idCliente))
                .ToListAsync();

            if (clientes == null)
            {
                return BadRequest();
            }

            return Ok(clientes);
        }

        [HttpPost("RegistrarCliente")]
        public async Task<ActionResult<Cliente>> Add([FromBody]Cliente cliente)
        {
            if(cliente == null)
            {
                log.LogError("Ingreso datos vacios");
                return BadRequest("Ingreso datos vacios");
            }

            try
            {
                dbContext.Entry(cliente).State = EntityState.Added;
                await dbContext.SaveChangesAsync();

                log.LogInformation("Se agregó nuevo cliente: {0}", cliente.Nombre);
                return Created("", cliente);
            }
            catch (Exception ex)
            {
                log.LogError("Sucedio una excepcion:\n" + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpPut("actualizarCliente")]
        public async Task<ActionResult<Cliente>> update([FromBody]Cliente cliente)
        {
            if(cliente == null)
            {
                log.LogError("No se encontro el cliente a actualizar");
                return BadRequest("Cliente no encontrado");
            }

            try
            {
                var miCliente = dbContext.Clientes.SingleOrDefault(c => c.Id == cliente.Id);
                miCliente = cliente;
                await dbContext.SaveChangesAsync();
                
                log.LogInformation("Se actualizo el cliente: {0}", cliente.Nombre);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                log.LogError("Ocurrio un problema:\n" + ex.Message);
                return BadRequest(ex);
            }
        }
    }
}