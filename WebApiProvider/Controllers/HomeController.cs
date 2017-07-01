using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Interfaces;

namespace WebApiProvider.Controllers
{
    public class HomeController: ApiController
    {
        IObserver _provider;
        public HomeController(IObserver provider) {
            _provider = provider;
        }
        public bool GetNextValue(Guid id,long current) {            
            _provider.Invoke(id, current);
            return true;
        }
        

    }
}
