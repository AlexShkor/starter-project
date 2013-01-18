using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DQF.Platform.Validation;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Bson;
using Nancy.Json;
using Newtonsoft.Json;

namespace DQF.Platform.ViewModels
{
    public abstract class BaseViewModel
    {
        private List<ValidationError> _errors;
        private string _referrerUrl;
        public string ReferrerUrl
        {
            get
            {
                var request = HttpContext.Current.Request;
                return _referrerUrl ?? (request.UrlReferrer != null ? request.UrlReferrer.LocalPath : "/");
            }
            set { _referrerUrl = value; }
        }



        public void RedirectToRefferer()
        {
            RedirectUrl = ReferrerUrl;
        }

        public string RedirectUrl { get; set; }
        public string SuccessMessage { get; set; }
        public List<ValidationError> Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                if (value != null && value.Any()) SuccessMessage = null;
            }
        }

        protected BaseViewModel()
        {
            Errors = new List<ValidationError>();
        }

        [JsonIgnore, ScriptIgnore]
        public bool IsValid
        {
            get
            {
                var result = Validate();
                Errors = result.Errors.Select(Map).ToList();
                return result.IsValid;
            }
        }

        public bool IsValidWith(IValidator validator)
        {
            var result = validator.Validate(this);
            Errors = result.Errors.Select(Map).ToList();
            return result.IsValid;
        }

        protected abstract ValidationResult Validate();

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static string ToJson(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        private static ValidationError Map(ValidationFailure failure)
        {
            return new ValidationError
            {
                PropertyName = failure.PropertyName,
                ErrorMessage = failure.ErrorMessage,
            };
        }

        protected ValidationResult ValidateAs<TModel>(Action<GenericValidator<TModel>> action) where  TModel: BaseViewModel 
        {
            var validator = new GenericValidator<TModel>();
            action(validator);
            return validator.Validate((TModel)this);
        }
    }
}