using FluentValidation;
using LetsDoIt.Moody.Web.Entities.Requests;

namespace LetsDoIt.Moody.Web.Validations
{
    public class SaveClientRequestValidator:AbstractValidator<SaveClientRequest>
    {
        public SaveClientRequestValidator()
        {
            RuleFor(scr => scr.Username).NotEmpty();
            RuleFor(scr => scr.Password).NotEmpty();
        }
    }
}
