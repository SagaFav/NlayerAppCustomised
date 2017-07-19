

using Application.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TestModule
{
    public interface ITestApp 
    {
        List<TestDTO> Getall();
        void Update(TestDTO obj);
        string Add(TestDTO obj);
        string AddTestForeign(TestForeign dto);
    }
}
