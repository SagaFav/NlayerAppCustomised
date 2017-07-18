
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Cache
{
    public class MCacheFactory : ICacheFactory
    {
        public ICache Create()
        {
            return new MCache();
        }
    }
}
