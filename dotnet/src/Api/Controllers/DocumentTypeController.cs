using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chaffinch.Api.Models;

namespace Chaffinch.Api.Controllers
{
    [Route("api/[controller]")]
    public class DocumentTypeController : Controller
    {    
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateDocumentType value)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            return new OkResult();
        }
    }
}
