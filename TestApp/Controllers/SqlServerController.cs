using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using System;
using System.Data.Common;
using System.Linq;

namespace TestApp.Controllers
{
    public class SqlServerController : Controller
    {
        TestContext _context;
        bool _sqlEnabled = false;
        ILogger<SqlServerController> _logger;

        public SqlServerController([FromServices] TestContext context, [FromServices] ILogger<SqlServerController> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            SqlServerServiceInfo si = config.GetSingletonServiceInfo<SqlServerServiceInfo>();
            _sqlEnabled = si != null;

            if(!_sqlEnabled)
            {
                _logger.LogTrace("No bound SQL Database detected");
            }
            else
            {
                _logger.LogTrace("Found SQL Database: {Host:{0}, Port:{1}, User: {2}", si.Host, si.Port, si.UserName);
            }
        }
        public IActionResult Index()
        {
            DbConnection connection = _context.Database.GetDbConnection();
            ViewData["sqlEnabled"] = _sqlEnabled;
            if ((bool)ViewData["sqlEnabled"])
            {
                _logger.LogTrace($"Retrieving data from {connection.DataSource}/{connection.Database}");
                return View(_context.TestData.ToList());
            } else
            {
                return View();
            }
        }
    }
}
