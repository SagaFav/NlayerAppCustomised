using Domain.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using System.Data.Entity;
namespace Domain.Repositories
{
    /// <summary>
    /// 任务相关数据库操作的仓储实现
    /// </summary>
    public class TestObjRepository : Repository<TestObjs>, ITestObjRepository
    {
        MainUnitOfWork _unitofwork;
        public TestObjRepository(IQueryableUnitOfWork unitofwork)
            : base(unitofwork)
        {
            _unitofwork = unitofwork as MainUnitOfWork;
        }



        public void Test()
        {
            throw new NotImplementedException();
        }
    }
}
