using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chaffinch.Api.Models;
using Chaffinch.Core.CQRS;

namespace Chaffinch.Api.Controllers
{
    [Route("api/[controller]")]
    public class DocumentTypeController : Controller
    {
        private readonly ICommandSender _commandSender;

        public DocumentTypeController(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateDocumentTypeModel model)
        {
            if (!ModelState.IsValid)            
                return new BadRequestObjectResult(ModelState);            

            var command = new Core.WriteModel.Commands.CreateDocumentType(Guid.NewGuid(), model.Name);
            await _commandSender.Send(command);

            return new OkResult();
        }
    }
}
