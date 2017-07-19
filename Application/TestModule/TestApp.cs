using Application.DTO;
using Domain.Models;
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
        public List<TestDTO> Getall()
        {
            return _repository.GetAll().Select(s=>new TestDTO(s)).ToList();
        }
        public void Update(TestDTO obj)
        {
            var current=_repository.Get(obj.Id);
            obj.TestForeign = current.TestForeign;
            _repository.Merge(obj, current);
            _repository.UnitOfWork.Commit();
        }
        public void Add(TestDTO obj)
        {
            obj.GenerateNewIdentity();
            _repository.Add(obj);
            _repository.UnitOfWork.Commit();
        }
        public void AddTestForeign(TestForeign dto)
        {
            dto.GenerateNewIdentity();
            _repository.AddForeign(dto);
            _repository.UnitOfWork.Commit();
        }
    }
}
