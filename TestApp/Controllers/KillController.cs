using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Controllers
{
    public class KillController : Controller
    {
        private CloudFoundryApplicationOptions CloudFoundryApplication { get; set; }
        private IApplicationLifetime ApplicationLifetime { get; set; }

        public KillController(IOptions<CloudFoundryApplicationOptions> appOptions, IApplicationLifetime applicationLifetime)
        {
            if (appOptions != null)
                CloudFoundryApplication = appOptions.Value;
            ApplicationLifetime = applicationLifetime;
        }
        public IActionResult Index()
        {
            ViewData["instanceNumber"] = CloudFoundryApplication != null ? CloudFoundryApplication.InstanceIndex.ToString() : "?";
            return View();
        }

        [HttpPost]
        public void Kill()
        {
            HttpContext.Response.StatusCode = 200;
            HttpContext.Response.Body.Flush();
            ApplicationLifetime.StopApplication();
        }
    }
}