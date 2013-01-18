using System;
using DQF.Domain.Aggregates.User;
using DQF.Domain.Aggregates.User.Commands;
using DQF.Platform.ViewModels;
using FluentValidation;
using FluentValidation.Results;

namespace DQF.Web.Models.User
{
    public class AddUserViewModel: BaseViewModel
    {
        public string UserOwnerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public AddUserViewModel()
        {
            
        }

        protected override ValidationResult Validate()
        {
            return ValidateAs<AddUserViewModel>(validator =>
            {
                validator.RuleFor(x => x.UserName).NotEmpty();
                validator.RuleFor(x => x.Password).NotEmpty();
                validator.RuleFor(x => x.ConfirmPassword).Must(x => x == Password).WithMessage(
                    "Password confirmation should be the same as password.");
            });
        }

        public CreateUser ToCommand()
        {
            var cmd = new CreateUser
            {
                Email = Email,
                Password = Password,
                Phone = Password,
                UserName = UserName,
                Role = UserRoleEnum.User
            };
            return cmd;
        }
    }
}