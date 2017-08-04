

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
            return _repository.GetAll("TestForeign").Select(s => TestConvertor.ConvertToDTO(s)).ToList();
        }
        public void Update(TestDTO obj)
        {
            _repository.Modify(TestConvertor.ConvertToEntity(obj));
            _repository.UnitOfWork.Commit();

        }
        public string Add(TestDTO obj)
        {
            obj.GenerateNewIdentity();
            _repository.Add(TestConvertor.ConvertToEntity(obj));
            _repository.UnitOfWork.Commit();
            return obj.Id;
        }
        public string AddTestForeign(TestForeign dto)
        {
            dto.GenerateNewIdentity();
            _repository.AddForeign(dto);
            _repository.UnitOfWork.Commit();
            return dto.Id;
        }


        public void Remove(string key)
        {
            try
            {

                var data = _repository.Get(key);
                _repository.Remove(data);
                _repository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
