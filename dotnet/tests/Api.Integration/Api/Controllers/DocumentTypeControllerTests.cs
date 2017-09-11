using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chaffinch.Api.Api.Controllers;
using Chaffinch.Api.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Chaffinch.Api.Integration.Api.Controllers
{
    public class DocumentTypeControllerTests
    {
        [Fact]
        public async Task Should_create_new_document_type()
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
