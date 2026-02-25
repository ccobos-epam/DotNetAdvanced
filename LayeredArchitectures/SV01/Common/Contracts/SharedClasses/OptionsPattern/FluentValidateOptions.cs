using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SharedClasses.OptionsPattern;

public class FluentValidateOptions<TOptions>(IServiceProvider serviceProvider, string? name)
  : IValidateOptions<TOptions>
  where TOptions : class
{
  public ValidateOptionsResult Validate(string? name1, TOptions options)
  {
    if (name is not null && name != name1)
    {
      return ValidateOptionsResult.Skip;
    }
    
    ArgumentNullException.ThrowIfNull(options);

    using AsyncServiceScope localScope = serviceProvider.CreateAsyncScope();

    var validator = localScope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
    
    var result = validator.Validate(options);

    if (result.IsValid)
      return ValidateOptionsResult.Success;
    
    var type = options.GetType().Name;
    var errors = new List<string>();
    
    foreach (var failure in result.Errors)
    {
      errors.Add($"Validation failed for {type}.{failure.PropertyName} with the error: {failure.ErrorMessage}");
    }
    
    return ValidateOptionsResult.Fail(errors);
  }
}