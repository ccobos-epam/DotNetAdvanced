using FluentValidation;

namespace SharedClasses.OptionsPattern.PostgreSQL;

public class PostgreSqlUserValidator : AbstractValidator<PostgreSqlUserOptions>
{
    public PostgreSqlUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        
        RuleFor(x => x.Password).NotEmpty();
        
        RuleFor(x => x.Schema).NotEmpty();
    }
}