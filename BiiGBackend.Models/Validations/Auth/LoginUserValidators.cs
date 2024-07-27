using BiiGBackend.Models.Requests;
using FluentValidation;
namespace TutorApplication.SharedModels.Validations.Auth
{
	public class LoginUserValidators : AbstractValidator<LoginUserRequest>
	{
		public LoginUserValidators()
		{
			RuleFor(x => x.Email)
		   .EmailAddress().WithMessage("Should be an Email")
		   .NotEmpty().WithMessage("Email should not be empty.")
		   .NotNull().WithMessage("Email should not be null.");


			RuleFor(x => x.Password)
		   .NotEmpty().WithMessage("Passwords should not be empty.")
		   .NotNull().WithMessage("Passwords should not be null.");

		}
	}
}
