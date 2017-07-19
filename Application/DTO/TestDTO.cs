using Domain.Datas;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class TestDTO : TestObjs
    {

        public string Fname { get; set; }
    }
    public static  class TestConvertor
    {
        public static TestDTO ConvertToDTO(TestObjs obj)
        {
            var result = new TestDTO();
            result.Fname =obj.TestForeign==null?null: obj.TestForeign.Fname;
            result.ForeinKey = obj.ForeinKey;
            result.Id = obj.Id;
            result.TestForeign = obj.TestForeign;
            result.Name = obj.Name;
            return result;
        }
        public static TestObjs ConvertToEntity(TestDTO obj)
        {
            var result = new TestObjs();
            result.ForeinKey = obj.ForeinKey;
            result.Id = obj.Id;
            result.TestForeign = obj.TestForeign;
            result.Name = obj.Name;
            return result;
        }
    }
}
