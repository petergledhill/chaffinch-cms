using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chaffinch.Api.Controllers;
using Chaffinch.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Chaffinch.Api.Integration.Api.Controllers
{
    public class DocumentTypeControllerTests
    {
        [Fact]
        public async Task Should_return_success_with_valid_command()
        {
            var controller = new DocumentTypeController();         
            var newDocumentType = new CreateDocumentType
            {
                Name = "test"
            };
            var result = await controller.Post(newDocumentType);
            var response = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);           
        }
    }
}
