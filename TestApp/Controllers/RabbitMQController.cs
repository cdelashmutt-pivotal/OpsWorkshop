using System.Security.Authentication;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Net.Security;
using Microsoft.Extensions.Logging;

namespace TestApp.Controllers
{
    public class RabbitMQController : Controller
    {
        ConnectionFactory _rabbitConnection;
        bool rabbitEnabled = true;
        ILogger<RabbitMQController> _logger;

        public RabbitMQController([FromServices] ConnectionFactory rabbitConnection, [FromServices] ILogger<RabbitMQController> logger)
        {
            _rabbitConnection = rabbitConnection;
            _logger = logger;

            SslOption opt = _rabbitConnection.Ssl;
            if (opt != null && opt.Enabled)
            {
                opt.Version = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;

                // Only needed if want to disable certificate validations
                opt.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateChainErrors |
                    SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateNotAvailable;
            }
            _logger.LogDebug("RabbitMQ Connection: " + _rabbitConnection.Uri.ToString());
            if (_rabbitConnection.Uri == null || _rabbitConnection.Uri.ToString() == "amqp://127.0.0.1:5672/")
            {
                rabbitEnabled = false;
            }
        }


        public IActionResult Receive()
        {
            _logger.LogTrace("Calling RabbitMQController.Recieve");
            if(rabbitEnabled)
            {
                using (var connection = _rabbitConnection.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    CreateQueue(channel);
                    var data = channel.BasicGet("rabbit-test", true);
                    if (data != null)
                    {
                        ViewData["message"] = Encoding.UTF8.GetString(data.Body);
                    }
                }
            }
            ViewData["rabbitEnabled"] = rabbitEnabled;
            _logger.LogTrace("Returning View for RabbitMQController.Recieve");

            return View();
        }

        public IActionResult Send(string message)
        {
            _logger.LogTrace("Calling RabbitMQController.Send with message: " + message);
            if (rabbitEnabled && message != null && message != "") {
                using (var connection = _rabbitConnection.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    CreateQueue(channel);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                         routingKey: "rabbit-test",
                                         basicProperties: null,
                                         body: body);
                }
            }
            ViewData["rabbitEnabled"] = rabbitEnabled;
            _logger.LogTrace("Returning view for RabbitMQController.Send");

            return View();
        }

        protected void CreateQueue(IModel channel)
        {
            channel.QueueDeclare(queue: "rabbit-test",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        }
    }
}
