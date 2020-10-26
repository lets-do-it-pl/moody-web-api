using FluentValidation;
using LetsDoIt.Moody.Web.Entities.Requests;

namespace LetsDoIt.Moody.Web.Validations
{
    public class SaveUserRequestValidator:AbstractValidator<SaveUserRequest>
    {
        public SaveUserRequestValidator()
        {
            RuleFor(sur => sur.Email).EmailAddress();

            RuleFor(sur => sur.Email).NotEmpty();

            RuleFor(sur => sur.Name).NotEmpty();

            RuleFor(sur => sur.Password).NotEmpty();

            RuleFor(sur => sur.Username).NotEmpty();

            RuleFor(sur => sur.Surname).NotEmpty();
        }
    }
}
