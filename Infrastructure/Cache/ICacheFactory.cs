using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Cache
{
    public interface ICacheFactory
    {
        ICache Create();
    }
}
