using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;

namespace DQF.Platform.Validation
{
    public static class Ext
    {
         public static IRuleBuilderOptions<T,string> DateTime<T>(this IRuleBuilder<T,string> ruleBuilder)
         {
             return ruleBuilder.SetValidator(new DateTimeValidator());
         } 
    }
}