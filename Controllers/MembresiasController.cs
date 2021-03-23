using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSClientes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MSClientes.Controllers
{
    
   
    
    [Route("[controller]")]
    [ApiController]
    public class MembresiasController : ControllerBase
    {
        private ILogger<MembresiasController> _logger;
        private clientesContext dbContext;

        public MembresiasController(ILogger<MembresiasController> logger)
        {
            _logger = logger;
            dbContext = new clientesContext();
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<Membresia>> Get([FromQuery] int idCliente = -1, [FromQuery] int tipo = -1)
        {
            List<Membresia> membresia = null;

            membresia = await dbContext.Membresias
                .Where(p => (p.IdCliente >= 0 && p.IdCliente == idCliente))
                .Where(p => (p.Tipo >= 0 && p.Tipo == tipo))
                .ToListAsync();
            if (membresia == null) return BadRequest();
            else return Ok(membresia);
        }
        
        [HttpPost("crear")]
        public async Task<ActionResult<Membresia>> create([FromBody]Membresia membresia)
        {   
            if(membresia == null)
            {
                return BadRequest("membresia nula");
            }

            try
            {
                dbContext.Entry(membresia).State = EntityState.Added;
                await dbContext.SaveChangesAsync();

                return Ok(membresia);
            }
            catch(Exception excepcion)
            {        
                 return BadRequest(excepcion);
            }
          
        }

        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult<Membresia>> Update(int id, [FromBody]Membresia cambioMembresia)
        {
            if(cambioMembresia == null)
            {
                _logger.LogError("Body es null");
                return BadRequest("Body es null");
            }

            try
            {
                dbContext.Entry(cambioMembresia).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Se actualizó Membresía: {0}", cambioMembresia.Id);
                return Ok(cambioMembresia);
            }
            catch(DbUpdateConcurrencyException dbex)
            {
                var item = await dbContext.Membresias.FirstOrDefaultAsync(m => m.Id == id);
                if(item == null)
                {
                    _logger.LogDebug("No se encontró una membresía con ID {0}", id.ToString());
                    _logger.LogError("Ocurrió una excepción al actualizar:\n" + dbex.Message);
                    return NotFound();
                } else return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió una excepción al conectarse a la bd:\n" + ex.Message);
                return BadRequest(ex);
            }
        }
    }
}