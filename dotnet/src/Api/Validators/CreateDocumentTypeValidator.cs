using Chaffinch.Api.Models;
using FluentValidation;

namespace Chaffinch.Api.Validators
{
    public class CreateDocumentTypeValidator : AbstractValidator<CreateDocumentTypeModel>
    {
        public CreateDocumentTypeValidator()
        {
            RuleFor(dt => dt.Name).NotEmpty();
        }
    }
}
