using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public SayHelloResultArgs Get(SayHelloArgs args)
        {
            return new SayHelloResultArgs { Message = $"Hello {args.Name}" };

        }
    }
}
