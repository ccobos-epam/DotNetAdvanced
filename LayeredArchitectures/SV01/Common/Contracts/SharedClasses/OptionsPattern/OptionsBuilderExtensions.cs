using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SharedClasses.OptionsPattern;

public static class OptionsBuilderExtensions
{
  extension<TOptions>(OptionsBuilder<TOptions> builder)
    where TOptions : class
  {
    public OptionsBuilder<TOptions> ValidateFluentValidation()
    {
      builder.Services.AddSingleton<IValidateOptions<TOptions>>(
        serviceProvider => new FluentValidateOptions<TOptions>(
          serviceProvider,
          builder.Name));

      return builder;

    }
  }
}