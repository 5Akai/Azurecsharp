using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Azure.Functions.FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Función C# HTTP trigger, comienza el procesamiento de la petición.");

            string nombre = req.Query["nombre"];

            string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
            dynamic datos = JsonConvert.DeserializeObject<dynamic>(requestBody);

            nombre = nombre ?? datos?.nombre;
            // Lo anterior es igual al if de abajo simplificado.
            if (nombre == null) nombre = datos.nombre;
            else nombre = nombre;
                
            string mensaje = string.IsNullOrEmpty(nombre)
                ? "Función C# HTTP trigger ejecutada correctamente. Puedes pasar NOMBRE para una respuesta personalizada"
                : $"Hola {nombre}, función C# HTTP trigger ejecutada correctamente.";

            return new OkObjectResult(mensaje);
        }
    }
}
