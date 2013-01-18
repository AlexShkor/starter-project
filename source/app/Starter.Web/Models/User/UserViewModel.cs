using System;
using System.Collections.Generic;
using System.Linq;
using DQF.Domain.Aggregates.User;
using DQF.Domain.Aggregates.User.Commands;
using DQF.Platform.Extensions;
using DQF.Platform.ViewModels;
using DQF.Views;
using FluentValidation;
using FluentValidation.Results;

namespace DQF.Web.Models.User
{
    public class UserViewModel : BaseViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<EmailItem> AdditionalEmails { get; set; }

        public UserViewModel()
        {
            AdditionalEmails = new List<EmailItem>();
        }

        public UserViewModel(UserView user)
        {
            UserId = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            AdditionalEmails = user.AdditionalEmails.Select(x => new EmailItem(x)).ToList();
        }

        protected override ValidationResult Validate()
        {
            return ValidateAs<UserViewModel>(validator =>
            {
                validator.RuleFor(x => x.UserName).NotEmpty();
                validator.RuleFor(x => x.AdditionalEmails).Must(
                    x => x.All(e => e.Email.HasValue())).WithMessage("All additional emails should have value.");
            });
        }

        public UpdateUserDetails ToUpdateCommand()
        {
            return new UpdateUserDetails
            {
                Id = UserId,
                UserName = UserName,
                PhoneNumber = PhoneNumber,
                AdditionalEmails = AdditionalEmails.Select(x => x.Email).ToList()
            };
        }
    }
}