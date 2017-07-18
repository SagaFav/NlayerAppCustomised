
using Application.TestModule;
using Domain.Models;
using Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Services
{
    public class TestController : ApiController
    {
        ITestApp _app;
        public TestController(ITestApp app)
        {
            _app = app;
        }
        [HttpGet]
        public async Task<List<TestObjs>> GetAll()
        {
            var result = await Task.Run(() =>
            {
                return _app.Getall();
            });
            return result;
        }
    }
}
