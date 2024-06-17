using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Formacion.Azure.Functions.FunctionApp1
{
    public class Function2
    {
        private readonly ILogger<Function2> _logger;

        public Function2(ILogger<Function2> logger)
        {
            _logger = logger;
        }

        [Function("Function2")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("Function2 C# HTTP trigger -> iniciando el proceso de la petición.");

                string nombre = req.Query["nombre"];

                string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                nombre = nombre ?? data?.nombre;

                string mensaje = string.IsNullOrEmpty(nombre)
                    ? "Esta función activada por HTTP se ejecutó correctamente. Pase un nombre en la cadena de consulta o en el cuerpo de la solicitud para obtener una respuesta personalizada."
                    : $"Hola, {nombre}. Esta función activada por HTTP se ejecutó con éxito.";

                _logger.LogInformation("Function2 C# HTTP trigger -> proceso finalizado.");



                return new OkObjectResult(mensaje);
            }
            catch (Exception e)
            {
                return new ConflictObjectResult(e.Message);
            }
        }
    }
}