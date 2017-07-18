using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TestModule
{
    public class TestApp : ITestApp
    {
        private ITestObjRepository _repository;
        public TestApp(ITestObjRepository rep)
        {
            _repository = rep;
        }
        public List<Domain.Models.TestObjs> Getall()
        {
            return _repository.GetAll().ToList();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
