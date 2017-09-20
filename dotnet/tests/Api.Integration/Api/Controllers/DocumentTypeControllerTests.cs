using Chaffinch.Api.Controllers;
using Chaffinch.Api.Models;
using Chaffinch.Core.WriteModel.Commands;
using Chaffinch.CQRS.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chaffinch.Api.Integration.Api.Controllers
{
    public class DocumentTypeControllerTests : IDisposable
    {
        DocumentTypeController _controller;
        Mock<ICommandSender> _commandSender;

        CreateDocumentTypeModel _goodPayload;

        public DocumentTypeControllerTests()
        {
            _commandSender = new Mock<ICommandSender>();
            _controller = new DocumentTypeController(_commandSender.Object);

            _goodPayload = new CreateDocumentTypeModel
            {
                Name = "test"
            };
        }

        public void Dispose()
        {
            _commandSender = null;
            _controller.Dispose();
        }

        [Fact]                
        public async Task Create_should_return_success_with_valid_command()
        {             
            var result = await _controller.Post(_goodPayload);
            var response = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);           
        }


        [Fact]
        public async Task Create_should_return_failure_with_invalid_command()
        {
            _controller.ModelState.AddModelError("error", "error");

            var result = await _controller.Post(null);
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_call_command_sender()
        {          
            var result = await _controller.Post(_goodPayload);

            _commandSender.Verify(m => m.Send(
                It.Is<CreateDocumentType>(p => p.Name == "test"), 
                It.IsAny<CancellationToken>())
            );
        }
    }
    
}
