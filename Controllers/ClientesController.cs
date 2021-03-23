using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSClientes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        clientesContext dbContext;
        private readonly ILogger<ClientesController> log;

        [HttpPost("actualizarCliente")]
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
                return Created("", cliente);
            }
            catch (Exception ex)
            {
                log.LogError("Ocurrio un problema:\n" + ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
