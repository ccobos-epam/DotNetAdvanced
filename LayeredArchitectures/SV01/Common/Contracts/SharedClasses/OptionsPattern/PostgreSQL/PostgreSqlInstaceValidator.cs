using FluentValidation;

namespace SharedClasses.OptionsPattern.PostgreSQL;

public class PostgreSqlInstaceValidator : AbstractValidator<PostgreSqlInstanceOptions>
{
    public PostgreSqlInstaceValidator()
    {
        RuleFor(x => x.Database).NotEmpty();
        
        RuleFor(x => x.Host).NotEmpty();

        RuleFor(x => x.Port).InclusiveBetween(10000, 60000);
    }
}