using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WEB.Util
{
    public class MoedaAttribute : ValidationAttribute
    {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                try
                {
                    var moeda = Convert.ToDecimal(value, new CultureInfo("en-US"));
                }
                catch (Exception)
                {
                    return new ValidationResult("Coin in format invalid.");
                }

                return ValidationResult.Success;
            }
    }


    public class MoedaAttributeAdapter : AttributeAdapterBase<MoedaAttribute>
    {

        public MoedaAttributeAdapter(MoedaAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        { }
        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-moeda", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Coin in format invalid (Message Error Personalized with AttributeAdapterBase).";
        }


        public class MoedaValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
        {
            private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();
            public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
            {
                if (attribute is MoedaAttribute moedaAttribute)
                {
                    return new MoedaAttributeAdapter(moedaAttribute, stringLocalizer);
                }

                return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}
