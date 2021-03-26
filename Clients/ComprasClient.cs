using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using MSCompras.Models;

namespace GatewayTienda.Clients
{
    public class ComprasClient
    {
        HttpClient client;
        private string urlServicio = "";

        public ComprasClient()
        {
            urlServicio = Environment.GetEnvironmentVariable("URL_MS_COMPRAS");
            client = new HttpClient();
        }

        public async Task<Compra[]> BuscarCompra(int CompraId = -1)
        {
            Compra[] comprasEncontradas;
            string urlBuscar = urlServicio + "/compras/buscar?";

            if(CompraId >= 0) 
            {
                urlBuscar += "CompraId =" + CompraId;
            }
            try
            {
                string datosCompra = await client.GetStringAsync(urlBuscar);
                comprasEncontradas = JsonConvert.DeserializeObject<Compra[]>(datosCompra);
                return comprasEncontradas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine("\nError. No se logró obtener una respuesta del servicio.");
                return null;
            }
        }

        public async Task<Compra> detallarCompra(int id)
        {
            Compra compraEncontrada;
            string urlBuscar = urlServicio + "/compras/detalles/";

            if(id >= 0) 
            {
                urlBuscar += id;
            }
            try
            {
                string datosCompra = await client.GetStringAsync(urlBuscar);
                
                compraEncontrada = JsonConvert.DeserializeObject<Compra>(datosCompra);
                return compraEncontrada;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine("\nError. No se logró obtener una respuesta del servicio.");
                return null;
            }
        }
    }
}