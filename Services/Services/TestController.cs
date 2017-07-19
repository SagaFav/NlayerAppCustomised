
using Application.DTO;
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
        public List<TestDTO> GetAll()
        {
            return _app.Getall();
        }

        [HttpPost]
        public void Add(TestDTO obj)
        {
            _app.Add(obj);
        }
        [HttpPost]
        public void AddForeign(TestForeign obj)
        {
            _app.AddTestForeign(obj);
        }
    }
}
