using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Cache
{
    public static class CacheFactory
    {
        #region Members

        static ICacheFactory _currentFactory = null;

        #endregion

        #region Public Methods
        public static void SetCurrent(ICacheFactory factory)
        {
            _currentFactory = factory;
        }

        public static ICache CreateCache()
        {
            return (_currentFactory != null) ? _currentFactory.Create() : null;
        }

        #endregion
    }
}
