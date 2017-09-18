using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Chaffinch.Api.Functional.Routes
{
    public class DocumentTypeRouteTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public DocumentTypeRouteTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async void Create_documenttype_should_return_success_with_valid_payload()
        {
            var model = new { Name = "Test" };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/DocumentType", stringContent);
         
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async void Create_documenttype_should_return_400_with_invalid_payload()
        {
            var model = new {  };
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/DocumentType", stringContent);
            var body = await response.Content.ReadAsStringAsync();
                        
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("{\"Name\":[\"'Name' should not be empty.\"]}", body);
        }
    }
}
