using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chaffinch.Api.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chaffinch.Api.Api.Controllers
{
    [Route("api/[controller]")]
    public class DocumentTypeController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateDocumentType value)
        {            
            return new OkResult();
        }
    }
}
