﻿using FluentValidation;

namespace COO.Business.Logic.Account.Read.Authentication
{
    public class AuthenticationQueryValidator : AbstractValidator<AuthenticationQuery>
    {
        public AuthenticationQueryValidator()
        {
            RuleFor(x => x.Login).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
