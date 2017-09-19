using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Chaffinch.Api.Models;
using Chaffinch.Api.Validators;
using Xunit;

namespace Chaffinch.Api.Unit.Validators
{
    public class CreateDocumentTypeValidatorTests
    {
        [Fact]
        public void Should_validate_successfully_with_valid_data()
        {
            var model = new CreateDocumentTypeModel{ Name = "Author"};
            var validator = new CreateDocumentTypeValidator();
            var results = validator.Validate(model);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void Should_require_name()
        {
            var model = new CreateDocumentTypeModel();
            var validator = new CreateDocumentTypeValidator();
            var results = validator.Validate(model);

            Assert.Contains(results.Errors, e => e.PropertyName == "Name");
        }
    }
}
