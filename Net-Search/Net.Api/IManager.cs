using iBoxDB.LocalServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Api
{
  interface IManager
    {
      bool Create<T>(T t);
      IBEnumerable<T> Search<T>() where T : class, new();
      bool Update<T>(T t);
      bool Delete<T>(T t);
    }
}
