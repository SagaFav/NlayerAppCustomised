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
        public TestDTO(TestObjs obj)
        {
            this.Id = obj.Id;
            this.ForeinKey = obj.ForeinKey;
            this.Name = obj.Name;
            this.TestForeign = obj.TestForeign;
        }
        public string Fname { get; set; }
    }
}
